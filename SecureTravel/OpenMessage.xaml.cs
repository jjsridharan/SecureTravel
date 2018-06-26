using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace SecureTravel
{
    /// <summary>
    /// Interaction logic for OpenMessage.xaml
    /// </summary>
    public partial class OpenMessage : Window
    {
        private AfterLogin previouswindow;
        String email, password, username;
        int id;
        private SecurityController securecontroller = new SecurityController();
        private static IMongoClient Client = new MongoClient("mongodb://jjsridharan:test123@ds016068.mlab.com:16068/securetravel");
        private IMongoDatabase Database = Client.GetDatabase("securetravel");
        private IMongoCollection<BsonDocument> Collection;
        public OpenMessage(String email,String password,String username,AfterLogin prev_window,String id)
        {
            InitializeComponent();
            this.email = email; 
            this.password = password;
            this.id = int.Parse(id);
            this.username = username;
            previouswindow = prev_window;
            OpenMessageText();
        }
        private void OpenMessageText()
        {
            Collection = Database.GetCollection<BsonDocument>(email + "_messages");
            var results = Collection.Find(Builders<BsonDocument>.Filter.Eq("id",id)).ToList();
            if(results.Count==1)
            {
                from_address.Content = results[0]["from"].ToString();
                subject.Content = results[0]["subject"].ToString();
                byte[] encrypted = (byte[])results[0]["message"];
                Collection = Database.GetCollection<BsonDocument>(email + "_private_key");                
                results = Collection.Find(Builders<BsonDocument>.Filter.Eq("id", id)).ToList();
                var res = results[0];
                string privatet_key = res["key"].ToString();
                privatet_key = securecontroller.Decrypt(password,privatet_key);
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(privatet_key);
                byte[] message = rsa.Decrypt(encrypted,false);
                Message.Text= Encoding.ASCII.GetString(message);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            previouswindow.Show();
        }
        private void Delete_Message()
        {
            Collection = Database.GetCollection<BsonDocument>(email + "_messages");
            var results = Collection.Find(Builders<BsonDocument>.Filter.Eq("id", id)).ToList();
            if (results.Count == 1)
            {
                Collection.UpdateOne(Builders<BsonDocument>.Filter.Eq("id", id), Builders<BsonDocument>.Update.Set("from", "").Set("message","").Set("subject",""));
            }
        }
        private void Delete_Mail(object sender, RoutedEventArgs e)
        {
            Delete_Message();
            this.Close();
            new AfterLogin(email,password,username).Show();

        }

        private void Forward_Mail(object sender, RoutedEventArgs e)
        {
            new ComposeMessage(email, password, previouswindow, subject.Content.ToString(), Message.Text).Show();
            this.Close();
        }
    }
}
