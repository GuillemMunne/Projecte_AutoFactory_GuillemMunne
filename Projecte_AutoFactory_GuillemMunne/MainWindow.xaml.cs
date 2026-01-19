using AutoFactoryProjecte.Finestres.FinestreMainWindow;
using ProjecteAutoFactory.Finestres.FinestreMainWindow;
using ProjecteAutoFactory.Finestres.FinestreNormal;
using System;
using System.Windows;
using System.Windows.Controls;

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

            if (botonSeleccionado != null)
            {
                botonSeleccionado.Style = TryFindResource("MainButtonMenuSelected") as Style;
            }
        }

        private void clickGestioProductes(object sender, RoutedEventArgs e)
        {
            MostrarSeccio(new GestioProductes(), btnGestioProductes);
        }

        private void clickComponentsPrimaris(object sender, RoutedEventArgs e)
        {
            MostrarSeccio(new GestioComponents(), btnComponentsPrimaris);
        }

        private void clickProveidors(object sender, RoutedEventArgs e)
        {
            MostrarSeccio(new Proveidors(), btnProveidors);
        }
        private void clickFinestraAjuda(object sender, RoutedEventArgs e)
        {
            MostrarSeccio(new Ajuda(), btnFinestraAjuda);
        }

        private void clickLogin(object sender, RoutedEventArgs e)
        {
            var login = new Login();
            login.Show();
            this.Close();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MostrarSeccio(new GestioProductes(), btnGestioProductes);
        }

        private void AccedirInforme(object sender, RoutedEventArgs e)
        {

        }

        private void MostrarSeccio(UserControl seccio, Button boto)
        {
            ContenidoBotones.Content = seccio;

            if (ReferenceEquals(botonSeleccionado, boto))
            {
                return;
            }

            DeseleccionarBoton();
            SeleccionarBoton(boto);
        }

        private void SeleccionarBoton(Button boton)
        {
            if (botonSeleccionado != null)
            {
                botonSeleccionado.Style = TryFindResource("MainButtonMenu") as Style;
            }

            boton.Style = TryFindResource("MainButtonMenuSelected") as Style;
            botonSeleccionado = boton;
        }

        private void DeseleccionarBoton()
        {
            if (botonSeleccionado != null)
            {
                botonSeleccionado.Style = TryFindResource("MainButtonMenu") as Style;
            }
            botonSeleccionado = null;
        }


    }
}
