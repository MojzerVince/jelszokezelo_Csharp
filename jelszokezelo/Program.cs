using System;
using System.Runtime.InteropServices;

namespace jelszokezelo
{
    internal class Program
    {
        static string[] chars = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "!", "@", "#", "$", "%", "&", "*", "(", ")", "_", "+", "-", "=", "{", "}", "[", "]", "|", ":", ";", "'", "<", ">", ",", ".", "?", "/" };
        static int index = 0;
        static int lengthIndex = 0;

        static byte passLength = 0;

        static void Main(string[] args)
        {
            string jelszo = "";
            
            byte szamE;
            byte szamH;
    
            byte length = byte.Parse(Console.ReadLine());
            passLength = length;
            LengthIndexGenerator();


            Console.WriteLine($"length random indexe: {lengthIndex}");            
            Console.WriteLine();

            for (int i = 0; i < length; i++)
            {
                Generator();
                if (i == lengthIndex)
                {
                    while (!byte.TryParse(chars[index], out szamE))
                    {
                        Generator();
                        if (byte.TryParse(chars[index], out szamH))
                        {
                            
                            jelszo += chars[index];                            
                        }
                    }
                                        
                    Console.Write($"{lengthIndex + 1}. helyen : Egy szám a jelszó {lengthIndex}. indexén ");
                }
                else
                {
                    jelszo += chars[index];                    
                }
                Console.WriteLine($"{i + 1}. Karakter: {chars[index]}");
            }
            Console.WriteLine();
            Console.WriteLine($"Jelszó: {jelszo} ({length} karakter hosszú)");
            
        }

        static void Generator()
        {
            Random r = new Random();
            index = r.Next(0, chars.Count());
        }

        static void LengthIndexGenerator()
        {
            Random r = new Random();
            lengthIndex = r.Next(1, passLength);            
        }
    }
}
