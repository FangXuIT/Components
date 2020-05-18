using Opc.Ua;
using OpcUaHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpcUaClientTest
{
    public class OpcUaOperater
    {
        public OpcUaClient client { set; get; }

        public OpcUaOperater()
        {
            client = new OpcUaClient();
            client.ReconnectPeriod = 10;
            client.UserIdentity = new UserIdentity("Administrator", "123456");            
            client.ConnectComplete += Client_ConnectComplete;
        }

        public async Task ConnectServer()
        {
            await client.ConnectServer("opc.tcp://192.168.11.30:50000");
        }

        private void Client_ConnectComplete(object sender, EventArgs e)
        {
            Console.WriteLine("{0} ConnectCompleted",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            
            
        }

        public async Task ReadTagsAsync()
        {
            //var preTagName = "ns=2;s=QGD.S71500.";
            var preTagName = "ns=2;s=Tag.";
            if (client.Connected)
            {
                try
                {
                    var iVal = await client.ReadNodeAsync<Int64>(string.Format("{0}Year", preTagName));
                    Console.WriteLine(string.Format("{0}年", iVal));

                    iVal = await client.ReadNodeAsync<Int64>(string.Format("{0}Month", preTagName));
                    Console.WriteLine(string.Format("{0}月", iVal));

                    iVal = await client.ReadNodeAsync<Int64>(string.Format("{0}Day", preTagName));
                    Console.WriteLine(string.Format("{0}日", iVal));

                    iVal = await client.ReadNodeAsync<Int64>(string.Format("{0}Hour", preTagName));
                    Console.WriteLine(string.Format("{0}时", iVal));

                    iVal = await client.ReadNodeAsync<Int64>(string.Format("{0}Minute", preTagName));
                    Console.WriteLine(string.Format("{0}分", iVal));

                    iVal = await client.ReadNodeAsync<Int64>(string.Format("{0}Second", preTagName));
                    Console.WriteLine(string.Format("{0}秒", iVal));

                    var sVal = await client.ReadNodeAsync<string>(string.Format("{0}Date", preTagName));
                    Console.WriteLine(string.Format("日期：{0}", sVal));
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("DisConnected");
            }
        }

        public void Disconnect()
        {
            if (client.Connected) client.Disconnect();
        }
    }
}
