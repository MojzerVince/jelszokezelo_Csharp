using System.Reflection;
using System.Xml.Serialization;

namespace jelszokezelo
{
    internal class Program
    {
        static List<Passwords> passwords = new List<Passwords>();
        static Passwords[] passwords1 = new Passwords[50];
        static List<Translate> tranlate = new List<Translate>();
        static string[] login = new string[2];

        static int n = 0;

        static string[] chars = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "!", "@", "#", "$", "%", "&", "*", "(", ")", "_", "+", "-", "=", "{", "}", "[", "]", ":", ";", "'", "<", ">", ",", ".", "?", "/" };
        static int index = 0; //random választott betűk indexe
        static byte passLength = 0;

        static string pass = ""; //jelszó változó     

        static bool DEBUG = true; //Kiadásnál át kell rakni false-ra és minden debug funkció kikapcsol majd

        static bool log = true;

        static Passwords actualQuery;
        static void Main()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(" Welcome back! \n");
            Console.ResetColor();

            //Characters(); // NE NYYÚLJ HOZZÁ!!! legjobb ha töröljük de véletlen se nyúlunk hozzá 
            CheckForFile("n.txt");
            LoadTranslateList("trans.txt");
            Load("n.txt");
            Menu();
        }

        static void LoginCheck()
        {
            LoginLoad("login.txt");
            StreamReader sr = new StreamReader("login.txt");

            string onoff = sr.ReadLine();
            sr.Close();

            if (onoff == "ON")
            {
                if (login[1] == "")
                {
                    Register();
                }
                else if (login.Length == 2)
                {
                    Login();
                }
            }
            else if (onoff == "OFF" && new FileInfo("login.txt").Length == 1)
            {
                Register();
            }
        }

        static void Menu() //Menürendszer
        {
            Load("n.txt");
            if (log == true)
            {
                LoginCheck();
            }
            log = false;
            Console.WriteLine("Options:   | 0: Generate new password | 1: Show passwords | 2: Add an existing password | 3: Settings | ESC: Exit");

            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);

            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.NumPad0:
                case ConsoleKey.D0:
                    Console.Clear();
                    Input();
                    Translator();
                    break;
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    Console.Clear();
                    Query();                 //Jelszólekérés
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    Console.WriteLine("Exiting...");
                    break;
                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                    AddPass();
                    break;
                case ConsoleKey.NumPad3:
                case ConsoleKey.D3:
                    Settings();
                    break;
                case ConsoleKey.NumPad8:
                case ConsoleKey.D8:
                    if(DEBUG)
                        FileDelete("n.txt");
                    break;
                case ConsoleKey.NumPad9:
                case ConsoleKey.D9:
                    if(DEBUG)
                        FileExport();
                    break;
                default:
                    Console.Clear();
                    Menu();
                    break;
            }
        }

        /* !!! NEM MŰKÖDIK EZÉRT KIMÁSOLTAM A TARTALMÁT A MAINBE !!!
        static void CheckForLoginData()
        {
            if (new FileInfo("login.txt").Length == 0)
            {
                Register();
            }
            else
            {
                Login();
            }
        }
        */

        static void Settings()
        {
            Console.WriteLine("1: Turn on/off login section ");

            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);

            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    TurnOffOn();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Please choose from the options!");
                    Settings();
                    break;
            }
        }

        static void TurnOffOn()
        {
            StreamReader sr = new StreamReader("login.txt");
            sr.ReadLine();
            string pin = sr.ReadLine();
            sr.Close();

            StreamWriter sw = new StreamWriter("login.txt", false);

            Console.Write("Do you want to turn off / on login section? Write \"off\"/\"on\": ");
            string inp = Console.ReadLine().ToUpper();

            if (inp == "OFF")
            {
                sw.WriteLine("OFF");
                Console.Clear();
                Console.Write("Login is turned ");
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(" OFF \n");
                Console.ResetColor();
            }
            else if (inp == "ON")
            {
                sw.WriteLine("ON");
                sw.WriteLine(pin);
                Console.Clear();
                Console.Write("Login is turned ");
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(" ON \n");
                Console.ResetColor();
            }
            sw.Close();
            log = true;

            Menu();
        }

        static void LoginLoad(string file)
        {
            StreamReader sr = new StreamReader(file);

            while (!sr.EndOfStream)
            {
                login[0] = sr.ReadLine();
                login[1] = sr.ReadLine();
            }
            sr.Close();
        }

        static void Register()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("REGISTRATION");
            Console.ResetColor();
            Console.Write("Add login PIN: ");
            string pass = Console.ReadLine();

            StreamWriter sw = new StreamWriter("login.txt", false);

            sw.WriteLine("ON");
            sw.WriteLine(pass);

            sw.Close();
        }

        static void Login()
        {
            Console.WriteLine("LOGIN");
            Console.Write("PIN: ");
            string pass = Console.ReadLine();

            using (StreamReader sr = new StreamReader("login.txt"))
            {
                sr.ReadLine();
                string row = sr.ReadLine();

                if (pass == row)
                {
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(" Login was successfull! ");
                    Console.ResetColor();
                }
                else
                {
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(" Wrong PIN! ");
                    Console.ResetColor();
                    Login();
                }
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
                            Translator();
                            Save();
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
                            Translator();
                            Save();
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
            }
        }

        static void PassGenerator() //Jelszó karaktereinek generálása
        {
            //byte szamE;
            bool hasUpperChar;
            bool hasLowerChar;
            bool hasNumber;
            bool hasSymbol;

            do
            {
                hasUpperChar = false;
                hasLowerChar = false;
                hasNumber = false;
                hasSymbol = false;

                for (int i = 0; i < passLength; i++)
                {
                    Random r = new Random();
                    index = r.Next(0, chars.Count());

                    pass += chars[index];
                }

                for (int i = 0; i < passLength; i++)
                {
                    if (char.IsUpper(pass[i]))
                    {
                        hasUpperChar = true;
                    }
                    if (char.IsLower(pass[i]))
                    {
                        hasLowerChar = true;
                    }
                    if (char.IsDigit(pass[i]))
                    {
                        hasNumber = true;
                    }
                    if (char.IsSymbol(pass[i]))
                    {
                        hasSymbol = true;
                    }
                }
            }
            while (hasUpperChar == false && hasLowerChar == false && hasNumber == false && hasSymbol == false);

            Console.WriteLine();
            Console.Write($"Pass: ");
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($"{pass}");
            Console.ResetColor();
            Console.WriteLine();

            //Console.WriteLine();
            //Console.WriteLine(pass.Count());
        }

        static void PinGenerator() //PIN kód karakterek generálása
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

        static void AddPass() //Custom jelszó hozzáadás
        {
            string usern;
            string email;
            string website;
            string password;

            do
            {
                Console.Write("Username: ");
                usern = Console.ReadLine();
            } while (usern == "" || usern.Contains(" "));
            do
            {
                Console.Write("Email: ");
                email = Console.ReadLine();
            } while (email == "" || email.Contains(" "));
            do
            {
                Console.Write("Website: ");
                website = Console.ReadLine();
            } while (website == "" || website.Contains(" "));
            do
            {
                Console.Write("Password: ");
                password = Console.ReadLine();
            } while (password == "" || password.Contains(" "));

            Console.Write("Check it! Is it right? Want to save it? Y/N: ");

            string t_pass = "";

            for (int i = 0; i < password.Length; i++)
            {
                foreach (Translate item1 in tranlate)
                {
                    if (item1.letter.ToString() == password[i].ToString())
                    {
                        t_pass += item1.code + "|";
                    }
                }
            }

            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);

            using (StreamWriter sw = new StreamWriter("n.txt", true)) //true - hozzáír a fájlhoz 
            {
                switch (consoleKeyInfo.Key)
                {
                    case ConsoleKey.Y:
                        sw.WriteLine($"{t_pass} {usern} {email} {website}");
                        sw.Close();
                        Console.Clear();
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("Password saved");
                        Console.ResetColor();
                        Menu();
                        break;
                    case ConsoleKey.N:
                        Console.Clear();
                        sw.Close();
                        Menu();
                        break;
                }
            }
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
                    } while (usern.Contains(" ") || usern == "");

                    do
                    {
                        Console.Write("Email: ");
                        email = Console.ReadLine();
                    } while (email.Contains(" ") || email == "");

                    do
                    {
                        Console.Write("Website: ");
                        website = Console.ReadLine();
                    } while (website.Contains(" ") || website == "");

                    sw.WriteLine($"{pass} {usern} {email} {website}");
                    sw.Close();
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("Password saved");
                    Console.ResetColor();
                    pass = ""; //változó reset       
                    Menu();
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

        static void Translator()
        {
            string password = "";
            for (int i = 0; i < pass.Length; i++)
            {
                foreach (Translate item in tranlate)
                {
                    if (item.letter.ToString() == pass[i].ToString())
                    {
                        password += item.code + "|";
                    }
                }
            }
            pass = password;
        }

        //NINCS HASZNÁLATBAN
        static string TranslateInverse(string[] split)
        {
            string t_pass = "";

            for (int i = 0; i < split.Length; i++)
            {
                foreach (Translate item1 in tranlate)
                {
                    if (item1.code.ToString() == split[i].ToString())
                    {
                        t_pass += item1.code + "|";
                    }
                }
            }
            return t_pass;
        }

        static void LoadTranslateList(string file)
        {
            tranlate.Clear();
            using (StreamReader sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    string row = sr.ReadLine();
                    string[] data = row.Split(" ");
                    Translate character = new Translate();
                    character.letter = data[0];
                    character.code = data[1];
                    tranlate.Add(character);
                }
            }
        }

        static void Load(string file) //Jelszavak betöltése
        {
            passwords.Clear();
            n = 0;

            using (StreamReader sr = new StreamReader(file))
            {
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
            }

            using (StreamReader sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    string row = sr.ReadLine();
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
            }
        }

        static void Query() //Jelszavak kiíratása
        {
            Load("n.txt");

            bool exist = false;
            string t_pass = "";

            Console.WriteLine("Saved passwords from: ");

            foreach (Passwords item in passwords)
                Console.WriteLine(item.website);

            Console.WriteLine();
            Console.Write("Enter website name to show password, delete or modify, R to return: ");
            string input = Console.ReadLine();

            if (input.ToLower() == "r")
            {
                Console.Clear();
                Menu();
            }
            else
            {
                Console.Clear();

                foreach (Passwords item in passwords)
                {
                    if (input == item.website)
                    {
                        string[] spliteltpass = item.password.Split("|");
                        for (int i = 0; i < spliteltpass.Length; i++)
                        {
                            foreach (Translate item1 in tranlate)
                            {
                                if (item1.code.ToString() == spliteltpass[i].ToString())
                                    t_pass += item1.letter;
                            }
                        }
                        exist = true;
                        Console.WriteLine($"Website: {item.website} \nEmail: {item.email}\nUsername: {item.username}\nPassword: {t_pass} \n");
                        Console.WriteLine();
                        t_pass = "";
                        actualQuery = item;
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
            Console.Write("Please enter the new website: ");
            string newWebsite = Console.ReadLine();

            using (StreamWriter sw = new StreamWriter("n.txt", false))
            {
                for (int i = 0; i < n; i++)
                {
                    //átirandó !!!!!!!
                    if (passwords1[i].password == actualQuery.password)
                    {
                        passwords1[i].website = newWebsite;
                        sw.WriteLine($"{passwords1[i].password} {passwords1[i].username} {passwords1[i].email} {newWebsite}");
                    }
                    else
                        sw.WriteLine($"{passwords1[i].password} {passwords1[i].username} {passwords1[i].email} {passwords1[i].website}");
                }
            }

            Console.Clear();
            Menu();
        }

        static void NewEmail()
        {
            Console.Write("Please enter the new email: ");
            string newEmail = Console.ReadLine();

            using (StreamWriter sw = new StreamWriter("n.txt", false))
            {
                for (int i = 0; i < n; i++)
                {
                    //átirandó
                    if (passwords1[i].password == actualQuery.password)
                    {
                        passwords1[i].email = newEmail;
                        sw.WriteLine($"{passwords1[i].password} {passwords1[i].username} {newEmail} {passwords1[i].website}");
                    }
                    else
                        sw.WriteLine($"{passwords1[i].password} {passwords1[i].username} {passwords1[i].email} {passwords1[i].website}");
                }
            }

            Console.Clear();
            Menu();
        }

        static void NewUsername()
        {
            Console.Write("Please enter the new username: ");
            string newUsername = Console.ReadLine();

            using (StreamWriter sw = new StreamWriter("n.txt", false))
            {
                for (int i = 0; i < n; i++)
                {
                    //átirandó
                    if (passwords1[i].password == actualQuery.password)
                    {
                        passwords1[i].username = newUsername;
                        sw.WriteLine($"{passwords1[i].password} {newUsername} {passwords1[i].email} {passwords1[i].website}");
                    }
                    else
                        sw.WriteLine($"{passwords1[i].password} {passwords1[i].username} {passwords1[i].email} {passwords1[i].website}");
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.Clear();
            Menu();
        }

        static void Delete() //Jelszó adatok törlése
        {
            bool found = false;

            for (int i = 0; i < n; i++)
            {
                if (passwords1[i].password == actualQuery.password)
                {
                    found = true;
                    passwords.RemoveAt(i);
                }
            }

            using (StreamWriter sw = new StreamWriter("n.txt", false)) //újraírja a fájlt
            {
                foreach (Passwords item in passwords)
                {
                    sw.WriteLine($"{item.password} {item.username} {item.email} {item.website}");
                }
            }

            if (found)
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Password successfully deleted!");
                Console.ResetColor();
                Menu();
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

        /* EHHEZ SE NYÚLJ!!!!
        static void Characters()
        {
            StreamWriter sw = new StreamWriter("trans.txt", false);
            
            for (int j = 0; j < chars.Length; j++)
            {
                Random r = new Random();
                string randomCode = "";

                while (randomCode.Count() < 10)
                {
                    index = r.Next(0, chars.Count());
                    randomCode += chars[index];                    
                }
                sw.WriteLine($"{chars[j]} {randomCode}");
            }
            sw.Close();
        }
        */

        static void CheckForFile(string file)
        {
            if (!File.Exists(file))
            {
                FileStream fs = File.Create("n.txt");
                fs.Close();
            }
        }

        static void FileDelete(string file) //DEBUG ONLY, fájl törlés
        {
            File.Delete(file);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Password File Deleted!");
            Console.ResetColor();

            Menu();
        }

        static void FileExport() //DEBUG ONLY, fájl exportálás
        {
            File.Create("export.txt").Close();
            StreamWriter sw = new StreamWriter("export.txt", false);

            foreach (Passwords item in passwords)
                sw.WriteLine($"{item.password} {item.username} {item.email} {item.website}");
            sw.Close();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Password File Exported!");
            Console.ResetColor();
            Thread.Sleep(1000);
            Menu();
        }
    }
}