using Domain.Modeli;

namespace Domain.BazaPodataka
{
    public class TabeleBazaPodataka
    {
        public List<Korisnik> Korisnici { get; set; } = [];
        public List<VinovaLoza> Loze { get; set; }
        // public List<Vino> Vina { get; set; }    
        public List<Paleta> Palete { get; set; }
        //  public List<VinskiPodrum> VinskiPodrumi { get; set; }
        //  public List<Faktura> Fakture { get; set; }

        public TabeleBazaPodataka()
        {
            Korisnici = new();
            Loze = new();
            //   Vina = new();
            Palete = new();
            //  VinskiPodrumi = new();
            //  Fakture = new();
        }
    }
}
