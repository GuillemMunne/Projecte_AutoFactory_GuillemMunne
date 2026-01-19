using Microsoft.Win32;
using ProjecteAutoFactory.Clases;
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
using ProjecteAutoFactory.Finestres.FinestreMainWindow;
using ProjecteAutoFactory.Finestres.FinestreNormal;
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
            if (txtNom.Text != null && txtNom.Text != "" &&
                txtCodi.Text != null && txtCodi.Text != "" &&
                txtStock.Text != null && txtStock.Text != "")
            {
          
                    if (int.Parse(txtStock.Text) >= 0 && txtDescripcio.Text.Length <= 400 && txtNom.Text.Length <= 100)
                    {
                        NouProducte = new Productes(
                            codi,
                            txtNom.Text,
                            txtDescripcio.Text,
                            int.Parse(txtStock.Text),
                            txtFoto.Text
                        );

                        this.DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Si us plau, les dades introduides han de ser correctes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                
            }
            else
            {
                MessageBox.Show("Si us plau, ompliu tots els camps correctament.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        private void inserirImatge(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Arxius d'imatge|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            dlg.Title = "Selecciona una imatge";

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
            this.DialogResult = false;
            this.Close();
        }

        private void guardarProducte_Click(object sender, RoutedEventArgs e)
        {
            // Aquí puedes agregar la lógica para guardar el producto
            var finestra = new PopUpDependDe();

            finestra.ShowDialog();
        }
    }
}