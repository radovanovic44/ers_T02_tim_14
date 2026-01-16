using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Domain.PomocneMetode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public List<Vino> ZahtevZaVino(KategorijaVina kategorijaVina, int kolicina)
        {
            if (kolicina <= 0) { throw new ArgumentException("Kolicina mora biti veca od nule"); }
            ;
            var rezultat = new List<Vino>();

            while (rezultat.Count < kolicina && _proizvedena.Count > 0)
            {
                var v = _proizvedena.Dequeue();
                if (v.Kategorija == kategorijaVina)
                {
                    rezultat.Add(v);
                }
            }
            if(rezultat.Count < kolicina)
            {
                _logger.EvidentirajDogadjaj(TipEvidencije.WARNING, $"Trazeno {kolicina} vina ({kategorijaVina}), dostupno {rezultat.Count}");
            }



            return rezultat;
        }

        public void zapocniFermentaciju(KategorijaVina kategorijaVina, int brojFlasa, double zapreminaFlase)
        {
            if(brojFlasa <= 0)
            {
                throw new ArgumentException("Broj flasa mora biti veci od nule.");
            }
            if(zapreminaFlase != 0.75 && zapreminaFlase != 1.5)
            {
                throw new ArgumentException("Zapremina flase more biti 0.75 L ili 1.5 L");
            }


            // izracunavanje potrebnih loza jedna loza je 1.2l
            double ukupnoLitara = brojFlasa * zapreminaFlase;
            int potrebnoLoza = (int)Math.Ceiling(ukupnoLitara / 1.2);

            _logger.EvidentirajDogadjaj(TipEvidencije.INFO, $"Start fermentacije : {kategorijaVina}, {brojFlasa}x{zapreminaFlase}L => potrebno loza : {potrebnoLoza}");


            var obrane = _lozaRepo.VratiSve()
                .Where(l => l.Faza == FazaZrelosti.Obrana)
                .ToList();


            //automatska sadnja

            while(obrane.Count < potrebnoLoza)
            {
                var nova = _vinogradarstvo.PosadiNovuLozu("AutomatskaLoza", DateTime.Now.Year, "Toskana");
                nova.Faza = FazaZrelosti.Obrana;

                _lozaRepo.Azuriraj(nova);
                obrane.Add(nova);

                _logger.EvidentirajDogadjaj(TipEvidencije.WARNING, "Nema dovoljan broj loza, automatski posadjena nova loza za fermentaciju.");

            }

            //balansiranje secera

            foreach(var loza in obrane.Take(potrebnoLoza)){
                
                if(loza.NivoSecera > 24.0)
                {
                    double visak = Math.Round(loza.NivoSecera - 24.0, 2);

                    var kompenzaciona = _vinogradarstvo.PosadiKompenzacionuLozu(visak);

                    kompenzaciona.Faza = FazaZrelosti.Obrana;
                    _lozaRepo.Azuriraj(kompenzaciona);

                    obrane.Add(kompenzaciona);

                    _logger.EvidentirajDogadjaj(TipEvidencije.INFO, $"Balans secera: loza{loza.Id} ima {loza.NivoSecera} Brix, visak = {visak}");

                }
            }


            var lozeZaProces = obrane.Take(potrebnoLoza).ToList();

            for(int i = 0; i < brojFlasa; i++)
            {
                var loza = lozeZaProces[i % lozeZaProces.Count];
                var id = Guid.NewGuid();
                var sifra = GeneratorSifreVina.GenerisiSifruVina(id);
                var vino = new Vino(
                    id,
                    $"{kategorijaVina} vino",
                    kategorijaVina,
                    zapreminaFlase,                 
                    sifra,           
                    loza.Id,                       
                    DateTime.Now
                 );

                _vinoRepo.Dodaj(vino);
                _proizvedena.Enqueue(vino);

            }
        }


        


    }
}
