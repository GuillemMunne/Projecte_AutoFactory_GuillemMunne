using ProjecteAutoFactory.Finestres;
using ProjecteAutoFactory.Clases;
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

namespace ProjecteAutoFactory.Finestres.FinestreMainWindow
{
    /// <summary>
    /// Lógica de interacción para GestioProductes.xaml
    /// </summary>
    public partial class GestioProductes : UserControl
    {
        public GestioProductes()
        {
            InitializeComponent();
        }

        #region GestioProductes;


        private void ObrirAfegirProducte_Click(object sender, RoutedEventArgs e)
        {
            var finestra = new AfegirElProducte();
            

            if (finestra.ShowDialog() == true)
            {
                Productes p = finestra.NouProducte;
                Productes.LlistaProductes.Add(p);
                tbLlistaProductes.ItemsSource = Productes.LlistaProductes;
                tbLlistaProductes.DisplayMemberPath = "CodiNom";
                tbLlistaProductes.Items.Refresh();

                
            }
        }

        private void tbCercaProducte_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                busquedaProducte(sender, e);


            }
        }
        /*   private void tbCercaProducte_LostFocus(object sender, RoutedEventArgs e)
           {
               busquedaProducte(sender, e);
           }


        */

        private void tbLlistaProductes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            actualitzarDescripcioProducte();
            actualitzarArbreDom();
        }
        private void actualitzarArbreDom()
        {
            Productes producteSeleccionat = (Productes)tbLlistaProductes.SelectedItem;

            if (producteSeleccionat != null)
            {
                //CODI PER MOSTRAR DOM
            }
            else
            {
                tbDetallProducte.Content = "Selecciona un producte per veure l'arbre DOM.";
            }
        }


        private void actualitzarDescripcioProducte()
        {
            Productes producteSeleccionat = (Productes)tbLlistaProductes.SelectedItem;

            if (producteSeleccionat != null)
            {
                tbDetallProducte.Content = "Descripció: " + producteSeleccionat.Descripcio;
            }
            else
            {
                tbDetallProducte.Content = "Selecciona un producte per veure la descripció.";
            }
        }



        private void busquedaProducte(object sender, RoutedEventArgs e)
        {
            string textCercat = tbCercaProducte.Text.Trim().ToLower();

            // Si està buit, restaurem la llista completa i sortim sense missatge
            if (string.IsNullOrEmpty(textCercat))
            {
                tbLlistaProductes.ItemsSource = Productes.LlistaProductes;
                return;
            }

            HashSet<Productes> resultats = new HashSet<Productes>();

            foreach (var p in Productes.LlistaProductes)
            {
                bool nomCoincide = p.Nom.ToLower().StartsWith(textCercat);
                bool codiCoincide = p.Codi.ToString().StartsWith(textCercat);

                if (nomCoincide || codiCoincide)
                {
                    resultats.Add(p);
                }
            }

            if (resultats.Count == 0)
            {
                MessageBox.Show("No s'han trobat resultats.", "Sense coincidències", MessageBoxButton.OK, MessageBoxImage.Information);
                tbLlistaProductes.ItemsSource = null;
            }
            else
            {
                tbLlistaProductes.ItemsSource = resultats;
                MessageBox.Show($"S'han trobat {resultats.Count} productes que coincideixen amb la cerca.",
                    "Resultats de la cerca", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }



        private void ModificarProducte_Click(object sender, RoutedEventArgs e)
        {
            
            Productes seleccionat = (Productes)tbLlistaProductes.SelectedItem;
            if (seleccionat != null)
            {


                var modificarWindow = new ModificarProducte(seleccionat);

            if (modificarWindow.ShowDialog() == true)
                {
                    tbLlistaProductes.ItemsSource = Productes.LlistaProductes;
                    tbLlistaProductes.DisplayMemberPath = "CodiNom";
                    tbLlistaProductes.Items.Refresh();
                    actualitzarDescripcioProducte();
                }
            }
            else
            {
                MessageBox.Show("Si us plau, selecciona un producte per modificar.", "Error: Cap producte seleccionat", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void EliminarProducte(object sender, RoutedEventArgs e)
        {
            Productes p = (Productes)tbLlistaProductes.SelectedItem;
            if (Productes.LlistaProductes.Contains(p))
            {
                Productes.LlistaProductes.Remove(p);
                tbLlistaProductes.DisplayMemberPath = "CodiNom";
                tbLlistaProductes.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Si us plau, selecciona un producte per eliminar.", "Error: Cap producte seleccionat", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void AccedirInforme(object sender, RoutedEventArgs e)
        {
            // Aquí puedes agregar la lógica para acceder al informe
            MessageBox.Show("Accedint a l'informe del producte.");
        }
        #endregion
    }
}
