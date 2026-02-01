using NUnit.Framework;
using Domain.Modeli;
using Domain.Enumeracije;

namespace Tests.Domain
{
    [TestFixture]
    public class VinovaLozaTests
    {
        [Test]
        public void Konstruktor_Popunjava_Polja()
        {
            var loza = new VinovaLoza("Cabernet", 22.5, 2020, "Toskana", FazaZrelosti.Obrana);

            Assert.That(loza.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(loza.Naziv, Is.EqualTo("Cabernet"));
            Assert.That(loza.NivoSecera, Is.EqualTo(22.5));
        }
    }
}
