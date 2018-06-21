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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        private void Handle()
        {
            String email = "";
            Email.Dispatcher.Invoke(new Action(() => email = Email.Text));
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
            var filter = builder.Eq("mailid", email) & builder.Eq("password", password);
            results = Collection.Find(filter).ToList();
            Console.Write(results);
            if (results.Count == 1)
            {
                this.Dispatcher.Invoke(new Action(()=>afterlogin = new AfterLogin(email, password)));
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
            DisplayWarning("Processing your request....",30000);
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
            Console.WriteLine(password);
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
