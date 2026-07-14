using System;

namespace HelloWorld
{
    public class Program
    {
        public static string GetMessage()
        {
            return "Hello, World!";
        }

        public static void Main(string[] args)
        {
            Console.WriteLine(GetMessage());
        }
    }
}
