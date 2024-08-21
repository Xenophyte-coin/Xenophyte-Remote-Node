﻿using System;
using System.IO;
using System.Text;
using System.Threading;
using Xenophyte_RemoteNode.Data;

namespace Xenophyte_RemoteNode.RemoteNode
{
    public class ClassRemoteNodeSave
    {
        public static readonly string BlockchainTransactonDatabase = "transaction.xirdb";
        private static readonly string BlockchainBlockDatabase = "block.xirdb";
        private static readonly string BlockchainDirectory = "\\Blockchain\\";
        private static readonly string BlockchainTransactionDirectory = "\\Blockchain\\Transaction\\";
        private static readonly string BlockchainBlockDirectory = "\\Blockchain\\Block\\";
        private static StreamWriter BlockchainTransactionWriter;
        private static StreamWriter BlockchainBlockWriter;

        public static bool InSaveTransactionDatabase;
        public static bool InSaveBlockDatabase;

        public static long TotalTransactionSaved;
        public static int TotalBlockSaved;
        public static string DataTransactionSaved;
        public static string DataBlockSaved;


        private static Thread _threadAutoSaveTransaction;

        private static Thread _threadAutoSaveBlock;

        /// <summary>
        ///     Initialize Path make them if they not exist.
        /// </summary>
        public static void InitializePath()
        {
            if (!Directory.Exists(GetCurrentPath() + GetBlockchainPath()))
                Directory.CreateDirectory(GetCurrentPath() + GetBlockchainPath());
            if (!Directory.Exists(GetCurrentPath() + GetBlockchainBlockPath()))
                Directory.CreateDirectory(GetCurrentPath() + GetBlockchainBlockPath());
            if (!Directory.Exists(GetCurrentPath() + GetBlockchainTransactionPath()))
                Directory.CreateDirectory(GetCurrentPath() + GetBlockchainTransactionPath());
        }

        public static string ConvertPath(string path)
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                path = path.Replace("\\", "/");
            }
            return path;
        }

        /// <summary>
        ///     Get Current Path of the program.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentPath()
        {
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            if (Environment.OSVersion.Platform == PlatformID.Unix) path = path.Replace("\\", "/");
            return path;
        }

        /// <summary>
        ///     Get Blockchain Path.
        /// </summary>
        /// <returns></returns>
        private static string GetBlockchainPath()
        {
            var path = BlockchainDirectory;
            if (Environment.OSVersion.Platform == PlatformID.Unix) path = path.Replace("\\", "/");
            return path;
        }

        /// <summary>
        ///     Get blockchain path of transaction database.
        /// </summary>
        /// <returns></returns>
        public static string GetBlockchainTransactionPath()
        {
            var path = BlockchainTransactionDirectory;
            if (Environment.OSVersion.Platform == PlatformID.Unix) path = path.Replace("\\", "/");
            return path;
        }

        /// <summary>
        ///     Get blockchain path of block database.
        /// </summary>
        /// <returns></returns>
        private static string GetBlockchainBlockPath()
        {
            var path = BlockchainBlockDirectory;
            if (Environment.OSVersion.Platform == PlatformID.Unix) path = path.Replace("\\", "/");
            return path;
        }

        /// <summary>
        ///     Load transaction(s) database file.
        /// </summary>
        public static bool LoadBlockchainTransaction()
        {
            if (File.Exists(GetCurrentPath() + GetBlockchainTransactionPath() + BlockchainTransactonDatabase))
            {
                Console.WriteLine("Load transaction database file..");

                var counter = 0;

                using (FileStream fs = File.Open(GetCurrentPath() + GetBlockchainTransactionPath() + BlockchainTransactonDatabase, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (BufferedStream bs = new BufferedStream(fs))
                using (StreamReader sr = new StreamReader(bs))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        counter++;
                        try
                        {
                            if (line.Contains("%"))
                            {
                                var splitTransaction = line.Split(new[] { "%" }, StringSplitOptions.None);
                                long idTransaction = long.Parse(splitTransaction[0]);
                                ClassRemoteNodeSortingTransactionPerWallet.AddNewTransactionSortedPerWallet(splitTransaction[1], idTransaction);
                                ClassRemoteNodeSync.ListOfTransaction.InsertTransaction(idTransaction, splitTransaction[1], 0);
                            }
                            else
                            {
                                long totalTransaction = ClassRemoteNodeSync.ListOfTransaction.Count;
                                ClassRemoteNodeSortingTransactionPerWallet.AddNewTransactionSortedPerWallet(line, totalTransaction);

                                ClassRemoteNodeSync.ListOfTransaction.InsertTransaction(totalTransaction, line, 0);
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Transaction database seems corrupted, clearing transaction database and resync..");
                            ClassRemoteNodeSync.ListOfTransaction.Clear();
                            ClassRemoteNodeSync.ListOfTransactionHash.Clear();
                            ClassRemoteNodeSync.ListTransactionPerWallet.Clear();
                            return true;
                        }
                    }
                }

                TotalTransactionSaved = counter;
                Console.WriteLine(counter + " transaction successfully loaded and included on memory..");
            }
            else
            {
                File.Create(GetCurrentPath() + GetBlockchainTransactionPath() + BlockchainTransactonDatabase).Close();
            }
            return true;
        }

        /// <summary>
        ///     Load block(s) database file.
        /// </summary>
        public static void LoadBlockchainBlock()
        {
            if (File.Exists(GetCurrentPath() + GetBlockchainBlockPath() + BlockchainBlockDatabase))
            {
                Console.WriteLine("Load block database file..");


                var counter = 0;


                using (FileStream fs = File.Open(GetCurrentPath() + GetBlockchainBlockPath() + BlockchainBlockDatabase, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BufferedStream bs = new BufferedStream(fs))
                    {
                        using (StreamReader sr = new StreamReader(bs))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                counter++;
                                var splitLine = line.Split(new[] { "#" }, StringSplitOptions.None);
                                string blockHash = splitLine[1];
                                long blockId = long.Parse(splitLine[0]);
                                if (!ClassRemoteNodeSync.ListOfBlock.ContainsKey(blockId - 1))
                                {
                                    if (ClassRemoteNodeSync.ListOfBlockHash.GetBlockIdFromHash(blockHash) == -1)
                                    {
                                        ClassRemoteNodeSync.ListOfBlock.Add(blockId - 1, line);
                                        ClassRemoteNodeSync.ListOfBlockHash.InsertBlockHash(blockHash, blockId - 1);
                                    }
                                }
                            }
                        }
                    }
                }

                TotalBlockSaved = counter;
                Console.WriteLine(counter + " block successfully loaded and included on memory..");
            }
            else
            {
                File.Create(GetCurrentPath() + GetBlockchainBlockPath() + BlockchainBlockDatabase).Close();
            }
        }

        /// <summary>
        /// Force to save transaction.
        /// </summary>
        public static bool SaveTransaction(bool auto = true)
        {
            if (!InSaveTransactionDatabase)
            {
                if (_threadAutoSaveTransaction != null &&
                (_threadAutoSaveTransaction.IsAlive || _threadAutoSaveTransaction != null))
                {
                    _threadAutoSaveTransaction.Abort();
                    GC.SuppressFinalize(_threadAutoSaveTransaction);
                }

                if (auto)
                {
                    _threadAutoSaveTransaction = new Thread(async delegate ()
                    {
                        CancellationTokenSource cancellationIgnored = new CancellationTokenSource();

                        while (!Program.Closed)
                        {
                            try
                            {

                                if (!File.Exists(GetCurrentPath() + GetBlockchainTransactionPath() +
                                                BlockchainTransactonDatabase))
                                {
                                    File.Create(GetCurrentPath() + GetBlockchainTransactionPath() +
                                                BlockchainTransactonDatabase).Close();
                                }
                                if (BlockchainTransactionWriter == null)
                                {
                                    BlockchainTransactionWriter = new StreamWriter(GetCurrentPath() + GetBlockchainTransactionPath() +
                                                                                 BlockchainTransactonDatabase, true, Encoding.UTF8, 8192)
                                    { AutoFlush = true };
                                }

                                InSaveTransactionDatabase = true;

                                if (ClassRemoteNodeSync.ListOfTransaction != null)
                                    if (ClassRemoteNodeSync.ListOfTransaction.Count > 0)
                                    {

                                        if (TotalTransactionSaved != ClassRemoteNodeSync.ListOfTransaction.Count)
                                        {

                                            for (var i = TotalTransactionSaved; i < ClassRemoteNodeSync.ListOfTransaction.Count; i++)
                                            {
                                                if (ClassRemoteNodeSync.ListOfTransaction.ContainsKey(i))
                                                {
                                                    var transactionObject = await ClassRemoteNodeSync.ListOfTransaction.GetTransaction(i, cancellationIgnored);

                                                    BlockchainTransactionWriter.Write(transactionObject.Id + "%" + transactionObject.TransactionData + "\n");

                                                }
                                            }
                                            TotalTransactionSaved = ClassRemoteNodeSync.ListOfTransaction.Count;
                                        }
                                    }

                                InSaveTransactionDatabase = false;

                            }
                            catch (Exception error)
                            {
#if DEBUG
                            Console.WriteLine("Can't save transaction(s) to database file: " + error.Message);
#endif
                                ClearTransactionSyncSave();
                            }
                            Thread.Sleep(1000);
                        }
                    });
                    _threadAutoSaveTransaction.Start();
                }
                else
                {
                    try
                    {

                        if (BlockchainTransactionWriter != null)
                        {
                            ClearTransactionSyncSave();
                        }

                        File.Create(GetCurrentPath() + GetBlockchainTransactionPath() +
                                    BlockchainTransactonDatabase).Close();


                        if (!InSaveTransactionDatabase)
                        {
                            CancellationTokenSource cancellationIgnored = new CancellationTokenSource();

                            InSaveTransactionDatabase = true;

                            if (ClassRemoteNodeSync.ListOfTransaction != null)
                                if (ClassRemoteNodeSync.ListOfTransaction.Count > 0)
                                {

                                    using (var sw = new StreamWriter(GetCurrentPath() + GetBlockchainTransactionPath() +
                                                                                 BlockchainTransactonDatabase, true, Encoding.UTF8, 8192)
                                    { AutoFlush = true })
                                    {
                                        for (var i = 0; i < ClassRemoteNodeSync.ListOfTransaction.Count; i++)
                                        {

                                            if (ClassRemoteNodeSync.ListOfTransaction.ContainsKey(i))
                                            {
                                                var transactionObject = ClassRemoteNodeSync.ListOfTransaction.GetTransaction(i, cancellationIgnored).Result;

                                                sw.Write(transactionObject.Id + "%" + transactionObject.TransactionData + "\n");

                                            }
                                        }
                                        TotalTransactionSaved = ClassRemoteNodeSync.ListOfTransaction.Count;

                                    }


                                }

                            InSaveTransactionDatabase = false;
                        }
                    }
                    catch (Exception error)
                    {
#if DEBUG
                        Console.WriteLine("Can't save transaction(s) to database file: " + error.Message);
#endif
                        ClearTransactionSyncSave();

                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Force to save block.
        /// </summary>
        public static bool SaveBlock(bool auto = true)
        {

            if (!InSaveBlockDatabase)
            {
                if (_threadAutoSaveBlock != null && (_threadAutoSaveBlock.IsAlive || _threadAutoSaveBlock != null))
                {
                    _threadAutoSaveBlock.Abort();
                    GC.SuppressFinalize(_threadAutoSaveBlock);
                }
                if (auto)
                {
                    _threadAutoSaveBlock = new Thread(delegate ()
                    {
                        while (!Program.Closed)
                        {
                            try
                            {
                                if (!File.Exists(GetCurrentPath() + GetBlockchainBlockPath() +
                                                BlockchainBlockDatabase))
                                {
                                    File.Create(GetCurrentPath() + GetBlockchainBlockPath() +
                                                BlockchainBlockDatabase).Close();
                                }

                                if (BlockchainBlockWriter == null)
                                {
                                    BlockchainBlockWriter = new StreamWriter(GetCurrentPath() + GetBlockchainBlockPath() + BlockchainBlockDatabase, true, Encoding.UTF8, 8192) { AutoFlush = true };
                                }


                                InSaveBlockDatabase = true;
                                if (ClassRemoteNodeSync.ListOfBlock != null)
                                    if (ClassRemoteNodeSync.ListOfBlock.Count > 0)
                                    {

                                        if (TotalBlockSaved != ClassRemoteNodeSync.ListOfBlock.Count)
                                        {

                                            for (var i = TotalBlockSaved; i < ClassRemoteNodeSync.ListOfBlock.Count; i++)
                                            {
                                                if (ClassRemoteNodeSync.ListOfBlock.ContainsKey(i))
                                                {
                                                    BlockchainBlockWriter.Write(ClassRemoteNodeSync.ListOfBlock[i] + "\n");
                                                }
                                            }

                                            TotalBlockSaved = ClassRemoteNodeSync.ListOfBlock.Count;
                                        }
                                    }

                                InSaveBlockDatabase = false;

                            }
                            catch (Exception error)
                            {
#if DEBUG
                            Console.WriteLine("Can't save block(s) to database file: " + error.Message);
#endif
                                ClearBlockSyncSave();

                            }
                            Thread.Sleep(1000);
                        }
                    });
                    _threadAutoSaveBlock.Start();
                }
                else
                {
                    try
                    {

                        ClearBlockSyncSave();

                        File.Create(GetCurrentPath() + GetBlockchainBlockPath() +
                                    BlockchainBlockDatabase).Close();



                        InSaveBlockDatabase = true;
                        if (ClassRemoteNodeSync.ListOfBlock != null)
                        {
                            if (ClassRemoteNodeSync.ListOfBlock.Count > 0)
                            {


                                using (var sw = new StreamWriter(GetCurrentPath() + GetBlockchainBlockPath() +
                                                                         BlockchainBlockDatabase, true, Encoding.UTF8, 8192)
                                { AutoFlush = true })
                                {
                                    for (var i = 0; i < ClassRemoteNodeSync.ListOfBlock.Count; i++)
                                    {
                                        if (ClassRemoteNodeSync.ListOfBlock.ContainsKey(i))
                                        {
                                            sw.Write(ClassRemoteNodeSync.ListOfBlock[i] + "\n");
                                        }
                                    }
                                }

                                TotalBlockSaved = ClassRemoteNodeSync.ListOfBlock.Count;

                            }
                        }
                        InSaveBlockDatabase = false;

                    }
                    catch (Exception error)
                    {
#if DEBUG
                        Console.WriteLine("Can't save block(s) to database file: " + error.Message);
#endif
                        ClearBlockSyncSave();
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Force to clear blocks saved.
        /// </summary>
        public static void ClearBlockSyncSave()
        {
            try
            {
                if (BlockchainBlockWriter != null)
                {
                    BlockchainBlockWriter?.Close();
                    BlockchainBlockWriter?.Dispose();
                    BlockchainBlockWriter = null;
                }
                TotalBlockSaved = 0;
                File.Create(GetCurrentPath() + GetBlockchainBlockPath() + BlockchainBlockDatabase).Close();
                InSaveBlockDatabase = false;
            }
            catch
            {

            }
        }


        /// <summary>
        /// Force to clear transactions saved.
        /// </summary>
        public static void ClearTransactionSyncSave()
        {
            try
            {
                if (BlockchainTransactionWriter != null)
                {
                    BlockchainTransactionWriter?.Close();
                    BlockchainTransactionWriter?.Dispose();
                    BlockchainTransactionWriter = null;
                }
                TotalTransactionSaved = 0;
                File.Create(GetCurrentPath() + GetBlockchainTransactionPath() + BlockchainTransactonDatabase).Close();
                InSaveTransactionDatabase = false;
            }
            catch
            {

            }
        }
    }
}