using Microsoft.Win32;
using ProjecteAutoFactory.Clases;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ProjecteAutoFactory.Finestres.FinestreNormal
{
    /// <summary>
    /// Lógica de interacción para AfegirProducte.xaml
    /// </summary>
    public partial class AfegirElProducte : Window
    {
        public Productes NouProducte { get; private set; }
        public AfegirElProducte()
        {
            InitializeComponent();
        }
        private int CrearCodi()
        {
            int codi = 1;
            while (Productes.LlistaProductes.Any(p => p.Codi == codi))
            {
                codi++;
            }
            return codi;
        }
        private void Acceptar_Click(object sender, RoutedEventArgs e)
        {
            int codi = CrearCodi();
            if (string.IsNullOrWhiteSpace(txtNom.Text) ||
                string.IsNullOrWhiteSpace(txtCodi.Text) ||
                string.IsNullOrWhiteSpace(txtStock.Text))
            {
                MessageBox.Show("Si us plau, ompliu tots els camps correctament.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stock) ||
                stock < 0 ||
                txtDescripcio.Text.Length > 400 ||
                txtNom.Text.Length > 100)
            {
                MessageBox.Show("Si us plau, les dades introduides han de ser correctes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NouProducte = new Productes(
                codi,
                txtNom.Text,
                txtDescripcio.Text,
                stock,
                txtFoto.Text
            );

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




        /*
        <!--private void AfegirProducte(object sender, RoutedEventArgs e)
        {
            int codi = 1;
            String nom = "Roda";
            int descompte = 5;
            int stock = 100;
            String foto = "roda.jpg";
            Productes p1 = new Productes(codi, nom, descompte, stock, foto);
            Productes.LlistaProductes.Add(p1);

            tbLlistaProductes.Items.Refresh();
        }
        -->
        */
        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void guardarProducte_Click(object sender, RoutedEventArgs e)
        {
            // Aquí puedes agregar la lógica para guardar el producto
            var finestra = new PopUpDependDe();

            finestra.ShowDialog();
        }
    }
}
