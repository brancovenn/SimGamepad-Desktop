using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimGamepad.Services
{
    public class UdpReceiver
    {
        private UdpClient _client;
        public event Action<string> OnJson;

        public void Start(int port)
        {
            _client = new UdpClient(port);

            Task.Run(async () =>
            {
                while (true)
                {
                    var result = await _client.ReceiveAsync();
                    OnJson?.Invoke(Encoding.UTF8.GetString(result.Buffer));
                }
            });
        }

        public void Stop()
        {
            _client?.Close();
        }
    }
}
