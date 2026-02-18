using Newtonsoft.Json;
using SimGamepad.Input;
using SimGamepad.Models;
using SimGamepad.Services;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SimGamepad
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly VirtualXboxController _controller;
        private UdpReceiver _udp;
        private BluetoothService _bluetooth;
        private UsbReceiver _usb;

        public string ActiveModeText { get; set; }
        public string StatusText { get; set; }
        public ImageSource QrImage { get; set; }
        public string ConnectionString { get; set; }

        public double WifiGlowOpacity { get; set; }
        public double BluetoothGlowOpacity { get; set; }
        public double UsbGlowOpacity { get; set; }

        public Visibility WifiQrVisibility { get; set; }
        public Visibility BluetoothInfoVisibility { get; set; }
        public Visibility UsbInfoVisibility { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            StateChanged += MainWindow_StateChanged;
            DataContext = this;

            _controller = new VirtualXboxController();

            string ip = NetworkService.GetLocalIPAddress();
            ConnectionString = $"simgamepad://{ip}:5555";
            QrImage = QrService.Generate(ConnectionString);

            // DEFAULT MODE = WIFI (IDLE)
            ActiveModeText = "Wi-Fi";
            StatusText = "Idle";

            WifiGlowOpacity = 1;
            BluetoothGlowOpacity = 0;
            UsbGlowOpacity = 0;

            WifiQrVisibility = Visibility.Visible;
            BluetoothInfoVisibility = Visibility.Collapsed;
            UsbInfoVisibility = Visibility.Collapsed;
        }

        private void Wifi_Click(object sender, RoutedEventArgs e)
        {
            StopAll();

            _udp = new UdpReceiver();
            _udp.OnJson += HandleJson;
            _udp.Start(5555);

            ActiveModeText = "Wi-Fi";
            StatusText = "Waiting for connection";

            WifiGlowOpacity = 1;
            BluetoothGlowOpacity = 0;
            UsbGlowOpacity = 0;

            WifiQrVisibility = Visibility.Visible;
            BluetoothInfoVisibility = Visibility.Collapsed;
            UsbInfoVisibility = Visibility.Collapsed;

            RaiseAll();
        }

        private void Bluetooth_Click(object sender, RoutedEventArgs e)
        {
            StopAll();

            _bluetooth = new SimGamepad.Services.BluetoothService();
            _bluetooth.OnJson += HandleJson;
            _bluetooth.Start();

            ActiveModeText = "Bluetooth";
            StatusText = "Waiting for pairing";

            WifiGlowOpacity = 0;
            BluetoothGlowOpacity = 1;
            UsbGlowOpacity = 0;

            WifiQrVisibility = Visibility.Collapsed;
            BluetoothInfoVisibility = Visibility.Visible;
            UsbInfoVisibility = Visibility.Collapsed;

            RaiseAll();
        }

        private void Usb_Click(object sender, RoutedEventArgs e)
        {
            StopAll();

            _usb = new UsbReceiver();
            _usb.Start();

            ActiveModeText = "USB";
            StatusText = "Waiting for device";

            WifiGlowOpacity = 0;
            BluetoothGlowOpacity = 0;
            UsbGlowOpacity = 1;

            WifiQrVisibility = Visibility.Collapsed;
            BluetoothInfoVisibility = Visibility.Collapsed;
            UsbInfoVisibility = Visibility.Visible;

            RaiseAll();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Apply Modern Backdrop
            WindowBackdrop.ApplyBackdrop(this, WindowBackdrop.BackdropType.Acrylic); 
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
             // No-op for now
        }

        private void UpdateMaximizeIcon()
        {
            if (BtnMaximize == null) return;
            BtnMaximize.ToolTip = WindowState == WindowState.Maximized ? "Restore" : "Maximize";
        }

        // Maximizing logic might change with WindowChrome, but standard checks are okay.
        private void BrancoVenn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "https://brancovenn.com",
                    UseShellExecute = true
                });
            }
            catch { }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void StopAll()
        {
            _udp?.Stop();
            _bluetooth?.Stop();
            _usb?.Stop();
        }

        private void HandleJson(string json)
        {
            var state = JsonConvert.DeserializeObject<GamepadState>(json);

            Dispatcher.Invoke(() =>
            {
                StatusText = "Receiving input";
                Raise(nameof(StatusText));
            });

            _controller.Update(state);
        }

        private void Raise(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void RaiseAll()
        {
            Raise(nameof(ActiveModeText));
            Raise(nameof(StatusText));
            Raise(nameof(WifiGlowOpacity));
            Raise(nameof(BluetoothGlowOpacity));
            Raise(nameof(UsbGlowOpacity));
            Raise(nameof(WifiQrVisibility));
            Raise(nameof(BluetoothInfoVisibility));
            Raise(nameof(UsbInfoVisibility));
        }
    }
}
