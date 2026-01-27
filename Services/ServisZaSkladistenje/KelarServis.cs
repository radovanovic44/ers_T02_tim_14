using Domain.BazaPodataka;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.SkladistenjeServisi
{
    public class LokalniKelarServis : ISkladistenjeServis
    {
        private readonly IPaleteRepozitorijum paleteRepozitorijum;
        private readonly ILoggerServis loggerServis;
        private readonly TabeleBazaPodataka _bazapodataka;

        public LokalniKelarServis(
            IPaleteRepozitorijum repozitorijum,
            ILoggerServis logger,
            TabeleBazaPodataka bazapodataka)
        {
            paleteRepozitorijum = repozitorijum;
            loggerServis = logger;
            _bazapodataka = bazapodataka;
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

            int max = Math.Min(brojPaleta, 2);

            foreach (var paleta in paleteRepozitorijum.SvePalete())
            {
                if (isporucene.Count == max)
                    break;

                if (paleta.Status == StatusPalete.Otpremljena)
                {
                    Thread.Sleep(1800); // 1.8s po paleti

                    isporucene.Add(paleta);
                }
            }

            loggerServis.EvidentirajDogadjaj(
                TipEvidencije.INFO,
                $"Lokalni kelar isporucio {isporucene.Count} paleta.");

            return isporucene;
        }
        public bool ImaNaStanju(Guid vinoID)
        {
            return _bazapodataka.Palete.Any(p => p.VinoId == vinoID);
        }
    }
}
