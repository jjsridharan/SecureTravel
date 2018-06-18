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
    /// Interaction logic for OpenMessage.xaml
    /// </summary>
    public partial class OpenMessage : Window
    {
        private AfterLogin previouswindow;
        private ComposeMessage composemessage;
        String username, password;
        public OpenMessage(String username,String password,AfterLogin prev_window)
        {
            InitializeComponent();
            this.username = username;
            this.password = password;
            previouswindow = prev_window;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            previouswindow.Show();
        }

        private void Delete_Mail(object sender, RoutedEventArgs e)
        {

        }

        private void Forward_Mail(object sender, RoutedEventArgs e)
        {
            composemessage = new ComposeMessage(username,password,previouswindow,"Subject","Content");
            composemessage.Show();
            this.Hide();
        }
    }
}
