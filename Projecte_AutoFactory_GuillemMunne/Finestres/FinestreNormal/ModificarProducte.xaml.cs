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
            this.DataContext = producteSeleccionat;
            txtCodi.Text = producteSeleccionat.Codi.ToString();
            txtNom.Text = producteSeleccionat.Nom;
            txtDescripcio.Text = producteSeleccionat.Descripcio;
            txtStock.Text = producteSeleccionat.Stock.ToString();
            txtFoto.Text = producteSeleccionat.Foto;
        }

      

        private void Acceptar_Click(object sender, RoutedEventArgs e)
        {
            if (txtNom.Text != null && txtNom.Text != "" &&
                txtCodi.Text != null && txtCodi.Text != "" &&
                txtStock.Text != null && txtStock.Text != "")
            {
                if (int.Parse(txtCodi.Text) > 0 &&
                    int.Parse(txtStock.Text) >= 0 && txtDescripcio.Text.Length <= 400 && txtNom.Text.Length <= 100)
                {
                    producteSeleccionat.Codi = int.Parse(txtCodi.Text);
                    producteSeleccionat.Nom = txtNom.Text;
                    producteSeleccionat.Descripcio = txtDescripcio.Text;
                    producteSeleccionat.Stock = int.Parse(txtStock.Text);
                    producteSeleccionat.Foto = txtFoto.Text;

                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Si us plau, les dades introduides han de ser correctes.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Si us plau, ompliu tots els camps correctament.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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




        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void txtDescripcio_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}