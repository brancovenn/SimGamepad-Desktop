using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using SimGamepad.Models;

namespace SimGamepad.Input
{
    public class VirtualXboxController
    {
        private readonly IXbox360Controller _controller;

        public VirtualXboxController()
        {
            var client = new ViGEmClient();
            _controller = client.CreateXbox360Controller();
            _controller.Connect();
        }

        public void Update(GamepadState s)
        {
            _controller.SetAxisValue(Xbox360Axis.LeftThumbX, (short)(s.LeftStickX * short.MaxValue));
            _controller.SetAxisValue(Xbox360Axis.LeftThumbY, (short)(s.LeftStickY * short.MaxValue));
        }
    }
}
