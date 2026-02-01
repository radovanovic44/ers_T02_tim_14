using NUnit.Framework;
using Domain.Enumeracije;
using Domain.Repozitorijumi;
using Services.VinogradarstvoServisi;
using Services.LoggerServisi;
using Tests.TestDoubles;
using Database.Repozitorijumi;

namespace Tests.Services
{
    [TestFixture]
    public class VinogradarstvoServisTests
    {
        [Test]
        public void PromeniNivoSecera_VracaTrue_KadLozaPostoji()
        {
            var baza = new InMemoryBaza();
            var repo = new VinovaLozaRepozitorijum(baza);
            var logger = new Evidencija("testlog.txt");
            var servis = new VinogradarstvoServis(repo, logger);

            var loza = servis.PosadiNovuLozu("Cabernet", 2020, "Toskana");
            loza.Faza = FazaZrelosti.SpremnaZaBerbu;
            repo.Azuriraj(loza);

            var ok = servis.PromeniNivoSecera(loza.Id, 10);

            Assert.That(ok, Is.True);
            var posle = repo.PronadjiPoID(loza.Id);
            Assert.That(posle!.NivoSecera, Is.GreaterThan(loza.NivoSecera * 0.9));
        }
    }
}
