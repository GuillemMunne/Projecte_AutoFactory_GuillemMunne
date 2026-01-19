using System.Collections.Generic;
using AutoFactory.Model;

namespace AutoFactory.IDAO
{
    public interface IDAOProducte
    {
        void AfegirProducte(Producte producte);
        void EliminarProducte(int codiProducte);
        void ModificarProducte(Producte producte);
        Producte ObtenirProducte(int codiProducte);
        List<Producte> CarregarProductes();

       
        Dictionary<Item, int> ObtenirSubitems(Producte prod);

        // Obtain any Item (product or component) by its code
        Item ObtenirItem(int codi);

        void AfegirSubitem(Item itemFill, Producte productePare, int quantitat);
        void EliminarSubitem(Item itemFill, Producte productePare);
        void ModificarSubitem(Item itemFill, Producte productePare, int novaQuantitat);

        void ValidarCanvis();
        void DesferCanvis();
        void TancarCapa();
    }
}