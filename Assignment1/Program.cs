using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Security;
using System.Net.Mail;
using System.Net;




namespace Assignment1
{
    
    class Program
    {
        List<Account> accountList = new List<Account>();
        static void Main(string[] args)
        {
            
            //basic UI
            
            Console.WriteLine(" ");
            Console.WriteLine("Welcome to Simple Banking \n" +
               "Log in to start \n" +
               "Username: \n" +
               "Password: \n");


            Console.SetCursorPosition(11, 3);
            string username = Console.ReadLine();
            Console.SetCursorPosition(11, 4);
           
            SecureString securePassword = MaskInputString();

            //convert SecureString to a normal String to compare in log in file later
            string password = new System.Net.NetworkCredential(string.Empty, securePassword).Password;


            //read login.txt file and try to log in

            string docPath = System.AppDomain.CurrentDomain.BaseDirectory;

            string fileName = "login.txt";

            string[] fileLine = File.ReadAllLines(docPath + "\\" + fileName);

            foreach (var value in fileLine)
            {
                string[] newVal = value.Split("|");
                if(newVal[0] == username)
                {
                    if(newVal[1] == password)
                    {
                        
                        //Email not working 
                        /*
                         * 
                        var smtpClient = new SmtpClient("smtp.gmail.com")
                        {
                            Port = 587,
                            Credentials = new NetworkCredential("harrisonbenn4@gmail.com", "ujoyhjtpyukubkao"),
                            EnableSsl = true,
                        };

                        smtpClient.Send("harrisonbenn4@gmail.com", "harrisonbennlol@gmail.com", "ASsignment1", "testing");

                         * 
                         */



                        Program program = new Program();
                        
                        program.DisplayInfoScreen();
                        
                    }
                   
                }
            }
            
            Console.WriteLine("Invalid Login.");
            Console.ReadKey();
            Console.Clear();
            Main(args);


            static SecureString MaskInputString()
            {
                //make a secure string to store the password
                SecureString pass = new SecureString();
                ConsoleKeyInfo keyInfo;
                do
                {
                    keyInfo = Console.ReadKey(true);
                    //add the character to the pass variable and display an '*' on the screen instead
                    if (!char.IsControl(keyInfo.KeyChar))
                    {
                        pass.AppendChar(keyInfo.KeyChar);
                        Console.Write("*");
                    }
                    //if backspace pressed then reduce size of pass by 1
                    else if (keyInfo.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass.RemoveAt(pass.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                //check while enter key hasnt been pressed. if so then finish
                while (keyInfo.Key != ConsoleKey.Enter);
                {
                    return pass;
                }
            }

            
            
        }
        public void DisplayInfoScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("| Welcome to the banking system\n" +
                "| 1. Create a new account\n" +
                "| 2. Search for an account\n" +
                "| 3. Deposit\n" +
                "| 4. Withdraw\n" +
                "| 5. A/C Statement\n" +
                "| 6. Delete Account\n" +
                "| 7. Exit\n");
            Console.Write("| Choose an option (1-7): ");

            int choice = -1;
            try
            {
                choice = Int32.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("| Invalid choice");
                Console.ReadKey();

                ClearToDisplay();
            }
            if (choice < 0 || choice > 7)
            {
                ClosingStatement("Invalid input");

                ClearToDisplay();
            }

            if (choice == 1)
            {
                CreateNewAccount();
            }
            else if (choice == 2)
            {
                SearchAccount();
            }
            else if (choice == 3)
            {
                Deposit();
            }
            else if (choice == 4)
            {
                Withdraw();
            }
            else if (choice == 5)
            {
                ACStatement();
            }
            else if (choice == 6)
            {
                DeleteAccount();
            }
            else if (choice == 7)
            {
                Environment.Exit(0);
            }
        }
        void CreateNewAccount()
        {
            //Display info screen, use the setCursor to place inputs in right space
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("| First Name: \n" +
                "| Last Name: \n" +
                "| Address \n" +
                "| Phone \n" +
                "| Email \n" +
                "| Is this correct ? y / n \n" +
                "| \n");

            Console.SetCursorPosition(27, 2);

            string FName = Console.ReadLine();
            Console.SetCursorPosition(27, 3);

            string LName = Console.ReadLine();
            Console.SetCursorPosition(27, 4);

            string Address = Console.ReadLine();
            Console.SetCursorPosition(27, 5);

            int Phone = -1;
            int balance = 0;
            try
            {
                Phone = Int32.Parse(Console.ReadLine());
            }
            catch
            {
                ClosingStatement("Invalid phone number");
                ClearToCreateNewAccount();
            }

            Console.SetCursorPosition(27, 6);
            string Email = Console.ReadLine();

            //ask if correct
            Console.SetCursorPosition(27, 7);

            string correct = Console.ReadLine();

            //check if phone is right length and contains email parameters (@, gmail, uts, etc)

            if (correct == "y" && Phone.ToString().Length <= 10 && (Email.Contains("@") && (Email.Contains("gmail.com") || Email.Contains("uts.edu.au") || Email.Contains("outlook.com"))))
            {

                Random rnd = new Random();
                int accountNumber = rnd.Next(100000, 99999999);
               

                while(searchForAccountInList(accountNumber) == null)
                {
                    accountNumber = rnd.Next(100000, 99999999);
                }
                Account newAccount = new Account(FName, LName, Address, Phone, Email, balance, accountNumber);

                //add account to list
                accountList.Add(newAccount);
                newAccount.MakeFile();
                
                
                //email account details to email id provided
                //SendEmail();



                Console.WriteLine("| Your banking number: " + accountNumber);
                Console.WriteLine();
                Console.ReadKey();

                //CLEAR SCREEN ON KEY PRESS AND GO BACK TO MENU
                ClearToDisplay();


            }
            else
            {
                ClosingStatement("Invalid details");
                ClearToCreateNewAccount();
            }

        }
        void SearchAccount()
        {
            Console.Clear();
            Console.Write("Account Number: ");
            int findAccount = 0;
            //check valid account number to find
            try
            {
                findAccount = Int32.Parse(Console.ReadLine());
            }
            catch
            {
                ClosingStatement("Invalid number");

                ClearToSearchAccount();
            }


            if (!(findAccount.ToString().Length > 10))
            {
                //search for <accountNum>.txt

                //check if file exists
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\Uni\\.Net\\AccountNumbers";
                string fileName = findAccount.ToString() + ".txt";
                if (File.Exists(docPath + "\\" + fileName));
                {
                    //print each line of the file
                    string[] contents = System.IO.File.ReadAllLines(docPath + "\\" + findAccount.ToString() + ".txt");
                    foreach (string line in contents)
                    {
                        Console.WriteLine("\t" + line);
                    }

                    //check if user wants to view another account
                    Console.WriteLine("Check another account (y/n)?");
                    string checkAgain = Console.ReadLine();
                    if (checkAgain == "y")
                    {
                        ClearToSearchAccount();
                    }
                    else
                    {
                        ClearToDisplay();
                    }
                }

                //clear screen on input
                ClosingStatement("Account not found");

                ClearToDisplay();



            }
            //run if account number is too long
            else
            {
                Console.WriteLine("Account Number too long");
                Console.Write("Check another account (y/n)?");
                string checkAgain = Console.ReadLine();
                if (checkAgain == "y")
                {
                    ClearToSearchAccount();
                }
                else
                {
                    ClearToDisplay();
                }


            }





        }

        void Deposit()
        {
            Console.Clear();
            Console.WriteLine("Amount to Deposit: ");
            Console.WriteLine("Account number: ");

            int accountNumber = -1;
            int amount = 0;
            try
            {
                Console.SetCursorPosition(20, 0);

                amount = Int32.Parse(Console.ReadLine());
                Console.SetCursorPosition(20, 1);

                accountNumber = Int32.Parse(Console.ReadLine());

            }
            catch
            {
                ClosingStatement("Invalid number, try again");
                Deposit();
            }

            if (!(accountNumber.ToString().Length > 10 || amount < 0))
            {
                Account thisAcc = searchForAccountInList(accountNumber);

                if (thisAcc == null)
                {
                    ClosingStatement("No account, try again");

                    Deposit();
                }
                

               
                thisAcc.AddBalance(amount);
                thisAcc.UpdateFileBalance();
                thisAcc.AddToFile("Deposit", amount.ToString());
                

                Console.WriteLine("Would you like to deposit again (y/n)? ");
                string answer = Console.ReadLine();
                if (answer == "y")
                {
                    Console.Clear();
                    Deposit();
                }
                ClearToDisplay();


            }
        }

            void Withdraw()
        {
            Console.Clear();
            Console.WriteLine("Amount to Withdraw: ");
            Console.WriteLine("Account number: ");

            Console.SetCursorPosition(15, 10);
            int accountNumber = -1;
            int amount = 0;
            try
            {
                Console.SetCursorPosition(20, 0);
                amount = Int32.Parse(Console.ReadLine());
                Console.SetCursorPosition(20, 1);
                accountNumber = Int32.Parse(Console.ReadLine());
               

            }
            catch
            {

                ClosingStatement("Invalid number, try again");
                
                Withdraw();
            }

            if (!(accountNumber.ToString().Length > 10 || amount < 0))
            {
                Account thisAcc = searchForAccountInList(accountNumber);

                if (thisAcc == null)
                {
                    Console.Write("No Account, Try again");
                    Console.ReadKey();
                    Withdraw();
                }
                //returns true if after withdrawl balance is > 0

                if (thisAcc.RemoveBalance(amount))
                {
                   
                    thisAcc.UpdateFileBalance();
                    thisAcc.AddToFile("Withdraw", amount.ToString());
                }
                else
                {
                    Console.WriteLine("Insufficient Funds");
                }

                Console.WriteLine("Would you like to withdraw again (y/n)? ");
                string answer = Console.ReadLine();
                if (answer == "y")
                {
                    Console.Clear();
                    Withdraw();
                }
                ClearToDisplay();


            }
        }

        Account searchForAccountInList(int accountNumber)
        {
            for (int i = 0; i < accountList.Count; i++)
            {

                if (accountList[i].GetAccountNum() == accountNumber)
                {
                    return accountList[i];
                }
            }
            return null;
        }
    
        void ACStatement()
        {
            Console.Clear();
            Console.WriteLine("Enter account number");
            int accountNumber = 0 ;
            try
            {
                accountNumber = Int32.Parse(Console.ReadLine());
            }
            catch
            {
                ClosingStatement("Invalid number, try again");
                
                ACStatement();
            }
            
            Account thisAcc = searchForAccountInList(accountNumber);
            if(thisAcc == null)
            {
                Console.WriteLine("Invalid Account Number");
                Console.WriteLine("Would you like to try again (y/n)?");
                string input = Console.ReadLine();
                if (input == "y")
                {

                    ACStatement();
                }
                ClearToDisplay();
            }
            //display thisAcc statement + 5 transactions
            thisAcc.DisplayStatement();

            Console.WriteLine("Would you like to send email (y/n)? ");
            string answer = Console.ReadLine();
            if (answer == "y")
            {
                Console.WriteLine("SENDING EMAIL");
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("harrisonbenn4@gmail.com", "ujoyhjtpyukubkao"),
                    EnableSsl = true,
                };

                smtpClient.Send("harrisonbenn4@gmail.com", "harrisonbennlol@gmail.com", "ASsignment1", "testing");






                ClearToDisplay();
            
            }
            else
            {
                Console.Clear();
                ACStatement();
            }



            //email to client
            


        }
        void DeleteAccount()
        {
            Console.Clear();
            Console.WriteLine("Enter account number");
            int accountNumber = 0;
            try
            {
                accountNumber = Int32.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Invalid Account Number, Try again");
                Console.WriteLine("Would you like to deposit again (y/n)? ");
                string answer = Console.ReadLine();
                if (answer == "y")
                {
                    Console.Clear();
                    DeleteAccount();
                }
                ClearToDisplay();
            }

            Account searchAcc = searchForAccountInList(accountNumber);

            searchAcc.DisplayAccountDetails();

            
            
            Console.WriteLine("Would you like to delete this account (y/n)? ");
            string input = Console.ReadLine();
            if (input == "y")
            {
                searchAcc.RemoveFile();
                accountList.Remove(searchAcc);
                ClosingStatement("Account Deleted!");
                
            }
            
            ClearToDisplay();



        }

        void ClearToDisplay()
        {
            Console.Clear();
            DisplayInfoScreen();
        }
        void ClearToSearchAccount()
        {
            Console.Clear();
            SearchAccount();
        }
        void ClearToCreateNewAccount()
        {
            Console.Clear();
            CreateNewAccount();
        }

        void ClosingStatement(string statement)
        {
            Console.WriteLine(statement);
            Console.ReadKey();
            Console.Clear();
        }
        

    }
}
