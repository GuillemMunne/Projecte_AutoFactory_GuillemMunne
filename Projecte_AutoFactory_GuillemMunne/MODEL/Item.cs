using System;

namespace AutoFactory.Model
{
    public abstract class Item
    {
        private int codi;
        private string nom;
        private string descripcio;
        private int stock;
        private byte[] foto;

        public Item() { }

        public Item(int codi, string nom, string descripcio, int stock, byte[] foto)
        {
            this.codi = codi;
            this.nom = nom;
            this.descripcio = descripcio;
            this.stock = stock;
            this.foto = foto;
        }

        public int Codi { get => codi; set => codi = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Descripcio { get => descripcio; set => descripcio = value; }
        public int Stock { get => stock; set => stock = value; }
        public byte[] Foto { get => foto; set => foto = value; }

        public override bool Equals(object obj)
        {
            return obj is Item item && codi == item.codi;
        }

        public override int GetHashCode()
        {
            return 2108858624 + codi.GetHashCode();
        }
    }
}