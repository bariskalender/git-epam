using System;

namespace FancyCalc
{
    public static class Operations
    {
        public static int Plus(int x, int y)
        {
            return x + y;
        }

        public static int Minus(int x, int y)
        {
            return x - y;
        }

        public static int Multiply(int x, int y)
        {
            int result = x * y;
            return result;
        }

        public static int Sum(int x1, int x2, int x3)
        {
            return x1 + x2 + x3;
        }
    }
}
