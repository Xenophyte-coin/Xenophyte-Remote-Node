namespace Xenophyte_RemoteNode.Setting
{
    public class ClassRemoteNodeSetting
    {
        public string wallet_address;
        public bool enable_public_mode;
        public bool enable_api_http;
        public int api_http_port;
        public int log_level;
        public bool write_log;
        public bool enable_filtering_system;
        public string chain_filtering_system;
        public string name_filtering_system;
        public bool enable_save_sync_raw = true;
        public bool enable_disk_cache_mode = true;
        public int max_delay_transaction_memory = 3600;
        public long max_keep_alive_transaction_memory = 1_000_000;
    }
}
