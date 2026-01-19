using ProjecteAutoFactory.Finestres;
using ProjecteAutoFactory.Clases;
using ProjecteAutoFactory.Finestres.FinestreMainWindow;
using ProjecteAutoFactory.Finestres.FinestreNormal;

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
using AutoFactoryProjecte.Finestres.FinestreMainWindow;

namespace ProjecteAutoFactory
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private bool temaOscuro = false;

        private Button botonSeleccionado;

        private void BtnCambiarTema_Click(object sender, RoutedEventArgs e)
        {
            temaOscuro = !temaOscuro;

            Application.Current.Resources.MergedDictionaries.Clear();

            var diccionario = new ResourceDictionary();
            diccionario.Source = new Uri(
                temaOscuro ? "src/Themes/Dark.xaml" : "src/Themes/Light.xaml", UriKind.Relative);

            Application.Current.Resources.MergedDictionaries.Add(diccionario);

            if (botonSeleccionado != null) {
                botonSeleccionado.Style = TryFindResource("MainButtonMenuSelected") as Style;
            }
        }

        private void clickGestioProductes(object sender, RoutedEventArgs e)
        {
            ContenidoBotones.Content = new GestioProductes();
            if (!botonSeleccionado.Equals(btnGestioProductes))
            {
                DeseleccionarBoton();
                 botonSeleccionado = btnGestioProductes;
                SeleccionarBoton(btnGestioProductes);
            }

        }

        private void clickComponentsPrimaris(object sender, RoutedEventArgs e)
        {
            ContenidoBotones.Content = new GestioComponents();

            if (!botonSeleccionado.Equals(btnComponentsPrimaris))
            {
                DeseleccionarBoton();
                botonSeleccionado = btnComponentsPrimaris;
                SeleccionarBoton(btnComponentsPrimaris);
            }
        }

        private void clickProveidors(object sender, RoutedEventArgs e)
        {
            ContenidoBotones.Content = new Proveidors();

            if (!botonSeleccionado.Equals(btnProveidors))
            {
                DeseleccionarBoton();
                botonSeleccionado = btnProveidors;
                SeleccionarBoton(btnProveidors);
            }

        }
        private void clickFinestraAjuda(object sender, RoutedEventArgs e)
        {
            ContenidoBotones.Content = new Ajuda();

            if (!botonSeleccionado.Equals(btnFinestraAjuda))
            {
                DeseleccionarBoton();
                botonSeleccionado = btnFinestraAjuda;
                SeleccionarBoton(btnFinestraAjuda);
            }

        }

        private void clickLogin(object sender, RoutedEventArgs e)
        {
            var login = new Login();
            login.Show();
            this.Close();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ContenidoBotones.Content = new GestioProductes();
            botonSeleccionado = btnGestioProductes;
        }

        private void AccedirInforme(object sender, RoutedEventArgs e)
        {

        }

        private void SeleccionarBoton(Button boton)
        {
            if (botonSeleccionado != null)
                botonSeleccionado.Style = TryFindResource("MainButtonMenu") as Style;

            boton.Style = TryFindResource("MainButtonMenuSelected") as Style;
            botonSeleccionado = boton;
        }

        private void DeseleccionarBoton()
        {
            if (botonSeleccionado != null)
                botonSeleccionado.Style = TryFindResource("MainButtonMenu") as Style;
            botonSeleccionado = null;
        }


    }
}
