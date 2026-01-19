using System.Collections.Generic;
using AutoFactory.Model;

namespace AutoFactory.IDAO
{
    public interface IProvinciaDao
    {
        IReadOnlyList<Provincia> ObtenirTots();
        Provincia? ObtenirPerCodi(int codi);
        void Afegir(Provincia provincia);
        void Actualitzar(Provincia provincia);
        void Eliminar(int codi);
    }
}
