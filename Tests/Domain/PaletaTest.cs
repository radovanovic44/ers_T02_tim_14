using System;
using System.Collections.Generic;
using NUnit.Framework;
using Domain.Modeli;
using Domain.Enumeracije;

namespace Tests.Domain
{
    [TestFixture]
    public class PaletaTests
    {
        [Test]
        public void Paleta_Cuva_Vino_I_Status()
        {
            var vinoId = Guid.NewGuid();
            var vino = new Vino(
                vinoId,
                "TestVino",
                KategorijaVina.Stolno,
                0.75,
                "S1",
                new List<VinovaLoza>(),
                DateTime.Now,
                24);

            var podrumId = Guid.NewGuid();
            var paleta = new Paleta("P1", "BG", podrumId, vino, StatusPalete.Upakovana);

            Assert.That(paleta.Vino, Is.Not.Null);
            Assert.That(paleta.Vino.Id, Is.EqualTo(vinoId));
            Assert.That(paleta.Vino.KolicinaFlasa, Is.EqualTo(24));
            Assert.That(paleta.Status, Is.EqualTo(StatusPalete.Upakovana));
        }
    }
}
