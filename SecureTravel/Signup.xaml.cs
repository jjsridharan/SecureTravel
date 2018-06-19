using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SecureTravel
{
    /// <summary>
    /// Interaction logic for Signup.xaml
    /// </summary>
    
    public partial class Signup : Window
    {
        private IMongoClient Client;
        private IMongoDatabase Database;
        private IMongoCollection<BsonDocument> Collection;
        public Signup()
        {
            InitializeComponent();
        }
        private void Login(object e, RoutedEventArgs senders)
        {
            new MainWindow().Show();
            this.Close();
        }
        private void username_got_focus(object e, RoutedEventArgs senders)
        {
            if(((TextBox)e).Text.Equals("Username")==true)
            {
                ((TextBox)e).Text = "";
            }
        }
        private void username_lost_focus(object e, RoutedEventArgs senders)
        {
            if (((TextBox)e).Text.Equals("") == true)
            {
                ((TextBox)e).Text = "Username";
            }
        }
        private void mailid_got_focus(object e, RoutedEventArgs senders)
        {
            if (((TextBox)e).Text.Equals("Mail Id") == true)
            {
                ((TextBox)e).Text = "";
            }
        }

        private void mailid_lost_focus(object e, RoutedEventArgs senders)
        {
            if (((TextBox)e).Text.Equals("") == true)
            {
                ((TextBox)e).Text = "Mail Id";
            }
        }
      
        private void Opassword_lost_focus(object e, RoutedEventArgs senders)
        {
            if (((PasswordBox)e).Password.Equals("") == true)
            {
                Opassword.Visibility = Visibility.Hidden;
                Password.Visibility = Visibility.Visible;
            }
        }
       
        private void OCpassword_lost_focus(object e, RoutedEventArgs senders)
        {
            if (((PasswordBox)e).Password.Equals("") == true)
            {
                OCpassword.Visibility = Visibility.Hidden;
                CPassword.Visibility = Visibility.Visible;
            }
        }
        private void password_got_focus(object e, RoutedEventArgs senders)
        {
            if (((TextBox)e).Text.Equals("Password") == true)
            {
                Password.Visibility = Visibility.Hidden;
                Opassword.Visibility = Visibility.Visible;
                Opassword.Focus();
            }
        }
       
        private void Cpassword_got_focus(object e, RoutedEventArgs senders)
        {
            if (((TextBox)e).Text.Equals("Confirm Password") == true)
            {
                CPassword.Visibility = Visibility.Hidden;
                OCpassword.Visibility = Visibility.Visible;
                OCpassword.Focus();
            }
        }
        private void User_Signup(object e, RoutedEventArgs senders)
        {
            String username = this.username.Text;
            String mailid = this.mailid.Text;
            String password = this.Opassword.Password;
            Client = new MongoClient("mongodb://jjsridharan:test123@ds016068.mlab.com:16068/securetravel");
            Database = Client.GetDatabase("securetravel");
            Collection = Database.GetCollection<BsonDocument>("user");
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("username", username);
            var results = Collection.Find(filter).ToList();
            Console.Write(results.ToJson());
            if (results.Count == 0)
            {
                var document = new BsonDocument{
                    { "username" , username},
                    { "mailid" , mailid },
                    { "password", password }
                };
                Collection.InsertOne(document);
                Console.WriteLine("Inserted");
            }
        }
    }
}
