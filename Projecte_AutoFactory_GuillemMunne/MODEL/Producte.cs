using System;
using System.Collections.Generic;

namespace AutoFactory.Model
{
    public class Producte : Item
    {
      
        private Dictionary<Item, int> conte;

        public Producte() : base()
        {
            conte = new Dictionary<Item, int>();
        }

        public Producte(int codi, string nom, string descripcio, int stock, byte[] foto)
            : base(codi, nom, descripcio, stock, foto)
        {
            conte = new Dictionary<Item, int>();
        }

        public Dictionary<Item, int> Conte { get => conte; }

        public void AfegirFill(Item item, int quantitat)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (quantitat <= 0) throw new ArgumentOutOfRangeException(nameof(quantitat));

            if (conte.ContainsKey(item))
                conte[item] = quantitat; 
            else
                conte.Add(item, quantitat);
        }

        public void EliminarFill(Item item)
        {
            if (conte.ContainsKey(item))
                conte.Remove(item);
        }
    }
}