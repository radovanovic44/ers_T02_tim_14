using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.SkladistenjeServisi
{
    public class VinskiPodrumServis : ISkladistenjeServis
    {
        private readonly IPaleteRepozitorijum paleteRepozitorijum;
        private readonly ILoggerServis loggerServis;

        public VinskiPodrumServis(
            IPaleteRepozitorijum repozitorijum,
            ILoggerServis logger)
        {
            paleteRepozitorijum = repozitorijum;
            loggerServis = logger;
        }
        public void PrimiPaletu(Paleta paleta)
        {
            // paleta je sada fizicki u ovom skladistu
            paleteRepozitorijum.DodajPaletu(paleta);

            loggerServis.EvidentirajDogadjaj(
                TipEvidencije.INFO,
                $"Paleta {paleta.Sifra} primljena u skladiste.");
        }


        public List<Paleta> IsporuciPalete(int brojPaleta)
        {
            List<Paleta> isporucene = new();

            int max = Math.Min(brojPaleta, 5);

            foreach (var paleta in paleteRepozitorijum.SvePalete())
            {
                if (isporucene.Count == max)
                    break;

                if (paleta.Status == StatusPalete.Otpremljena)
                {
                    Thread.Sleep(300); // 0.3s po paleti
                    isporucene.Add(paleta);
                }
            }

            loggerServis.EvidentirajDogadjaj(
                TipEvidencije.INFO,
                $"Vinski podrum isporucio {isporucene.Count} paleta.");

            return isporucene;
        }
    }
}
