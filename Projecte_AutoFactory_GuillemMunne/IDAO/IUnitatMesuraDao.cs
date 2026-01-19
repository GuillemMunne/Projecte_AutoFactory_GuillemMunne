using System.Collections.Generic;
using AutoFactory.Model;

namespace AutoFactory.IDAO
{
    public interface IUnitatMesuraDao
    {
        IReadOnlyList<UnitatMesura> ObtenirTots();
        UnitatMesura? ObtenirPerCodi(int codi);
        void Afegir(UnitatMesura unitatMesura);
        void Actualitzar(UnitatMesura unitatMesura);
        void Eliminar(int codi);
    }
}
