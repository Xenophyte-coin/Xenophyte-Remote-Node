using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xenophyte_RemoteNode.Object.Sub;
using Xenophyte_RemoteNode.Utils;

namespace Xenophyte_RemoteNode.Object
{
    public class ClassTransactionObject
    {
        public long transaction_id;
        public string transaction_wallet_sender_id;
        public decimal transaction_fake_amount;
        public decimal transaction_fake_fee;
        public string transaction_wallet_receiver_id;
        public long transaction_send_date;
        public string transaction_hash;
        public long transaction_recv_date;
        public string transaction_block_height;
        public string transaction_encrypted_information;
    }

    public class ClassTransactionUtility
    {
        /// <summary>
        /// Build original data of a transaction from a transaction object.
        /// </summary>
        /// <param name="transactionObject"></param>
        /// <returns></returns>
        public static string BuildTransactionRaw(ClassTransactionObject transactionObject)
        {
            return transactionObject.transaction_wallet_sender_id + "-" +
                   transactionObject.transaction_fake_amount + "-" +
                   transactionObject.transaction_fake_fee + "-" +
                   transactionObject.transaction_wallet_receiver_id + "-" +
                   transactionObject.transaction_send_date + "-" +
                   transactionObject.transaction_hash + "-" +
                   transactionObject.transaction_recv_date + "-" +
                   transactionObject.transaction_block_height + "#" +
                   transactionObject.transaction_encrypted_information;
        }

        /// <summary>
        /// Build transaction object from original transaction data.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static ClassTransactionObject BuildTransactionObjectFromRaw(long transactionId, string transaction)
        {
            var splitTransaction = transaction.Split(new[] { "-" }, StringSplitOptions.None);
            var splitTransactionInformation = splitTransaction[7].Split(new[] { "#" }, StringSplitOptions.None);

            return new ClassTransactionObject
            {
                transaction_id = transactionId,
                transaction_wallet_sender_id = splitTransaction[0],
                transaction_fake_amount = 0,
                transaction_fake_fee = 0,
                transaction_wallet_receiver_id = splitTransaction[3],
                transaction_hash = splitTransaction[5],
                transaction_send_date = long.Parse(splitTransaction[4]),
                transaction_recv_date = long.Parse(splitTransaction[6]),
                transaction_block_height = splitTransactionInformation[0],
                transaction_encrypted_information = splitTransactionInformation[1] + "#" +splitTransactionInformation[2]
            };
        }
    }

    public class BigDictionaryTransaction // Limited to 1 009 999 999 transactions around 39,2GB of ram
    {
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction1; // 0 - 9 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction2; // 9 999 999 - 19 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction3; // 19 999 999 - 29 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction4; // 29 999 999 - 39 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction5; // 39 999 999 - 49 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction6; // 99 999 999 - 59 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction7; // 59 999 999 - 69 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction8; // 79 999 999 - 89 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction9; // 89 999 999 - 99 999 999 
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction10; // 99 999 999 - 109 999 999 ~3.962 GB


        private Dictionary<long,TransactionObject> _bigDictionaryTransaction11; // 109 999 999 - 119 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction12; // 119 999 999 - 129 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction13; // 129 999 999 - 139 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction14; // 139 999 999 - 149 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction15; // 149 999 999 - 159 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction16; // 159 999 999 - 169 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction17; // 169 999 999 - 179 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction18; // 179 999 999 - 189 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction19; // 189 999 999 - 199 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction20; // 199 999 999 - 209 999 999 ~7.924 GB

        private Dictionary<long,TransactionObject> _bigDictionaryTransaction21; // 209 999 999 - 219 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction22; // 219 999 999 - 229 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction23; // 229 999 999 - 239 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction24; // 239 999 999 - 249 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction25; // 249 999 999 - 259 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction26; // 259 999 999 - 269 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction27; // 269 999 999 - 279 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction28; // 279 999 999 - 289 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction29; // 289 999 999 - 299 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction30; // 299 999 999 - 309 999 999 ~11,886 GB

        private Dictionary<long,TransactionObject> _bigDictionaryTransaction31; // 309 999 999 - 319 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction32; // 319 999 999 - 329 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction33; // 329 999 999 - 339 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction34; // 339 999 999 - 349 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction35; // 349 999 999 - 359 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction36; // 359 999 999 - 369 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction37; // 369 999 999 - 379 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction38; // 379 999 999 - 389 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction39; // 389 999 999 - 399 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction40; // 399 999 999 - 409 999 999 ~15.848 GB

        private Dictionary<long,TransactionObject> _bigDictionaryTransaction41; // 409 999 999 - 419 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction42; // 419 999 999 - 429 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction43; // 429 999 999 - 439 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction44; // 439 999 999 - 449 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction45; // 449 999 999 - 459 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction46; // 459 999 999 - 469 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction47; // 469 999 999 - 479 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction48; // 479 999 999 - 489 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction49; // 489 999 999 - 499 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction50; // 499 999 999 - 509 999 999 ~19.810 GB

        private Dictionary<long,TransactionObject> _bigDictionaryTransaction51; // 509 999 999 - 519 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction52; // 519 999 999 - 529 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction53; // 529 999 999 - 539 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction54; // 539 999 999 - 549 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction55; // 549 999 999 - 559 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction56; // 559 999 999 - 569 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction57; // 569 999 999 - 579 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction58; // 579 999 999 - 589 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction59; // 589 999 999 - 599 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction60; // 599 999 999 - 609 999 999 ~23.772 GB

        private Dictionary<long,TransactionObject> _bigDictionaryTransaction61; // 609 999 999 - 619 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction62; // 619 999 999 - 629 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction63; // 629 999 999 - 639 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction64; // 639 999 999 - 649 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction65; // 649 999 999 - 659 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction66; // 659 999 999 - 669 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction67; // 669 999 999 - 679 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction68; // 679 999 999 - 689 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction69; // 689 999 999 - 699 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction70; // 699 999 999 - 709 999 999 ~27.734 GB

        private Dictionary<long,TransactionObject> _bigDictionaryTransaction71; // 709 999 999 - 719 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction72; // 719 999 999 - 729 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction73; // 729 999 999 - 739 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction74; // 739 999 999 - 749 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction75; // 749 999 999 - 759 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction76; // 759 999 999 - 769 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction77; // 769 999 999 - 779 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction78; // 779 999 999 - 789 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction79; // 789 999 999 - 799 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction80; // 799 999 999 - 809 999 999 ~31.696 GB

        private Dictionary<long,TransactionObject> _bigDictionaryTransaction81; // 809 999 999 - 819 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction82; // 819 999 999 - 829 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction83; // 829 999 999 - 839 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction84; // 839 999 999 - 849 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction85; // 849 999 999 - 859 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction86; // 859 999 999 - 869 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction87; // 869 999 999 - 879 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction88; // 879 999 999 - 889 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction89; // 889 999 999 - 899 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction90; // 899 999 999 - 909 999 999 ~35.658 GB

        private Dictionary<long,TransactionObject> _bigDictionaryTransaction91; // 909 999 999 - 919 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction92; // 919 999 999 - 929 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction93; // 929 999 999 - 939 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction94; // 939 999 999 - 949 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction95; // 949 999 999 - 959 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction96; // 959 999 999 - 969 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction97; // 969 999 999 - 979 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction98; // 979 999 999 - 989 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction99; // 989 999 999 - 999 999 999
        private Dictionary<long,TransactionObject> _bigDictionaryTransaction100; // 999 999 999 - 1 009 999 999 ~39.620 GB

        public ConcurrentDictionary<long, long> DictionaryStreamPosition;

        public const int MaxTransactionPerDictionary = 10000000; // 10 millions of transactions per dictionary


        private bool _enableDiskCache;
        private bool _readerOpened;
        private string _transactionPath;
        public SemaphoreSlim SemaphoreSlimTransactionReader;
        private StreamReader _transactionReader;

        public BigDictionaryTransaction(bool enableDiskCache, string transactionPath)
        {
            _enableDiskCache = enableDiskCache;
            _transactionPath = transactionPath;
            SemaphoreSlimTransactionReader = new SemaphoreSlim(1, 1);

            DictionaryStreamPosition = new ConcurrentDictionary<long, long>();

            #region Big transaction.

            _bigDictionaryTransaction1 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction2 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction3 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction4 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction5 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction6 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction7 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction8 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction9 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction10 = new Dictionary<long,TransactionObject>();

            _bigDictionaryTransaction11 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction12 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction13 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction14 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction15 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction16 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction17 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction18 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction19 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction20 = new Dictionary<long,TransactionObject>();

            _bigDictionaryTransaction21 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction22 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction23 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction24 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction25 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction26 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction27 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction28 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction29 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction30 = new Dictionary<long,TransactionObject>();

            _bigDictionaryTransaction31 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction32 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction33 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction34 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction35 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction36 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction37 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction38 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction39 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction40 = new Dictionary<long,TransactionObject>();

            _bigDictionaryTransaction41 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction42 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction43 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction44 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction45 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction46 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction47 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction48 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction49 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction50 = new Dictionary<long,TransactionObject>();

            _bigDictionaryTransaction51 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction52 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction53 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction54 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction55 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction56 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction57 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction58 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction59 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction60 = new Dictionary<long,TransactionObject>();

            _bigDictionaryTransaction61 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction62 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction63 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction64 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction65 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction66 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction67 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction68 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction69 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction70 = new Dictionary<long,TransactionObject>();

            _bigDictionaryTransaction71 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction72 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction73 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction74 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction75 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction76 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction77 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction78 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction79 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction80 = new Dictionary<long,TransactionObject>();

            _bigDictionaryTransaction81 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction82 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction83 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction84 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction85 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction86 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction87 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction88 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction89 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction90 = new Dictionary<long,TransactionObject>();

            _bigDictionaryTransaction91 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction92 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction93 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction94 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction95 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction96 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction97 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction98 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction99 = new Dictionary<long,TransactionObject>();
            _bigDictionaryTransaction100 = new Dictionary<long,TransactionObject>();

            #endregion
        }

        public long Count => CountTransaction();


        /// <summary>
        /// Open the transaction reader.
        /// </summary>
        /// <param name="cancellation"></param>
        public async Task<bool> OpenTransactionReader(CancellationTokenSource cancellation)
        {
            bool semaphoreUsed = false;

            try
            {

                try
                {
                    await SemaphoreSlimTransactionReader.WaitAsync(cancellation.Token);
                    semaphoreUsed = true;
                    if (_readerOpened)
                    {
                        try
                        {
                            _transactionReader.Close();
                            _readerOpened = false;
                            //_transactionReader.Dispose();
                        }
                        catch
                        {
                            // Ignored.
                        }
                    }

                    if (!_readerOpened)
                    {
                        _transactionReader = new StreamReader(_transactionPath, Encoding.UTF8, true);
                        _readerOpened = true;
                    }
                }
                catch
                {
                    // Just in case.
                }
            }
            finally
            {
                if (semaphoreUsed)
                    SemaphoreSlimTransactionReader.Release();
            }
            return _readerOpened;
        }

        /// <summary>
        /// Close the transaction reader.
        /// </summary>
        public async Task<bool> CloseTransactionReader(CancellationTokenSource cancellation)
        {
            if (_enableDiskCache)
            {
                bool semaphoreUsed = false;
                try
                {
                    try
                    {
                        await SemaphoreSlimTransactionReader.WaitAsync(cancellation.Token);
                        semaphoreUsed = true;

                        if (_readerOpened)
                        {
                            try
                            {
                                _transactionReader.Close();
                                _readerOpened = false;
                            }
                            catch
                            {
                                // Ignored.
                            }
                        }
                    }
                    catch
                    {
                        // Ignored.
                    }
                }
                finally
                {
                    if (semaphoreUsed)
                        SemaphoreSlimTransactionReader.Release();
                }
            }

            return _readerOpened;
        }

        /// <summary>
        /// Insert transaction
        /// </summary>
        /// <param name="id"></param>
        /// <param name="transaction"></param>
        public bool InsertTransaction(long id, string transaction, long position)
        {
            bool result = true;


            try
            {

                long idDictionary = (long)Math.Ceiling((double)(id / MaxTransactionPerDictionary));
                if (idDictionary >= 0 &&
                    !string.IsNullOrEmpty(transaction))
                {
                    switch (idDictionary)
                    {
                        case 0:
                            _bigDictionaryTransaction1.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 1:
                            _bigDictionaryTransaction2.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 2:
                            _bigDictionaryTransaction3.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 3:
                            _bigDictionaryTransaction4.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 4:
                            _bigDictionaryTransaction5.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 5:
                            _bigDictionaryTransaction6.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 6:
                            _bigDictionaryTransaction7.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 7:
                            _bigDictionaryTransaction8.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 8:
                            _bigDictionaryTransaction9.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 9:
                            _bigDictionaryTransaction10.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 10:
                            _bigDictionaryTransaction11.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 11:
                            _bigDictionaryTransaction12.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 12:
                            _bigDictionaryTransaction13.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 13:
                            _bigDictionaryTransaction14.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 14:
                            _bigDictionaryTransaction15.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 15:
                            _bigDictionaryTransaction16.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 16:
                            _bigDictionaryTransaction17.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 17:
                            _bigDictionaryTransaction18.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 18:
                            _bigDictionaryTransaction19.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 19:
                            _bigDictionaryTransaction20.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 20:
                            _bigDictionaryTransaction21.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 21:
                            _bigDictionaryTransaction22.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 22:
                            _bigDictionaryTransaction23.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 23:
                            _bigDictionaryTransaction24.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 24:
                            _bigDictionaryTransaction25.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 25:
                            _bigDictionaryTransaction26.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 26:
                            _bigDictionaryTransaction27.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 27:
                            _bigDictionaryTransaction28.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 28:
                            _bigDictionaryTransaction29.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 29:
                            _bigDictionaryTransaction30.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 30:
                            _bigDictionaryTransaction31.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 31:
                            _bigDictionaryTransaction32.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 32:
                            _bigDictionaryTransaction33.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 33:
                            _bigDictionaryTransaction34.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 34:
                            _bigDictionaryTransaction35.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 35:
                            _bigDictionaryTransaction36.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 36:
                            _bigDictionaryTransaction37.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 37:
                            _bigDictionaryTransaction38.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 38:
                            _bigDictionaryTransaction39.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 39:
                            _bigDictionaryTransaction40.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 40:
                            _bigDictionaryTransaction41.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 41:
                            _bigDictionaryTransaction42.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 42:
                            _bigDictionaryTransaction43.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 43:
                            _bigDictionaryTransaction44.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 44:
                            _bigDictionaryTransaction45.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 45:
                            _bigDictionaryTransaction46.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 46:
                            _bigDictionaryTransaction47.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 47:
                            _bigDictionaryTransaction48.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 48:
                            _bigDictionaryTransaction49.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 49:
                            _bigDictionaryTransaction50.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 50:
                            _bigDictionaryTransaction51.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 51:
                            _bigDictionaryTransaction52.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 52:
                            _bigDictionaryTransaction53.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 53:
                            _bigDictionaryTransaction54.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 54:
                            _bigDictionaryTransaction55.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 55:
                            _bigDictionaryTransaction56.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 56:
                            _bigDictionaryTransaction57.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 57:
                            _bigDictionaryTransaction58.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 58:
                            _bigDictionaryTransaction59.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 59:
                            _bigDictionaryTransaction60.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 60:
                            _bigDictionaryTransaction61.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 61:
                            _bigDictionaryTransaction62.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 62:
                            _bigDictionaryTransaction63.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 63:
                            _bigDictionaryTransaction64.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 64:
                            _bigDictionaryTransaction65.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 65:
                            _bigDictionaryTransaction66.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 66:
                            _bigDictionaryTransaction67.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 67:
                            _bigDictionaryTransaction68.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 68:
                            _bigDictionaryTransaction69.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 69:
                            _bigDictionaryTransaction70.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 70:
                            _bigDictionaryTransaction71.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 71:
                            _bigDictionaryTransaction72.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 72:
                            _bigDictionaryTransaction73.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 73:
                            _bigDictionaryTransaction74.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 74:
                            _bigDictionaryTransaction75.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 75:
                            _bigDictionaryTransaction76.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 76:
                            _bigDictionaryTransaction77.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 77:
                            _bigDictionaryTransaction78.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 78:
                            _bigDictionaryTransaction79.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 79:
                            _bigDictionaryTransaction80.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 80:
                            _bigDictionaryTransaction81.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 81:
                            _bigDictionaryTransaction82.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 82:
                            _bigDictionaryTransaction83.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 83:
                            _bigDictionaryTransaction84.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 84:
                            _bigDictionaryTransaction85.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 85:
                            _bigDictionaryTransaction86.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 86:
                            _bigDictionaryTransaction87.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 87:
                            _bigDictionaryTransaction88.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 88:
                            _bigDictionaryTransaction89.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 89:
                            _bigDictionaryTransaction90.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 90:
                            _bigDictionaryTransaction91.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 91:
                            _bigDictionaryTransaction92.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 92:
                            _bigDictionaryTransaction93.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 93:
                            _bigDictionaryTransaction94.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 94:
                            _bigDictionaryTransaction95.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 95:
                            _bigDictionaryTransaction96.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 96:
                            _bigDictionaryTransaction97.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 97:
                            _bigDictionaryTransaction98.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 98:
                            _bigDictionaryTransaction99.Add(id, new TransactionObject(id, transaction, position));
                            break;
                        case 99:
                            _bigDictionaryTransaction100.Add(id, new TransactionObject(id, transaction, position));
                            break;
                    }
                
                
                    if (Program.RemoteNodeSettingObject.enable_disk_cache_mode)
                    {
                        if (!DictionaryStreamPosition.ContainsKey(id))
                            DictionaryStreamPosition.TryAdd(id, position);
                        
                    }
                }


            }
            catch
            {
                result = false;
            }


            return result;
        }

        /// <summary>
        /// Get transaction from transaction file.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        private async Task<Tuple<string, long>> GetTransactionFromFile(long id, CancellationTokenSource cancellation)
        {
            if (id < 0)
                return new Tuple<string, long>(string.Empty, 0);

            string transactionResult = string.Empty;

            bool useSemaphore = false;

            try
            {
                try
                {

                    if (!await OpenTransactionReader(cancellation))
                        return new Tuple<string, long>(string.Empty, 0);

                    await SemaphoreSlimTransactionReader.WaitAsync(cancellation.Token);
                    useSemaphore = true;

                    if (useSemaphore)
                    {

                        bool cancelRead = false;

                        if (DictionaryStreamPosition.ContainsKey(id))
                        {
                            if (DictionaryStreamPosition[id] == 0 && id > 0)
                                cancelRead = true;
                        }

                        if (!cancelRead)
                        {

                            bool useSeek = true;

                            if (useSeek)
                            {
                                _transactionReader.BaseStream.Seek(
                                    DictionaryStreamPosition.ContainsKey(id) ?
                                    DictionaryStreamPosition[id] : 0, SeekOrigin.Begin);
                            }
                            else _transactionReader.BaseStream.Seek(0, SeekOrigin.Begin);

                            long countSleep = DictionaryStreamPosition.Count;
                            long countSleepDone = 0;

                            string line;

                            while ((line = _transactionReader.ReadLine()) != null)
                            {
                                try
                                {
                                    if (Program.RemoteNodeSettingObject.enable_save_sync_raw)
                                    {
                                        if (!line.Contains("%"))
                                            continue;


                                        Debug.WriteLine(line + " search: " + id);

                                        var splitTransactionLine = line.Split(new[] { "%" }, StringSplitOptions.None);
                                        long transactionId = long.Parse(splitTransactionLine[0]);
                                        if (transactionId == id)
                                        {
                                            transactionResult = splitTransactionLine[1];

                                            if (!DictionaryStreamPosition.ContainsKey(id))
                                                DictionaryStreamPosition.TryAdd(id, _transactionReader.BaseStream.Position - line.Length);
                                            else DictionaryStreamPosition[id] = _transactionReader.BaseStream.Position - line.Length;

                                            break;

                                        }

                                    }
                                    else
                                    {
                                        var transactionObject = JsonConvert.DeserializeObject<ClassTransactionObject>(line);

                                        if (transactionObject.transaction_id == id)
                                        {
                                            transactionResult = ClassTransactionUtility.BuildTransactionRaw(transactionObject);

                                            if (!DictionaryStreamPosition.ContainsKey(id))
                                                DictionaryStreamPosition.TryAdd(id, _transactionReader.BaseStream.Position - line.Length);
                                            else DictionaryStreamPosition[id] = _transactionReader.BaseStream.Position - line.Length;
                                            break;
                                        }
                                    }
                                }
                                catch
                                {

                                    // Ignored.
                                }

                                if (countSleepDone < countSleep)
                                {
                                    countSleepDone++;
                                    await Task.Delay(1);
                                }
                            }
                        }
                        
                    }
                }
                catch
                {
                    // Ignored, catch the exception once the task is cancelled.
                }
            }
            finally
            {
                if (useSemaphore)
                    SemaphoreSlimTransactionReader.Release();
            }


            return new Tuple<string, long>(transactionResult, DictionaryStreamPosition.ContainsKey(id) ? DictionaryStreamPosition[id] : 0);
        }

        /// <summary>
        /// Retrieve transaction information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TransactionObject> GetTransaction(long id, CancellationTokenSource cancellation)
        {
            TransactionObject result = new TransactionObject(-1, string.Empty, 0);

            try
            {
                if (id < 0)
                    return new TransactionObject(-1, string.Empty, 0);

                long idDictionary = (long)Math.Ceiling((double)(id / MaxTransactionPerDictionary));


                if (idDictionary < 0)
                    return new TransactionObject(-1, string.Empty, 0);

                #region From disk cache.

                if (_enableDiskCache || (_enableDiskCache && result.IsEmpty))
                {

                    var transactionResult = await GetTransactionFromFile(id, cancellation);

                    if (!string.IsNullOrEmpty(transactionResult.Item1) && transactionResult.Item1.Length > 0)
                    {
                        UpdateTransaction(id, transactionResult.Item1, transactionResult.Item2);
                        return new TransactionObject(id, transactionResult.Item1, transactionResult.Item2);
                    }

                    await Task.Delay(1);

                }

                #endregion

                #region From active memory.

                switch (idDictionary)
                {
                    case 0:
                        result = _bigDictionaryTransaction1[id];
                        break;
                    case 1:
                        result = _bigDictionaryTransaction2[id];
                        break;
                    case 2:
                        result = _bigDictionaryTransaction3[id];
                        break;
                    case 3:
                        result = _bigDictionaryTransaction4[id];
                        break;
                    case 4:
                        result = _bigDictionaryTransaction5[id];
                        break;
                    case 5:
                        result = _bigDictionaryTransaction6[id];
                        break;
                    case 6:
                        result = _bigDictionaryTransaction7[id];
                        break;
                    case 7:
                        result = _bigDictionaryTransaction8[id];
                        break;
                    case 8:
                        result = _bigDictionaryTransaction9[id];
                        break;
                    case 9:
                        result = _bigDictionaryTransaction10[id];
                        break;
                    case 10:
                        result = _bigDictionaryTransaction11[id];
                        break;
                    case 11:
                        result = _bigDictionaryTransaction12[id];
                        break;
                    case 12:
                        result = _bigDictionaryTransaction13[id];
                        break;
                    case 13:
                        result = _bigDictionaryTransaction14[id];
                        break;
                    case 14:
                        result = _bigDictionaryTransaction15[id];
                        break;
                    case 15:
                        result = _bigDictionaryTransaction16[id];
                        break;
                    case 16:
                        result = _bigDictionaryTransaction17[id];
                        break;
                    case 17:
                        result = _bigDictionaryTransaction18[id];
                        break;
                    case 18:
                        result = _bigDictionaryTransaction19[id];
                        break;
                    case 19:
                        result = _bigDictionaryTransaction20[id];
                        break;
                    case 20:
                        result = _bigDictionaryTransaction21[id];
                        break;
                    case 21:
                        result = _bigDictionaryTransaction22[id];
                        break;
                    case 22:
                        result = _bigDictionaryTransaction23[id];
                        break;
                    case 23:
                        result = _bigDictionaryTransaction24[id];
                        break;
                    case 24:
                        result = _bigDictionaryTransaction25[id];
                        break;
                    case 25:
                        result = _bigDictionaryTransaction26[id];
                        break;
                    case 26:
                        result = _bigDictionaryTransaction27[id];
                        break;
                    case 27:
                        result = _bigDictionaryTransaction28[id];
                        break;
                    case 28:
                        result = _bigDictionaryTransaction29[id];
                        break;
                    case 29:
                        result = _bigDictionaryTransaction30[id];
                        break;
                    case 30:
                        result = _bigDictionaryTransaction31[id];
                        break;
                    case 31:
                        result = _bigDictionaryTransaction32[id];
                        break;
                    case 32:
                        result = _bigDictionaryTransaction33[id];
                        break;
                    case 33:
                        result = _bigDictionaryTransaction34[id];
                        break;
                    case 34:
                        result = _bigDictionaryTransaction35[id];
                        break;
                    case 35:
                        result = _bigDictionaryTransaction36[id];
                        break;
                    case 36:
                        result = _bigDictionaryTransaction37[id];
                        break;
                    case 37:
                        result = _bigDictionaryTransaction38[id];
                        break;
                    case 38:
                        result = _bigDictionaryTransaction39[id];
                        break;
                    case 39:
                        result = _bigDictionaryTransaction40[id];
                        break;
                    case 40:
                        result = _bigDictionaryTransaction41[id];
                        break;
                    case 41:
                        result = _bigDictionaryTransaction42[id];
                        break;
                    case 42:
                        result = _bigDictionaryTransaction43[id];
                        break;
                    case 43:
                        result = _bigDictionaryTransaction44[id];
                        break;
                    case 44:
                        result = _bigDictionaryTransaction45[id];
                        break;
                    case 45:
                        result = _bigDictionaryTransaction46[id];
                        break;
                    case 46:
                        result = _bigDictionaryTransaction47[id];
                        break;
                    case 47:
                        result = _bigDictionaryTransaction48[id];
                        break;
                    case 48:
                        result = _bigDictionaryTransaction49[id];
                        break;
                    case 49:
                        result = _bigDictionaryTransaction50[id];
                        break;
                    case 50:
                        result = _bigDictionaryTransaction51[id];
                        break;
                    case 51:
                        result = _bigDictionaryTransaction52[id];
                        break;
                    case 52:
                        result = _bigDictionaryTransaction53[id];
                        break;
                    case 53:
                        result = _bigDictionaryTransaction54[id];
                        break;
                    case 54:
                        result = _bigDictionaryTransaction55[id];
                        break;
                    case 55:
                        result = _bigDictionaryTransaction56[id];
                        break;
                    case 56:
                        result = _bigDictionaryTransaction57[id];
                        break;
                    case 57:
                        result = _bigDictionaryTransaction58[id];
                        break;
                    case 58:
                        result = _bigDictionaryTransaction59[id];
                        break;
                    case 59:
                        result = _bigDictionaryTransaction60[id];
                        break;
                    case 60:
                        result = _bigDictionaryTransaction61[id];
                        break;
                    case 61:
                        result = _bigDictionaryTransaction62[id];
                        break;
                    case 62:
                        result = _bigDictionaryTransaction63[id];
                        break;
                    case 63:
                        result = _bigDictionaryTransaction64[id];
                        break;
                    case 64:
                        result = _bigDictionaryTransaction65[id];
                        break;
                    case 65:
                        result = _bigDictionaryTransaction66[id];
                        break;
                    case 66:
                        result = _bigDictionaryTransaction67[id];
                        break;
                    case 67:
                        result = _bigDictionaryTransaction68[id];
                        break;
                    case 68:
                        result = _bigDictionaryTransaction69[id];
                        break;
                    case 69:
                        result = _bigDictionaryTransaction70[id];
                        break;
                    case 70:
                        result = _bigDictionaryTransaction71[id];
                        break;
                    case 71:
                        result = _bigDictionaryTransaction72[id];
                        break;
                    case 72:
                        result = _bigDictionaryTransaction73[id];
                        break;
                    case 73:
                        result = _bigDictionaryTransaction74[id];
                        break;
                    case 74:
                        result = _bigDictionaryTransaction75[id];
                        break;
                    case 75:
                        result = _bigDictionaryTransaction76[id];
                        break;
                    case 76:
                        result = _bigDictionaryTransaction77[id];
                        break;
                    case 77:
                        result = _bigDictionaryTransaction78[id];
                        break;
                    case 78:
                        result = _bigDictionaryTransaction79[id];
                        break;
                    case 79:
                        result = _bigDictionaryTransaction80[id];
                        break;
                    case 80:
                        result = _bigDictionaryTransaction81[id];
                        break;
                    case 81:
                        result = _bigDictionaryTransaction82[id];
                        break;
                    case 82:
                        result = _bigDictionaryTransaction83[id];
                        break;
                    case 83:
                        result = _bigDictionaryTransaction84[id];
                        break;
                    case 84:
                        result = _bigDictionaryTransaction85[id];
                        break;
                    case 85:
                        result = _bigDictionaryTransaction86[id];
                        break;
                    case 86:
                        result = _bigDictionaryTransaction87[id];
                        break;
                    case 87:
                        result = _bigDictionaryTransaction88[id];
                        break;
                    case 88:
                        result = _bigDictionaryTransaction89[id];
                        break;
                    case 89:
                        result = _bigDictionaryTransaction90[id];
                        break;
                    case 90:
                        result = _bigDictionaryTransaction91[id];
                        break;
                    case 91:
                        result = _bigDictionaryTransaction92[id];
                        break;
                    case 92:
                        result = _bigDictionaryTransaction93[id];
                        break;
                    case 93:
                        result = _bigDictionaryTransaction94[id];
                        break;
                    case 94:
                        result = _bigDictionaryTransaction95[id];
                        break;
                    case 95:
                        result = _bigDictionaryTransaction96[id];
                        break;
                    case 96:
                        result = _bigDictionaryTransaction97[id];
                        break;
                    case 97:
                        result = _bigDictionaryTransaction98[id];
                        break;
                    case 98:
                        result = _bigDictionaryTransaction99[id];
                        break;
                    case 99:
                        result = _bigDictionaryTransaction100[id];
                        break;
                }

                #endregion

                return result;
            }
#if DEBUG
            catch(Exception error)
            {
                Debug.WriteLine("Error to get the transaction id: "+id+" | Exception: "+error.Message);
#else
            catch
            {


#endif
                return new TransactionObject(0, string.Empty, 0);
            }
        }

        /// <summary>
        /// Clear a transaction target.
        /// </summary>
        /// <param name="id"></param>
        public void ClearTransaction(long id)
        {
            if (id < 0)
                return;

            long idDictionary = (long)Math.Ceiling((double)(id / MaxTransactionPerDictionary));


            if (idDictionary >= 0)
            {
                switch (idDictionary)
                {
                    case 0:
                        _bigDictionaryTransaction1[id].TransactionData = null;
                        break;
                    case 1:
                        _bigDictionaryTransaction2[id].TransactionData = null;
                        break;
                    case 2:
                        _bigDictionaryTransaction3[id].TransactionData = null;
                        break;
                    case 3:
                        _bigDictionaryTransaction4[id].TransactionData = null;
                        break;
                    case 4:
                        _bigDictionaryTransaction5[id].TransactionData = null;
                        break;
                    case 5:
                        _bigDictionaryTransaction6[id].TransactionData = null;
                        break;
                    case 6:
                        _bigDictionaryTransaction7[id].TransactionData = null;
                        break;
                    case 7:
                        _bigDictionaryTransaction8[id].TransactionData = null;
                        break;
                    case 8:
                        _bigDictionaryTransaction9[id].TransactionData = null;
                        break;
                    case 9:
                        _bigDictionaryTransaction10[id].TransactionData = null;
                        break;
                    case 10:
                        _bigDictionaryTransaction11[id].TransactionData = null;
                        break;
                    case 11:
                        _bigDictionaryTransaction12[id].TransactionData = null;
                        break;
                    case 12:
                        _bigDictionaryTransaction13[id].TransactionData = null;
                        break;
                    case 13:
                        _bigDictionaryTransaction14[id].TransactionData = null;
                        break;
                    case 14:
                        _bigDictionaryTransaction15[id].TransactionData = null;
                        break;
                    case 15:
                        _bigDictionaryTransaction16[id].TransactionData = null;
                        break;
                    case 16:
                        _bigDictionaryTransaction17[id].TransactionData = null;
                        break;
                    case 17:
                        _bigDictionaryTransaction18[id].TransactionData = null;
                        break;
                    case 18:
                        _bigDictionaryTransaction19[id].TransactionData = null;
                        break;
                    case 19:
                        _bigDictionaryTransaction20[id].TransactionData = null;
                        break;
                    case 20:
                        _bigDictionaryTransaction21[id].TransactionData = null;
                        break;
                    case 21:
                        _bigDictionaryTransaction22[id].TransactionData = null;
                        break;
                    case 22:
                        _bigDictionaryTransaction23[id].TransactionData = null;
                        break;
                    case 23:
                        _bigDictionaryTransaction24[id].TransactionData = null;
                        break;
                    case 24:
                        _bigDictionaryTransaction25[id].TransactionData = null;
                        break;
                    case 25:
                        _bigDictionaryTransaction26[id].TransactionData = null;
                        break;
                    case 26:
                        _bigDictionaryTransaction27[id].TransactionData = null;
                        break;
                    case 27:
                        _bigDictionaryTransaction28[id].TransactionData = null;
                        break;
                    case 28:
                        _bigDictionaryTransaction29[id].TransactionData = null;
                        break;
                    case 29:
                        _bigDictionaryTransaction30[id].TransactionData = null;
                        break;
                    case 30:
                        _bigDictionaryTransaction31[id].TransactionData = null;
                        break;
                    case 31:
                        _bigDictionaryTransaction32[id].TransactionData = null;
                        break;
                    case 32:
                        _bigDictionaryTransaction33[id].TransactionData = null;
                        break;
                    case 33:
                        _bigDictionaryTransaction34[id].TransactionData = null;
                        break;
                    case 34:
                        _bigDictionaryTransaction35[id].TransactionData = null;
                        break;
                    case 35:
                        _bigDictionaryTransaction36[id].TransactionData = null;
                        break;
                    case 36:
                        _bigDictionaryTransaction37[id].TransactionData = null;
                        break;
                    case 37:
                        _bigDictionaryTransaction38[id].TransactionData = null;
                        break;
                    case 38:
                        _bigDictionaryTransaction39[id].TransactionData = null;
                        break;
                    case 39:
                        _bigDictionaryTransaction40[id].TransactionData = null;
                        break;
                    case 40:
                        _bigDictionaryTransaction41[id].TransactionData = null;
                        break;
                    case 41:
                        _bigDictionaryTransaction42[id].TransactionData = null;
                        break;
                    case 42:
                        _bigDictionaryTransaction43[id].TransactionData = null;
                        break;
                    case 43:
                        _bigDictionaryTransaction44[id].TransactionData = null;
                        break;
                    case 44:
                        _bigDictionaryTransaction45[id].TransactionData = null;
                        break;
                    case 45:
                        _bigDictionaryTransaction46[id].TransactionData = null;
                        break;
                    case 46:
                        _bigDictionaryTransaction47[id].TransactionData = null;
                        break;
                    case 47:
                        _bigDictionaryTransaction48[id].TransactionData = null;
                        break;
                    case 48:
                        _bigDictionaryTransaction49[id].TransactionData = null;
                        break;
                    case 49:
                        _bigDictionaryTransaction50[id].TransactionData = null;
                        break;
                    case 50:
                        _bigDictionaryTransaction51[id].TransactionData = null;
                        break;
                    case 51:
                        _bigDictionaryTransaction52[id].TransactionData = null;
                        break;
                    case 52:
                        _bigDictionaryTransaction53[id].TransactionData = null;
                        break;
                    case 53:
                        _bigDictionaryTransaction54[id].TransactionData = null;
                        break;
                    case 54:
                        _bigDictionaryTransaction55[id].TransactionData = null;
                        break;
                    case 55:
                        _bigDictionaryTransaction56[id].TransactionData = null;
                        break;
                    case 56:
                        _bigDictionaryTransaction57[id].TransactionData = null;
                        break;
                    case 57:
                        _bigDictionaryTransaction58[id].TransactionData = null;
                        break;
                    case 58:
                        _bigDictionaryTransaction59[id].TransactionData = null;
                        break;
                    case 59:
                        _bigDictionaryTransaction60[id].TransactionData = null;
                        break;
                    case 60:
                        _bigDictionaryTransaction61[id].TransactionData = null;
                        break;
                    case 61:
                        _bigDictionaryTransaction62[id].TransactionData = null;
                        break;
                    case 62:
                        _bigDictionaryTransaction63[id].TransactionData = null;
                        break;
                    case 63:
                        _bigDictionaryTransaction64[id].TransactionData = null;
                        break;
                    case 64:
                        _bigDictionaryTransaction65[id].TransactionData = null;
                        break;
                    case 65:
                        _bigDictionaryTransaction66[id].TransactionData = null;
                        break;
                    case 66:
                        _bigDictionaryTransaction67[id].TransactionData = null;
                        break;
                    case 67:
                        _bigDictionaryTransaction68[id].TransactionData = null;
                        break;
                    case 68:
                        _bigDictionaryTransaction69[id].TransactionData = null;
                        break;
                    case 69:
                        _bigDictionaryTransaction70[id].TransactionData = null;
                        break;
                    case 70:
                        _bigDictionaryTransaction71[id].TransactionData = null;
                        break;
                    case 71:
                        _bigDictionaryTransaction72[id].TransactionData = null;
                        break;
                    case 72:
                        _bigDictionaryTransaction73[id].TransactionData = null;
                        break;
                    case 73:
                        _bigDictionaryTransaction74[id].TransactionData = null;
                        break;
                    case 74:
                        _bigDictionaryTransaction75[id].TransactionData = null;
                        break;
                    case 75:
                        _bigDictionaryTransaction76[id].TransactionData = null;
                        break;
                    case 76:
                        _bigDictionaryTransaction77[id].TransactionData = null;
                        break;
                    case 77:
                        _bigDictionaryTransaction78[id].TransactionData = null;
                        break;
                    case 78:
                        _bigDictionaryTransaction79[id].TransactionData = null;
                        break;
                    case 79:
                        _bigDictionaryTransaction80[id].TransactionData = null;
                        break;
                    case 80:
                        _bigDictionaryTransaction81[id].TransactionData = null;
                        break;
                    case 81:
                        _bigDictionaryTransaction82[id].TransactionData = null;
                        break;
                    case 82:
                        _bigDictionaryTransaction83[id].TransactionData = null;
                        break;
                    case 83:
                        _bigDictionaryTransaction84[id].TransactionData = null;
                        break;
                    case 84:
                        _bigDictionaryTransaction85[id].TransactionData = null;
                        break;
                    case 85:
                        _bigDictionaryTransaction86[id].TransactionData = null;
                        break;
                    case 86:
                        _bigDictionaryTransaction87[id].TransactionData = null;
                        break;
                    case 87:
                        _bigDictionaryTransaction88[id].TransactionData = null;
                        break;
                    case 88:
                        _bigDictionaryTransaction89[id].TransactionData = null;
                        break;
                    case 89:
                        _bigDictionaryTransaction90[id].TransactionData = null;
                        break;
                    case 90:
                        _bigDictionaryTransaction91[id].TransactionData = null;
                        break;
                    case 91:
                        _bigDictionaryTransaction92[id].TransactionData = null;
                        break;
                    case 92:
                        _bigDictionaryTransaction93[id].TransactionData = null;
                        break;
                    case 93:
                        _bigDictionaryTransaction94[id].TransactionData = null;
                        break;
                    case 94:
                        _bigDictionaryTransaction95[id].TransactionData = null;
                        break;
                    case 95:
                        _bigDictionaryTransaction96[id].TransactionData = null;
                        break;
                    case 96:
                        _bigDictionaryTransaction97[id].TransactionData = null;
                        break;
                    case 97:
                        _bigDictionaryTransaction98[id].TransactionData = null;
                        break;
                    case 98:
                        _bigDictionaryTransaction99[id].TransactionData = null;
                        break;
                    case 99:
                        _bigDictionaryTransaction100[id].TransactionData = null;
                        break;
                }
            }
        }

        /// <summary>
        /// Update a transaction data.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="transactionData"></param>
        /// <param name="position"></param>
        private void UpdateTransaction(long id, string transactionData, long position)
        {
            if (id < 0)
                return;

            long idDictionary = (long)Math.Ceiling((double)(id / MaxTransactionPerDictionary));

            if (idDictionary >= 0)
            {
                switch (idDictionary)
                {
                    case 0:
                        _bigDictionaryTransaction1[id].TransactionData = transactionData;
                        _bigDictionaryTransaction1[id].Position = position;
                        break;
                    case 1:
                        _bigDictionaryTransaction2[id].TransactionData = transactionData;
                        _bigDictionaryTransaction2[id].Position = position;
                        break;
                    case 2:
                        _bigDictionaryTransaction3[id].TransactionData = transactionData;
                        _bigDictionaryTransaction3[id].Position = position;
                        break;
                    case 3:
                        _bigDictionaryTransaction4[id].TransactionData = transactionData;
                        _bigDictionaryTransaction4[id].Position = position;
                        break;
                    case 4:
                        _bigDictionaryTransaction5[id].TransactionData = transactionData;
                        _bigDictionaryTransaction5[id].Position = position;
                        break;
                    case 5:
                        _bigDictionaryTransaction6[id].TransactionData = transactionData;
                        _bigDictionaryTransaction6[id].Position = position;
                        break;
                    case 6:
                        _bigDictionaryTransaction7[id].TransactionData = transactionData;
                        _bigDictionaryTransaction7[id].Position = position;
                        break;
                    case 7:
                        _bigDictionaryTransaction8[id].TransactionData = transactionData;
                        _bigDictionaryTransaction8[id].Position = position;
                        break;
                    case 8:
                        _bigDictionaryTransaction9[id].TransactionData = transactionData;
                        _bigDictionaryTransaction9[id].Position = position;
                        break;
                    case 9:
                        _bigDictionaryTransaction10[id].TransactionData = transactionData;
                        _bigDictionaryTransaction10[id].Position = position;
                        break;
                    case 10:
                        _bigDictionaryTransaction11[id].TransactionData = transactionData;
                        _bigDictionaryTransaction11[id].Position = position;
                        break;
                    case 11:
                        _bigDictionaryTransaction12[id].TransactionData = transactionData;
                        _bigDictionaryTransaction12[id].Position = position;
                        break;
                    case 12:
                        _bigDictionaryTransaction13[id].TransactionData = transactionData;
                        _bigDictionaryTransaction13[id].Position = position;
                        break;
                    case 13:
                        _bigDictionaryTransaction14[id].TransactionData = transactionData;
                        _bigDictionaryTransaction14[id].Position = position;
                        break;
                    case 14:
                        _bigDictionaryTransaction15[id].TransactionData = transactionData;
                        _bigDictionaryTransaction15[id].Position = position;
                        break;
                    case 15:
                        _bigDictionaryTransaction16[id].TransactionData = transactionData;
                        _bigDictionaryTransaction16[id].Position = position;
                        break;
                    case 16:
                        _bigDictionaryTransaction17[id].TransactionData = transactionData;
                        _bigDictionaryTransaction17[id].Position = position;
                        break;
                    case 17:
                        _bigDictionaryTransaction18[id].TransactionData = transactionData;
                        _bigDictionaryTransaction18[id].Position = position;
                        break;
                    case 18:
                        _bigDictionaryTransaction19[id].TransactionData = transactionData;
                        _bigDictionaryTransaction19[id].Position = position;
                        break;
                    case 19:
                        _bigDictionaryTransaction20[id].TransactionData = transactionData;
                        _bigDictionaryTransaction20[id].Position = position;
                        break;
                    case 20:
                        _bigDictionaryTransaction21[id].TransactionData = transactionData;
                        _bigDictionaryTransaction21[id].Position = position;
                        break;
                    case 21:
                        _bigDictionaryTransaction22[id].TransactionData = transactionData;
                        _bigDictionaryTransaction22[id].Position = position;
                        break;
                    case 22:
                        _bigDictionaryTransaction23[id].TransactionData = transactionData;
                        _bigDictionaryTransaction23[id].Position = position;
                        break;
                    case 23:
                        _bigDictionaryTransaction24[id].TransactionData = transactionData;
                        _bigDictionaryTransaction24[id].Position = position;
                        break;
                    case 24:
                        _bigDictionaryTransaction25[id].TransactionData = transactionData;
                        _bigDictionaryTransaction25[id].Position = position;
                        break;
                    case 25:
                        _bigDictionaryTransaction26[id].TransactionData = transactionData;
                        _bigDictionaryTransaction26[id].Position = position;
                        break;
                    case 26:
                        _bigDictionaryTransaction27[id].TransactionData = transactionData;
                        _bigDictionaryTransaction27[id].Position = position;
                        break;
                    case 27:
                        _bigDictionaryTransaction28[id].TransactionData = transactionData;
                        _bigDictionaryTransaction28[id].Position = position;
                        break;
                    case 28:
                        _bigDictionaryTransaction29[id].TransactionData = transactionData;
                        _bigDictionaryTransaction29[id].Position = position;
                        break;
                    case 29:
                        _bigDictionaryTransaction30[id].TransactionData = transactionData;
                        _bigDictionaryTransaction30[id].Position = position;
                        break;
                    case 30:
                        _bigDictionaryTransaction31[id].TransactionData = transactionData;
                        _bigDictionaryTransaction31[id].Position = position;
                        break;
                    case 31:
                        _bigDictionaryTransaction32[id].TransactionData = transactionData;
                        _bigDictionaryTransaction32[id].Position = position;
                        break;
                    case 32:
                        _bigDictionaryTransaction33[id].TransactionData = transactionData;
                        _bigDictionaryTransaction33[id].Position = position;
                        break;
                    case 33:
                        _bigDictionaryTransaction34[id].TransactionData = transactionData;
                        _bigDictionaryTransaction34[id].Position = position;
                        break;
                    case 34:
                        _bigDictionaryTransaction35[id].TransactionData = transactionData;
                        _bigDictionaryTransaction35[id].Position = position;
                        break;
                    case 35:
                        _bigDictionaryTransaction36[id].TransactionData = transactionData;
                        _bigDictionaryTransaction36[id].Position = position;
                        break;
                    case 36:
                        _bigDictionaryTransaction37[id].TransactionData = transactionData;
                        _bigDictionaryTransaction37[id].Position = position;
                        break;
                    case 37:
                        _bigDictionaryTransaction38[id].TransactionData = transactionData;
                        _bigDictionaryTransaction38[id].Position = position;
                        break;
                    case 38:
                        _bigDictionaryTransaction39[id].TransactionData = transactionData;
                        _bigDictionaryTransaction39[id].Position = position;
                        break;
                    case 39:
                        _bigDictionaryTransaction40[id].TransactionData = transactionData;
                        _bigDictionaryTransaction40[id].Position = position;
                        break;
                    case 40:
                        _bigDictionaryTransaction41[id].TransactionData = transactionData;
                        _bigDictionaryTransaction41[id].Position = position;
                        break;
                    case 41:
                        _bigDictionaryTransaction42[id].TransactionData = transactionData;
                        _bigDictionaryTransaction42[id].Position = position;
                        break;
                    case 42:
                        _bigDictionaryTransaction43[id].TransactionData = transactionData;
                        _bigDictionaryTransaction43[id].Position = position;
                        break;
                    case 43:
                        _bigDictionaryTransaction44[id].TransactionData = transactionData;
                        _bigDictionaryTransaction44[id].Position = position;
                        break;
                    case 44:
                        _bigDictionaryTransaction45[id].TransactionData = transactionData;
                        _bigDictionaryTransaction45[id].Position = position;
                        break;
                    case 45:
                        _bigDictionaryTransaction46[id].TransactionData = transactionData;
                        _bigDictionaryTransaction46[id].Position = position;
                        break;
                    case 46:
                        _bigDictionaryTransaction47[id].TransactionData = transactionData;
                        _bigDictionaryTransaction47[id].Position = position;
                        break;
                    case 47:
                        _bigDictionaryTransaction48[id].TransactionData = transactionData;
                        _bigDictionaryTransaction48[id].Position = position;
                        break;
                    case 48:
                        _bigDictionaryTransaction49[id].TransactionData = transactionData;
                        _bigDictionaryTransaction49[id].Position = position;
                        break;
                    case 49:
                        _bigDictionaryTransaction50[id].TransactionData = transactionData;
                        _bigDictionaryTransaction50[id].Position = position;
                        break;
                    case 50:
                        _bigDictionaryTransaction51[id].TransactionData = transactionData;
                        _bigDictionaryTransaction51[id].Position = position;
                        break;
                    case 51:
                        _bigDictionaryTransaction52[id].TransactionData = transactionData;
                        _bigDictionaryTransaction52[id].Position = position;
                        break;
                    case 52:
                        _bigDictionaryTransaction53[id].TransactionData = transactionData;
                        _bigDictionaryTransaction53[id].Position = position;
                        break;
                    case 53:
                        _bigDictionaryTransaction54[id].TransactionData = transactionData;
                        _bigDictionaryTransaction54[id].Position = position;
                        break;
                    case 54:
                        _bigDictionaryTransaction55[id].TransactionData = transactionData;
                        _bigDictionaryTransaction55[id].Position = position;
                        break;
                    case 55:
                        _bigDictionaryTransaction56[id].TransactionData = transactionData;
                        _bigDictionaryTransaction56[id].Position = position;
                        break;
                    case 56:
                        _bigDictionaryTransaction57[id].TransactionData = transactionData;
                        _bigDictionaryTransaction57[id].Position = position;
                        break;
                    case 57:
                        _bigDictionaryTransaction58[id].TransactionData = transactionData;
                        _bigDictionaryTransaction58[id].Position = position;
                        break;
                    case 58:
                        _bigDictionaryTransaction59[id].TransactionData = transactionData;
                        _bigDictionaryTransaction59[id].Position = position;
                        break;
                    case 59:
                        _bigDictionaryTransaction60[id].TransactionData = transactionData;
                        _bigDictionaryTransaction60[id].Position = position;
                        break;
                    case 60:
                        _bigDictionaryTransaction61[id].TransactionData = transactionData;
                        _bigDictionaryTransaction61[id].Position = position;
                        break;
                    case 61:
                        _bigDictionaryTransaction62[id].TransactionData = transactionData;
                        _bigDictionaryTransaction62[id].Position = position;
                        break;
                    case 62:
                        _bigDictionaryTransaction63[id].TransactionData = transactionData;
                        _bigDictionaryTransaction63[id].Position = position;
                        break;
                    case 63:
                        _bigDictionaryTransaction64[id].TransactionData = transactionData;
                        _bigDictionaryTransaction64[id].Position = position;
                        break;
                    case 64:
                        _bigDictionaryTransaction65[id].TransactionData = transactionData;
                        _bigDictionaryTransaction65[id].Position = position;
                        break;
                    case 65:
                        _bigDictionaryTransaction66[id].TransactionData = transactionData;
                        _bigDictionaryTransaction66[id].Position = position;
                        break;
                    case 66:
                        _bigDictionaryTransaction67[id].TransactionData = transactionData;
                        _bigDictionaryTransaction67[id].Position = position;
                        break;
                    case 67:
                        _bigDictionaryTransaction68[id].TransactionData = transactionData;
                        _bigDictionaryTransaction68[id].Position = position;
                        break;
                    case 68:
                        _bigDictionaryTransaction69[id].TransactionData = transactionData;
                        _bigDictionaryTransaction69[id].Position = position;
                        break;

                    case 69:
                        _bigDictionaryTransaction70[id].TransactionData = transactionData;
                        _bigDictionaryTransaction70[id].Position = position;
                        break;
                    case 70:
                        _bigDictionaryTransaction71[id].TransactionData = transactionData;
                        _bigDictionaryTransaction71[id].Position = position;
                        break;
                    case 71:
                        _bigDictionaryTransaction72[id].TransactionData = transactionData;
                        _bigDictionaryTransaction72[id].Position = position;
                        break;
                    case 72:
                        _bigDictionaryTransaction73[id].TransactionData = transactionData;
                        _bigDictionaryTransaction73[id].Position = position;
                        break;
                    case 73:
                        _bigDictionaryTransaction74[id].TransactionData = transactionData;
                        _bigDictionaryTransaction74[id].Position = position;
                        break;
                    case 74:
                        _bigDictionaryTransaction75[id].TransactionData = transactionData;
                        _bigDictionaryTransaction75[id].Position = position;
                        break;
                    case 75:
                        _bigDictionaryTransaction76[id].TransactionData = transactionData;
                        _bigDictionaryTransaction76[id].Position = position;
                        break;
                    case 76:
                        _bigDictionaryTransaction77[id].TransactionData = transactionData;
                        _bigDictionaryTransaction77[id].Position = position;
                        break;
                    case 77:
                        _bigDictionaryTransaction78[id].TransactionData = transactionData;
                        _bigDictionaryTransaction78[id].Position = position;
                        break;
                    case 78:
                        _bigDictionaryTransaction79[id].TransactionData = transactionData;
                        _bigDictionaryTransaction79[id].Position = position;
                        break;
                    case 79:
                        _bigDictionaryTransaction80[id].TransactionData = transactionData;
                        _bigDictionaryTransaction80[id].Position = position;
                        break;
                    case 80:
                        _bigDictionaryTransaction81[id].TransactionData = transactionData;
                        _bigDictionaryTransaction81[id].Position = position;
                        break;
                    case 81:
                        _bigDictionaryTransaction82[id].TransactionData = transactionData;
                        _bigDictionaryTransaction82[id].Position = position;
                        break;
                    case 82:
                        _bigDictionaryTransaction83[id].TransactionData = transactionData;
                        _bigDictionaryTransaction83[id].Position = position;
                        break;
                    case 83:
                        _bigDictionaryTransaction84[id].TransactionData = transactionData;
                        _bigDictionaryTransaction84[id].Position = position;
                        break;
                    case 84:
                        _bigDictionaryTransaction85[id].TransactionData = transactionData;
                        _bigDictionaryTransaction85[id].Position = position;
                        break;
                    case 85:
                        _bigDictionaryTransaction86[id].TransactionData = transactionData;
                        _bigDictionaryTransaction86[id].Position = position;
                        break;
                    case 86:
                        _bigDictionaryTransaction87[id].TransactionData = transactionData;
                        _bigDictionaryTransaction87[id].Position = position;
                        break;
                    case 87:
                        _bigDictionaryTransaction88[id].TransactionData = transactionData;
                        _bigDictionaryTransaction88[id].Position = position;
                        break;
                    case 88:
                        _bigDictionaryTransaction89[id].TransactionData = transactionData;
                        _bigDictionaryTransaction89[id].Position = position;
                        break;
                    case 89:
                        _bigDictionaryTransaction90[id].TransactionData = transactionData;
                        _bigDictionaryTransaction90[id].Position = position;
                        break;
                    case 90:
                        _bigDictionaryTransaction91[id].TransactionData = transactionData;
                        _bigDictionaryTransaction91[id].Position = position;
                        break;
                    case 91:
                        _bigDictionaryTransaction92[id].TransactionData = transactionData;
                        _bigDictionaryTransaction92[id].Position = position;
                        break;
                    case 92:
                        _bigDictionaryTransaction93[id].TransactionData = transactionData;
                        _bigDictionaryTransaction93[id].Position = position;
                        break;
                    case 93:
                        _bigDictionaryTransaction94[id].TransactionData = transactionData;
                        _bigDictionaryTransaction94[id].Position = position;
                        break;
                    case 94:
                        _bigDictionaryTransaction95[id].TransactionData = transactionData;
                        _bigDictionaryTransaction95[id].Position = position;
                        break;
                    case 95:
                        _bigDictionaryTransaction96[id].TransactionData = transactionData;
                        _bigDictionaryTransaction96[id].Position = position;
                        break;
                    case 96:
                        _bigDictionaryTransaction97[id].TransactionData = transactionData;
                        _bigDictionaryTransaction97[id].Position = position;
                        break;
                    case 97:
                        _bigDictionaryTransaction98[id].TransactionData = transactionData;
                        _bigDictionaryTransaction98[id].Position = position;
                        break;
                    case 98:
                        _bigDictionaryTransaction99[id].TransactionData = transactionData;
                        _bigDictionaryTransaction99[id].Position = position;
                        break;
                    case 99:
                        _bigDictionaryTransaction100[id].TransactionData = transactionData;
                        _bigDictionaryTransaction100[id].Position = position;
                        break;
                }
            }

        }

        /// <summary>
        /// Retrieve total transaction saved.
        /// </summary>
        /// <returns></returns>
        public long CountTransaction()
        {
 

            return _bigDictionaryTransaction1.Count +
                _bigDictionaryTransaction2.Count +
                _bigDictionaryTransaction3.Count +
                _bigDictionaryTransaction4.Count +
                _bigDictionaryTransaction5.Count +
                _bigDictionaryTransaction6.Count +
                _bigDictionaryTransaction7.Count +
                _bigDictionaryTransaction8.Count +
                _bigDictionaryTransaction9.Count +
                _bigDictionaryTransaction10.Count +
                _bigDictionaryTransaction11.Count +
                _bigDictionaryTransaction12.Count +
                _bigDictionaryTransaction13.Count +
                _bigDictionaryTransaction14.Count +
                _bigDictionaryTransaction15.Count +
                _bigDictionaryTransaction16.Count +
                _bigDictionaryTransaction17.Count +
                _bigDictionaryTransaction18.Count +
                _bigDictionaryTransaction19.Count +
                _bigDictionaryTransaction20.Count +
                _bigDictionaryTransaction21.Count +
                _bigDictionaryTransaction22.Count +
                _bigDictionaryTransaction23.Count +
                _bigDictionaryTransaction24.Count +
                _bigDictionaryTransaction25.Count +
                _bigDictionaryTransaction26.Count +
                _bigDictionaryTransaction27.Count +
                _bigDictionaryTransaction28.Count +
                _bigDictionaryTransaction29.Count +
                _bigDictionaryTransaction30.Count +
                _bigDictionaryTransaction31.Count +
                _bigDictionaryTransaction32.Count +
                _bigDictionaryTransaction33.Count +
                _bigDictionaryTransaction34.Count +
                _bigDictionaryTransaction35.Count +
                _bigDictionaryTransaction36.Count +
                _bigDictionaryTransaction37.Count +
                _bigDictionaryTransaction38.Count +
                _bigDictionaryTransaction39.Count +
                _bigDictionaryTransaction40.Count +
                _bigDictionaryTransaction41.Count +
                _bigDictionaryTransaction42.Count +
                _bigDictionaryTransaction43.Count +
                _bigDictionaryTransaction44.Count +
                _bigDictionaryTransaction45.Count +
                _bigDictionaryTransaction46.Count +
                _bigDictionaryTransaction47.Count +
                _bigDictionaryTransaction48.Count +
                _bigDictionaryTransaction49.Count +
                _bigDictionaryTransaction50.Count +
                _bigDictionaryTransaction51.Count +
                _bigDictionaryTransaction52.Count +
                _bigDictionaryTransaction53.Count +
                _bigDictionaryTransaction54.Count +
                _bigDictionaryTransaction55.Count +
                _bigDictionaryTransaction56.Count +
                _bigDictionaryTransaction57.Count +
                _bigDictionaryTransaction58.Count +
                _bigDictionaryTransaction59.Count +
                _bigDictionaryTransaction60.Count +
                _bigDictionaryTransaction61.Count +
                _bigDictionaryTransaction62.Count +
                _bigDictionaryTransaction63.Count +
                _bigDictionaryTransaction64.Count +
                _bigDictionaryTransaction65.Count +
                _bigDictionaryTransaction66.Count +
                _bigDictionaryTransaction67.Count +
                _bigDictionaryTransaction68.Count +
                _bigDictionaryTransaction69.Count +
                _bigDictionaryTransaction70.Count +
                _bigDictionaryTransaction71.Count +
                _bigDictionaryTransaction72.Count +
                _bigDictionaryTransaction73.Count +
                _bigDictionaryTransaction74.Count +
                _bigDictionaryTransaction75.Count +
                _bigDictionaryTransaction76.Count +
                _bigDictionaryTransaction77.Count +
                _bigDictionaryTransaction78.Count +
                _bigDictionaryTransaction79.Count +
                _bigDictionaryTransaction80.Count +
                _bigDictionaryTransaction81.Count +
                _bigDictionaryTransaction82.Count +
                _bigDictionaryTransaction83.Count +
                _bigDictionaryTransaction84.Count +
                _bigDictionaryTransaction85.Count +
                _bigDictionaryTransaction86.Count +
                _bigDictionaryTransaction87.Count +
                _bigDictionaryTransaction88.Count +
                _bigDictionaryTransaction89.Count +
                _bigDictionaryTransaction90.Count +
                _bigDictionaryTransaction91.Count +
                _bigDictionaryTransaction92.Count +
                _bigDictionaryTransaction93.Count +
                _bigDictionaryTransaction94.Count +
                _bigDictionaryTransaction95.Count +
                _bigDictionaryTransaction96.Count +
                _bigDictionaryTransaction97.Count +
                _bigDictionaryTransaction98.Count +
                _bigDictionaryTransaction99.Count +
                _bigDictionaryTransaction100.Count;
        }


        /// <summary>
        /// Retrieve total memory transaction saved.
        /// </summary>
        /// <returns></returns>
        public long CountTransactionOnActiveMemory()
        {
            return _bigDictionaryTransaction1.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction2.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction3.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction4.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction5.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction6.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction7.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction8.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction9.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction10.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction11.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction12.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction13.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction14.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction15.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction16.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction17.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction18.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction19.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction20.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction21.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction22.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction23.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction24.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction25.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction26.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction27.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction28.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction29.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction30.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction31.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction32.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction33.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction34.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction35.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction36.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction40.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction41.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction42.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction43.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction37.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction38.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction39.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction44.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction45.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction46.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction47.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction48.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction49.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction50.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction51.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction52.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction53.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction54.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction55.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction56.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction57.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction58.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction59.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction60.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction61.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction62.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction63.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction64.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction65.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction66.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction67.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction68.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction69.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction70.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction71.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction72.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction73.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction74.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction75.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction76.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction77.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction78.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction79.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction80.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction81.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction82.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction83.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction84.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction85.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction86.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction87.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction88.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction89.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction90.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction91.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction92.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction93.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction94.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction95.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction96.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction97.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction98.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction99.Count(x => !x.Value.IsEmpty) +
                _bigDictionaryTransaction100.Count(x => !x.Value.IsEmpty);
        }


        /// <summary>
        /// Check if the transaction exist.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ContainsKey(long id)
        {
            if (_bigDictionaryTransaction1.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction2.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction3.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction4.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction5.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction6.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction7.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction8.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction9.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction10.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction11.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction12.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction13.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction14.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction15.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction16.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction17.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction18.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction19.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction20.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction21.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction22.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction23.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction24.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction25.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction26.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction27.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction28.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction29.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction30.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction31.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction32.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction33.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction34.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction35.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction36.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction37.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction38.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction39.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction40.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction41.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction42.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction43.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction44.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction45.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction46.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction47.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction48.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction49.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction50.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction51.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction52.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction53.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction54.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction55.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction56.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction57.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction58.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction59.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction60.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction61.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction62.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction63.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction64.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction65.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction66.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction67.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction68.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction69.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction70.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction71.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction72.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction73.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction74.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction75.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction76.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction77.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction78.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction79.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction80.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction81.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction82.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction83.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction84.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction85.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction86.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction87.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction88.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction89.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction90.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction91.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction92.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction93.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction94.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction95.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction96.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction97.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction98.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction99.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction100.ContainsKey(id))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if a the transaction is on the active memory.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ContainsMemory(long id)
        {
            if (_bigDictionaryTransaction1.ContainsKey(id))
            {
                return !_bigDictionaryTransaction1[id].IsEmpty;
            }
            if (_bigDictionaryTransaction2.ContainsKey(id))
            {
                return !_bigDictionaryTransaction2[id].IsEmpty;
            }
            if (_bigDictionaryTransaction3.ContainsKey(id))
            {
                return !_bigDictionaryTransaction3[id].IsEmpty;
            }
            if (_bigDictionaryTransaction4.ContainsKey(id))
            {
                return !_bigDictionaryTransaction4[id].IsEmpty;
            }
            if (_bigDictionaryTransaction5.ContainsKey(id))
            {
                return !_bigDictionaryTransaction5[id].IsEmpty;
            }
            if (_bigDictionaryTransaction6.ContainsKey(id))
            {
                return !_bigDictionaryTransaction6[id].IsEmpty;
            }
            if (_bigDictionaryTransaction7.ContainsKey(id))
            {
                return !_bigDictionaryTransaction7[id].IsEmpty;
            }
            if (_bigDictionaryTransaction8.ContainsKey(id))
            {
                return !_bigDictionaryTransaction8[id].IsEmpty;
            }
            if (_bigDictionaryTransaction9.ContainsKey(id))
            {
                return !_bigDictionaryTransaction9[id].IsEmpty;
            }
            if (_bigDictionaryTransaction10.ContainsKey(id))
            {
                return !_bigDictionaryTransaction10[id].IsEmpty;
            }
            if (_bigDictionaryTransaction11.ContainsKey(id))
            {
                return !_bigDictionaryTransaction11[id].IsEmpty;
            }
            if (_bigDictionaryTransaction12.ContainsKey(id))
            {
                return !_bigDictionaryTransaction12[id].IsEmpty;
            }
            if (_bigDictionaryTransaction13.ContainsKey(id))
            {
                return !_bigDictionaryTransaction13[id].IsEmpty;
            }
            if (_bigDictionaryTransaction14.ContainsKey(id))
            {
                return !_bigDictionaryTransaction14[id].IsEmpty;
            }
            if (_bigDictionaryTransaction15.ContainsKey(id))
            {
                return !_bigDictionaryTransaction15[id].IsEmpty;
            }
            if (_bigDictionaryTransaction16.ContainsKey(id))
            {
                return !_bigDictionaryTransaction16[id].IsEmpty;
            }
            if (_bigDictionaryTransaction17.ContainsKey(id))
            {
                return !_bigDictionaryTransaction17[id].IsEmpty;
            }
            if (_bigDictionaryTransaction18.ContainsKey(id))
            {
                return !_bigDictionaryTransaction18[id].IsEmpty;
            }
            if (_bigDictionaryTransaction19.ContainsKey(id))
            {
                return !_bigDictionaryTransaction19[id].IsEmpty;
            }
            if (_bigDictionaryTransaction20.ContainsKey(id))
            {
                return !_bigDictionaryTransaction20[id].IsEmpty;
            }
            if (_bigDictionaryTransaction21.ContainsKey(id))
            {
                return !_bigDictionaryTransaction21[id].IsEmpty;
            }
            if (_bigDictionaryTransaction22.ContainsKey(id))
            {
                return !_bigDictionaryTransaction22[id].IsEmpty;
            }
            if (_bigDictionaryTransaction23.ContainsKey(id))
            {
                return !_bigDictionaryTransaction23[id].IsEmpty;
            }
            if (_bigDictionaryTransaction24.ContainsKey(id))
            {
                return !_bigDictionaryTransaction24[id].IsEmpty;
            }
            if (_bigDictionaryTransaction25.ContainsKey(id))
            {
                return !_bigDictionaryTransaction25[id].IsEmpty;
            }
            if (_bigDictionaryTransaction26.ContainsKey(id))
            {
                return !_bigDictionaryTransaction26[id].IsEmpty;
            }
            if (_bigDictionaryTransaction27.ContainsKey(id))
            {
                return !_bigDictionaryTransaction27[id].IsEmpty;
            }
            if (_bigDictionaryTransaction28.ContainsKey(id))
            {
                return !_bigDictionaryTransaction28[id].IsEmpty;
            }
            if (_bigDictionaryTransaction29.ContainsKey(id))
            {
                return !_bigDictionaryTransaction29[id].IsEmpty;
            }
            if (_bigDictionaryTransaction30.ContainsKey(id))
            {
                return !_bigDictionaryTransaction30[id].IsEmpty;
            }
            if (_bigDictionaryTransaction31.ContainsKey(id))
            {
                return !_bigDictionaryTransaction31[id].IsEmpty;
            }
            if (_bigDictionaryTransaction32.ContainsKey(id))
            {
                return !_bigDictionaryTransaction32[id].IsEmpty;
            }
            if (_bigDictionaryTransaction33.ContainsKey(id))
            {
                return !_bigDictionaryTransaction33[id].IsEmpty;
            }
            if (_bigDictionaryTransaction34.ContainsKey(id))
            {
                return !_bigDictionaryTransaction34[id].IsEmpty;
            }
            if (_bigDictionaryTransaction35.ContainsKey(id))
            {
                return !_bigDictionaryTransaction35[id].IsEmpty;
            }
            if (_bigDictionaryTransaction36.ContainsKey(id))
            {
                return !_bigDictionaryTransaction36[id].IsEmpty;
            }
            if (_bigDictionaryTransaction37.ContainsKey(id))
            {
                return !_bigDictionaryTransaction37[id].IsEmpty;
            }
            if (_bigDictionaryTransaction38.ContainsKey(id))
            {
                return !_bigDictionaryTransaction38[id].IsEmpty;
            }
            if (_bigDictionaryTransaction39.ContainsKey(id))
            {
                return !_bigDictionaryTransaction39[id].IsEmpty;
            }
            if (_bigDictionaryTransaction40.ContainsKey(id))
            {
                return !_bigDictionaryTransaction40[id].IsEmpty;
            }
            if (_bigDictionaryTransaction41.ContainsKey(id))
            {
                return !_bigDictionaryTransaction41[id].IsEmpty;
            }
            if (_bigDictionaryTransaction42.ContainsKey(id))
            {
                return !_bigDictionaryTransaction42[id].IsEmpty;
            }
            if (_bigDictionaryTransaction43.ContainsKey(id))
            {
                return !_bigDictionaryTransaction43[id].IsEmpty;
            }
            if (_bigDictionaryTransaction44.ContainsKey(id))
            {
                return !_bigDictionaryTransaction44[id].IsEmpty;
            }
            if (_bigDictionaryTransaction45.ContainsKey(id))
            {
                return !_bigDictionaryTransaction45[id].IsEmpty;
            }
            if (_bigDictionaryTransaction46.ContainsKey(id))
            {
                return !_bigDictionaryTransaction46[id].IsEmpty;
            }
            if (_bigDictionaryTransaction47.ContainsKey(id))
            {
                return !_bigDictionaryTransaction47[id].IsEmpty;
            }
            if (_bigDictionaryTransaction48.ContainsKey(id))
            {
                return !_bigDictionaryTransaction48[id].IsEmpty;
            }
            if (_bigDictionaryTransaction49.ContainsKey(id))
            {
                return !_bigDictionaryTransaction49[id].IsEmpty;
            }
            if (_bigDictionaryTransaction50.ContainsKey(id))
            {
                return !_bigDictionaryTransaction50[id].IsEmpty;
            }
            if (_bigDictionaryTransaction51.ContainsKey(id))
            {
                return !_bigDictionaryTransaction51[id].IsEmpty;
            }
            if (_bigDictionaryTransaction52.ContainsKey(id))
            {
                return !_bigDictionaryTransaction52[id].IsEmpty;
            }
            if (_bigDictionaryTransaction53.ContainsKey(id))
            {
                return !_bigDictionaryTransaction53[id].IsEmpty;
            }
            if (_bigDictionaryTransaction54.ContainsKey(id))
            {
                return !_bigDictionaryTransaction54[id].IsEmpty;
            }
            if (_bigDictionaryTransaction55.ContainsKey(id))
            {
                return !_bigDictionaryTransaction55[id].IsEmpty;
            }
            if (_bigDictionaryTransaction56.ContainsKey(id))
            {
                return !_bigDictionaryTransaction56[id].IsEmpty;
            }
            if (_bigDictionaryTransaction57.ContainsKey(id))
            {
                return !_bigDictionaryTransaction57[id].IsEmpty;
            }
            if (_bigDictionaryTransaction58.ContainsKey(id))
            {
                return !_bigDictionaryTransaction58[id].IsEmpty;
            }
            if (_bigDictionaryTransaction59.ContainsKey(id))
            {
                return !_bigDictionaryTransaction59[id].IsEmpty;
            }
            if (_bigDictionaryTransaction60.ContainsKey(id))
            {
                return !_bigDictionaryTransaction60[id].IsEmpty;
            }
            if (_bigDictionaryTransaction61.ContainsKey(id))
            {
                return !_bigDictionaryTransaction61[id].IsEmpty;
            }
            if (_bigDictionaryTransaction62.ContainsKey(id))
            {
                return !_bigDictionaryTransaction62[id].IsEmpty;
            }
            if (_bigDictionaryTransaction63.ContainsKey(id))
            {
                return !_bigDictionaryTransaction63[id].IsEmpty;
            }
            if (_bigDictionaryTransaction64.ContainsKey(id))
            {
                return !_bigDictionaryTransaction64[id].IsEmpty;
            }
            if (_bigDictionaryTransaction65.ContainsKey(id))
            {
                return !_bigDictionaryTransaction65[id].IsEmpty;
            }
            if (_bigDictionaryTransaction66.ContainsKey(id))
            {
                return !_bigDictionaryTransaction66[id].IsEmpty;
            }
            if (_bigDictionaryTransaction67.ContainsKey(id))
            {
                return !_bigDictionaryTransaction67[id].IsEmpty;
            }
            if (_bigDictionaryTransaction68.ContainsKey(id))
            {
                return !_bigDictionaryTransaction68[id].IsEmpty;
            }
            if (_bigDictionaryTransaction69.ContainsKey(id))
            {
                return !_bigDictionaryTransaction69[id].IsEmpty;
            }
            if (_bigDictionaryTransaction70.ContainsKey(id))
            {
                return !_bigDictionaryTransaction70[id].IsEmpty;
            }
            if (_bigDictionaryTransaction71.ContainsKey(id))
            {
                return !_bigDictionaryTransaction71[id].IsEmpty;
            }
            if (_bigDictionaryTransaction72.ContainsKey(id))
            {
                return !_bigDictionaryTransaction72[id].IsEmpty;
            }
            if (_bigDictionaryTransaction73.ContainsKey(id))
            {
                return !_bigDictionaryTransaction73[id].IsEmpty;
            }
            if (_bigDictionaryTransaction74.ContainsKey(id))
            {
                return !_bigDictionaryTransaction74[id].IsEmpty;
            }
            if (_bigDictionaryTransaction75.ContainsKey(id))
            {
                return !_bigDictionaryTransaction75[id].IsEmpty;
            }
            if (_bigDictionaryTransaction76.ContainsKey(id))
            {
                return !_bigDictionaryTransaction76[id].IsEmpty;
            }
            if (_bigDictionaryTransaction77.ContainsKey(id))
            {
                return !_bigDictionaryTransaction77[id].IsEmpty;
            }
            if (_bigDictionaryTransaction78.ContainsKey(id))
            {
                return !_bigDictionaryTransaction78[id].IsEmpty;
            }
            if (_bigDictionaryTransaction79.ContainsKey(id))
            {
                return !_bigDictionaryTransaction79[id].IsEmpty;
            }
            if (_bigDictionaryTransaction80.ContainsKey(id))
            {
                return !_bigDictionaryTransaction80[id].IsEmpty;
            }
            if (_bigDictionaryTransaction81.ContainsKey(id))
            {
                return !_bigDictionaryTransaction81[id].IsEmpty;
            }
            if (_bigDictionaryTransaction82.ContainsKey(id))
            {
                return !_bigDictionaryTransaction82[id].IsEmpty;
            }
            if (_bigDictionaryTransaction83.ContainsKey(id))
            {
                return !_bigDictionaryTransaction83[id].IsEmpty;
            }
            if (_bigDictionaryTransaction84.ContainsKey(id))
            {
                return !_bigDictionaryTransaction84[id].IsEmpty;
            }
            if (_bigDictionaryTransaction85.ContainsKey(id))
            {
                return !_bigDictionaryTransaction85[id].IsEmpty;
            }
            if (_bigDictionaryTransaction86.ContainsKey(id))
            {
                return !_bigDictionaryTransaction86[id].IsEmpty;
            }
            if (_bigDictionaryTransaction87.ContainsKey(id))
            {
                return !_bigDictionaryTransaction87[id].IsEmpty;
            }
            if (_bigDictionaryTransaction88.ContainsKey(id))
            {
                return !_bigDictionaryTransaction88[id].IsEmpty;
            }
            if (_bigDictionaryTransaction89.ContainsKey(id))
            {
                return !_bigDictionaryTransaction89[id].IsEmpty;
            }
            if (_bigDictionaryTransaction90.ContainsKey(id))
            {
                return !_bigDictionaryTransaction90[id].IsEmpty;
            }
            if (_bigDictionaryTransaction91.ContainsKey(id))
            {
                return !_bigDictionaryTransaction91[id].IsEmpty;
            }
            if (_bigDictionaryTransaction92.ContainsKey(id))
            {
                return !_bigDictionaryTransaction92[id].IsEmpty;
            }
            if (_bigDictionaryTransaction93.ContainsKey(id))
            {
                return !_bigDictionaryTransaction93[id].IsEmpty;
            }
            if (_bigDictionaryTransaction94.ContainsKey(id))
            {
                return !_bigDictionaryTransaction94[id].IsEmpty;
            }
            if (_bigDictionaryTransaction95.ContainsKey(id))
            {
                return !_bigDictionaryTransaction95[id].IsEmpty;
            }
            if (_bigDictionaryTransaction96.ContainsKey(id))
            {
                return !_bigDictionaryTransaction96[id].IsEmpty;
            }
            if (_bigDictionaryTransaction97.ContainsKey(id))
            {
                return !_bigDictionaryTransaction97[id].IsEmpty;
            }
            if (_bigDictionaryTransaction98.ContainsKey(id))
            {
                return !_bigDictionaryTransaction98[id].IsEmpty;
            }
            if (_bigDictionaryTransaction99.ContainsKey(id))
            {
                return !_bigDictionaryTransaction99[id].IsEmpty;
            }
            if (_bigDictionaryTransaction100.ContainsKey(id))
            {
                return !_bigDictionaryTransaction100[id].IsEmpty;
            }

            return false;
        }

        /// <summary>
        /// Check if the transaction as expired of the active memory
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool TransactionExpired(long id, int delay)
        {
            if (_bigDictionaryTransaction1.ContainsKey(id))
            {
                return _bigDictionaryTransaction1[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction2.ContainsKey(id))
            {
                return _bigDictionaryTransaction2[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction3.ContainsKey(id))
            {
                return _bigDictionaryTransaction3[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction4.ContainsKey(id))
            {
                return _bigDictionaryTransaction4[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction5.ContainsKey(id))
            {
                return _bigDictionaryTransaction5[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction6.ContainsKey(id))
            {
                return _bigDictionaryTransaction6[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction7.ContainsKey(id))
            {
                return _bigDictionaryTransaction7[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction8.ContainsKey(id))
            {
                return _bigDictionaryTransaction8[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction9.ContainsKey(id))
            {
                return _bigDictionaryTransaction9[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction10.ContainsKey(id))
            {
                return _bigDictionaryTransaction10[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction11.ContainsKey(id))
            {
                return _bigDictionaryTransaction11[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction12.ContainsKey(id))
            {
                return _bigDictionaryTransaction12[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction13.ContainsKey(id))
            {
                return _bigDictionaryTransaction13[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction14.ContainsKey(id))
            {
                return _bigDictionaryTransaction14[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction15.ContainsKey(id))
            {
                return _bigDictionaryTransaction15[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction16.ContainsKey(id))
            {
                return _bigDictionaryTransaction16[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction17.ContainsKey(id))
            {
                return _bigDictionaryTransaction17[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction18.ContainsKey(id))
            {
                return _bigDictionaryTransaction18[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction19.ContainsKey(id))
            {
                return _bigDictionaryTransaction19[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction20.ContainsKey(id))
            {
                return _bigDictionaryTransaction20[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction21.ContainsKey(id))
            {
                return _bigDictionaryTransaction21[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction22.ContainsKey(id))
            {
                return _bigDictionaryTransaction22[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction23.ContainsKey(id))
            {
                return _bigDictionaryTransaction23[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction24.ContainsKey(id))
            {
                return _bigDictionaryTransaction24[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction25.ContainsKey(id))
            {
                return _bigDictionaryTransaction25[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction26.ContainsKey(id))
            {
                return _bigDictionaryTransaction26[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction27.ContainsKey(id))
            {
                return _bigDictionaryTransaction27[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction28.ContainsKey(id))
            {
                return _bigDictionaryTransaction28[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction29.ContainsKey(id))
            {
                return _bigDictionaryTransaction29[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction30.ContainsKey(id))
            {
                return _bigDictionaryTransaction30[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction31.ContainsKey(id))
            {
                return _bigDictionaryTransaction31[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction32.ContainsKey(id))
            {
                return _bigDictionaryTransaction32[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction33.ContainsKey(id))
            {
                return _bigDictionaryTransaction33[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction34.ContainsKey(id))
            {
                return _bigDictionaryTransaction34[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction35.ContainsKey(id))
            {
                return _bigDictionaryTransaction35[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction36.ContainsKey(id))
            {
                return _bigDictionaryTransaction36[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction37.ContainsKey(id))
            {
                return _bigDictionaryTransaction37[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction38.ContainsKey(id))
            {
                return _bigDictionaryTransaction38[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction39.ContainsKey(id))
            {
                return _bigDictionaryTransaction39[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction40.ContainsKey(id))
            {
                return _bigDictionaryTransaction40[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction41.ContainsKey(id))
            {
                return _bigDictionaryTransaction41[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction42.ContainsKey(id))
            {
                return _bigDictionaryTransaction42[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction43.ContainsKey(id))
            {
                return _bigDictionaryTransaction43[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction44.ContainsKey(id))
            {
                return _bigDictionaryTransaction44[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction45.ContainsKey(id))
            {
                return _bigDictionaryTransaction45[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction46.ContainsKey(id))
            {
                return _bigDictionaryTransaction46[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction47.ContainsKey(id))
            {
                return _bigDictionaryTransaction47[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction48.ContainsKey(id))
            {
                return _bigDictionaryTransaction48[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction49.ContainsKey(id))
            {
                return _bigDictionaryTransaction49[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction50.ContainsKey(id))
            {
                return _bigDictionaryTransaction50[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction51.ContainsKey(id))
            {
                return _bigDictionaryTransaction51[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction52.ContainsKey(id))
            {
                return _bigDictionaryTransaction52[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction53.ContainsKey(id))
            {
                return _bigDictionaryTransaction53[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction54.ContainsKey(id))
            {
                return _bigDictionaryTransaction54[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction55.ContainsKey(id))
            {
                return _bigDictionaryTransaction55[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction56.ContainsKey(id))
            {
                return _bigDictionaryTransaction56[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction57.ContainsKey(id))
            {
                return _bigDictionaryTransaction57[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction58.ContainsKey(id))
            {
                return _bigDictionaryTransaction58[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction59.ContainsKey(id))
            {
                return _bigDictionaryTransaction59[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction60.ContainsKey(id))
            {
                return _bigDictionaryTransaction60[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction61.ContainsKey(id))
            {
                return _bigDictionaryTransaction61[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction62.ContainsKey(id))
            {
                return _bigDictionaryTransaction62[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction63.ContainsKey(id))
            {
                return _bigDictionaryTransaction63[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction64.ContainsKey(id))
            {
                return _bigDictionaryTransaction64[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction65.ContainsKey(id))
            {
                return _bigDictionaryTransaction65[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction66.ContainsKey(id))
            {
                return _bigDictionaryTransaction66[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction67.ContainsKey(id))
            {
                return _bigDictionaryTransaction67[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction68.ContainsKey(id))
            {
                return _bigDictionaryTransaction68[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction69.ContainsKey(id))
            {
                return _bigDictionaryTransaction69[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction70.ContainsKey(id))
            {
                return _bigDictionaryTransaction70[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction71.ContainsKey(id))
            {
                return _bigDictionaryTransaction71[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction72.ContainsKey(id))
            {
                return _bigDictionaryTransaction72[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction73.ContainsKey(id))
            {
                return _bigDictionaryTransaction73[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction74.ContainsKey(id))
            {
                return _bigDictionaryTransaction74[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction75.ContainsKey(id))
            {
                return _bigDictionaryTransaction75[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction76.ContainsKey(id))
            {
                return _bigDictionaryTransaction76[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction77.ContainsKey(id))
            {
                return _bigDictionaryTransaction77[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction78.ContainsKey(id))
            {
                return _bigDictionaryTransaction78[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction79.ContainsKey(id))
            {
                return _bigDictionaryTransaction79[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction80.ContainsKey(id))
            {
                return _bigDictionaryTransaction80[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction81.ContainsKey(id))
            {
                return _bigDictionaryTransaction81[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction82.ContainsKey(id))
            {
                return _bigDictionaryTransaction82[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction83.ContainsKey(id))
            {
                return _bigDictionaryTransaction83[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction84.ContainsKey(id))
            {
                return _bigDictionaryTransaction84[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction85.ContainsKey(id))
            {
                return _bigDictionaryTransaction85[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction86.ContainsKey(id))
            {
                return _bigDictionaryTransaction86[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction87.ContainsKey(id))
            {
                return _bigDictionaryTransaction87[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction88.ContainsKey(id))
            {
                return _bigDictionaryTransaction88[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction89.ContainsKey(id))
            {
                return _bigDictionaryTransaction89[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction90.ContainsKey(id))
            {
                return _bigDictionaryTransaction90[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction91.ContainsKey(id))
            {
                return _bigDictionaryTransaction91[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction92.ContainsKey(id))
            {
                return _bigDictionaryTransaction92[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction93.ContainsKey(id))
            {
                return _bigDictionaryTransaction93[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction94.ContainsKey(id))
            {
                return _bigDictionaryTransaction94[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction95.ContainsKey(id))
            {
                return _bigDictionaryTransaction95[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction96.ContainsKey(id))
            {
                return _bigDictionaryTransaction96[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction97.ContainsKey(id))
            {
                return _bigDictionaryTransaction97[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction98.ContainsKey(id))
            {
                return _bigDictionaryTransaction98[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction99.ContainsKey(id))
            {
                return _bigDictionaryTransaction99[id].IsExpired(delay);
            }
            if (_bigDictionaryTransaction100.ContainsKey(id))
            {
                return _bigDictionaryTransaction100[id].IsExpired(delay);
            }

            return false;
        }

        /// <summary>
        /// Clear dictionnary
        /// </summary>
        public void Clear()
        {
            _bigDictionaryTransaction1.Clear();
            _bigDictionaryTransaction2.Clear();
            _bigDictionaryTransaction3.Clear();
            _bigDictionaryTransaction4.Clear();
            _bigDictionaryTransaction5.Clear();
            _bigDictionaryTransaction6.Clear();
            _bigDictionaryTransaction7.Clear();
            _bigDictionaryTransaction8.Clear();
            _bigDictionaryTransaction9.Clear();
            _bigDictionaryTransaction10.Clear();
            _bigDictionaryTransaction11.Clear();
            _bigDictionaryTransaction12.Clear();
            _bigDictionaryTransaction13.Clear();
            _bigDictionaryTransaction14.Clear();
            _bigDictionaryTransaction15.Clear();
            _bigDictionaryTransaction16.Clear();
            _bigDictionaryTransaction17.Clear();
            _bigDictionaryTransaction18.Clear();
            _bigDictionaryTransaction19.Clear();
            _bigDictionaryTransaction20.Clear();
            _bigDictionaryTransaction21.Clear();
            _bigDictionaryTransaction22.Clear();
            _bigDictionaryTransaction23.Clear();
            _bigDictionaryTransaction24.Clear();
            _bigDictionaryTransaction25.Clear();
            _bigDictionaryTransaction26.Clear();
            _bigDictionaryTransaction27.Clear();
            _bigDictionaryTransaction28.Clear();
            _bigDictionaryTransaction29.Clear();
            _bigDictionaryTransaction30.Clear();
            _bigDictionaryTransaction31.Clear();
            _bigDictionaryTransaction32.Clear();
            _bigDictionaryTransaction33.Clear();
            _bigDictionaryTransaction34.Clear();
            _bigDictionaryTransaction35.Clear();
            _bigDictionaryTransaction36.Clear();
            _bigDictionaryTransaction37.Clear();
            _bigDictionaryTransaction38.Clear();
            _bigDictionaryTransaction39.Clear();
            _bigDictionaryTransaction40.Clear();
            _bigDictionaryTransaction41.Clear();
            _bigDictionaryTransaction42.Clear();
            _bigDictionaryTransaction43.Clear();
            _bigDictionaryTransaction44.Clear();
            _bigDictionaryTransaction45.Clear();
            _bigDictionaryTransaction46.Clear();
            _bigDictionaryTransaction47.Clear();
            _bigDictionaryTransaction48.Clear();
            _bigDictionaryTransaction49.Clear();
            _bigDictionaryTransaction50.Clear();
            _bigDictionaryTransaction51.Clear();
            _bigDictionaryTransaction52.Clear();
            _bigDictionaryTransaction53.Clear();
            _bigDictionaryTransaction54.Clear();
            _bigDictionaryTransaction55.Clear();
            _bigDictionaryTransaction56.Clear();
            _bigDictionaryTransaction57.Clear();
            _bigDictionaryTransaction58.Clear();
            _bigDictionaryTransaction59.Clear();
            _bigDictionaryTransaction60.Clear();
            _bigDictionaryTransaction61.Clear();
            _bigDictionaryTransaction62.Clear();
            _bigDictionaryTransaction63.Clear();
            _bigDictionaryTransaction64.Clear();
            _bigDictionaryTransaction65.Clear();
            _bigDictionaryTransaction66.Clear();
            _bigDictionaryTransaction67.Clear();
            _bigDictionaryTransaction68.Clear();
            _bigDictionaryTransaction69.Clear();
            _bigDictionaryTransaction70.Clear();
            _bigDictionaryTransaction71.Clear();
            _bigDictionaryTransaction72.Clear();
            _bigDictionaryTransaction73.Clear();
            _bigDictionaryTransaction74.Clear();
            _bigDictionaryTransaction75.Clear();
            _bigDictionaryTransaction76.Clear();
            _bigDictionaryTransaction77.Clear();
            _bigDictionaryTransaction78.Clear();
            _bigDictionaryTransaction79.Clear();
            _bigDictionaryTransaction80.Clear();
            _bigDictionaryTransaction81.Clear();
            _bigDictionaryTransaction82.Clear();
            _bigDictionaryTransaction83.Clear();
            _bigDictionaryTransaction84.Clear();
            _bigDictionaryTransaction85.Clear();
            _bigDictionaryTransaction86.Clear();
            _bigDictionaryTransaction87.Clear();
            _bigDictionaryTransaction88.Clear();
            _bigDictionaryTransaction89.Clear();
            _bigDictionaryTransaction90.Clear();
            _bigDictionaryTransaction91.Clear();
            _bigDictionaryTransaction92.Clear();
            _bigDictionaryTransaction93.Clear();
            _bigDictionaryTransaction94.Clear();
            _bigDictionaryTransaction95.Clear();
            _bigDictionaryTransaction96.Clear();
            _bigDictionaryTransaction97.Clear();
            _bigDictionaryTransaction98.Clear();
            _bigDictionaryTransaction99.Clear();
            _bigDictionaryTransaction100.Clear();
        }
    }
}