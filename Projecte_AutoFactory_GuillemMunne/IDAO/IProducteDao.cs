using System.Collections.Generic;
using AutoFactory.Model;

namespace AutoFactory.IDAO
{
    public interface IProducteDao
    {
        IReadOnlyList<Producte> ObtenirTots();
        Producte? ObtenirPerCodi(int codi);
        void Afegir(Producte producte);
        void Actualitzar(Producte producte);
        void Eliminar(int codi);
    }
}
