namespace Passcore.Android
{
    class PasswordLengthHelper
    {
        private static int[] WeakLength = { 4, 6, 8, 10 };
        private static int[] NormLength = { 12, 16, 24, 32, 48, 64, 128, 256, 384, 512 };

        public static int GetMax(bool isWeak = false)
        {
            if (isWeak)
                return WeakLength.Length - 1;
            return NormLength.Length - 1;
        }

        public static int GetLength(int index, bool isWeak = false)
        {
            if (isWeak)
                return WeakLength[index];
            return NormLength[index];
        }
    }
}