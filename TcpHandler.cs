using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HttpEdit
{
    public class TcpHandler
    {
        const int BUF_LEN = 256;

        public string RecordRequest(string target)
        {
            string[] t = target.Split(":");
            string address = t[0];
            int port = Convert.ToInt32(t[1]);

            var server = new TcpListener(IPAddress.Parse(address), port);
            server.Start();

            TcpClient client = server.AcceptTcpClient();
            string request = Recv(client.GetStream());
            client.Close();

            return request;
        }

        public string SendRequest(byte[] data, string target)
        {
            string[] t = target.Split(":");
            string ip = t[0];
            int port = Convert.ToInt32(t[1]);

            var client = new TcpClient(ip, port);

            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);
            stream.Flush();

            string response = Recv(stream);
            stream.Close();
            client.Close();

            return response;
        }

        string Recv(NetworkStream stream)
        {
            var buf = new byte[BUF_LEN];
            var data = new List<byte>();

            int recvLen;

            while ((recvLen = stream.Read(buf, 0, buf.Length)) != 0)
            {
                for (int i = 0; i < buf.Length; i++)
                {
                    if (buf[i] != 0x0)
                        data.Add(buf[i]);
                    else
                        break;
                }

                if (recvLen < buf.Length)
                    break;

                buf = new byte[BUF_LEN];
            }

            stream.Close();
            return Encoding.UTF8.GetString(data.ToArray());
        }
    }
}