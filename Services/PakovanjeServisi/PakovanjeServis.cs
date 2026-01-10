using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.PakovanjeServisi
{
    public class PakovanjeServis : IPakovanjeServis
    {
        IPaleteRepozitorijum paleteRepozitorijum;
        ILoggerServis loggerServis;

        public PakovanjeServis(IPaleteRepozitorijum repozitorijum, ILoggerServis logger)
        {
            paleteRepozitorijum = repozitorijum;
            loggerServis = logger;
        }

        public Paleta? PrvaDostupnaPaleta(Guid vinskiPodrumId, Guid vinoId)
        {
            try
            {
                foreach (Paleta paleta in paleteRepozitorijum.SvePalete())
                {
                    if (paleta.VinskiPodrumId == vinskiPodrumId &&
                        paleta.VinoId == vinoId &&
                        (paleta.Status == StatusPalete.Aktivna ||
                         paleta.Status == StatusPalete.Otvorena))
                    {
                        return paleta;
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public (bool, Paleta) PakujeVinoUPaletu(Guid vinskiPodrumId, Guid vinoId, int zeljenaKolicinaPakovanja)
        {
            try
            {
                if (zeljenaKolicinaPakovanja <= 0)
                {
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.WARNING,
                        "Pokusaj pakovanja sa nevalidnom kolicinom.");
                    return (false, new Paleta());
                }

                Paleta? paleta = PrvaDostupnaPaleta(vinskiPodrumId, vinoId);

                if (paleta == null || paleta.Sifra == string.Empty)
                {
                    paleta = KreirajNovuPaletu(vinskiPodrumId, vinoId);
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO,
                        $"Kreirana je nova paleta '{paleta.Sifra}' za vino '{vinoId}'.");
                }

                paleta.TrenutniBrojPakovanja += zeljenaKolicinaPakovanja;
                paleta.Status = StatusPalete.Otvorena;

                // posto IPaleteRepozitorijum nema AzurirajPaletu,
                // oslanjamo se na to da radimo nad istom listom u bazi
                // i da ce promena biti sacuvana prilikom sledeceg SacuvajPromene().

                loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO,
                    $"Upakovano je {zeljenaKolicinaPakovanja} pakovanja na paletu '{paleta.Sifra}'.");

                return (true, paleta);
            }
            catch
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR,
                    "Greska prilikom pakovanja vina u paletu.");
                return (false, new Paleta());
            }
        }

        private Paleta KreirajNovuPaletu(Guid vinskiPodrumId, Guid vinoId)
        {
            string novaSifra = Guid.NewGuid().ToString("N");

            Paleta novaPaleta = new Paleta(
                sifra: novaSifra,
                adresaOdredista: string.Empty,
                vinskiPodrumId: vinskiPodrumId,
                vinoId: vinoId,
                trenutniBrojPakovanja: 0,
                status: StatusPalete.Aktivna);

            return paleteRepozitorijum.DodajPaletu(novaPaleta);
        }
    }
}