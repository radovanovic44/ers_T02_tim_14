using Domain.Enumeracije;
using Domain.Modeli;
using Domain.PomocneMetode;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.ProizvodnjaServisi
{
    public class FermentacijaServisi : IProizvodnjaVinaServis
    {
        private readonly IVinovaLozaRepozitorijum _lozaRepo;
        private readonly Queue<Vino> _proizvedena = new();
        private readonly IVinoRepozitorijum _vinoRepo;
        private readonly IVinogradarstvoServis _vinogradarstvo;
        private readonly ILoggerServis _logger;

        public FermentacijaServisi(
            IVinovaLozaRepozitorijum lozaRepo,
            IVinoRepozitorijum vinoRepo,
            IVinogradarstvoServis vinogradarstvo,
            ILoggerServis logger)
        {
            _lozaRepo = lozaRepo;
            _vinoRepo = vinoRepo;
            _vinogradarstvo = vinogradarstvo;
            _logger = logger;
        }

        public Vino ZahtevZaVino(Guid id, int kolicina)
        {

            if (kolicina <= 0) throw new ArgumentException("Kolicina mora biti veca od nule.");

            Vino vino = _vinoRepo.PronadjiPoId(id);

            if (vino == null)
                throw new KeyNotFoundException("Vino nije pronadjeno.");

            if (vino.KolicinaFlasa < kolicina)
            {
                throw new InvalidOperationException("Nema dovoljno vina na stanju.");
                _logger.EvidentirajDogadjaj(
                    TipEvidencije.WARNING,
                    $"Trazeno {kolicina} vina ({vino.Kategorija}), dostupno {vino.KolicinaFlasa}.");
            }

            if (vino == null || vino.KolicinaFlasa < kolicina)
                return null;

            return vino;
        }

        public bool ZapocniFermentaciju(KategorijaVina kategorijaVina, int brojFlasa, double zapreminaFlase)
        {
            try
            {
                if (brojFlasa <= 0) throw new ArgumentException("Broj flasa mora biti veci od nule.");
                if (zapreminaFlase != 0.75 && zapreminaFlase != 1.5)
                    throw new ArgumentException("Zapremina flase mora biti 0.75 L ili 1.5 L.");

               
                double ukupnoLitara = brojFlasa * zapreminaFlase;
                int potrebnoLoza = (int)Math.Ceiling(ukupnoLitara / 1.2);

                _logger.EvidentirajDogadjaj(
                    TipEvidencije.INFO,
                    $"Start fermentacije: {kategorijaVina}, {brojFlasa}x{zapreminaFlase}L => potrebno loza: {potrebnoLoza}.");

                var obrane = _lozaRepo.VratiSve()
                    .Where(l => l.Faza == FazaZrelosti.Obrana)
                    .ToList();

                
                while (obrane.Count < potrebnoLoza)
                {
                    var nova = _vinogradarstvo.PosadiNovuLozu("AutomatskaLoza", DateTime.Now.Year, "Toskana");
                    nova.Faza = FazaZrelosti.Obrana;
                    _lozaRepo.Azuriraj(nova);
                    obrane.Add(nova);

                    _logger.EvidentirajDogadjaj(
                        TipEvidencije.WARNING,
                        "Nema dovoljan broj loza, automatski posadjena nova loza za fermentaciju.");
                }

              
                foreach (var loza in obrane.Take(potrebnoLoza))
                {
                    if (loza.NivoSecera > 24.0)
                    {
                        double visak = Math.Round(loza.NivoSecera - 24.0, 2);
                        var kompenzaciona = _vinogradarstvo.PosadiKompenzacionuLozu(visak);
                        kompenzaciona.Faza = FazaZrelosti.Obrana;
                        _lozaRepo.Azuriraj(kompenzaciona);
                        obrane.Add(kompenzaciona);

                        _logger.EvidentirajDogadjaj(
                            TipEvidencije.INFO,
                            $"Balans secera: loza {loza.Id} ima {loza.NivoSecera} Brix, visak={visak}.");
                    }
                }

                var lozeZaProces = obrane.Take(potrebnoLoza).ToList();
                var id = Guid.NewGuid();
                var sifra = GeneratorSifreVina.GenerisiSifruVina(id);
                var vino = new Vino(
                        id,
                        $"{kategorijaVina} vino",
                        kategorijaVina,
                        zapreminaFlase,
                        sifra,
                        lozeZaProces,
                        DateTime.Now,
                        brojFlasa
                    );
                _vinoRepo.Dodaj(vino);

                return true;
            }
            catch (Exception ex)
            {
                _logger.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Greska fermentacije: {ex.Message}");
                return false;
            }
        }
    }
}
