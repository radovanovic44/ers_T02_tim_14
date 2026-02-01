using Domain.Enumeracije;

namespace Domain.Modeli
{
    public class VinovaLoza
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Naziv { get; set; } = string.Empty;

        // u opsegu 15.0 - 28.0 Brix
        public double NivoSecera { get; set; }

        public int GodinaSadnje { get; set; }
        public string Region { get; set; } = string.Empty;
        public FazaZrelosti Faza { get; set; } = FazaZrelosti.Posadjena;

        public VinovaLoza() { }

        public VinovaLoza(string naziv, double nivoSecera, int godinaSadnje, string region, FazaZrelosti faza = FazaZrelosti.Posadjena)
        {
            Id = Guid.NewGuid();
            Naziv = naziv;
            NivoSecera = nivoSecera;
            GodinaSadnje = godinaSadnje;
            Region = region;
            Faza = faza;
        }
    }
}
