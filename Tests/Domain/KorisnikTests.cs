using NUnit.Framework;
using Domain.Modeli;
using Domain.Enumeracije;

namespace Tests.Domain
{
    [TestFixture]
    public class KorisnikTests
    {
        [Test]
        public void Konstruktor_Postavlja_Ulogu_I_ID()
        {
            var k = new Korisnik("test", "1234", "Test Test", TipKorisnika.KELAR_MAJSTOR);

            Assert.That(k.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(k.KorisnickoIme, Is.EqualTo("test"));
            Assert.That(k.Uloga, Is.EqualTo(TipKorisnika.KELAR_MAJSTOR));
        }
    }
}
