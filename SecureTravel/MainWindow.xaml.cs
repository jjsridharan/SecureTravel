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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String username = Username.Text;
            String password = TPassword.Password;
            afterlogin = new AfterLogin(username,password);
            afterlogin.Show();
            this.Close();
        }

        private void User_Lost_Focus(object sender, RoutedEventArgs e)
        {
            String username = ((TextBox)sender).Text;
            if(username.Equals("")==true)
            {
                ((TextBox)sender).Text = "Username";
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
            String username = ((TextBox)sender).Text;
            if (username.Equals("Username") == true)
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
