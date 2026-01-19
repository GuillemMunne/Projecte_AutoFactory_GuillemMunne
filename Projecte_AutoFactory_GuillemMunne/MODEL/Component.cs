using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AutoFactory.Model
{
    

    public class Component : Item
    {
        private int _codi_fabricant;
        private decimal _preu_mig;
        private UnitatMesura _unitat;
        private Dictionary<Proveidor, decimal> _proveidors;
        public override string ToString()
        {
            return Nom;
        }
        public void BuidaProveidors()
        {
            _proveidors.Clear();
            PreuMig = 0;
        }

        public int CodiFabricant
        {
            get { return _codi_fabricant; }
            set { _codi_fabricant = value; }
        }

        public decimal PreuMig
        {
            get { return _preu_mig; }
            set { _preu_mig = value; }
        }

        public UnitatMesura Unitat
        {
            get { return _unitat; }
            set { _unitat = value; }
        }


        public Component()
        {
            _proveidors = new Dictionary<Proveidor, decimal>();
        }

        ~Component()
        {
        }

        public void afegirProveidor(Proveidor proveidor, decimal preu)
        {
            _proveidors[proveidor] = preu;
            actualitzarPreuMig();
        }

        private void actualitzarPreuMig()
        {
            if (_proveidors.Count > 0)
                PreuMig = _proveidors.Values.Average();
        }

        public void modificarProveidorPreu(Proveidor proveidor, decimal preu)
        {
            if (_proveidors.ContainsKey(proveidor))
            {
                _proveidors[proveidor] = preu;
                actualitzarPreuMig();
            }
        }

        public IReadOnlyDictionary<Proveidor, decimal> ObtenirProveidors()
        {
            return new ReadOnlyDictionary<Proveidor, decimal>(_proveidors);
        }



    }
}
