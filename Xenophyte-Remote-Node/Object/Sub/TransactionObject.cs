

using Xenophyte_RemoteNode.Utils;

namespace Xenophyte_RemoteNode.Object.Sub
{
    public class TransactionObject
    {
        public long Id;
        private string _transactionData;


        public TransactionObject(long id, string transactionData)
        {
            Id = id;
            _transactionData = transactionData;
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

                _transactionData = value == null ?  string.Empty : value;
            } 
        }

        public bool IsEmpty => string.IsNullOrEmpty(_transactionData) || _transactionData.Length == 0;

        public bool IsExpired(int delay) => !IsEmpty && LastGetTimestamp + delay < ClassUtilsNode.GetCurrentTimestampInSecond();



        public long LastGetTimestamp;
    }
}
