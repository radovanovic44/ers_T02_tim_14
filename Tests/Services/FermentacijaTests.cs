using NUnit.Framework;
using Domain.Enumeracije;
using Services.LoggerServisi;
using Services.ProizvodnjaServisi;
using Services.VinogradarstvoServisi;
using Tests.TestDoubles;
using Database.Repozitorijumi;

namespace Tests.Services
{
    [TestFixture]
    public class FermentacijaServisiTests
    {
        [Test]
        public void ZapocniFermentaciju_Proizvede_Vina()
        {
            var baza = new InMemoryBaza();
            var lozeRepo = new VinovaLozaRepozitorijum(baza);
            var vinoRepo = new VinoRepozitorijum(baza);
            var logger = new Evidencija("testlog.txt");
            var vinograd = new VinogradarstvoServis(lozeRepo, logger);
            var servis = new FermentacijaServisi(lozeRepo, vinoRepo, vinograd, logger);

            // obezbedimo da postoje obrane loze
            var loza = vinograd.PosadiNovuLozu("Cabernet", 2020, "Toskana");
            loza.Faza = FazaZrelosti.Obrana;
            lozeRepo.Azuriraj(loza);

            var ok = servis.ZapocniFermentaciju(KategorijaVina.Stolno, 3, 0.75);
            Assert.That(ok, Is.True);

            /*var vina = servis.ZahtevZaVino(KategorijaVina.Stolno, 3);
            Assert.That(vina.Count, Is.EqualTo(3));*/
        }
    }
}
