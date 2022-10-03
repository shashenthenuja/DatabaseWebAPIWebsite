using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLib
{
    public class DatabaseClass
    {
        List<DataStruct> dataStruct;

        public DatabaseClass()
        {
            dataStruct = new List<DataStruct>();
            generateData();
        }

        public uint getAcctNoByIndex(int index)
        {
            return dataStruct.ElementAt(index).acctNo;
        }

        public uint getPINByIndex(int index)
        {
            return dataStruct.ElementAt(index).pin;
        }

        public string getFirstNameByIndex(int index)
        {
            return dataStruct.ElementAt(index).firstName;
        }

        public string getLastNameByIndex(int index)
        {
            return dataStruct.ElementAt(index).firstName;
        }

        public int getBalance(int index)
        {
            return dataStruct.ElementAt(index).balance;
        }

        public int GetNumRecords()
        {
            return dataStruct.Count;
        }

        public void generateData()
        {
            Database data = new Database();
            for (int i = 0; i < 100; i++)
            {
                DataStruct user = new DataStruct();
                data.getNextAccount(out user.pin, out user.acctNo, out user.firstName, out user.lastName, out user.balance, out user.image);
                dataStruct.Add(user);
            }
        }

        public List<DataStruct> getList()
        {
            return dataStruct;
        }

    }
}
