using Database.BazaPodataka;
using Database.Repozitorijumi;
using Domain.BazaPodataka;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Presentation.Authentifikacija;
using Presentation.Meni;
using Services.AutenftikacioniServisi;
using Services.LoggerServisi;
using Services.PakovanjeServisi;
using Services.ProdajaServisi;
using Services.ProizvodnjaServisi;
using Services.SkladistenjeServisi;
using Services.VinogradarstvoServisi;

namespace ERS_PROJEKAT
{
    public class Program
    {
        public static void Main()
        {
            // Baza podataka (XML)
            IBazaPodataka baza = new XMLBazaPodataka();

            // Repozitorijumi
            IKorisniciRepozitorijum korisniciRepo = new KorisniciRepozitorijum(baza);
            IVinovaLozaRepozitorijum lozeRepo = new VinovaLozaRepozitorijum(baza);
            IVinoRepozitorijum vinoRepo = new VinoRepozitorijum(baza);
            IPaleteRepozitorijum paleteRepo = new PaleteRepozitorijum(baza);
            IFakturaRepozitorijum faktureRepo = new FakturaRepozitorijum(baza);

            // Servisi
            ILoggerServis logger = new Evidencija();
            IAutentifikacijaServis auth = new AutentifikacioniServis(korisniciRepo, logger);

            IVinogradarstvoServis vinogradarstvo = new VinogradarstvoServis(lozeRepo, logger);
            IProizvodnjaVinaServis proizvodnja = new FermentacijaServisi(lozeRepo, vinoRepo, vinogradarstvo, logger);

            ISkladistenjeServis skladiste = new VinskiPodrumServis(paleteRepo, logger, baza.Tabele);

            IPakovanjeServis pakovanje = new PakovanjeServis(paleteRepo, vinoRepo, proizvodnja, skladiste, logger);
            IProdajaServis prodaja = new ProdajaServisi(vinoRepo, skladiste, faktureRepo, logger);

            // Meniji
            var katalogMeni = new KatalogVinaMeni(prodaja);
            var faktureMeni = new FaktureMeni(prodaja);


            var podrumId = baza.Tabele.VinskiPodrum.FirstOrDefault()?.Id ?? Guid.NewGuid();


            var am = new AutentifikacioniMeni(auth);

            while (true)
            {
                Korisnik prijavljen;

                // Autentifikacija (login/registracija). Ako korisnik izabere izlaz -> izlaz iz aplikacije.
                var ulogovan = am.Authenticate(out prijavljen);
                if (!ulogovan)
                    return;

                var opcije = new OpcijeMeni(prijavljen, proizvodnja, pakovanje, katalogMeni, faktureMeni, podrumId);
                opcije.PrikaziMeni();
            }
        }
    }
}
