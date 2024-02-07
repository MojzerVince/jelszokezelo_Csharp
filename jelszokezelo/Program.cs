using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace jelszokezelo
{
    internal class Program
    {
        static List<Passwords> passwords = new List<Passwords>();
        static int n = 0;
        static int c = 0;
        static Passwords[] passwords1 = new Passwords[50];        

        static string[] chars = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "!", "@", "#", "$", "%", "&", "*", "(", ")", "_", "+", "-", "=", "{", "}", "[", "]", ":", ";", "'", "<", ">", ",", ".", "?", "/", "|" };
        static int index = 0; //random választott betűk indexe

        //jelszó random indexének eltárolására felvett változó (random szám a <LengthIndexGenerator()> függvényből)
        static int numberIndex = 0;
        static int lowerCaseIndex = 0;
        static int upperCaseIndex = 0;
        static int specialCharacterIndex = 0;
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
                    Console.WriteLine("Exiting..."); //ha csak 1 E van, akkor valamiért fos
                    break;
                case ConsoleKey.D3:
                    Modify();
                    break;
                default:
                    break;
            }
        }

        static void Input() //Jelszóhosszúság ellenőrzése
        {
            bool legit = false;

            while (!legit)
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
                if (i == numberIndex) //ha a fix szám indexére jutott akkor...
                {
                    while (!byte.TryParse(chars[index], out szamE)) //megnézi, hogy a legutóbbi random karakter szám-e és ha nem...
                    {
                        index = r.Next(0, chars.Count()); //akkor generál egy újabb karaktert, ameddig szám nem lesz
                        if (byte.TryParse(chars[index], out szamE))
                            pass += chars[index];
                    }
                    //Console.Write($"{lengthIndex + 1}. helyen : Egy szám a jelszó {lengthIndex}. indexén ");
                }
                else //amennyiben nem a fix szám indexén van, akkor hozzáadja az aktuális karaktert a jelszóhoz
                    pass += chars[index];

                if (i == lowerCaseIndex)
                {
                    while (!chars[index].Any(char.IsLower))
                    {
                        index = r.Next(0, chars.Count());
                        if (chars[index].Any(char.IsLower))
                        {
                            pass += chars[index];
                        }
                    }
                }

                if (i == upperCaseIndex)
                {
                    while (!chars[index].Any(char.IsUpper))
                    {
                        index = r.Next(0, chars.Count());
                        if (chars[index].Any(char.IsUpper))
                        {
                            pass += chars[index];
                        }
                    }
                }

                if (i == specialCharacterIndex)
                {
                    while (chars[index].Any(char.IsLetter))
                    {
                        index = r.Next(0, chars.Count());
                        if (!chars[index].Any(char.IsLetter))
                        {
                            pass += chars[index];
                        }
                    }
                }
                //Console.WriteLine($"{i + 1}. Karakter: {chars[index]}");
            }

            Console.WriteLine();
            Console.Write($"Jelszó: ");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($"{pass}");
            Console.ResetColor();

            //Console.WriteLine($"Number random indexe: {numberIndex}");
            //Console.WriteLine($"Lowercase random indexe: {lowerCaseIndex}");
            //Console.WriteLine($"Uppercase random indexe: {upperCaseIndex}");
            //Console.WriteLine($"Special Character indexe: {specialCharacterIndex}");
            Console.WriteLine();
        }
        

        static void LengthIndexGenerator() //Generál egy FIX pozíciót egy szám karakternek
        {
            Random r = new Random();
            numberIndex = r.Next(1, passLength);
            lowerCaseIndex = r.Next(1, passLength);
            upperCaseIndex = r.Next(1, passLength);
            specialCharacterIndex = r.Next(1, passLength);
            while (numberIndex == lowerCaseIndex)
            {
                lowerCaseIndex = r.Next(1, passLength);
            }
            while (upperCaseIndex == lowerCaseIndex || upperCaseIndex == numberIndex)
            {
                upperCaseIndex = r.Next(1, passLength);
            }
            while (specialCharacterIndex == lowerCaseIndex || specialCharacterIndex == upperCaseIndex || specialCharacterIndex == numberIndex)
            {
                specialCharacterIndex = r.Next(1, passLength);
            }
        }

        static void Save() //Jelszó mentése egy txt fájlba
        {
            Console.Write("Save Password? Y/N ");

            StreamWriter sw = new StreamWriter("n.txt", true); //true - hozzáír a fájlhoz                            

            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);

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

                    sw.WriteLine($"{pass} {usern} {email} {website}");
                    sw.Close();

                    Console.Clear();
                    Console.WriteLine("Password saved");
                    pass = "";
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
            n = 0;

            StreamReader sr = new StreamReader(file);

            while (!sr.EndOfStream)
            {
                string row = sr.ReadLine();
                string[] data = row.Split(" ");

                Passwords pass = new Passwords();

                try //megnézi, hogy beolvashatóak-e az adatok
                {
                    pass.password = data[0];
                    pass.username = data[1];
                    pass.email = data[2];
                    pass.website = data[3];

                    passwords.Add(pass);
                }
                catch { } //ha nem, akkor nem csinál semmit xd
            }
            sr.Close();

            StreamReader sr1 = new StreamReader(file);

            while (!sr1.EndOfStream)
            {
                string row = sr1.ReadLine();
                string[] data = row.Split(" ");
                
                try //megnézi, hogy beolvashatóak-e az adatok
                {
                    passwords1[n].password = data[0];
                    passwords1[n].username = data[1];
                    passwords1[n].email = data[2];
                    passwords1[n].website = data[3];

                    n++;
                }
                catch { } //ha nem, akkor nem csinál semmit xd
            }            
            sr1.Close();            
        }

        static void Query()
        {
            Load("n.txt");
            bool exist = false;

            Console.WriteLine("Saved passwords from: ");

            foreach (Passwords item in passwords)
                Console.WriteLine(item.website);

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
            Console.WriteLine("Options: \t | M: Modify Query | D: Delete Query | ESC: Menu");

            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);

            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.M:
                    Modify();
                    break;
                case ConsoleKey.D:
                    Delete();
                    break;
                case ConsoleKey.Escape: //itt is fos valamiért és az első 2 betűt nem iratja ki az optionsból
                    Console.Clear();
                    Menu();
                    break;
                default:
                    break;
            }
        }

        static void Modify()
        {
            Console.WriteLine("Please enter a number what you want to modify! 1: Username | 2: Email | 3: Website");

            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);

            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    NewUsername();                                        
                    break;
                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                    NewEmail();
                    break;
                case ConsoleKey.NumPad3:
                case ConsoleKey.D3:
                    NewWebsite();
                    break;
            }
        }

        static void NewWebsite()
        {
            Console.Write("Please enter the password combination of the query! ");
            string password = Console.ReadLine();

            Console.Write("Please enter the new wewbsite: ");
            string newWebsite = Console.ReadLine();

            StreamWriter sw = new StreamWriter("n.txt", false);

            for (int i = 0; i < n; i++)
            {
                if (passwords1[i].password == password)
                {
                    passwords1[i].website = newWebsite;
                    sw.WriteLine($"{passwords1[i].password} {passwords1[i].username} {passwords1[i].email} {newWebsite}");
                }
                else
                    sw.WriteLine($"{passwords1[i].password} {passwords1[i].username} {passwords1[i].email} {passwords1[i].website}");
            }
            sw.Close();
            Main();
        }

        static void NewEmail()
        {
            Console.Write("Please enter the password combination of the query! ");
            string password = Console.ReadLine();

            Console.Write("Please enter the new email: ");
            string newEmail = Console.ReadLine();

            StreamWriter sw = new StreamWriter("n.txt", false);

            for (int i = 0; i < n; i++)
            {
                if (passwords1[i].password == password)
                {
                    passwords1[i].email = newEmail;
                    sw.WriteLine($"{passwords1[i].password} {passwords1[i].username} {newEmail} {passwords1[i].website}");
                }
                else
                    sw.WriteLine($"{passwords1[i].password} {passwords1[i].username} {passwords1[i].email} {passwords1[i].website}");
            }
            sw.Close();
            Main();
        }

        static void NewUsername()
        {
            Console.Write("Please enter the password combination of the query! ");
            string password = Console.ReadLine();

            Console.Write("Please enter the new username: ");
            string newUsername = Console.ReadLine();            

            StreamWriter sw = new StreamWriter("n.txt", false);

            for (int i = 0; i < n ; i++)
            {
                if (passwords1[i].password == password)
                {
                    passwords1[i].username = newUsername;
                    sw.WriteLine($"{passwords1[i].password} {newUsername} {passwords1[i].email} {passwords1[i].website}");
                }                
                else
                    sw.WriteLine($"{passwords1[i].password} {passwords1[i].username} {passwords1[i].email} {passwords1[i].website}");
            }             
            sw.Close();
            Main();                      
        }
        
        static void Delete()
        {
            Console.WriteLine("Please enter the password combination you want to delete from the query!");
            string pass = Console.ReadLine();

            bool found = false;

            foreach (Passwords item in passwords)
            {
                if (item.password == pass) //végigmegy a tárolt jelszavakon és megnézi van-e egyezés
                {
                    passwords.Remove(item);

                    StreamWriter sw = new StreamWriter("n.txt", false); //újraírja a fájlt
                    found = true;

                    foreach (Passwords item1 in passwords)
                        sw.WriteLine($"{item1.password} {item1.username} {item1.email} {item1.website}");                 

                    sw.Close();

                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("Password successfully deleted!");
                    Console.ResetColor();
                    Menu();
                }                
            }

            if(!found) //ha nem találja a törlendő jelszót, akkor újra meg kell adni, ez amúgy egy végtelen ciklus
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Not valid password given, try again!");
                Console.ResetColor();
                Delete();
            }
        }
    }
}