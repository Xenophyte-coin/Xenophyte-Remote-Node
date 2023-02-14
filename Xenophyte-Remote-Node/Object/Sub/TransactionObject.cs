

using Xenophyte_RemoteNode.Utils;

namespace Xenophyte_RemoteNode.Object.Sub
{
    public class TransactionObject
    {
        public long Id;
        private string _transactionData;
        public long Position;


        public TransactionObject(long id, string transactionData, long position)
        {
            Id = id;
            _transactionData = transactionData;
            Position = position;
            LastGetTimestamp = ClassUtilsNode.GetCurrentTimestampInSecond();
        }

        public string TransactionData
        {
            get 
            {
                if (!IsEmpty)
                    LastGetTimestamp = ClassUtilsNode.GetCurrentTimestampInSecond();

                return _transactionData; 
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && _transactionData?.Length > 0)
                    LastGetTimestamp = ClassUtilsNode.GetCurrentTimestampInSecond();

                _transactionData = value;
            } 
        }

        public bool IsEmpty => string.IsNullOrEmpty(_transactionData) || _transactionData.Length == 0;

        public bool IsExpired(int delay) => !IsEmpty && LastGetTimestamp + delay < ClassUtilsNode.GetCurrentTimestampInSecond();



        public long LastGetTimestamp;
    }
}
