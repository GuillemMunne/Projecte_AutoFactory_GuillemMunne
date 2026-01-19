using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFactory.Model
{
    public class ComponentProveidor
    {
        public int CodiComponent { get; set; }

        public int CodiProveidor { get; set; }

        public decimal Preu { get; set; }

        // Constructor vacío (necesario para DAOs)
        public ComponentProveidor()
        {
        }

        // Constructor útil
        public ComponentProveidor(int codiComponent, int codiProveidor, decimal preu)
        {
            CodiComponent = codiComponent;
            CodiProveidor = codiProveidor;
            Preu = preu;
        }
    }
}
