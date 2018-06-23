using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Security.Cryptography;

namespace SecureTravel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private AfterLogin afterlogin;
        private Timer timer = new Timer()
        {
            Interval = 3000 // it will Tick in 3 seconds
        };
        private IMongoClient Client;
        private IMongoDatabase Database;
        private IMongoCollection<BsonDocument> Collection;
        List<BsonDocument> results;
        private void DisplayWarning(String message,int Interval=3000)
        {
            timer.Interval = Interval;
            warning.Dispatcher.Invoke(new Action(() => warning.Content = message));            
            warning.Dispatcher.Invoke(new Action(() => warning.Visibility = Visibility.Visible));
            timer.Elapsed += (s, en) => {
                warning.Dispatcher.Invoke(new Action(() => warning.Visibility = Visibility.Hidden));
                timer.Stop();
            };
            timer.Start();
        }
        private string Hash(string stringToHash)
        {
            using (var sha1 = new SHA1Managed())
            {
                return BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(stringToHash)));
            }
        }
        private void Handle()
        {
            String email = "";
            Email.Dispatcher.Invoke(new Action(() => email = Email.Text.ToLower()));
            String password = "";
            TPassword.Dispatcher.Invoke(new Action(() => password = TPassword.Password));
            if (SecureTravel.Signup.IsValidEmail(email) == false)
            {
                DisplayWarning("Not a valid Mail id!");
                return;
            }
            if (password.Equals(""))
            {
                DisplayWarning("Password is empty!");             
                return;
            }
            Client = new MongoClient("mongodb://jjsridharan:test123@ds016068.mlab.com:16068/securetravel");
            Database = Client.GetDatabase("securetravel");
            Collection = Database.GetCollection<BsonDocument>("user");
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("mailid", email) & builder.Eq("password", Hash(password));
            results = Collection.Find(filter).ToList();
            if (results.Count == 1)
            {
                this.Dispatcher.Invoke(new Action(()=>afterlogin = new AfterLogin(email, password,results[0]["username"].ToString())));
                afterlogin.Dispatcher.Invoke(new Action(() => afterlogin.Show()));
                this.Dispatcher.Invoke(new Action(() => this.Close()));                
            }
            else
            {
                DisplayWarning("Mail id or Password is wrong");
            }
        }
        private void Login(object sender, RoutedEventArgs e)
        {
            warning.Content="Processing your request....";
            warning.Visibility = Visibility.Visible;
            Task.Run(()=>Handle());           
        }

        private void User_Lost_Focus(object sender, RoutedEventArgs e)
        {
            String email = ((TextBox)sender).Text;
            if(email.Equals("")==true)
            {
                ((TextBox)sender).Text = "Email Id";
            }
        }

        private void Password_Lost_Focus(object sender, RoutedEventArgs e)
        {
            String password=TPassword.Password;
            if(password.Equals(""))
            {
                Password.Visibility = System.Windows.Visibility.Visible;
                TPassword.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void User_Got_Focus(object sender, RoutedEventArgs e)
        {
            String email = ((TextBox)sender).Text;
            if (email.Equals("Email Id") == true)
            {
                ((TextBox)sender).Text = "";
            }
        }

        private void Password_Got_Focus(object sender, RoutedEventArgs e)
        {
            if ((TPassword.Password).Equals("") == true)
            {
                Password.Visibility = System.Windows.Visibility.Hidden;
                TPassword.Visibility = System.Windows.Visibility.Visible;
                TPassword.Focus();
            }
        }

        private void Signup(object sender, RoutedEventArgs e)
        {
            new Signup().Show();
            this.Close();
        }

     
    }
}
