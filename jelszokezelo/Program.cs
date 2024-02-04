using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace jelszokezelo
{
    internal class Program
    {
        static List<Passwords> passwords = new List<Passwords>();

        static string[] chars = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "!", "@", "#", "$", "%", "&", "*", "(", ")", "_", "+", "-", "=", "{", "}", "[", "]", ":", ";", "'", "<", ">", ",", ".", "?", "/", "|" };
        static int index = 0; //random választott betűk indexe

        //jelszó random indexének eltárolására felvett változó (random szám a <LengthIndexGenerator()> függvényből)
        static int lengthIndex = 0;

        //jelszó karakterszámának eltárolására felvett változó
        static byte passLength = 0;
     
        static string pass = ""; //jelszó változó
              
        static void Main()
        {
            Load("n.txt"); //mentett jelszavak betöltése
            Menu(); //menükód bekérése
        }

        static void Menu()
        {
            Console.WriteLine("Options: \t | 0: Generate new password | 1: Show passwords | ESC: Exit");

            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);

            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.NumPad0:
                case ConsoleKey.D0:
                    Console.Clear();
                    Input();
                    LengthIndexGenerator();  //Erre az indexre tesz majd FIXEN egy számot
                    Generator();             //jelszó generálása
                    Save();                  //jelszó mentése egy txt fájlba
                    Menu();                  //navigálómenü
                    break;
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    Console.Clear();
                    Query();                 //Jelszólekérés
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    Console.WriteLine("EExiting..."); //ha csak 1 E van, akkor valamiért fos
                    break;
                default:
                    break;
            }
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

            StreamWriter sw = new StreamWriter("n.txt", true); //true - hozzáír a fájlhoz    
                        

            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(false);

            switch (consoleKeyInfo.Key)
            {              
                case ConsoleKey.Y:
                    Console.Clear();
                    Console.Write("Username: ");
                    string usern = Console.ReadLine();

                    Console.Write("Email: ");
                    string email = Console.ReadLine();

                    Console.Write("Website: ");
                    string website = Console.ReadLine();

                    sw.WriteLine($"{usern} {email} {pass} {website}");
                    sw.Close();

                    Console.Clear();
                    Console.WriteLine("Password saved");
                    break;
                case ConsoleKey.N:
                    Console.WriteLine("Exiting...");
                    pass = ""; //törli a változó értékét
                    Console.Clear();
                    sw.Close(); //zárja a fájlt, különben nem tudná újra megnyitni
                    Menu();
                    break;
                default:
                    Console.Clear();
                    sw.Close();
                    Save(); 
                    break;
            }
        }

        static void Load(string file)
        {
            passwords.Clear();
            StreamReader sr = new StreamReader(file);

            while (!sr.EndOfStream)
            {                
                string row = sr.ReadLine();
                string[] data = row.Split(" ");

                Passwords pass = new Passwords();

                pass.username = data[0];
                pass.email = data[1];
                pass.password = data[2];
                pass.website = data[3];

                passwords.Add(pass);
            }
            sr.Close();
        }

        static void Query()
        {
            Load("n.txt");
            bool exist = false;

            Console.WriteLine("Saved passwords from: ");

            foreach (Passwords item in passwords)
            {
                Console.WriteLine(item.website);               
            }
            Console.WriteLine();
            Console.Write("Enter website name to show password, delete or modify: ");             
            string input = Console.ReadLine();

            Console.Clear();
            foreach (Passwords item in passwords)
            {
                if (item.website == input)
                {
                    exist = true;                    
                    Console.WriteLine($"Website: {item.website} \nEmail: {item.email}\nUsername: {item.username}\nPassword: {item.password} \n");                                   
                }              
            }

            if (!exist)
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Not valid query! Try again :(");
                Console.ResetColor();
                Query();
            }
            else Manipulate();
        }

        static void Manipulate()
        {
            Console.WriteLine("Options: \t | M: Modify password | D: Delete Password | ESC: Menu");

            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);

            switch (consoleKeyInfo.Key)
            {
                /*case ConsoleKey.M:
                    Console.Clear();
                    break;*/
                case ConsoleKey.D:
                    Delete();
                    break;
                case ConsoleKey.Escape: //itt is fos valamiért és az első 2 betűt nem iratja ki az optionsból
                    Menu();
                    break;
                default:
                    break;
            }
        }

        static void Delete()
        {
            Console.WriteLine("Please enter the password combination you want to delete!");
            string pass = Console.ReadLine();

            foreach (Passwords item in passwords)
            {
                if (item.password == pass)
                {
                    Console.WriteLine("EZT KÉNE TÖRÖLNI");
                    //--FÁJLÚJRAÍRÁS KÓD--
                }
                else
                {
                    Console.WriteLine("Not valid password given, try again!");
                    Delete();
                }
            }
        }
    }
}