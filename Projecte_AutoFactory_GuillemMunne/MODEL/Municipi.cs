
using System;

namespace AutoFactory.Model
{
        public class Municipi
        {
        public int CodiMunicipi { get; }
        public string Nom { get; }
        public Provincia Provincia { get; }

        public Municipi(int codiMunicipi, string nom, Provincia provincia)
        {
                CodiMunicipi = codiMunicipi;
                Nom = nom;
                Provincia = provincia;
            }
        

    }
}
