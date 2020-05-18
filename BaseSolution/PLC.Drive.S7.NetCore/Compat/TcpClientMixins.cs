using System.Net.Sockets;

namespace PLC.Drive.S7.NetCore.Compat
{
    public static class TcpClientMixins
    {
        public static void Close(this TcpClient tcpClient)
        {
            tcpClient.Dispose();
        }

        public static void Connect(this TcpClient tcpClient, string host, int port)
        {
            tcpClient.ConnectAsync(host, port).GetAwaiter().GetResult();
        }
    }
}
