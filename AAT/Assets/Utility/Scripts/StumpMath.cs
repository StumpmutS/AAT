namespace Utility.Scripts
{
    public static class StumpMath
    {
        public static int Factorial(int n)
        {
            if (n < 2) return 1;
            
            int result = n;
            for (int i = 2; i < n; i++)
            {
                result *= i;
            }

            return result;
        }
    }
}