using System.Collections.Generic;
using AutoFactory.Model;

namespace AutoFactory.IDAO
{
    public interface IDAOComponent
    {
        void AfegirComponent(Component component);
        void EliminarComponent(int codiComponent);
        void ModificarComponent(Component component);
        Component ObtenirComponent(int codiComponent);
        List<Component> CarregarComponents();
        List<ComponentProveidor> ObtenirProveidorsComponent(int codiComponent);

        void ValidarCanvis();
        void DesferCanvis();
        void TancarCapa();
    }
}