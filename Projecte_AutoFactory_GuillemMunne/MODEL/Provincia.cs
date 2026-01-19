

using System;

namespace AutoFactory.Model
{
    public class Provincia
    {
        public int Codi { get; }
        public string Nom { get; }

        public Provincia(int codi, string nom)
        {
            Codi = codi;
            Nom = nom;
        }
    }
}