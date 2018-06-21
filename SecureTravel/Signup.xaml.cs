﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Timers;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
        private Timer timer = new Timer()
        {
            Interval = 3000 // it will Tick in 3 seconds
        };
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
        private void DisplayWarning(String message, int Interval = 3000)
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
        public static bool IsValidEmail(string email)
        {
            Regex rx = new Regex(
            @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");
            return rx.IsMatch(email);
        }
        private void Handle()
        {
            String username = "";
            this.username.Dispatcher.Invoke(new Action(() => username = this.username.Text));
            String mailid = "";
            this.mailid.Dispatcher.Invoke(new Action(() => mailid = this.mailid.Text));
            String password = "";
            Opassword.Dispatcher.Invoke(new Action(() => password = Opassword.Password));
            String opassword = "";
            OCpassword.Dispatcher.Invoke(new Action(() => opassword = OCpassword.Password));
            if (username.Equals("Username"))
            {
                DisplayWarning("Username is empty");
                return;
            }
            if (mailid.Equals("Mail Id") || !IsValidEmail(mailid))
            {
                DisplayWarning("Mail id Is not valid");
                return;
            }
            if (password.Length < 9)
            {
                DisplayWarning("password should be atleas eight charcters");
                return;
            }
            if (password.Equals(opassword) == false)
            {
                DisplayWarning("Password and confirm password are not same");
                return;
            }
            Client = new MongoClient("mongodb://jjsridharan:test123@ds016068.mlab.com:16068/securetravel");
            Database = Client.GetDatabase("securetravel");
            Collection = Database.GetCollection<BsonDocument>("user");
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("mailid", mailid);
            var results = Collection.Find(filter).ToList();
            if (results.Count == 0)
            {
                var document = new BsonDocument{
                    { "username" , username},
                    { "mailid" , mailid },
                    { "password", password }
                };
                Collection.InsertOne(document);
                DisplayWarning("Account Successfully Created! Login to send messages");
            }
            else
            {
                DisplayWarning("Mail id already exists");
            }
        }
        private void User_Signup(object e, RoutedEventArgs senders)
        {
            DisplayWarning("Processing your request", 300000);
            Task.Run(() => Handle());
        }
    }
}
