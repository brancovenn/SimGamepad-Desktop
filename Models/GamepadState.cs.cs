namespace SimGamepad.Models
{
    public class GamepadState
    {
        public float LeftStickX { get; set; }
        public float LeftStickY { get; set; }

        public float RightStickX { get; set; }
        public float RightStickY { get; set; }

        public bool A { get; set; }
        public bool B { get; set; }
        public bool X { get; set; }
        public bool Y { get; set; }

        public bool LB { get; set; }
        public bool RB { get; set; }

        public float LT { get; set; }
        public float RT { get; set; }

        public float GyroX { get; set; }
        public float GyroY { get; set; }
        public float GyroZ { get; set; }
    }
}
