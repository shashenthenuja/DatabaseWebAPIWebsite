using DataAccessLayer.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string searchText;
        public RestClient client;
        private RestResponse numOfThings;
        private List<BankData> list;
        public string URL = null;
        public MainWindow()
        {
            InitializeComponent();

            // Default URL
            if (URL == null)
            {
                URL = "http://localhost:52604/";
                client = new RestClient(URL);
            }
            RestRequest request = new RestRequest("api/getalldata");
            numOfThings = client.Get(request);
            list = JsonConvert.DeserializeObject<List<BankData>>(numOfThings.Content);
            if (list.Count == 0)
            {
                RestRequest request2 = new RestRequest("api/generatedata/?index=1", Method.Post);
                RestResponse restResponse = client.Execute(request2);
                var data = (JObject)JsonConvert.DeserializeObject(restResponse.Content, new JsonSerializerSettings() { DateParseHandling = DateParseHandling.None });
                var status = (string)data["Status"];
                if (status.Equals("Success"))
                {
                    MessageBox.Show("Generated 100 Records!");
                    TotalNum.Text = "Total Records : 100";
                }
            }
            else
            {
                TotalNum.Text = "Total Records : " + list.Count.ToString();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            //On click, Get the index....
            bool isNumber = int.TryParse(IndexNum.Text, out int n);
            if (isNumber)
            {
                try
                {
                    RestRequest request = new RestRequest("api/getalldata");
                    RestResponse resp = client.Get(request);
                    List<BankData> data = JsonConvert.DeserializeObject<List<BankData>>(resp.Content);

                    index = Int32.Parse(IndexNum.Text);
                    int length = data.Count;
                    if (index > 0 && index <= length)
                    {
                        foreach (BankData item in data)
                        {
                            if (item.Id.Equals(index))
                            {
                                uint pin = (uint)item.Pin;
                                uint balance = (uint)item.Balance;
                                FNameBox.Text = item.FirstName;
                                LNameBox.Text = item.LastName;
                                BalanceBox.Text = balance.ToString("C");
                                AcctNoBox.Text = item.AccNum.ToString();
                                PinBox.Text = pin.ToString("D4");
                                image.Source = new BitmapImage(new Uri(item.Image));
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " Invalid URL!");
                }
            }
        }

        private async void findBtn_Click(object sender, RoutedEventArgs e)
        {
            bool isText = Regex.IsMatch(searchBox.Text, "[a-zA-Z]");
            if (isText)
            {
                searchText = searchBox.Text;
                //On click, Get the data....
                Task<BankData> task = new Task<BankData>(SearchName);
                // Display LoadBar and lock textboxes
                findBtn.IsEnabled = false;
                goBtn.IsEnabled = false;
                FNameBox.IsReadOnly = true;
                LNameBox.IsReadOnly = true;
                BalanceBox.IsReadOnly = true;
                AcctNoBox.IsReadOnly = true;
                PinBox.IsReadOnly = true;
                searchBox.IsReadOnly = true;
                IndexNum.IsReadOnly = true;
                loadBar.Visibility = Visibility.Visible;
                loadBar.IsIndeterminate = true;
                task.Start();
                BankData db = await task;
                loadBar.IsIndeterminate = false;
                loadBar.Visibility = Visibility.Hidden;
                DisplaySearchData(db);
                findBtn.IsEnabled = true;
                goBtn.IsEnabled = true;
                FNameBox.IsReadOnly = false;
                LNameBox.IsReadOnly = false;
                BalanceBox.IsReadOnly = false;
                AcctNoBox.IsReadOnly = false;
                PinBox.IsReadOnly = false;
                searchBox.IsReadOnly = false;
                IndexNum.IsReadOnly = false;
            }
            else
            {
                searchBox.Text = "Enter a Name!";
            }
        }

        private void DisplaySearchData(BankData data)
        {
            bool isText = Regex.IsMatch(searchBox.Text, "[a-zA-Z]");
            if (isText)
            {
                if (data != null)
                {
                    if (data.LastName != null)
                    {
                        //Set the values in the GUI!
                        image.Source = new BitmapImage(new Uri(data.Image));
                        uint pin = (uint)data.Pin;
                        uint balance = (uint)data.Balance;
                        FNameBox.Text = data.FirstName;
                        LNameBox.Text = data.LastName;
                        BalanceBox.Text = balance.ToString("C");
                        AcctNoBox.Text = data.AccNum.ToString();
                        PinBox.Text = pin.ToString("D4");
                        IndexNum.Text = data.Id.ToString();
                    }
                    else
                    {
                        searchBox.Text = "Not Found!";
                    }
                }
                else
                {
                    MessageBox.Show("Something Went Wrong!");
                }

            }
        }

        private BankData SearchName()
        {
            try
            {
                //On click, Get the data....
                RestRequest request = new RestRequest("api/searchdata/?name=" + searchText);
                RestResponse resp = client.Post(request);
                BankData result = JsonConvert.DeserializeObject<BankData>(resp.Content);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    searchBox.Text = "Not Found!";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Invalid URL!");
            }
            return null;
        }

        private void insertBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op;
            if (!FNameBox.Text.Equals("") && !LNameBox.Text.Equals("") && !BalanceBox.Text.Equals("") && !AcctNoBox.Text.Equals("") && !PinBox.Text.Equals(""))
            {
                string fname = FNameBox.Text;
                string lname = LNameBox.Text;
                int accNo = 0;
                int pin = 0;
                int balance = 0;
                try
                {
                    accNo = Int32.Parse(AcctNoBox.Text);
                    pin = Int32.Parse(PinBox.Text);
                    balance = Int32.Parse(BalanceBox.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Enter valid data!");
                }
                
                op = new OpenFileDialog();
                op.Title = "Select a profile picture";
                op.Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif";
                if (op.ShowDialog() == true)
                {
                    image.Source = new BitmapImage(new Uri(op.FileName));
                    RestRequest request = new RestRequest("api/getalldata");
                    RestResponse resp = client.Get(request);
                    List<BankData> data = JsonConvert.DeserializeObject<List<BankData>>(resp.Content);
                    int index = data.Count + 1 ;
                    BankData bankData = new BankData();
                    bankData.Id = index;
                    bankData.FirstName = fname;
                    bankData.LastName = lname;
                    bankData.AccNum = accNo;
                    bankData.Pin = pin;
                    bankData.Balance = balance;
                    bankData.Image = op.FileName;
                   
                    RestRequest restRequest = new RestRequest("api/adddata", Method.Post);
                    restRequest.AddJsonBody(JsonConvert.SerializeObject(bankData));
                    RestResponse restResponse = client.Execute(restRequest);
                    BankData result = JsonConvert.DeserializeObject<BankData>(restResponse.Content);
                    if (result != null)
                    {
                        TotalNum.Text = "Total Records : " + index.ToString();
                        MessageBox.Show("Data Successfully Inserted");
                    }
                    else
                    {
                        MessageBox.Show("Error details:" + restResponse.Content);
                    }
                }

            }
            else
            {
                MessageBox.Show("Please enter all data..");
            }
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op;
            if (!FNameBox.Text.Equals("") && !LNameBox.Text.Equals("") && !BalanceBox.Text.Equals("") && !AcctNoBox.Text.Equals("") && !PinBox.Text.Equals(""))
            {
                string fname = FNameBox.Text;
                string lname = LNameBox.Text;
                int accNo = 0;
                int pin = 0;
                int balance = 0;
                int index = 0;
                try
                {
                    index = Int32.Parse(IndexNum.Text);
                    accNo = Int32.Parse(AcctNoBox.Text);
                    pin = Int32.Parse(PinBox.Text);
                    balance = Int32.Parse(BalanceBox.Text);

                    op = new OpenFileDialog();
                    op.Title = "Select a profile picture";
                    op.Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif";
                    if (op.ShowDialog() == true)
                    {
                        BankData bankData = new BankData();
                        bankData.Id = index;
                        bankData.FirstName = fname;
                        bankData.LastName = lname;
                        bankData.AccNum = accNo;
                        bankData.Pin = pin;
                        bankData.Balance = balance;
                        bankData.Image = op.FileName;

                        DisplaySearchData(bankData);
                        RestRequest restRequest = new RestRequest("api/updatedata/?id=" + index, Method.Post);
                        restRequest.AddJsonBody(JsonConvert.SerializeObject(bankData));
                        client.Execute(restRequest);
                        MessageBox.Show("Data Successfully Updated");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Enter valid data!");
                }
            }
            else
            {
                MessageBox.Show("Please enter all data..");
            }
        }

        private void generateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RestRequest request = new RestRequest("api/getalldata");
                RestResponse resp = client.Get(request);
                List<BankData> list = JsonConvert.DeserializeObject<List<BankData>>(resp.Content);

                int index = list.Count + 1;

                RestRequest request2 = new RestRequest("api/generatedata/?index=" + index.ToString(), Method.Post);
                RestResponse restResponse = client.Execute(request2);
                var data = (JObject)JsonConvert.DeserializeObject(restResponse.Content, new JsonSerializerSettings() { DateParseHandling = DateParseHandling.None });
                var status = (string)data["Status"];
                if (status.Equals("Success"))
                {
                    MessageBox.Show("Generated 100 Records!");
                    int total = list.Count + 100;
                    TotalNum.Text = "Total Records : "  + total.ToString();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something Went Wrong!");
            }
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = Int32.Parse(IndexNum.Text);
                RestRequest restRequest = new RestRequest("api/deletedata/?id=" + index, Method.Delete);
                RestResponse restResponse = client.Execute(restRequest);
                BankData result = JsonConvert.DeserializeObject<BankData>(restResponse.Content);
                if (result != null)
                {
                    RestRequest request = new RestRequest("api/getalldata");
                    RestResponse resp = client.Get(request);
                    List<BankData> list = JsonConvert.DeserializeObject<List<BankData>>(resp.Content);

                    TotalNum.Text = "Total Records : " + list.Count;
                    MessageBox.Show("Data Successfully Deleted");
                }
                else
                {
                    MessageBox.Show("Error details:" + restResponse.Content);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something Went Wrong!");
            }

        }

        public string FactorString(string val)
        {
            string pattern = @"^(\[){1}(.*?)(\]){1}$";
            if (val != null)
            {
                return Regex.Replace(val, pattern, "$2");
            }
            else
            {
                return null;
            }

        }
    }
}
