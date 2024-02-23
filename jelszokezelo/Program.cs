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
        static Passwords[] passwords1 = new Passwords[50];        

        static string[] chars = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "!", "@", "#", "$", "%", "&", "*", "(", ")", "_", "+", "-", "=", "{", "}", "[", "]", ":", ";", "'", "<", ">", ",", ".", "?", "/", "|" };
        static int index = 0; //random választott betűk indexe

        //jelszó random indexének eltárolására felvett változó (random szám a <LengthIndexGenerator()> függvényből)
        /*static int numberIndex = 0;
        static int lowerCaseIndex = 0;
        static int upperCaseIndex = 0;
        static int specialCharacterIndex = 0;*/
        //jelszó karakterszámának eltárolására felvett változó
        static byte passLength = 0;

        static string pass = ""; //jelszó változó

        static void Main()
        {
            Load("n.txt"); //mentett jelszavak betöltése
            Menu(); //menükód bekérése
        }

        static void Menu() //Menürendszer
        {
            Console.WriteLine("Options: \t | 0: Generate new password | 1: Show passwords | 2: Add an existing password | ESC: Exit");

            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);

            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.NumPad0:
                case ConsoleKey.D0:
                    Console.Clear();
                    Input();
                    Save();                  //jelszó mentése egy txt fájlba
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
                case ConsoleKey.D2:
                    AddPass();
                    break;
                case ConsoleKey.D3:
                    Modify();
                    break;
                default:
                    Console.Clear();
                    Menu();
                    break;
            }
        }

        static void Input() //Jelszóhosszúság ellenőrzése
        {
            bool legit = false;

            Console.WriteLine("What type of password you want to generate? \t 0: Password | 1: PIN | ESC: Back to menu");

            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.NumPad0:
                case ConsoleKey.D0:
                    while (!legit)
                        try
                        {
                            Console.WriteLine("Passlength  between 8-20");
                            Console.Write("Password Length: ");
                            passLength = byte.Parse(Console.ReadLine()); //jelszó hosszúságának bekérése
                            legit = true;

                            //LengthIndexGenerator();  //Erre az indexre tesz majd FIXEN egy számot
                            PassGenerator();             //jelszó generálása

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
                    break;
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    while (!legit)
                        try
                        {
                            Console.WriteLine("PIN length  between 4-10");
                            Console.Write("PIN Length: ");
                            passLength = byte.Parse(Console.ReadLine());
                            legit = true;

                            PinGenerator();

                            while (passLength < 4 || passLength > 10)
                            {
                                Console.Clear();
                                Console.WriteLine("Please enter a PIN length above 3 and below 11!");
                                Console.Write("PIN Length: ");
                                passLength = byte.Parse(Console.ReadLine());
                            }
                        }
                        catch
                        {
                            Console.Clear();
                            Console.WriteLine("Plese enter a number!");
                        }
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    Menu();
                    break;
                default:
                    Console.Clear();
                    Input();
                    break;
            }            
        }

        static void PassGenerator() //Jelszó karaktereinek generálása
        {
            //byte szamE;

            for (int i = 0; i < passLength; i++)
            {
                Random r = new Random();
                index = r.Next(0, chars.Count()); //0 és a karakterek száma közt dob egy random számot
                /*if (i == numberIndex) //ha a fix szám indexére jutott akkor...
                {
                    while (!byte.TryParse(chars[index], out szamE)) //megnézi, hogy a legutóbbi random karakter szám-e és ha nem...
                    {
                        index = r.Next(0, chars.Count()); //akkor generál egy újabb karaktert, ameddig szám nem lesz
                        if (byte.TryParse(chars[index], out szamE))
                            pass += chars[index];
                    }
                    //Console.Write($"{lengthIndex + 1}. helyen : Egy szám a jelszó {lengthIndex}. indexén ");
                }*/              
                /*else if (i == lowerCaseIndex)
                {
                    while (!chars[index].Any(char.IsLower))
                    {
                        index = r.Next(0, chars.Count());
                        if (chars[index].Any(char.IsLower))
                            pass += chars[index];
                    }
                }
                else if (i == upperCaseIndex)
                {
                    while (!chars[index].Any(char.IsUpper))
                    {
                        index = r.Next(0, chars.Count());
                        if (chars[index].Any(char.IsUpper))
                            pass += chars[index];
                    }
                }
                else if (i == specialCharacterIndex)
                {
                    while (chars[index].Any(char.IsLetter))
                    {
                        index = r.Next(0, chars.Count());
                        if (!chars[index].Any(char.IsLetter))
                            pass += chars[index];
                    }
                }*/
                //amennyiben nem a fix indexeken van, akkor hozzáadja az aktuális karaktert a jelszóhoz
                pass += chars[index];

                //Console.WriteLine($"{i + 1}. Karakter: {chars[index]}");
            }

            Console.WriteLine();
            Console.Write($"Pass: ");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($"{pass}");
            Console.ResetColor();
            Console.WriteLine();

            /*Console.WriteLine($"Number random indexe: {numberIndex}");
            Console.WriteLine($"Lowercase random indexe: {lowerCaseIndex}");
            Console.WriteLine($"Uppercase random indexe: {upperCaseIndex}");
            Console.WriteLine($"Special Character indexe: {specialCharacterIndex}");*/

            //Console.WriteLine();
            //Console.WriteLine(pass.Count());
        }

        static void PinGenerator()
        {
            for (int i = 0; i < passLength; i++)
            {
                Random r = new Random();

                pass += r.Next(0, 9);
            }

            Console.WriteLine();
            Console.Write($"PIN: ");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($"{pass}");
            Console.ResetColor();
            Console.WriteLine();
        }

        /*static void LengthIndexGenerator() //Generál egy FIX pozíciót egy szám karakternek
        {
            Random r = new Random();
            numberIndex = r.Next(1, passLength);
            lowerCaseIndex = r.Next(1, passLength);
            upperCaseIndex = r.Next(1, passLength);
            specialCharacterIndex = r.Next(1, passLength);

            while (specialCharacterIndex == lowerCaseIndex || specialCharacterIndex == upperCaseIndex || specialCharacterIndex == numberIndex)
                lowerCaseIndex = r.Next(1, passLength);
            while (specialCharacterIndex == lowerCaseIndex || specialCharacterIndex == upperCaseIndex || specialCharacterIndex == numberIndex)
                upperCaseIndex = r.Next(1, passLength);
            while (specialCharacterIndex == lowerCaseIndex || specialCharacterIndex == upperCaseIndex || specialCharacterIndex == numberIndex)
                specialCharacterIndex = r.Next(1, passLength);
        }*/

        static void AddPass()
        {
            StreamWriter sw = new StreamWriter("n.txt", true); //true - hozzáír a fájlhoz 

            string usern;
            string email;
            string website;
            string password;

            do
            {
                Console.Write("Username: ");
                usern = Console.ReadLine();
            } while (usern == "");
            do
            {
                Console.Write("Email: ");
                email = Console.ReadLine();
            } while (email == "");
            do
            {
                Console.Write("Website: ");
                website = Console.ReadLine();
            } while (website == "");
            do
            {
                Console.Write("Password: ");
                password = Console.ReadLine();
            } while (password == "");

            Console.Write("Check it! Is it right? Y/N: ");

            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);

            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.Y:
                    sw.WriteLine($"{password} {usern} {email} {website}");
                    sw.Close();
                    Menu();
                    break;
                case ConsoleKey.N:
                    Console.Clear();
                    sw.Close();
                    AddPass();                                     
                    break;
            }
            
            sw.Close();

            Console.Clear();
            Console.WriteLine("Password saved");
        }
        static void Save() //Jelszó mentése egy txt fájlba
        {
            Console.Write("Save Password? Y/N\n");

            StreamWriter sw = new StreamWriter("n.txt", true); //true - hozzáír a fájlhoz                            

            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);

            string usern;
            string email;
            string website;

            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.Y:
                    Console.Clear();
                    do
                    {
                        Console.Write("Username: ");
                        usern = Console.ReadLine();
                    } while (usern == "");
                    do
                    {
                        Console.Write("Email: ");
                        email = Console.ReadLine();
                    } while (email == "");
                    do
                    {
                        Console.Write("Website: ");
                        website = Console.ReadLine();
                    } while (website == "");

                    sw.WriteLine($"{pass} {usern} {email} {website}");
                    sw.Close();

                    Console.Clear();
                    Console.WriteLine("Password saved");
                    pass = ""; //változó reset       
                    break;
                case ConsoleKey.N:
                    pass = ""; //törli a változó értékét
                    Console.Clear();
                    sw.Close(); //zárja a fájlt, különben nem tudná újra megnyitni
                    Menu();
                    break;
                default:
                    sw.Close();
                    Save();
                    break;
            }
        }

        static void Load(string file) //Jelszavak betöltése
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

        static void Query() //Jelszavak kiíratása
        {
            Load("n.txt");
            bool exist = false;

            Console.WriteLine("Saved passwords from: ");

            foreach (Passwords item in passwords)
                Console.WriteLine(item.website);

            Console.WriteLine();
            Console.Write("Enter website name to show password, delete or modify, R to return: ");
            string input = Console.ReadLine();

            if(input.ToLower() == "r")
            {
                Console.Clear();
                Menu();
            }
            else
            {
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
        }

        static void Manipulate() //Jelszavak módosítására lehetőség
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
                case ConsoleKey.Escape:
                    Console.Clear();
                    Menu();
                    break;
            }
        }

        static void Modify() //Jelszó módosítás
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
                default:
                    Modify();
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
        
        static void Delete() //Jelszó adatok törlése
        {
            Console.WriteLine("Please enter the password combination you want to delete from the query!");
            string pass = Console.ReadLine();            

            bool found = false;

            for (int i = 0; i < n ; i++) 
            { 
                if (passwords1[i].password == pass)
                {
                    found = true;                    
                    passwords.RemoveAt(i);
                }
            }

            StreamWriter sw = new StreamWriter("n.txt", false); //újraírja a fájlt

            foreach (Passwords item in passwords)
            {                
                sw.WriteLine($"{item.password} {item.username} {item.email} {item.website}");                               
            }
            sw.Close();

            if (found)
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Password successfully deleted!");
                Console.ResetColor();
                Main();
                Console.WriteLine();  
            }           
            
            if (!found) //ha nem találja a törlendő jelszót, akkor újra meg kell adni, ez amúgy egy végtelen ciklus
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