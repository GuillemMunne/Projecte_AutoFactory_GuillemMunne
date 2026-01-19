using System.Collections.Generic;
using AutoFactory.Model;

namespace AutoFactory.IDAO
{
    public interface IDAOItem
    {
        void AfegirItem(Item item);
        void EliminarItem(int codi);
        void ModificarItem(Item item);
        Item ObtenirItem(int codi);
    }
}
