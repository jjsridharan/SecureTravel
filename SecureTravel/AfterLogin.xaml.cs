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
    /// Interaction logic for AfterLogin.xaml
    /// </summary>
    public partial class AfterLogin : Window
    {
        private OpenMessage openmessage;
        private AfterLogin currentwindow;
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
            openmessage = new OpenMessage(currentwindow);
            openmessage.Show();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
