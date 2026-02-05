using NUnit.Framework;
using Domain.Enumeracije;
using Services.LoggerServisi;
using Services.ProizvodnjaServisi;
using Services.VinogradarstvoServisi;
using Tests.TestDoubles;
using Database.Repozitorijumi;
using System.Linq;

namespace Tests.Services
{
    [TestFixture]
    public class FermentacijaServisiTests
    {
        [Test]
        public void ZapocniFermentaciju_Proizvede_Vino_Sa_Tacnom_Kolicinom()
        {
            var baza = new InMemoryBaza();
            var lozeRepo = new VinovaLozaRepozitorijum(baza);
            var vinoRepo = new VinoRepozitorijum(baza);
            var logger = new Evidencija("testlog.txt");
            var vinograd = new VinogradarstvoServis(lozeRepo, logger);
            var servis = new FermentacijaServisi(lozeRepo, vinoRepo, vinograd, logger);

            // obezbedimo da postoji bar jedna obrana loza (servis ce po potrebi dosaditi nove)
            var loza = vinograd.PosadiNovuLozu("Cabernet", 2020, "Toskana");
            loza.Faza = FazaZrelosti.Obrana;
            lozeRepo.Azuriraj(loza);

            var ok = servis.ZapocniFermentaciju(KategorijaVina.Stolno, 3, 0.75);

            Assert.That(ok, Is.True);

            var proizvedena = vinoRepo.VratiSve().ToList();
            Assert.That(proizvedena.Count, Is.EqualTo(1));

            var vino = proizvedena.Single();
            Assert.That(vino.Kategorija, Is.EqualTo(KategorijaVina.Stolno));
            Assert.That(vino.Zapremina, Is.EqualTo(0.75));
            Assert.That(vino.KolicinaFlasa, Is.EqualTo(3));
        }
    }
}
