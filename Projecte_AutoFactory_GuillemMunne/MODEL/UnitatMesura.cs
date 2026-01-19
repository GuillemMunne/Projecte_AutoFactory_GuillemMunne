
using System;
using System.Collections.Generic;

namespace AutoFactory.Model
{
    public class UnitatMesura
    {
        private int codi;
        private string nom;
        private Dictionary<int, string> llistaUnitatsMesura;

        public UnitatMesura(int codi, string nom)
        {
            this.codi = codi;
            this.nom = nom;
            this.llistaUnitatsMesura = new Dictionary<int, string>();
        }

        public int GetCodi()
        {
            return codi;
        }

        public string GetNom()
        {
            return nom;
        }

        public Dictionary<int, string> GetLlistaUM()
        {
            return llistaUnitatsMesura;
        }
    }//end UnitatMesura
}//end namespace AutoFactory.Model
