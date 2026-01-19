using AutoFactory.Model;
using System.Collections.Generic;

namespace AutoFactory.IDAO
{
    public interface IDAOProvincia
    {
        Provincia ObtenirProvincia(int codi);
        List<Provincia> CarregarProvincia();

        void TancarCapa();
    }
}