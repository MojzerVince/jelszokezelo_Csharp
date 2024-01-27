using System;
using System.Runtime.InteropServices;

namespace jelszokezelo
{
    internal class Program
    {
        static string[] chars = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "!", "@", "#", "$", "%", "&", "*", "(", ")", "_", "+", "-", "=", "{", "}", "[", "]", "|", ":", ";", "'", "<", ">", ",", ".", "?", "/" };
        static int index = 0;

        //jelszó random indexének eltárolására felvett változó (random szám a <LengthIndexGenerator()> függvényből)
        static int lengthIndex = 0;

        //jelszó karakterszámának eltárolására felvett változó
        static byte passLength = 0;

        static string pass = "";

        static void Main(string[] args)
        {                       
            byte szamE;
    
            passLength = byte.Parse(Console.ReadLine());

            //Számgenerálás 0 és a kért hosszúságú jelszó között
            LengthIndexGenerator();


            Console.WriteLine($"length random indexe: {lengthIndex}");            
            Console.WriteLine();

            for (int i = 0; i < passLength; i++)
            {
                Generator();
                if (i == lengthIndex)
                {
                    while (!byte.TryParse(chars[index], out szamE))
                    {
                        Generator();
                        if (byte.TryParse(chars[index], out szamE))
                        {
                            
                            pass += chars[index];                            
                        }
                    }
                                        
                    Console.Write($"{lengthIndex + 1}. helyen : Egy szám a jelszó {lengthIndex}. indexén ");
                }
                else
                {
                    pass += chars[index];                    
                }
                Console.WriteLine($"{i + 1}. Karakter: {chars[index]}");
            }
            Console.WriteLine();
            Console.WriteLine($"Jelszó: {pass} ({passLength} karakter hosszú)");

            Save();
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

        static void Save()
        {
            Console.Write("Username/Email: ");
            string usern = Console.ReadLine();            

            StreamWriter sw = new StreamWriter("n.txt", true);
            sw.WriteLine($"{usern}|{pass}");

            sw.Close();

            Console.Clear();
            Console.WriteLine("Password saved");
        }
    }
}