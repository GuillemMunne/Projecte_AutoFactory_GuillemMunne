using AutoFactory.Model;
using System.Collections.Generic;

namespace AutoFactory.IDAO
{
    public interface IDAOMunicipi
    {
        Municipi ObtenirMunicipi(int codi);
        List<Municipi> CarregarMunicipi();

        void TancarCapa();
    }
}