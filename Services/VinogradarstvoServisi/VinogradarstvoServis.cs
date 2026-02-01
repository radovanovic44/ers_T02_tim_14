using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.VinogradarstvoServisi
{
    public class VinogradarstvoServis : IVinogradarstvoServis
    {
        private readonly IVinovaLozaRepozitorijum _repo;
        private readonly ILoggerServis _logger;
        private readonly Random _random = new();

        public VinogradarstvoServis(IVinovaLozaRepozitorijum repo, ILoggerServis logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public VinovaLoza PosadiNovuLozu(string naziv, int godinaSadnje, string region)
        {
            var loza = new VinovaLoza(
                naziv: naziv,
                nivoSecera: RandomSecer(),
                godinaSadnje: godinaSadnje,
                region: region,
                faza: FazaZrelosti.Posadjena
            );

            _repo.Dodaj(loza);
            _logger.EvidentirajDogadjaj(TipEvidencije.INFO, $"Posađena nova loza ({loza.NivoSecera} Brix).");
            return loza;
        }

        public bool PromeniNivoSecera(Guid lozaId, double procenat)
        {
            var loza = _repo.PronadjiPoID(lozaId);
            if (loza == null)
            {
                _logger.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Loza sa ID {lozaId} ne postoji.");
                return false;
            }

            loza.NivoSecera += loza.NivoSecera * procenat / 100.0;
            loza.NivoSecera = Math.Round(loza.NivoSecera, 2);

            var ok = _repo.Azuriraj(loza);
            _logger.EvidentirajDogadjaj(TipEvidencije.INFO, $"Promenjen nivo secera loze {lozaId} za {procenat}%.");
            return ok;
        }

        public List<VinovaLoza> OberiLoze(string nazivSorte, int kolicina)
        {
            var dostupne = _repo.VratiSve()
                .Where(l => l.Naziv == nazivSorte && l.Faza == FazaZrelosti.SpremnaZaBerbu)
                .Take(kolicina)
                .ToList();

            foreach (var loza in dostupne)
            {
                loza.Faza = FazaZrelosti.Obrana;
                _repo.Azuriraj(loza);
            }

            _logger.EvidentirajDogadjaj(TipEvidencije.INFO, $"Odbrano {dostupne.Count} loza sorte {nazivSorte}.");
            return dostupne;
        }

        public VinovaLoza PosadiKompenzacionuLozu(double visakSecera)
        {
            var novaLoza = PosadiNovuLozu("Kompenzaciona", DateTime.Now.Year, "Toskana");
            novaLoza.NivoSecera = Math.Round(novaLoza.NivoSecera - visakSecera, 2);

            _repo.Azuriraj(novaLoza);
            _logger.EvidentirajDogadjaj(TipEvidencije.INFO, $"Kompenzacija secera: -{visakSecera} Brix.");
            return novaLoza;
        }

        private double RandomSecer() => Math.Round(15.0 + _random.NextDouble() * 13.0, 2);
    }
}
