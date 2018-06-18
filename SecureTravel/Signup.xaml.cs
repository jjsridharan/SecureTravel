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

namespace SecureTravel
{
    /// <summary>
    /// Interaction logic for Signup.xaml
    /// </summary>
    public partial class Signup : Window
    {
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
            //Signup Logic
        }
    }
}
