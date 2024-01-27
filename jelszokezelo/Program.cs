using System;

namespace jelszokezelo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string jelszo = "";
            byte szam = 0;
            byte szamE;

            string chars = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#$%&*/\\?";
            byte length = byte.Parse(Console.ReadLine());

            for (int i = 0; i < length; i++)
            {
                Random r = new Random();
                int index = r.Next(0, 71);

                //byte.TryParse(chars[index], out szamE);
                jelszo += chars[index];
            }

            Console.WriteLine(jelszo);
        }
    }
}
