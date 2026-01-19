using System.Collections.Generic;
using AutoFactory.Model;

namespace AutoFactory.IDAO
{
    public interface IProveidorDao
    {
        IReadOnlyList<Proveidor> ObtenirTots();
        Proveidor? ObtenirPerCodi(int codi);
        void Afegir(Proveidor proveidor);
        void Actualitzar(Proveidor proveidor);
        void Eliminar(int codi);
    }
}
