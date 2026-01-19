using System.Windows;

namespace ProjecteAutoFactory.Finestres.FinestreMainWindow
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void LoginButton_click(object sender, RoutedEventArgs e)
        {
            var finestreMain = new MainWindow();
            finestreMain.Show();
            this.Close();
        }
    }
}
