using System.Collections.Generic;
using AutoFactory.Model;

namespace AutoFactory.IDAO
{
    public interface IComponentDao
    {
        IReadOnlyList<Component> ObtenirTots();
        Component? ObtenirPerCodi(int codi);
        void Afegir(Component component);
        void Actualitzar(Component component);
        void Eliminar(int codi);
    }
}
