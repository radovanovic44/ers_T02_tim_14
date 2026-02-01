using NUnit.Framework;
using Domain.Modeli;
using Domain.Enumeracije;

namespace Tests.Domain
{
    [TestFixture]
    public class PaletaTests
    {
        [Test]
        public void Paleta_Cuva_Listu_Vina()
        {
            var vino1 = Guid.NewGuid();
            var vino2 = Guid.NewGuid();

            var paleta = new Paleta("P1", "BG", Guid.NewGuid(), new[] { vino1, vino2 }, StatusPalete.Upakovana);

            Assert.That(paleta.VinaIds.Count, Is.EqualTo(2));
            Assert.That(paleta.VinaIds, Does.Contain(vino1));
            Assert.That(paleta.Status, Is.EqualTo(StatusPalete.Upakovana));
        }
    }
}
