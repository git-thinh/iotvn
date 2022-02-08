using StackExchange.Redis;

namespace Iotvn
{
    public class oRedisInerface
    {
        public bool isConnected { set; get; }
        public bool isFull { set; get; }

        public IServer Server { set; get; }
        public IDatabase DB { set; get; }
    }

    public class oRedisConfig
    {
        public string name { set; get; }
        public string description { set; get; }
        public string data_type { set; get; }
        public string email { set; get; }
        public string host { set; get; }
        public string password { set; get; }
        public int port { set; get; }

        public oRedisConfig() { }
        public oRedisConfig(string host, int port, string pass)
        {
            this.host = host;
            this.password = pass;
            this.port = port;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1},allowAdmin=true,password={2},abortConnect=false,defaultDatabase=0,syncTimeout=5000", this.host, this.port, this.password);
        }
    }
}
