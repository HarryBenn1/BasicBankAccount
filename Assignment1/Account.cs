using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Assignment1
{

    public class Account
    {
        //------------------------------------------------------------------------------------------------

        //------------------------------------------------------------------------------------------------



        string firstName;
        string lastName;
        string address;
        int phone;
        string email;
        int balance;
        int accountNumber;

        string fileName;
        string docPath = System.AppDomain.CurrentDomain.BaseDirectory;

        public Account(string firstName, string lastName, string address, int phone, string email, int balance, int accountNumber)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.address = address;
            this.phone = phone;
            this.email = email;
            this.balance = balance;
            this.accountNumber = accountNumber;
            fileName = accountNumber.ToString() + ".txt";
        }

        public string GetFName()
        {
            return this.firstName;
        }
        public string GetLName()
        {
            return this.lastName;
        }
        public string GetAddress()
        {
            return this.address;
        }
        public int GetPhone()
        {
            return this.phone;
        }
        public string GetEmail()
        {
            return this.email;
        }
        public int GetBalance()
        {
            return this.balance;
        }
        public int GetAccountNum()
        {
            return this.accountNumber;
        }
        public void AddBalance(int amount)
        {
            this.balance = this.balance + amount;
        }
        public bool RemoveBalance(int amount)
        {
            if(this.balance - amount < 0)
            {
                return false;
            }
            this.balance = this.balance - amount;
            return true;
        }
        //parameters: transaction type "withdraw"/"deposit"
        //            amount   in string
        public void AddToFile(string transaction, string amount)
        {
            //add each transaction to file
            Console.WriteLine("Adding to file");
            using (StreamWriter sw = File.AppendText(docPath + "\\" + fileName))
            {
                
                sw.WriteLine(DateTime.Now.ToString() + "|" + transaction + "|" + amount + "|" + this.balance.ToString());

            }
        }
        public void UpdateFileBalance()
        {
            
            string[] arrLine = File.ReadAllLines(docPath + "\\" + fileName);
            arrLine[6] = "Balance|" + this.GetBalance();
            File.WriteAllLines(docPath + "\\" + fileName, arrLine);

            
            
        }
        public void MakeFile()
        {
            
            //UPDATE THIS ACCOUNTS FILE
            //if a file already exists(which it shouldnt) then delete it. Acc numbers cant be shared
            if (File.Exists(docPath + "\\" + fileName))
            {
            
                File.Delete(docPath + "\\" + fileName);
            }

            using (StreamWriter sw = File.CreateText(docPath + "\\" + fileName))
            {
                sw.WriteLine("First Name|" + this.GetFName());
                sw.WriteLine("Last Name|" + this.GetLName());
                sw.WriteLine("Address|" + this.GetAddress());
                sw.WriteLine("Phone|" + this.GetPhone());
                sw.WriteLine("Email|" + this.GetEmail());
                sw.WriteLine("AccountNumber|" + this.GetAccountNum());
                sw.WriteLine("Balance|" + this.GetBalance());



            }
        }
        public void DisplayAccountDetails()
        {
            Console.WriteLine("First Name: " + this.GetFName());
            Console.WriteLine("Last Name: " + this.GetLName());
            Console.WriteLine("Address: " + this.GetAddress());
            Console.WriteLine("Phone: " + this.GetPhone());
            Console.WriteLine("Email: " + this.GetEmail());
            Console.WriteLine("Account Number: " + this.GetAccountNum());
            Console.WriteLine("Balance: " + this.GetBalance());
        }
        public void DisplayStatement()
        {
            //displays account data
            DisplayAccountDetails();

            string[] fileLine = File.ReadAllLines(docPath + "\\" + fileName);
            //main file is at least 7 rows long
            int i;
            int counter = 0;
            //reverse through the array, making sure i doesnt go lower than 7 because thats the account information
            for(i = fileLine.Length-1; i >= 7; i--)
            {
                Console.WriteLine(fileLine[i]);
                //counter to make sure only 5 last transactions are printed
                counter++;
                if(counter >= 5)
                {
                    break;
                }
            }

        }
        public void RemoveFile()
        {
            if (File.Exists(docPath + "\\" + fileName))
            {

                File.Delete(docPath + "\\" + fileName);
            }
        }
        

    }

}
