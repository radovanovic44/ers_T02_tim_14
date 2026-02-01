using NUnit.Framework;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Services.LoggerServisi;
using Services.ProdajaServisi;
using Tests.TestDoubles;
using Database.Repozitorijumi;

namespace Tests.Services
{
    // minimalni fake skladiste
    public class FakeSkladiste : ISkladistenjeServis
    {
        private readonly List<Paleta> _palete = new();

        public FakeSkladiste(IEnumerable<Paleta> palete) => _palete = palete.ToList();

        public bool PrimiPaletu(Paleta paleta)
        {
            _palete.Add(paleta);
            return true;
        }

        public List<Paleta> IsporuciPalete(int brojPaleta)
            => _palete.Where(p => p.Status == StatusPalete.Otpremljena).Take(brojPaleta).ToList();

        public bool ImaNaStanju(Guid vinoId)
            => _palete.Any(p => p.Status == StatusPalete.Otpremljena && p.Vino.Id == vinoId);
    }

   // [TestFixture]
   /* public class ProdajaServisiTests
    {
        [Test]
        public void KreirajFakturu_VracaFakturu_KadImaStanja()
        {
            var baza = new InMemoryBaza();
            var vinoRepo = new VinoRepozitorijum(baza);
            var faktureRepo = new FakturaRepozitorijum(baza);
            var logger = new Evidencija("testlog.txt");

            var vinoId = Guid.NewGuid();
            vinoRepo.Dodaj(new Vino(vinoId, "TestVino", KategorijaVina.Stolno, 0.75, "S1", Guid.NewGuid(), DateTime.Now));

            var paleta = new Paleta("P1", "BG", Guid.NewGuid(), new[] { vinoId, vinoId, vinoId }, StatusPalete.Otpremljena);
            var skladiste = new FakeSkladiste(new[] { paleta });

            var prodaja = new ProdajaServisi(vinoRepo, skladiste, faktureRepo, logger);

            var f = prodaja.KreirajFakturu(vinoId, 2, TipProdaje.RestoranskaProdaja, NacinPlacanja.Gotovina, 100);

            Assert.That(f, Is.Not.Null);
            Assert.That(f.UkupanIznos, Is.EqualTo(200));
        }
    }*/
}
