using System;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace jelszokezelo
{
    internal class Program
    {
        static string[] chars = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "!", "@", "#", "$", "%", "&", "*", "(", ")", "_", "+", "-", "=", "{", "}", "[", "]", ":", ";", "'", "<", ">", ",", ".", "?", "/", "|" };
        static int index = 0; //random választott betűk indexe

        //jelszó random indexének eltárolására felvett változó (random szám a <LengthIndexGenerator()> függvényből)
        static int lengthIndex = 0;

        //jelszó karakterszámának eltárolására felvett változó
        static byte passLength = 0;
     
        static string pass = ""; //jelszó változó

        static void Main()
        {
            Input();

            LengthIndexGenerator(); //Erre az indexre tesz majd FIXEN egy számot

            Generator(); //jelszó generálása     
            
            Save(); //jelszó mentése egy txt fájlba
        }

        static void Input() //Jelszóhosszúság ellenőrzése
        {
            bool legit = false;

            while(!legit)
            try
            {
                Console.WriteLine("Passlength  between 8-20");
                Console.Write("Password Length: ");
                passLength = byte.Parse(Console.ReadLine()); //jelszó hosszúságának bekérése
                legit = true;

                while (passLength < 8 || passLength > 20) //ha kisebb mint 8 vagy nagyobb mint 20 bekéri újra
                {
                    Console.Clear();
                    Console.WriteLine("Please enter a pass length above 7 and below 21!");
                    Console.Write("Password Length: ");
                    passLength = byte.Parse(Console.ReadLine());
                }
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Plese enter a number!");
            }
        }

        static void Generator() //Jelszó karaktereinek generálása
        {
            byte szamE;

            for (int i = 0; i < passLength; i++)
            {
                Random r = new Random();
                index = r.Next(0, chars.Count()); //0 és a karakterek száma közt dob egy random számot
                if (i == lengthIndex) //ha a fix szám indexére jutott akkor...
                {
                    while (!byte.TryParse(chars[index], out szamE)) //megnézi, hogy a legutóbbi random karakter szám-e és ha nem...
                    {
                        index = r.Next(0, chars.Count()); //akkor generál egy újabb karaktert, ameddig szám nem lesz
                        if (byte.TryParse(chars[index], out szamE))
                            pass += chars[index];
                    }
                    Console.Write($"{lengthIndex + 1}. helyen : Egy szám a jelszó {lengthIndex}. indexén ");
                }
                else //amennyiben nem a fix szám indexén van, akkor hozzáadja az aktuális karaktert a jelszóhoz
                    pass += chars[index];

                Console.WriteLine($"{i + 1}. Karakter: {chars[index]}");
            }

            Console.WriteLine();
            Console.WriteLine($"Jelszó: {pass} ({passLength} karakter hosszú)");

            Console.WriteLine($"length random indexe: {lengthIndex}");
            Console.WriteLine();
        }

        static void LengthIndexGenerator() //Generál egy FIX pozíciót egy szám karakternek
        {
            Random r = new Random();
            lengthIndex = r.Next(1, passLength);    
        }

        static void Save() //Jelszó mentése egy txt fájlba
        {
            Console.Write("Save Password? Y/N ");
            string ans = Console.ReadLine();

            StreamWriter sw = new StreamWriter("n.txt", true); //true - hozzáír a fájlhoz    

            switch (ans)
            {
                case "Y":
                    Console.Write("Username/Email: ");
                    string usern = Console.ReadLine();

                    sw.WriteLine($"{usern} {pass}");
                    sw.Close();

                    Console.Clear();
                    Console.WriteLine("Password saved");
                    break;
                case "N":
                    Console.Write("Do you want to generate new one? Y/N ");
                    string gen = Console.ReadLine();
                    if (gen == "Y")
                    {
                        pass = ""; //törli a változó értékét
                        Console.Clear();
                        sw.Close(); //zárja a fájlt, különben nem tudná újra megnyitni
                        Main();
                    }
                    else Console.WriteLine("Exiting...");
                    break;
                default:
                    break;
            }
        }
    }
}