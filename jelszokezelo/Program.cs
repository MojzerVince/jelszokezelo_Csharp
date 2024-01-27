using System;
using System.Runtime.InteropServices;

namespace jelszokezelo
{
    internal class Program
    {
        static string[] chars = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "!", "@", "#", "$", "%", "&", "*", "(", ")", "_", "+", "-", "=", "{", "}", "[", "]", "|", ":", ";", "'", "<", ">", ",", ".", "?", "/" };
        static int index = 0;

        static void Main(string[] args)
        {
            string jelszo = "";
            byte szam = 0;
            byte szamE;
    
            byte length = byte.Parse(Console.ReadLine());

            for (int i = 0; i < length; i++)
            {
                Generator();
                if (szam < 1 && i == length - 1)
                {
                    while (!byte.TryParse(chars[index], out szamE))
                    {
                        Generator();
                        if (byte.TryParse(chars[index], out szamE))
                        {
                            szam++;
                            jelszo += chars[index];
                        }
                    }
                    
                    Console.WriteLine("NIGA");
                }
                else
                {
                    jelszo += chars[index];                    
                }             
            }

            Console.WriteLine(jelszo);
        }

        static void Generator()
        {
            Random r = new Random();
            index = r.Next(0, chars.Count());
        }
    }
}
