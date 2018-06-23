using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace SecureTravel
{
    /// <summary>
    /// Interaction logic for ComposeMessage.xaml
    /// </summary>
    public partial class ComposeMessage : Window
    {
        private AfterLogin previouswindow;
        private SecurityController securecontroller = new SecurityController();
        String email, password;
        private static IMongoClient Client = new MongoClient("mongodb://jjsridharan:test123@ds016068.mlab.com:16068/securetravel");
        private IMongoDatabase Database = Client.GetDatabase("securetravel");
        private IMongoCollection<BsonDocument> Collection;
        public ComposeMessage(String email,String password,AfterLogin prev_window, String subject = "Subject", String content = "Your Message goes here...")
        {
            InitializeComponent();
            this.email = email;
            this.password = password;
            original_message.Text = content;
            this.subject.Text = subject;
            previouswindow = prev_window;
        }

        private void Return_Inbox(object sender, RoutedEventArgs e)
        {
            this.Close();
            previouswindow.Show();
        }

        private void To_Got_Focus(object sender, RoutedEventArgs e)
        {
            String username = ((TextBox)sender).Text;
            if (username.Equals("To") == true)
            {
                ((TextBox)sender).Text = "";
            }
        }

        private void Subject_Got_Focus(object sender, RoutedEventArgs e)
        {
            String username = ((TextBox)sender).Text;
            if (username.Equals("Subject") == true)
            {
                ((TextBox)sender).Text = "";
            }
        }

        private void Subject_Lost_Focus(object sender, RoutedEventArgs e)
        {
            String username = ((TextBox)sender).Text;
            if (username.Equals("") == true)
            {
                ((TextBox)sender).Text = "Subject";
            }
        }

        private void Message_Got_Focus(object sender, RoutedEventArgs e)
        {
            String username = ((TextBox)sender).Text;
            if (username.Equals("Your Message goes here...") == true)
            {
                ((TextBox)sender).Text = "";
            }
        }

        private void Message_Lost_Focus(object sender, RoutedEventArgs e)
        {
            String username = ((TextBox)sender).Text;
            if (username.Equals("") == true)
            {
                ((TextBox)sender).Text = "Your Message goes here...";
            }
        }

        private void To_Lost_Focus(object sender, RoutedEventArgs e)
        {
            String username = ((TextBox)sender).Text;
            if (username.Equals("") == true)
            {
                ((TextBox)sender).Text = "To";
            }
        }

        private void DisplayWarning(String message, int Interval = 3000)
        {
            Timer timer = new Timer();
            timer.Interval = Interval;
            warning.Dispatcher.Invoke(new Action(() => warning.Content = message));
            warning.Dispatcher.Invoke(new Action(() => warning.Visibility = Visibility.Visible));
            timer.Elapsed += (s, en) => {
                warning.Dispatcher.Invoke(new Action(() => warning.Visibility = Visibility.Hidden));
                timer.Stop();
            };
            timer.Start();
        }
        private void Handle(String mailid,String message,String subject)
        {
            Collection = Database.GetCollection<BsonDocument>("user");
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("mailid", mailid);
            var results = Collection.Find(filter).ToList();
            if (results.Count == 1)
            {
                var res = results[0];
                int count = res["count"].ToInt32();
                Collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("count", count+1));
                Collection = Database.GetCollection<BsonDocument>(mailid + "_public_key");
                builder = Builders<BsonDocument>.Filter;
                filter = builder.Eq("id", count);
                results = Collection.Find(filter).ToList();
                res = results[0];
                string public_key = res["key"].ToString();
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(public_key);
                byte[] encrypted = rsa.Encrypt(Encoding.ASCII.GetBytes(message),false);
                Collection = Database.GetCollection<BsonDocument>(mailid + "_messages");
                Collection.UpdateOne(Builders<BsonDocument>.Filter.Eq("id", count), Builders<BsonDocument>.Update.Set("message", encrypted).Set("from",email).Set("subject",subject));
                DisplayWarning("Message sent successfully");
            }
            else
            {
                DisplayWarning("Mail id doesn't exists!");
            }
        }
        private void Encrypt_Message(object sender, RoutedEventArgs e)
        {
           warning.Content = "Processing Request";
           warning.Visibility = Visibility.Visible;
           String message = original_message.Text;
           if (message.Equals("Your Message goes here...") ==true)
           {
               DisplayWarning("Please enter message...");
               return;
           }
           original_message.IsReadOnly = true;
           String to_address = this.to_address.Text;
           String subject = this.subject.Text;
           if(to_address.Equals(email))
            {
                DisplayWarning("Cannot send mail to entered mail id");
                return;
            }
           if(subject.Length > 100)
            {
                DisplayWarning("Subject Message too long");
                return;
            }
           Task.Run(() => Handle(to_address,message,subject));
        }
    }
}
