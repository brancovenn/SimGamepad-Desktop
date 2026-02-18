using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SimGamepad.Services
{
    public class BluetoothService
    {
        private BluetoothListener _listener;
        public event Action<string> OnJson;

        public void Start()
        {
            _listener = new BluetoothListener(InTheHand.Net.Bluetooth.BluetoothService.SerialPort);
            _listener.Start();

            Task.Run(() =>
            {
                try
                {
                    var client = _listener.AcceptBluetoothClient();
                    using (var reader = new StreamReader(client.GetStream()))
                    {
                        while (true)
                        {
                            var line = reader.ReadLine();
                            if (line != null)
                                OnJson?.Invoke(line);
                        }
                    }
                }
                catch { }
            });
        }

        public void Stop() => _listener?.Stop();
    }
}
