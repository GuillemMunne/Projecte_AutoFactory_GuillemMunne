using AutoFactory.Model;
using System.Collections.Generic;

namespace AutoFactory.IDAO
{
    public interface IDAOProveidor
    {
        Proveidor ObtenirProveidor(int codi);
        List<Proveidor> CarregarProveidors();

        void ValidarCanvis();
        void DesferCanvis();
        void TancarCapa();
    }
}