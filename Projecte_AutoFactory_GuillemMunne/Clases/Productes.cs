using System;
using System.Collections.Generic;
namespace ProjecteAutoFactory.Clases
{
    public class Productes
    {
        public int Codi { get; set; }
        public string Nom { get; set; }
        public String Descripcio { get; set; }
        public int Stock { get; set; }
        public string Foto { get; set; }
        public static HashSet<Productes> LlistaProductes { get; } = new HashSet<Productes>();
        public string CodiNom => $"{Codi} - {Nom}";

        public Productes() { }

        public Productes(int codi, string nom, String descripcio, int stock, string foto)
        {
            Codi = codi;
            Nom = nom;
            Descripcio = descripcio;
            Stock = stock;
            Foto = foto;
        }

    }
}
