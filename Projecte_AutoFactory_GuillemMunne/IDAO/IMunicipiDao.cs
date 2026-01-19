using System.Collections.Generic;
using AutoFactory.Model;

namespace AutoFactory.IDAO
{
    public interface IMunicipiDao
    {
        IReadOnlyList<Municipi> ObtenirTots();
        Municipi? ObtenirPerCodi(int codi);
        void Afegir(Municipi municipi);
        void Actualitzar(Municipi municipi);
        void Eliminar(int codi);
    }
}
