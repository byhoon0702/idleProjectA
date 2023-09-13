// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("jOi41RrWXA3Te19TnUhZm3bumEniXJyf9AvQkUlCddcD67eVyRb/QlrzBWTHfE0nKlYpuOP2croalv9rf/WB8ckpqUkSKYW1csw98xKVe5d9fM7qMrdYgMmuwDo44IfP5H11bahT5BQQqv19bMOuIjaiBgmIMShi93R6dUX3dH9393R0dfSDkP3gYtI+gzhGrGKZli0W/NbXA5rFqVDKEQXriAK1uDyGt6lKJINbcFb11uU4/FGde2GxKoU6KkzO/yG7q6CAtcaNouhMf86TAF5oKFq9CjXLy0ImH0X3dFdFeHN8X/M984J4dHR0cHV2WEaO7BdwEMvONx136a9mTXZEWwEg92Y/g10CI8bww3vmGmJUJMYDam1xtZ8dWg6NpHd2dHV0");
        private static int[] order = new int[] { 12,7,8,11,9,10,7,7,9,10,10,12,13,13,14 };
        private static int key = 117;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
