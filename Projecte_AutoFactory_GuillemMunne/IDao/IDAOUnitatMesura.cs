using AutoFactory.Model;
using System.Collections.Generic;

namespace AutoFactory.IDAO
{
    public interface IDAOUnitatMesura
    {
        UnitatMesura ObtenirUnitatMesura(int codi);
        List<UnitatMesura> CarregarUnitatsMesura();
        void TancarCapa();
    }
}