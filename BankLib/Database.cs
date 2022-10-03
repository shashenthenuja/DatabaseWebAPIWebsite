using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLib
{
    public class Database
    {
        private static Random rnd = new Random();
        private string GetFirstname()
        {
            string[] firstNames = new string[] { "Bob", "John", "Sarah", "Alex", "Austin" };
            int index = rnd.Next(firstNames.Length);
            return firstNames[index];
        }

        private string GetLastname()
        {
            string[] lastNames = new string[] { "Spencer", "Anderson", "Clay", "Morrow", "Perkins" };
            int index = rnd.Next(lastNames.Length);
            return lastNames[index];
        }

        private uint getPin()
        {
            return (uint)rnd.Next(1000, 9999);
        }

        private uint getAcctNo()
        {
            return (uint)rnd.Next(100000, 999999);
        }

        private int getBalance()
        {
            return rnd.Next(0, 9999);
        }

        private string getImageUrl()
        {
            int picId = rnd.Next(100, 200);
            string url = "https://picsum.photos/id/" + picId.ToString() + "/200/";
            return url;
        }

        public void getNextAccount(out uint pin, out uint acctNo, out string firstName, out string lastName, out int balance, out string image)
        {
            pin = getPin();
            acctNo = getAcctNo();
            firstName = GetFirstname();
            lastName = GetLastname();
            balance = getBalance();
            image = getImageUrl();
        }
    }
}
