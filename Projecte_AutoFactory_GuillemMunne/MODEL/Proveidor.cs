namespace AutoFactory.Model
{
    public class Proveidor
    {
        private string cif;
        private int codi;
        private string linia_adreca_facturacio;
        private string persona_contacte;
        private string rao_social;
        private int telf_contacte;
        private Municipi municipi;

        public Proveidor(int codi, string cif, string raoSocial, string personaContacte, string liniaAdreca, int telefon, Municipi municipi)
        {
            this.codi = codi;
            this.cif = cif;
            this.rao_social = raoSocial;
            this.persona_contacte = personaContacte;
            this.linia_adreca_facturacio = liniaAdreca;
            this.telf_contacte = telefon;
            this.municipi = municipi;
        }

        public string GetCif() => cif;
        public int GetCodi() => codi;
        public string GetLAF() => linia_adreca_facturacio;
        public Municipi GetMunicipi() => municipi;
        public string GetPersonaContacte() => persona_contacte;
        public string GetRS() => rao_social;
        public int GetTelefonContacte() => telf_contacte;
    }
}
