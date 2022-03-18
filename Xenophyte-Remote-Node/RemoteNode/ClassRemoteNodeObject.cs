﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xenophyte_Connector_All.Remote;
using Xenophyte_Connector_All.Seed;
using Xenophyte_Connector_All.Setting;
using Xenophyte_Connector_All.Utils;
using Xenophyte_RemoteNode.Data;
using Xenophyte_RemoteNode.Log;

namespace Xenophyte_RemoteNode.RemoteNode
{
    public class ClassRemoteNodeObject
    {
        #region Variable/Object

        /// <summary>
        ///     Type
        /// </summary>
        public string RemoteNodeObjectType;

        /// <summary>
        ///     Status
        /// </summary>
        public bool RemoteNodeObjectConnectionStatus;

        public bool RemoteNodeObjectThreadStatus;

        public bool RemoteNodeObjectLoginStatus;

        public long RemoteNodeObjectLastPacketReceived;

        /// <summary>
        ///     Network object
        /// </summary>
        public ClassSeedNodeConnector RemoteNodeObjectTcpClient;

        /// <summary>
        ///     Setting
        /// </summary>
        private const int RemoteNodeObjectLoopSendRequestInterval = 1;
        private const int RemoteNodeObjectLoopSendRequestInterval2 = 1000;

        private long _lastKeepAlivePacketSent; // Last keep alive packet sent.
        private const int KeepAlivePacketDelay = 5; // Send keep alive packet every 1 second.

        /// <summary>
        ///     Reserved to type of transaction sync.
        /// </summary>
        public bool RemoteNodeObjectInSyncTransaction;

        public bool RemoteNodeObjectInReceiveTransaction;

        /// <summary>
        ///     Reserved to type of block sync.
        /// </summary>
        public bool RemoteNodeObjectInSyncBlock;

        public bool RemoteNodeObjectInReceiveBlock;

        public const int MaxTransactionRange = 10;
        public static bool EnableTransactionRange = true;
        public CancellationTokenSource CancellationRemoteNodeObject;

        #endregion

        /// <summary>
        ///     Constructor, initialize remote node object of sync.
        /// </summary>
        /// <param name="type"></param>
        public ClassRemoteNodeObject(string type)
        {
            RemoteNodeObjectType = type;
        }

        /// <summary>
        ///     Start connection, return if the connection is okay.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> StartConnectionAsync()
        {
            try
            {
                if (CancellationRemoteNodeObject != null)
                {
                    if (!CancellationRemoteNodeObject.IsCancellationRequested)
                    {
                        CancellationRemoteNodeObject.Cancel();
                    }
                }
            }
            catch
            {

            }
            CancellationRemoteNodeObject = new CancellationTokenSource();

            if (RemoteNodeObjectTcpClient == null)
                RemoteNodeObjectTcpClient = new ClassSeedNodeConnector();
            else // For be sure.
                await StopConnection(string.Empty);

            if (RemoteNodeObjectType == SyncEnumerationObject.ObjectToBePublic)
            {
                ClassRemoteNodeSync.ImPublicNode = false;
                ClassRemoteNodeSync.ListOfPublicNodes.Clear();
                ClassRemoteNodeSync.MyOwnIP = string.Empty;
            }
            if (await RemoteNodeObjectTcpClient
                .StartConnectToSeedAsync(string.Empty))
            {
                RemoteNodeObjectConnectionStatus = true;
                RemoteNodeListenNetworkAsync();
                if (await RemoteNodeObjectTcpClient
                    .SendPacketToSeedNodeAsync(Program.Certificate, string.Empty))
                    if (await RemoteNodeObjectTcpClient
                        .SendPacketToSeedNodeAsync(ClassConnectorSettingEnumeration.RemoteLoginType + ClassConnectorSetting.PacketContentSeperator + Program.RemoteNodeWalletAddress, Program.Certificate,
                            false, true))
                    {
                        RemoteNodeSendNetworkAsync();
                        return true;
                    }

                RemoteNodeObjectConnectionStatus = false;
                return false;
            }

            RemoteNodeObjectConnectionStatus = false;
            return false;
        }

        /// <summary>
        ///     Stop sync connection, close every thread.
        /// </summary>
        public async Task StopConnection(string disconnectType)
        {
            switch (disconnectType)
            {
                case ClassRemoteNodeObjectStopConnectionEnumeration.Timeout:
                    ClassLog.Log(
                        "Object sync " + RemoteNodeObjectType +
                        " not keep alive packet received since a long time. Reconnect it.", 0, 2);
                    break;
                case ClassRemoteNodeObjectStopConnectionEnumeration.End:
                    ClassLog.Log("Object sync " + RemoteNodeObjectType + " disconnected from exit command line.", 0, 2);
                    break;
            }

            RemoteNodeObjectLoginStatus = false;
            RemoteNodeObjectThreadStatus = false;
            RemoteNodeObjectConnectionStatus = false;
            RemoteNodeObjectInReceiveBlock = false;
            RemoteNodeObjectInReceiveTransaction = false;
            RemoteNodeObjectInSyncBlock = false;
            RemoteNodeObjectInSyncTransaction = false;
            try
            {
                RemoteNodeObjectTcpClient?.DisconnectToSeed();
                RemoteNodeObjectTcpClient?.Dispose();
            }
            catch
            {
                // Ignored.
            }

            if (RemoteNodeObjectType == SyncEnumerationObject.ObjectToBePublic)
            {
                ClassRemoteNodeSync.ImPublicNode = false;
                ClassRemoteNodeSync.ListOfPublicNodes.Clear();
                ClassRemoteNodeSync.MyOwnIP = string.Empty;
            }

            if (!string.IsNullOrEmpty(disconnectType))
            {
                await Task.Delay(100);
            }


            try
            {
                if (CancellationRemoteNodeObject != null)
                {
                    if (!CancellationRemoteNodeObject.IsCancellationRequested)
                    {
                        CancellationRemoteNodeObject.Cancel();
                    }
                }
            }
            catch
            {
                // Ignored.
            }

        }

        /// <summary>
        ///     Listen packet from the network.
        /// </summary>
        private async void RemoteNodeListenNetworkAsync()
        {
            try
            {
                if (CancellationRemoteNodeObject.IsCancellationRequested)
                {
                    CancellationRemoteNodeObject = new CancellationTokenSource();
                }
                await Task.Factory.StartNew(async () =>
                {
                    RemoteNodeObjectThreadStatus = true;
                    while (RemoteNodeObjectConnectionStatus && RemoteNodeObjectThreadStatus)
                    {
                        try
                        {
                            if (!RemoteNodeObjectTcpClient.ReturnStatus())
                            {
                                RemoteNodeObjectConnectionStatus = false;
                                RemoteNodeObjectThreadStatus = false;
                                break;
                            }
                            var packetReceived = await RemoteNodeObjectTcpClient.ReceivePacketFromSeedNodeAsync(Program.Certificate, false, true);


                            if (packetReceived == ClassSeedNodeStatus.SeedError)
                            {
                                RemoteNodeObjectConnectionStatus = false;
                                RemoteNodeObjectThreadStatus = false;
                                break;
                            }


                            if (packetReceived.Contains(ClassConnectorSetting.PacketSplitSeperator))
                            {
                                var splitPacketReceived = packetReceived.Split(new[] { ClassConnectorSetting.PacketSplitSeperator }, StringSplitOptions.None);
                                if (splitPacketReceived.Length > 1)
                                {
                                    foreach (var packet in splitPacketReceived)
                                        if (!string.IsNullOrEmpty(packet))
                                            if (packet.Length > 1)
                                            {
                                                var packetRecv = packet.Replace(ClassConnectorSetting.PacketSplitSeperator, "");
                                                ClassLog.Log("Packet received from blockchain: " + packet, 4, 0);

                                                if (packetRecv == ClassSeedNodeStatus.SeedError)
                                                {
                                                    RemoteNodeObjectConnectionStatus = false;
                                                    RemoteNodeObjectThreadStatus = false;
                                                    break;
                                                }

                                                await RemoteNodeHandlePacketNetworkAsync(packetRecv);
                                            }
                                }
                                else
                                {
                                    ClassLog.Log("Packet received from blockchain: " + packetReceived, 4, 0);


                                    if (packetReceived.Replace(ClassConnectorSetting.PacketSplitSeperator, "") == ClassSeedNodeStatus.SeedError)
                                    {
                                        RemoteNodeObjectConnectionStatus = false;
                                        RemoteNodeObjectThreadStatus = false;
                                        break;
                                    }

                                    await RemoteNodeHandlePacketNetworkAsync(packetReceived.Replace(ClassConnectorSetting.PacketSplitSeperator, ""));

                                }
                            }
                            else
                            {
                                if (packetReceived != ClassSeedNodeStatus.SeedNone)
                                    RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();

                                if (packetReceived == ClassSeedNodeStatus.SeedError)
                                {
                                    RemoteNodeObjectConnectionStatus = false;
                                    RemoteNodeObjectThreadStatus = false;
                                    break;
                                }

                                await RemoteNodeHandlePacketNetworkAsync(packetReceived);

                            }
                        }
                        catch
                        {
                            RemoteNodeObjectThreadStatus = false;
                            RemoteNodeObjectConnectionStatus = false;
                            break;
                        }
                    }
                    RemoteNodeObjectThreadStatus = false;
                }, CancellationRemoteNodeObject.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current).ConfigureAwait(false);
            }
            catch (Exception error)
            {
                RemoteNodeObjectConnectionStatus = false;
                RemoteNodeObjectThreadStatus = false;
            }
        }

        /// <summary>
        ///     Depending of the type of sync, the remote node will send the right packet to sync the right information.
        /// </summary>
        private async void RemoteNodeSendNetworkAsync()
        {
            try
            {
                if (CancellationRemoteNodeObject.IsCancellationRequested)
                {
                    CancellationRemoteNodeObject = new CancellationTokenSource();
                }
                await Task.Factory.StartNew(async () =>
                {
                    while (RemoteNodeObjectConnectionStatus && RemoteNodeObjectThreadStatus)
                    {
                        if (RemoteNodeObjectLoginStatus)
                        {
                            switch (RemoteNodeObjectType)
                            {
                                #region Sync Block

                                case SyncEnumerationObject.ObjectBlock:
                                    if (!string.IsNullOrEmpty(ClassRemoteNodeSync.TotalBlockMined) && !string.IsNullOrEmpty(ClassRemoteNodeSync.TotalTransaction) && !string.IsNullOrEmpty(ClassRemoteNodeSync.CoinCirculating) && !string.IsNullOrEmpty(ClassRemoteNodeSync.CoinMaxSupply) && !string.IsNullOrEmpty(ClassRemoteNodeSync.CurrentBlockLeft) && !string.IsNullOrEmpty(ClassRemoteNodeSync.CurrentDifficulty) && !string.IsNullOrEmpty(ClassRemoteNodeSync.CurrentHashrate) && !string.IsNullOrEmpty(ClassRemoteNodeSync.CurrentTotalFee))
                                    {
                                        if (long.TryParse(ClassRemoteNodeSync.TotalBlockMined, out var askBlock))
                                        {
                                            if (!string.IsNullOrEmpty(ClassRemoteNodeSync.TotalBlockMined)
                                            ) // Ask Blocks only when this information is sync.
                                            {
                                                if (ClassRemoteNodeSync.ListOfBlock.Count.ToString() !=
                                                    ClassRemoteNodeSync.TotalBlockMined)
                                                {
                                                    if (long.TryParse(ClassRemoteNodeSync.TotalBlockMined,
                                                        out var totalBlockMined))
                                                    {
                                                        if (ClassRemoteNodeSync.ListOfBlock.Count > totalBlockMined + 1)
                                                        {
                                                            ClassLog.Log("Too much block, clean sync: ", 2, 3);
                                                            ClassRemoteNodeSync.ListOfBlock.Clear();
                                                            ClassRemoteNodeSync.ListOfBlockHash.Clear();
                                                            ClassRemoteNodeKey.DataBlockRead = string.Empty;
                                                        }

                                                        askBlock -= ClassRemoteNodeSync.ListOfBlock.Count;
                                                        if (askBlock > 0)
                                                        {
                                                            long totalBlockSaved = ClassRemoteNodeSync.ListOfBlock.Count;
                                                            for (var i = 0; i < askBlock; i++)
                                                            {
                                                                var cancelBlock = false;
                                                                long blockIdAsked = totalBlockSaved + i;
                                                                RemoteNodeObjectInReceiveBlock = true;
                                                                if (!await RemoteNodeObjectTcpClient
                                                                    .SendPacketToSeedNodeAsync(
                                                                        ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                                                            .RemoteAskBlockPerId + ClassConnectorSetting.PacketContentSeperator + blockIdAsked.ToString("F0"),
                                                                        Program.Certificate,
                                                                        false, true))
                                                                {
                                                                    RemoteNodeObjectInReceiveBlock = false;
                                                                    RemoteNodeObjectConnectionStatus = false;
                                                                    break;
                                                                }

                                                                while (RemoteNodeObjectInReceiveBlock)
                                                                {
                                                                    var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectBlock
                                                                        .RemoteNodeObjectLastPacketReceived;
                                                                    var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                                                                    if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeSyncResponse < currentTimestamp)
                                                                    {
                                                                        ClassLog.Log(
                                                                            "Sync object block mined, take too much time to receive a block, cancel and retry now.",
                                                                            2, 3);
                                                                        cancelBlock = true;
                                                                        RemoteNodeObjectInSyncBlock = false;
                                                                        RemoteNodeObjectInReceiveBlock = false;
                                                                        break;
                                                                    }

                                                                    if (!RemoteNodeObjectConnectionStatus) break;

                                                                    await Task.Delay(1);
                                                                }

                                                                if (cancelBlock)
                                                                {
                                                                    RemoteNodeObjectConnectionStatus = false;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (!await RemoteNodeObjectTcpClient
                                                        .SendPacketToSeedNodeAsync(
                                                            ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                                                .RemoteAskTotalBlockMined, Program.Certificate, false, true)
                                                        )
                                                    {
                                                        RemoteNodeObjectConnectionStatus = false;
                                                        break;
                                                    }

                                                    await Task.Delay(100);
                                                }
                                            }

                                            RemoteNodeObjectInSyncBlock = false;
                                        }

                                        RemoteNodeObjectInSyncBlock = false;
                                    }
                                    break;

                                #endregion

                                #region Sync Transaction

                                case SyncEnumerationObject.ObjectTransaction:

                                    if (!string.IsNullOrEmpty(ClassRemoteNodeSync.TotalBlockMined) && !string.IsNullOrEmpty(ClassRemoteNodeSync.TotalTransaction) && !string.IsNullOrEmpty(ClassRemoteNodeSync.CoinCirculating) && !string.IsNullOrEmpty(ClassRemoteNodeSync.CoinMaxSupply) && !string.IsNullOrEmpty(ClassRemoteNodeSync.CurrentBlockLeft) && !string.IsNullOrEmpty(ClassRemoteNodeSync.CurrentDifficulty) && !string.IsNullOrEmpty(ClassRemoteNodeSync.CurrentHashrate) && !string.IsNullOrEmpty(ClassRemoteNodeSync.CurrentTotalFee))
                                    {
                                        if (long.TryParse(ClassRemoteNodeSync.TotalTransaction, out _))
                                        {
                                            if (!string.IsNullOrEmpty(ClassRemoteNodeSync.TotalTransaction)
                                            ) // Ask Transactions only when this information is sync.
                                            {
                                                RemoteNodeObjectInSyncTransaction = true;
                                                if (ClassRemoteNodeSync.ListOfTransaction.Count.ToString() !=
                                                    ClassRemoteNodeSync.TotalTransaction)
                                                {
                                                    if (long.TryParse(ClassRemoteNodeSync.TotalTransaction,
                                                        out var totalTransaction))
                                                    {


                                                        if (long.TryParse(ClassRemoteNodeSync.TotalTransaction,
                                                            out var askTransaction))
                                                        {
                                                            long totalTransactionSaved =
                                                                ClassRemoteNodeSync.ListOfTransaction.Count;

                                                            if (totalTransactionSaved < askTransaction)
                                                            {

                                                                long transactionIdAsked = ClassRemoteNodeSync.ListOfTransaction.Count;

                                                                if (transactionIdAsked <= askTransaction)
                                                                {

                                                                    RemoteNodeObjectInReceiveTransaction = true;
                                                                    if (EnableTransactionRange)
                                                                    {
                                                                        if (!await RemoteNodeObjectTcpClient
                                                                            .SendPacketToSeedNodeAsync(
                                                                                ClassRemoteNodeCommand
                                                                                    .ClassRemoteNodeSendToSeedEnumeration
                                                                                    .RemoteAskTransactionPerRange + ClassConnectorSetting.PacketContentSeperator +
                                                                                transactionIdAsked.ToString("F0") + ClassConnectorSetting.PacketContentSeperator + MaxTransactionRange, Program.Certificate, false, true))
                                                                        {
                                                                            RemoteNodeObjectConnectionStatus = false;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (!await RemoteNodeObjectTcpClient.SendPacketToSeedNodeAsync(ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteAskTransactionPerId + ClassConnectorSetting.PacketContentSeperator + transactionIdAsked.ToString("F0"), Program.Certificate, false, true))
                                                                        {
                                                                            RemoteNodeObjectConnectionStatus = false;
                                                                        }
                                                                    }

                                                                    while (RemoteNodeObjectInReceiveTransaction)
                                                                    {
                                                                        if (!RemoteNodeObjectConnectionStatus)
                                                                        {
                                                                            break;
                                                                        }

                                                                        if (ClassRemoteNodeSync.ListOfBlock.Count < int.Parse(ClassRemoteNodeSync.TotalBlockMined))
                                                                        {
                                                                            await Task.Delay(ClassUtils.GetRandomBetween(1, 100));
                                                                        }
                                                                        else
                                                                        {
                                                                            await Task.Delay(1);
                                                                        }
                                                                    }
                                                                }

                                                            }
                                                            else
                                                            {
                                                                if (long.TryParse(ClassRemoteNodeSync.TotalTransaction, out totalTransaction))
                                                                {
                                                                    if (totalTransactionSaved > totalTransaction + 1)
                                                                    {
                                                                        ClassLog.Log("Too much transaction, clean sync: ", 2, 3);
                                                                        ClassRemoteNodeSync.ListOfTransaction.Clear();
                                                                        ClassRemoteNodeSync.ListOfTransactionHash.Clear();
                                                                        ClassRemoteNodeSync.ListTransactionPerWallet.Clear();
                                                                        ClassRemoteNodeKey.DataTransactionRead = string.Empty;
                                                                        await StopConnection(string.Empty);
                                                                        break;
                                                                    }
                                                                    else
                                                                    {
                                                                        for (var i = 0; i < totalTransaction; i++) // Research missed tx.
                                                                        {
                                                                            if (!RemoteNodeObjectConnectionStatus)
                                                                            {
                                                                                break;
                                                                            }
                                                                            if (i < totalTransaction)
                                                                            {
                                                                                if (!ClassRemoteNodeSync.ListOfTransaction.ContainsKey(i))
                                                                                {
                                                                                    RemoteNodeObjectInReceiveTransaction = true;
                                                                                    if (!await RemoteNodeObjectTcpClient
                                                                                        .SendPacketToSeedNodeAsync(
                                                                                            ClassRemoteNodeCommand
                                                                                                .ClassRemoteNodeSendToSeedEnumeration
                                                                                                .RemoteAskTransactionPerId + ClassConnectorSetting.PacketContentSeperator +
                                                                                            i.ToString("F0"), Program.Certificate, false, true))
                                                                                    {
                                                                                        RemoteNodeObjectConnectionStatus = false;
                                                                                    }

                                                                                    while (RemoteNodeObjectInReceiveTransaction)
                                                                                    {
                                                                                        if (!RemoteNodeObjectConnectionStatus)
                                                                                        {
                                                                                            break;
                                                                                        }

                                                                                        if (ClassRemoteNodeSync.ListOfBlock.Count < int.Parse(ClassRemoteNodeSync.TotalBlockMined))
                                                                                        {
                                                                                            await Task.Delay(ClassUtils.GetRandomBetween(1, 100));
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            await Task.Delay(ClassUtils.GetRandomBetween(1, 10));
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }

                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (!await RemoteNodeObjectTcpClient
                                                        .SendPacketToSeedNodeAsync(
                                                            ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                                                .RemoteNumberOfTransaction, Program.Certificate, false, true))
                                                    {
                                                        RemoteNodeObjectConnectionStatus = false;
                                                        break;
                                                    }

                                                    await Task.Delay(100);
                                                }

                                                RemoteNodeObjectInSyncTransaction = false;
                                            }

                                        }
                                    }
                                    break;

                                #endregion

                                #region Sync Total Block Mined Information

                                case SyncEnumerationObject.ObjectBlockMined:
                                    if (!await RemoteNodeObjectTcpClient
                                        .SendPacketToSeedNodeAsync(
                                            ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                                .RemoteAskTotalBlockMined, Program.Certificate, false, true)
                                        )
                                        RemoteNodeObjectConnectionStatus = false;

                                    break;

                                #endregion

                                #region Sync Coin Circulating Information

                                case SyncEnumerationObject.ObjectCoinCirculating:
                                    if (!await RemoteNodeObjectTcpClient
                                        .SendPacketToSeedNodeAsync(
                                            ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                                .RemoteAskCoinCirculating, Program.Certificate, false, true)
                                        )
                                        RemoteNodeObjectConnectionStatus = false;

                                    break;

                                #endregion

                                #region Sync Max Coin Supply Information

                                case SyncEnumerationObject.ObjectCoinSupply:
                                    if (!await RemoteNodeObjectTcpClient
                                        .SendPacketToSeedNodeAsync(
                                            ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                                .RemoteAskCoinMaxSupply, Program.Certificate, false, true)
                                        )
                                        RemoteNodeObjectConnectionStatus = false;

                                    break;

                                #endregion

                                #region Sync Current Mining Difficulty Information

                                case SyncEnumerationObject.ObjectCurrentDifficulty:
                                    if (!await RemoteNodeObjectTcpClient
                                        .SendPacketToSeedNodeAsync(
                                            ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                                .RemoteAskCurrentDifficulty, Program.Certificate, false, true)
                                        )
                                        RemoteNodeObjectConnectionStatus = false;

                                    break;

                                #endregion

                                #region Sync Current Mining Hashrate Information

                                case SyncEnumerationObject.ObjectCurrentRate:
                                    if (!await RemoteNodeObjectTcpClient
                                        .SendPacketToSeedNodeAsync(
                                            ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                                .RemoteAskCurrentRate, Program.Certificate, false, true)
                                        )
                                        RemoteNodeObjectConnectionStatus = false;

                                    break;

                                #endregion

                                #region Sync Total Pending Transaction Information

                                case SyncEnumerationObject.ObjectPendingTransaction:
                                    if (!await RemoteNodeObjectTcpClient
                                        .SendPacketToSeedNodeAsync(
                                            ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                                .RemoteAskTotalPendingTransaction, Program.Certificate, false, true)
                                        )
                                        RemoteNodeObjectConnectionStatus = false;

                                    break;

                                #endregion

                                #region Sync Public Node List Information

                                case SyncEnumerationObject.ObjectToBePublic:
                                    if (string.IsNullOrEmpty(ClassRemoteNodeSync.MyOwnIP))
                                    {
                                        if (!await RemoteNodeObjectTcpClient
                                            .SendPacketToSeedNodeAsync(
                                                ClassSeedNodeCommand.ClassSendSeedEnumeration.RemoteAskOwnIP,
                                                Program.Certificate,
                                                false, true)

                                        )
                                        { // We ask seed nodes instead blockchain for get the public ip of the node.
                                            RemoteNodeObjectConnectionStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        if (!await RemoteNodeObjectTcpClient
                                            .SendPacketToSeedNodeAsync(
                                                ClassSeedNodeCommand.ClassSendSeedEnumeration.WalletAskRemoteNode,
                                                Program.Certificate, false, true)
                                            )
                                        { // We ask seed nodes instead blockchain.
                                            RemoteNodeObjectConnectionStatus = false;
                                        }
                                    }

                                    break;

                                #endregion

                                #region Sync Total Fee Information

                                case SyncEnumerationObject.ObjectTotalFee:
                                    if (!await RemoteNodeObjectTcpClient
                                        .SendPacketToSeedNodeAsync(
                                            ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteAskTotalFee,
                                            Program.Certificate, false, true))
                                        RemoteNodeObjectConnectionStatus = false;

                                    break;

                                #endregion

                                #region Sync Total Transaction Information

                                case SyncEnumerationObject.ObjectTotalTransaction:
                                    if (!await RemoteNodeObjectTcpClient
                                        .SendPacketToSeedNodeAsync(
                                            ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                                .RemoteNumberOfTransaction, Program.Certificate, false, true)
                                        )
                                        RemoteNodeObjectConnectionStatus = false;

                                    break;

                                    #endregion
                            }
                        }
                        if (RemoteNodeObjectType != SyncEnumerationObject.ObjectTransaction && RemoteNodeObjectType != SyncEnumerationObject.ObjectBlock)
                        {
                            await Task.Delay(RemoteNodeObjectLoopSendRequestInterval2); // Make a pause for the next request.
                        }
                        else
                        {
                            if (RemoteNodeObjectType == SyncEnumerationObject.ObjectTransaction)
                            {
                                if (long.TryParse(ClassRemoteNodeSync.TotalTransaction, out var totalTransactionToSync))
                                {
                                    if (totalTransactionToSync <= ClassRemoteNodeSync.ListOfTransaction.Count)
                                    {
                                        await Task.Delay(RemoteNodeObjectLoopSendRequestInterval2); // Make a pause for the next sync of transaction.
                                    }
                                    else
                                    {
                                        await Task.Delay(RemoteNodeObjectLoopSendRequestInterval);
                                    }
                                }
                            }
                            else
                            {
                                if (int.TryParse(ClassRemoteNodeSync.TotalBlockMined, out var totalBlockMinedToSync))
                                {
                                    if (totalBlockMinedToSync <= ClassRemoteNodeSync.ListOfBlock.Count)
                                    {
                                        await Task.Delay(RemoteNodeObjectLoopSendRequestInterval2); // Make a pause for the next sync of transaction.
                                    }
                                    else
                                    {
                                        await Task.Delay(RemoteNodeObjectLoopSendRequestInterval);
                                    }
                                }
                            }
                        }
                        if (RemoteNodeObjectLoginStatus)
                        {
                            if (!RemoteNodeObjectInSyncTransaction && !RemoteNodeObjectInSyncBlock && !RemoteNodeObjectInReceiveTransaction && !RemoteNodeObjectInReceiveBlock)
                            {
                                if (_lastKeepAlivePacketSent + KeepAlivePacketDelay < DateTimeOffset.Now.ToUnixTimeSeconds())
                                {
                                    _lastKeepAlivePacketSent = DateTimeOffset.Now.ToUnixTimeSeconds();
                                    if (!await RemoteNodeObjectTcpClient.SendPacketToSeedNodeAsync(ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteKeepAlive, Program.Certificate, false, true))
                                    {
                                        RemoteNodeObjectConnectionStatus = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    RemoteNodeObjectLoginStatus = false;
                    RemoteNodeObjectConnectionStatus = false;
                    RemoteNodeObjectInReceiveBlock = false;
                    RemoteNodeObjectInReceiveTransaction = false;
                    RemoteNodeObjectInSyncBlock = false;
                    RemoteNodeObjectInSyncTransaction = false;
                }, CancellationRemoteNodeObject.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current).ConfigureAwait(false);
            }
            catch
            {
                RemoteNodeObjectConnectionStatus = false;
                RemoteNodeObjectThreadStatus = false;
            }
        }

        /// <summary>
        ///     Handle packet received from the network.
        /// </summary>
        /// <param name="packet"></param>
        private async Task RemoteNodeHandlePacketNetworkAsync(string packet)
        {
            try
            {
                var packetSplit = packet.Split(new[] { ClassConnectorSetting.PacketContentSeperator }, StringSplitOptions.None);

                switch (packetSplit[0])
                {
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteAcceptedLogin: // if login is accepted, the remote node can start to sync informations.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        RemoteNodeObjectLoginStatus = true;
                        switch (RemoteNodeObjectType)
                        {
                            case SyncEnumerationObject.ObjectBlock:
                                ClassLog.Log("Blockchain accept your node to sync block.", 0, 1);

                                break;
                            case SyncEnumerationObject.ObjectBlockMined:
                                ClassLog.Log(
                                    "Blockchain accept your node to sync the total number of block mined information.",
                                    0, 1);
                                break;
                            case SyncEnumerationObject.ObjectCoinCirculating:
                                ClassLog.Log(
                                    "Blockchain accept your node to sync the total of coin circulating information.", 0,
                                    1);
                                break;
                            case SyncEnumerationObject.ObjectCoinSupply:
                                ClassLog.Log("Blockchain accept your node to sync coin max supply information.", 0, 1);
                                break;
                            case SyncEnumerationObject.ObjectCurrentDifficulty:
                                ClassLog.Log(
                                    "Blockchain accept your node to sync current mining difficulty information.", 0, 1);
                                break;
                            case SyncEnumerationObject.ObjectCurrentRate:
                                ClassLog.Log("Blockchain accept your node to sync current mining hashrate information.",
                                    0, 1);
                                break;
                            case SyncEnumerationObject.ObjectPendingTransaction:
                                ClassLog.Log(
                                    "Blockchain accept your node to sync total pending transaction information.", 0, 1);
                                break;
                            case SyncEnumerationObject.ObjectTotalFee:
                                ClassLog.Log("Blockchain accept your node to sync total fee information.", 0, 1);
                                break;
                            case SyncEnumerationObject.ObjectTotalTransaction:
                                ClassLog.Log(
                                    "Blockchain accept your node to sync total number of transaction information.", 0,
                                    1);
                                break;
                            case SyncEnumerationObject.ObjectTransaction:
                                ClassLog.Log("Blockchain accept your node to sync transaction.", 0, 1);
                                break;
                            case SyncEnumerationObject.ObjectToBePublic:
                                ClassLog.Log(
                                    "Blockchain accept your node to try to list your remote node on the public list.",
                                    0, 1);
                                break;
                        }

                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteKeepAlive: // Receive a keep alive packet.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendBlockPerId: // Receive a block information.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        var splitBlock = packetSplit[1].Split(new[] { "END" }, StringSplitOptions.None);
                        if (splitBlock.Length > 1)
                        {
                            for (var i = 0; i < splitBlock.Length; i++)
                            {
                                if (splitBlock[i] != null)
                                {
                                    if (splitBlock[i].Length > 0)
                                    {
                                        var blockSubString = splitBlock[i].Substring(0, splitBlock[i].Length - 1);
                                        var blockLineSplit = blockSubString.Split(new[] { "#" }, StringSplitOptions.None);
                                        var blockIdTmp = long.Parse(blockLineSplit[0]);
                                        if (!ClassRemoteNodeSync.ListOfBlock.ContainsKey(blockIdTmp - 1))
                                        {
                                            if (ClassRemoteNodeSync.ListOfBlockHash.GetBlockIdFromHash(blockLineSplit[1]) == -1)
                                            {
                                                try
                                                {
                                                    if (ClassRemoteNodeSync.ListOfBlockHash.InsertBlockHash(blockLineSplit[1], blockIdTmp - 1))
                                                    {
                                                        ClassRemoteNodeSync.ListOfBlock.Add(blockIdTmp - 1, blockSubString);
                                                        if (ClassRemoteNodeSync.ListOfBlock.Count.ToString("F0") ==
                                                            ClassRemoteNodeSync.TotalBlockMined)
                                                        {
                                                            ClassLog.Log(
                                                                "Block mined synced, " + ClassRemoteNodeSync.ListOfBlock.Count +
                                                                "/" + ClassRemoteNodeSync.TotalBlockMined, 0, 1);
                                                            if (!await RemoteNodeObjectTcpClient.SendPacketToSeedNodeAsync(ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteAskSchemaBlock, Program.Certificate, false, true))
                                                            {
                                                                RemoteNodeObjectConnectionStatus = false;
                                                                RemoteNodeObjectLoginStatus = false;
                                                                RemoteNodeObjectConnectionStatus = false;
                                                                RemoteNodeObjectInReceiveBlock = false;
                                                                RemoteNodeObjectInReceiveTransaction = false;
                                                                RemoteNodeObjectInSyncBlock = false;
                                                                RemoteNodeObjectInSyncTransaction = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ClassLog.Log(
                                                                "Block mined synced at: " + ClassRemoteNodeSync.ListOfBlock.Count +
                                                                "/" + ClassRemoteNodeSync.TotalBlockMined, 0, 2);
                                                        }
                                                    }
                                                }
                                                catch
                                                {
                                                    await StopConnection(string.Empty);
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var blockSubString = packetSplit[1].Substring(0, packetSplit[1].Length - 1);
                                        var blockLineSplit = blockSubString.Split(new[] { "#" }, StringSplitOptions.None);
                                        var blockIdTmp = long.Parse(blockLineSplit[0]);

                                        if (!ClassRemoteNodeSync.ListOfBlock.ContainsKey(blockIdTmp - 1))
                                        {
                                            if (ClassRemoteNodeSync.ListOfBlockHash.GetBlockIdFromHash(blockLineSplit[1]) == -1)
                                            {
                                                try
                                                {
                                                    if (ClassRemoteNodeSync.ListOfBlockHash.InsertBlockHash(blockLineSplit[1], blockIdTmp - 1))
                                                    {
                                                        ClassRemoteNodeSync.ListOfBlock.Add(blockIdTmp - 1, blockSubString);
                                                        if (ClassRemoteNodeSync.ListOfBlock.Count.ToString("F0") ==
                                                            ClassRemoteNodeSync.TotalBlockMined)
                                                        {
                                                            ClassLog.Log(
                                                                "Block mined synced, " + ClassRemoteNodeSync.ListOfBlock.Count + "/" +
                                                                ClassRemoteNodeSync.TotalBlockMined, 0, 1);
                                                            if (!await RemoteNodeObjectTcpClient.SendPacketToSeedNodeAsync(ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteAskSchemaBlock, Program.Certificate, false, true))
                                                            {
                                                                RemoteNodeObjectConnectionStatus = false;
                                                                RemoteNodeObjectLoginStatus = false;
                                                                RemoteNodeObjectConnectionStatus = false;
                                                                RemoteNodeObjectInReceiveBlock = false;
                                                                RemoteNodeObjectInReceiveTransaction = false;
                                                                RemoteNodeObjectInSyncBlock = false;
                                                                RemoteNodeObjectInSyncTransaction = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ClassLog.Log(
                                                                "Block mined synced at: " + ClassRemoteNodeSync.ListOfBlock.Count + "/" +
                                                                ClassRemoteNodeSync.TotalBlockMined, 0, 2);
                                                        }
                                                    }
                                                }
                                                catch
                                                {
                                                    await StopConnection(string.Empty);
                                                    return;
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }

                        RemoteNodeObjectInReceiveBlock = false;

                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendCoinCirculating: // Receive coin circulating information.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.CoinCirculating = packetSplit[1];
                        ClassLog.Log("Total Coin Circulating: " + packetSplit[1], 2, 2);
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendCoinMaxSupply: // Receive coin max supply information.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.CoinMaxSupply = packetSplit[1];
                        ClassLog.Log("Coin Max Supply: " + packetSplit[1], 2, 2);
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendCurrentDifficulty: // Receive current mining difficulty information.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.CurrentDifficulty = packetSplit[1];
                        ClassLog.Log("Current Mining Difficulty: " + packetSplit[1], 2, 2);
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendCurrentRate: // Receive current mining hashrate information.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.CurrentHashrate = packetSplit[1];
                        ClassLog.Log("Current Mining Hashrate: " + packetSplit[1], 2, 2);
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendNumberOfTransaction: // Receive total number of transaction information.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.TotalTransaction = packetSplit[1].Replace(ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendNumberOfTransaction, "");

                        ClassLog.Log("Total Transaction: " + packetSplit[1].Replace(ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendNumberOfTransaction, ""),
                            2, 2);
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendTotalBlockMined: // Receive total block mined information.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.TotalBlockMined = packetSplit[1];
                        if (ClassRemoteNodeSync.CoinMaxSupply != null)
                        {
                            if (int.TryParse(packetSplit[1], out var totalBlockMined))
                            {
                                var totalBlockLeft =
                                    Math.Round(
                                        (decimal.Parse(ClassRemoteNodeSync.CoinMaxSupply.Replace(".", ","), System.Globalization.NumberStyles.Any,
                                             Program.GlobalCultureInfo) / 10) - totalBlockMined, 0);
                                ClassRemoteNodeSync.CurrentBlockLeft = "" + totalBlockLeft;
                                ClassLog.Log("Total Block Left: " + ClassRemoteNodeSync.CurrentBlockLeft, 2, 2);
                            }
                        }

                        ClassLog.Log("Total Block Mined: " + packetSplit[1], 2, 2);
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendTotalFee: // Receive current total fee information.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.CurrentTotalFee = packetSplit[1];
                        ClassLog.Log("Current Total Fee: " + packetSplit[1], 2, 2);
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendTotalPendingTransaction: // Receive total number of pending transaction information.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.TotalPendingTransaction = packetSplit[1];
                        ClassLog.Log("Total Pending Transaction: " + packetSplit[1], 2, 2);
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendTransactionPerId: // Receive a transaction information.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();

                        if (packetSplit.Length > 1)
                        {
                            if (packet.Contains("$"))
                            {
                                var splitTransaction = packetSplit[1].Split(new[] { "$" }, StringSplitOptions.None);
                                if (splitTransaction.Length > 1)
                                {
                                    for (int i = 0; i < splitTransaction.Length; i++)
                                    {
                                        if (i < splitTransaction.Length)
                                        {
                                            if (splitTransaction[i] != null)
                                            {
                                                if (splitTransaction[i].Length > 0)
                                                {
                                                    var transactionSubString = splitTransaction[i].Replace("$", "");
                                                    var transactionSubSplit = transactionSubString.Split(new[] { "%" }, StringSplitOptions.None);
                                                    var dataTransactionSplit = transactionSubSplit[1].Split(new[] { "-" }, StringSplitOptions.None);

                                                    long transactionIdInsert = 0;
                                                    if (long.TryParse(transactionSubSplit[0], out transactionIdInsert))
                                                    {
                                                        ClassLog.Log("Transaction Received: " + transactionSubSplit[1], 2, 2);


                                                        if (!ClassRemoteNodeSync.ListOfTransaction.ContainsKey(transactionIdInsert))
                                                        {
                                                            if (ClassRemoteNodeSync.ListOfTransactionHash.ContainsKey(dataTransactionSplit[5]) != 1)
                                                            {
                                                                if (ClassRemoteNodeSortingTransactionPerWallet.AddNewTransactionSortedPerWallet(transactionSubSplit[1], transactionIdInsert))
                                                                {
                                                                    if (ClassRemoteNodeSync.ListOfTransaction.InsertTransaction(transactionIdInsert, transactionSubSplit[1]))
                                                                    {

                                                                        if ((ClassRemoteNodeSync.ListOfTransaction.Count).ToString("F0") == ClassRemoteNodeSync.TotalTransaction)
                                                                        {
                                                                            ClassLog.Log("Transaction synced, " + (ClassRemoteNodeSync.ListOfTransaction.Count) + "/" + ClassRemoteNodeSync.TotalTransaction, 0, 1);
                                                                            if (!await RemoteNodeObjectTcpClient.SendPacketToSeedNodeAsync(ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteAskSchemaTransaction, Program.Certificate, false, true))
                                                                            {
                                                                                RemoteNodeObjectConnectionStatus = false;
                                                                                RemoteNodeObjectLoginStatus = false;
                                                                                RemoteNodeObjectConnectionStatus = false;
                                                                                RemoteNodeObjectInReceiveBlock = false;
                                                                                RemoteNodeObjectInReceiveTransaction = false;
                                                                                RemoteNodeObjectInSyncBlock = false;
                                                                                RemoteNodeObjectInSyncTransaction = false;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            ClassLog.Log("Transaction synced at: " + (ClassRemoteNodeSync.ListOfTransaction.Count) + "/" + ClassRemoteNodeSync.TotalTransaction, 0, 2);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        ClassLog.Log("Transaction already exist: " + transactionSubSplit[1], 2, 2);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    ClassLog.Log("Transaction cannot be sorted by wallet: " + transactionSubSplit[1], 2, 2);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                ClassLog.Log("Transaction hash already saved: " + transactionSubSplit[1], 2, 2);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ClassLog.Log("Transaction ID " + transactionIdInsert + " already saved: " + transactionSubSplit[1], 2, 2);
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    }
                                    RemoteNodeObjectInReceiveTransaction = false;
                                }
                                else
                                {
                                    var transactionSubString = packetSplit[1].Replace("$", "");
                                    var transactionSubSplit = transactionSubString.Split(new[] { "%" }, StringSplitOptions.None); // Format : id transaction | content
                                    var dataTransactionSplit = transactionSubSplit[1].Split(new[] { "-" }, StringSplitOptions.None);
                                    long transactionIdInsert = 0;

                                    if (long.TryParse(transactionSubSplit[0], out transactionIdInsert))
                                    {
                                        ClassLog.Log("Transaction Received: " + transactionSubSplit[1], 2, 2);


                                        if (!ClassRemoteNodeSync.ListOfTransaction.ContainsKey(transactionIdInsert))
                                        {
                                            if (ClassRemoteNodeSync.ListOfTransactionHash.ContainsKey(dataTransactionSplit[5]) != 1)
                                            {
                                                if (ClassRemoteNodeSortingTransactionPerWallet.AddNewTransactionSortedPerWallet(transactionSubSplit[1], transactionIdInsert))
                                                {

                                                    if (ClassRemoteNodeSync.ListOfTransaction.InsertTransaction(transactionIdInsert, transactionSubSplit[1]))
                                                    {
                                                        if ((ClassRemoteNodeSync.ListOfTransaction.Count).ToString("F0") == ClassRemoteNodeSync.TotalTransaction)
                                                        {
                                                            ClassLog.Log("Transaction synced, " + (ClassRemoteNodeSync.ListOfTransaction.Count) + "/" + ClassRemoteNodeSync.TotalTransaction, 0, 1);
                                                            if (!await RemoteNodeObjectTcpClient.SendPacketToSeedNodeAsync(ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteAskSchemaTransaction, Program.Certificate, false, true))
                                                            {
                                                                RemoteNodeObjectConnectionStatus = false;
                                                                RemoteNodeObjectLoginStatus = false;
                                                                RemoteNodeObjectConnectionStatus = false;
                                                                RemoteNodeObjectInReceiveBlock = false;
                                                                RemoteNodeObjectInReceiveTransaction = false;
                                                                RemoteNodeObjectInSyncBlock = false;
                                                                RemoteNodeObjectInSyncTransaction = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ClassLog.Log("Transaction synced at: " + (ClassRemoteNodeSync.ListOfTransaction.Count) + "/" + ClassRemoteNodeSync.TotalTransaction, 0, 2);

                                                        }

                                                    }
                                                    else
                                                    {
                                                        ClassLog.Log("Transaction already exist: " + transactionSubSplit[1], 2, 2);
                                                    }
                                                }
                                                else
                                                {
                                                    ClassLog.Log("Transaction cannot be sorted by wallet: " + transactionSubSplit[1], 2, 2);
                                                }
                                            }
                                            else
                                            {
                                                ClassLog.Log("Transaction hash already saved: " + transactionSubSplit[1], 2, 2);
                                            }
                                        }
                                        else
                                        {
                                            ClassLog.Log("Transaction ID " + transactionIdInsert + " already saved: " + transactionSubSplit[1], 2, 2);
                                        }
                                    }
                                    RemoteNodeObjectInReceiveTransaction = false;
                                }
                            }
                        }


                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendCheckBlockPerId:
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (int.TryParse(packetSplit[1], out var blockId))
                            if (ClassRemoteNodeSync.ListOfBlock.Count > blockId)
                                if (!await RemoteNodeObjectTcpClient
                                    .SendPacketToSeedNodeAsync(
                                        ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                            .RemoteCheckBlockPerId + ClassConnectorSetting.PacketContentSeperator + ClassRemoteNodeSync.ListOfBlock[blockId],
                                        Program.Certificate, false, true))
                                {
                                    RemoteNodeObjectConnectionStatus = false;
                                    RemoteNodeObjectLoginStatus = false;
                                    RemoteNodeObjectConnectionStatus = false;
                                    RemoteNodeObjectInReceiveBlock = false;
                                    RemoteNodeObjectInReceiveTransaction = false;
                                    RemoteNodeObjectInSyncBlock = false;
                                    RemoteNodeObjectInSyncTransaction = false;
#if DEBUG
                                    Console.WriteLine(
                                        "Remote Node Object sync disconnected. Restart connection in a minute.");
#endif
                                }

                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendCheckTransactionPerId:
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (long.TryParse(packetSplit[1], out var transactionId))
                            if (ClassRemoteNodeSync.ListOfTransaction.Count > transactionId)
                                if (!await RemoteNodeObjectTcpClient.SendPacketToSeedNodeAsync(
                                    ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                        .RemoteCheckTransactionPerId + ClassConnectorSetting.PacketContentSeperator +
                                    ClassRemoteNodeSync.ListOfTransaction.GetTransaction(transactionId, false, CancellationRemoteNodeObject), Program.Certificate, false,
                                    true))
                                {
                                    RemoteNodeObjectConnectionStatus = false;
                                    RemoteNodeObjectLoginStatus = false;
                                    RemoteNodeObjectConnectionStatus = false;
                                    RemoteNodeObjectInReceiveBlock = false;
                                    RemoteNodeObjectInReceiveTransaction = false;
                                    RemoteNodeObjectInSyncBlock = false;
                                    RemoteNodeObjectInSyncTransaction = false;
#if DEBUG
                                    Console.WriteLine(
                                        "Remote Node Object sync disconnected. Restart connection in a minute.");
#endif
                                }

                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendCheckBlockHash:
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (!await RemoteNodeObjectTcpClient
                            .SendPacketToSeedNodeAsync(
                                ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteCheckBlockHash + ClassConnectorSetting.PacketContentSeperator +
                                ClassRemoteNodeSync.HashBlockList, Program.Certificate, false, true)
                            )
                        {
                            RemoteNodeObjectConnectionStatus = false;
                            RemoteNodeObjectLoginStatus = false;
                            RemoteNodeObjectConnectionStatus = false;
                            RemoteNodeObjectInReceiveBlock = false;
                            RemoteNodeObjectInReceiveTransaction = false;
                            RemoteNodeObjectInSyncBlock = false;
                            RemoteNodeObjectInSyncTransaction = false;
#if DEBUG
                                    Console.WriteLine(
                                        "Remote Node Object sync disconnected. Restart connection in a minute.");
#endif
                        }

                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendCheckTransactionHash:
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (!await RemoteNodeObjectTcpClient
                            .SendPacketToSeedNodeAsync(
                                ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteCheckTransactionHash +
                                ClassConnectorSetting.PacketContentSeperator + ClassRemoteNodeSync.HashTransactionList, Program.Certificate, false, true)
                            )
                        {
                            RemoteNodeObjectConnectionStatus = false;
                            RemoteNodeObjectLoginStatus = false;
                            RemoteNodeObjectConnectionStatus = false;
                            RemoteNodeObjectInReceiveBlock = false;
                            RemoteNodeObjectInReceiveTransaction = false;
                            RemoteNodeObjectInSyncBlock = false;
                            RemoteNodeObjectInSyncTransaction = false;
#if DEBUG
                                    Console.WriteLine(
                                        "Remote Node Object sync disconnected. Restart connection in a minute.");
#endif
                        }

                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendCheckTrustedKey:
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (!await RemoteNodeObjectTcpClient
                            .SendPacketToSeedNodeAsync(
                                ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteCheckTrustedKey +
                                ClassConnectorSetting.PacketContentSeperator + ClassRemoteNodeSync.TrustedKey, Program.Certificate, false, true)
                            )
                        {
                            RemoteNodeObjectConnectionStatus = false;
                            RemoteNodeObjectLoginStatus = false;
                            RemoteNodeObjectConnectionStatus = false;
                            RemoteNodeObjectInReceiveBlock = false;
                            RemoteNodeObjectInReceiveTransaction = false;
                            RemoteNodeObjectInSyncBlock = false;
                            RemoteNodeObjectInSyncTransaction = false;
#if DEBUG
                                    Console.WriteLine(
                                        "Remote Node Object sync disconnected. Restart connection in a minute.");
#endif
                        }

                        break;
                    case ClassSeedNodeCommand.ClassReceiveSeedEnumeration.WalletSendRemoteNode:
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (!string.IsNullOrEmpty(ClassRemoteNodeSync.MyOwnIP))
                        {
                            var splitRemoteNodeList = packet.Split(new[] { ClassConnectorSetting.PacketContentSeperator }, StringSplitOptions.None);
                            var imPublic = false;
                            var listTmpNode = new List<string>();
                            foreach (var node in splitRemoteNodeList)
                                if (node != ClassSeedNodeCommand.ClassReceiveSeedEnumeration.WalletSendRemoteNode)
                                    if (!string.IsNullOrEmpty(node))
                                    {
                                        listTmpNode.Add(node);
                                        ClassLog.Log("Public Remote Node Host: " + node, 1, 2);
                                        if (node == ClassRemoteNodeSync.MyOwnIP) imPublic = true;
                                    }

                            ClassRemoteNodeSync.ListOfPublicNodes = listTmpNode;
                            if (!imPublic)
                            {
                                ClassRemoteNodeSync.ImPublicNode = false;
                                ClassLog.Log("Your remote node is not listed on the public list.", 1, 2);

                                if (ClassRemoteNodeSync.ListOfBlock.Count ==
                                    int.Parse(ClassRemoteNodeSync.TotalBlockMined) &&
                                    ClassRemoteNodeSync.ListOfTransaction.Count ==
                                    long.Parse(ClassRemoteNodeSync.TotalTransaction))
                                {
                                    if (!await RemoteNodeObjectTcpClient.SendPacketToSeedNodeAsync(ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteAskSchemaTransaction, Program.Certificate, false, true))
                                    {
                                        RemoteNodeObjectConnectionStatus = false;
                                        RemoteNodeObjectLoginStatus = false;
                                        RemoteNodeObjectConnectionStatus = false;
                                        RemoteNodeObjectInReceiveBlock = false;
                                        RemoteNodeObjectInReceiveTransaction = false;
                                        RemoteNodeObjectInSyncBlock = false;
                                        RemoteNodeObjectInSyncTransaction = false;
#if DEBUG
                                    Console.WriteLine(
                                        "Remote Node Object sync disconnected. Restart connection in a minute.");
#endif
                                    }
                                    else
                                    {
                                        if (!await RemoteNodeObjectTcpClient.SendPacketToSeedNodeAsync(ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteAskSchemaBlock, Program.Certificate, false, true))
                                        {
                                            RemoteNodeObjectConnectionStatus = false;
                                            RemoteNodeObjectLoginStatus = false;
                                            RemoteNodeObjectConnectionStatus = false;
                                            RemoteNodeObjectInReceiveBlock = false;
                                            RemoteNodeObjectInReceiveTransaction = false;
                                            RemoteNodeObjectInSyncBlock = false;
                                            RemoteNodeObjectInSyncTransaction = false;
#if DEBUG
                                    Console.WriteLine(
                                        "Remote Node Object sync disconnected. Restart connection in a minute.");
#endif
                                        }
                                    }
                                    if (!await RemoteNodeObjectTcpClient
                                        .SendPacketToSeedNodeAsync(
                                            ClassSeedNodeCommand.ClassSendSeedEnumeration.RemoteAskToBePublic,
                                            Program.Certificate, false, true))
                                    {
                                        RemoteNodeObjectConnectionStatus = false;
                                        RemoteNodeObjectLoginStatus = false;
                                        RemoteNodeObjectConnectionStatus = false;
                                        RemoteNodeObjectInReceiveBlock = false;
                                        RemoteNodeObjectInReceiveTransaction = false;
                                        RemoteNodeObjectInSyncBlock = false;
                                        RemoteNodeObjectInSyncTransaction = false;
                                        ClassRemoteNodeSync.ListOfPublicNodes.Clear();
#if DEBUG
                                    Console.WriteLine(
                                        "Remote Node Object sync disconnected. Restart connection in a minute.");
#endif
                                    }
                                }
                                else
                                {
                                    ClassLog.Log("Your remote node will ask to be public once he get his sync finish.",
                                        1, 2);
                                }
                            }
                            else
                            {
                                ClassLog.Log("Your remote node is listed on the public list.", 1, 1);
                                ClassRemoteNodeSync.ImPublicNode = true;
                            }
                        }

                        break;
                    case ClassSeedNodeCommand.ClassReceiveSeedEnumeration.RemoteSendOwnIP:
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.MyOwnIP = packetSplit[1];
                        ClassLog.Log("Your own public IP is: " + ClassRemoteNodeSync.MyOwnIP, 2, 3);
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendSchemaTransaction:
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.SchemaHashTransaction = packetSplit[1];
                        ClassRemoteNodeKey.StartUpdateHashTransactionList();
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendSchemaBlock:
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.SchemaHashBlock = packetSplit[1];
                        ClassRemoteNodeKey.StartUpdateHashBlockList();
                        break;
                }
            }
            catch 
            {

                RemoteNodeObjectConnectionStatus = false;
                RemoteNodeObjectLoginStatus = false;
                RemoteNodeObjectInReceiveBlock = false;
                RemoteNodeObjectInReceiveTransaction = false;
                RemoteNodeObjectInSyncBlock = false;
                RemoteNodeObjectInSyncTransaction = false;
                if (RemoteNodeObjectType == SyncEnumerationObject.ObjectToBePublic)
                {
                    ClassRemoteNodeSync.ImPublicNode = false;
                    ClassRemoteNodeSync.ListOfPublicNodes.Clear();
                    ClassRemoteNodeSync.MyOwnIP = string.Empty;
                    ClassRemoteNodeSync.SchemaHashBlock = string.Empty;
                    ClassRemoteNodeSync.SchemaHashTransaction = string.Empty;
                }
#if DEBUG
                ClassLog.Log("Remote Node Object sync disconnected. Restart connection in a minute.", 2, 3);
#endif
            }
        }
    }
}