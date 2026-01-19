using System.Collections.Generic;
using System.Windows.Controls;

namespace AutoFactoryProjecte.Finestres.FinestreMainWindow
{
    public partial class Ajuda : UserControl
    {
        public class SeccioAjuda
        {
            public string Nom { get; set; }
            public string Descripcio { get; set; }
        }

        public Ajuda()
        {
            InitializeComponent();
            CarregarSeccions();
        }

        private void CarregarSeccions()
        {
            var seccions = new List<SeccioAjuda>
            {
                new SeccioAjuda
                {
                    Nom = "Introducció",
                    Descripcio = "El programa AutoFactory permet gestionar de manera senzilla tots els productes i components d’una empresa del sector de l’automoció. L’objectiu és facilitar la creació i manteniment del BOM (Bill of Materials)."
                },
                new SeccioAjuda
                {
                    Nom = "Productes",
                    Descripcio = "- Gestiona tots els productes fabricats.\n" +
                                 "- Llista de productes existents.\n" +
                                 "- Afegir, modificar o eliminar productes.\n" +
                                 "- Desglossament de materials (BOM) amb subproductes i components.\n" +
                                 "- Càlcul automàtic del cost total segons els components.\n" +
                                 "⚠️ No es poden eliminar productes o subproductes si formen part del BOM d’un altre producte."
                },
                new SeccioAjuda
                {
                    Nom = "Components+",
                    Descripcio = "- Gestiona els components que formen part dels productes.\n" +
                                 "- Permet veure i modificar nom, codi intern, unitat de mesura, preu i proveïdor.\n" +
                                 "- Els canvis afecten automàticament els costos dels productes que els utilitzen.\n" +
                                 "⚠️ No es poden eliminar components que estiguin assignats a algun producte."
                },
                new SeccioAjuda
                {
                    Nom = "Proveïdors",
                    Descripcio = "- Consulta la informació de totes les empreses subministradores.\n" +
                                 "- Veure raó social, adreça, NIF, persona de contacte i components associats.\n" +
                                 "- Clic sobre un proveïdor mostra els components associats.\n" +
                                 "- Doble clic obre la fitxa de detall.\n" +
                                 "⚠️ Els proveïdors no es poden crear, modificar ni eliminar."
                },
                new SeccioAjuda
                {
                    Nom = "Canvi de Tema",
                    Descripcio = "- Permet alternar entre mode clar i fosc.\n" +
                                 "- El tema seleccionat es manté la propera vegada que s’obre l’aplicació."
                },
                new SeccioAjuda
                {
                    Nom = "Navegació Principal",
                    Descripcio = "- La barra lateral permet accedir a totes les seccions del programa.\n" +
                                 "- Cada botó mostra el contingut corresponent.\n" +
                                 "⚠️ Si el contingut no canvia, comprovar que no hi hagi finestres emergents obertes."
                },
                new SeccioAjuda
                {
                    Nom = "Login",
                    Descripcio = "- Finestra d’inici de sessió amb usuari i contrasenya.\n" +
                                 "- Si les credencials són correctes, s’obre la finestra principal.\n" +
                                 "⚠️ No es pot tenir la finestra principal oberta mentre el login està actiu."
                },
                new SeccioAjuda
                {
                    Nom = "Informes",
                    Descripcio = "- Generació d’informes sobre productes, components i costos.\n" +
                                 "- Mostra informació detallada de cost total, components i proveïdors.\n" +
                                 "⚠️ Cal seleccionar un producte o proveïdor abans d’accedir a l’informe."
                }
            };

            lbCategoriesErrors.ItemsSource = seccions;
        }

        private void lbCategoriesErrors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var seccio = lbCategoriesErrors.SelectedItem as SeccioAjuda;
            if (seccio == null) return;

            txtTitolSeccio.Text = seccio.Nom;
            txtDescripcioSeccio.Text = seccio.Descripcio;
        }
    }
}
