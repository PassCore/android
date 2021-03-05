using System;

namespace Passcore.Android
{
    class KRNG
    {
        public static int GetInt(int min, int max)
            => new Random(Guid.NewGuid().GetHashCode()).Next(min, max);

        public static int GetInt()
            => new Random(Guid.NewGuid().GetHashCode()).Next();
    }
}