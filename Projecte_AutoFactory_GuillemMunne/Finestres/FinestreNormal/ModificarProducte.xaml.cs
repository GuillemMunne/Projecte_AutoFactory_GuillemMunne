using Microsoft.Win32;
using ProjecteAutoFactory.Clases;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ProjecteAutoFactory.Finestres.FinestreNormal
{
    /// <summary>
    /// Lógica de interacción para ModificarProducte.xaml
    /// </summary>
    public partial class ModificarProducte : Window
    {
        public Productes producteSeleccionat { get; private set; }

        public ModificarProducte()
        {
            InitializeComponent();
        }
        public ModificarProducte(Productes producteSeleccionat)
        {
            InitializeComponent();
            this.producteSeleccionat = producteSeleccionat;
            DataContext = producteSeleccionat;
            txtCodi.Text = producteSeleccionat.Codi.ToString();
            txtNom.Text = producteSeleccionat.Nom;
            txtDescripcio.Text = producteSeleccionat.Descripcio;
            txtStock.Text = producteSeleccionat.Stock.ToString();
            txtFoto.Text = producteSeleccionat.Foto;
        }

        private void Acceptar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text) ||
                string.IsNullOrWhiteSpace(txtCodi.Text) ||
                string.IsNullOrWhiteSpace(txtStock.Text))
            {
                MessageBox.Show("Si us plau, ompliu tots els camps correctament.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(txtCodi.Text, out int codi) ||
                !int.TryParse(txtStock.Text, out int stock) ||
                codi <= 0 ||
                stock < 0 ||
                txtDescripcio.Text.Length > 400 ||
                txtNom.Text.Length > 100)
            {
                MessageBox.Show("Si us plau, les dades introduides han de ser correctes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            producteSeleccionat.Codi = codi;
            producteSeleccionat.Nom = txtNom.Text;
            producteSeleccionat.Descripcio = txtDescripcio.Text;
            producteSeleccionat.Stock = stock;
            producteSeleccionat.Foto = txtFoto.Text;

            DialogResult = true;
            Close();
        }


        private void inserirImatge(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Arxius d'imatge|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Selecciona una imatge"
            };

            if (dlg.ShowDialog() == true)
            {
                string ruta = dlg.FileName;
                imgFoto.Source = new BitmapImage(new Uri(ruta, UriKind.Absolute));
                txtFoto.Text = ruta;
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void txtDescripcio_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
