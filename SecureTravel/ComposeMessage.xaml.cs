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
    /// Interaction logic for ComposeMessage.xaml
    /// </summary>
    public partial class ComposeMessage : Window
    {
        private AfterLogin previouswindow;
        public ComposeMessage(AfterLogin prev_window)
        {
            InitializeComponent();
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

        private void Encrypt_Message(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content.Equals("Encrypt and Send") == true)
            {
                original_message.IsReadOnly = true;
                //Encrypt Logic
                encrypted_message.Visibility = Visibility.Visible;
                send_button.Visibility = Visibility.Visible;
                encrypt_button.Content = "Edit";
            }
            else
            {
                original_message.IsReadOnly = false;
                encrypted_message.Visibility = Visibility.Hidden;
                send_button.Visibility = Visibility.Hidden;
                encrypt_button.Content = "Encrypt and Send";
            }
        }

        private void Send_Message(object sender, RoutedEventArgs e)
        {
            //Send Logic
        }
    }
}
