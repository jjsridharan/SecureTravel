using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SecureTravel
{
    /// <summary>
    /// Interaction logic for AfterLogin.xaml
    /// </summary>
    public partial class AfterLogin : Window
    {
        private OpenMessage openmessage;
        private AfterLogin currentwindow;
        private ComposeMessage composemessage;
        String username, password;
        public AfterLogin(String username, String password)
        {
            InitializeComponent();
            for (int i = 0; i < 10; ++i)
            {

                Button obutton = new Button();
                obutton.Tag = i + 1;
                obutton.Content = "From : JJSri" + i * 10 + "@gmail.com";
                obutton.BorderThickness = new Thickness(0);
                obutton.Background = Brushes.Transparent;
                obutton.Margin = new Thickness(36, 61 + i * 48, 0, 0);
                obutton.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                obutton.VerticalAlignment = VerticalAlignment.Top;
                obutton.Click += action_clicked;
                this.username = username;
                this.password = password;
                this.grid.Children.Add(obutton);

            }
        }
        /// <summary>
        /// When Open are Delete button is clicked for each mail.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void action_clicked(object sender, RoutedEventArgs e)
        {
            currentwindow = this;
            currentwindow.Hide();
            openmessage = new OpenMessage(username,password,currentwindow);
            openmessage.Show();
        }

        private void Compose_Message(object sender, RoutedEventArgs e)
        {
            currentwindow = this;
            currentwindow.Hide();
            composemessage = new ComposeMessage(username,password,currentwindow);
            composemessage.Show();
        }

        private void View_Requests(object sender, RoutedEventArgs e)
        {

        }

        private void New_Request(object sender, RoutedEventArgs e)
        {

        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}
