using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Servisi;
using Domain.Repozitorijumi;
using Domain.Modeli;
using Domain.Enumeracije;
namespace Services.VinogradarstvoServisi
{
    public class VinogradarstvoServis : IVinogradarstvoServis
    {
        private readonly IVinovaLozaRepozitorijum _repo;
        private readonly ILoggerServis _logger;
        private readonly Random _random = new();


        public VinogradarstvoServis(IVinovaLozaRepozitorijum repo,ILoggerServis logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public VinovaLoza PosadiNovuLozu(string naziv,int godinaSadnje, string region)
        {
            var loza = new VinovaLoza
            (
               Guid.NewGuid(),
               naziv,
               RandomSecer(),
               godinaSadnje,
               region,
               FazaZrelosti.Posadjena


            );
            _repo.Dodaj(loza);
            _logger.EvidentirajDogadjaj(TipEvidencije.INFO,$"Posađena nova loza ({loza.NivoSecera} Brix)");
            return loza;
        }

        public void promeniNivoSecera(Guid lozaId,double procenat)
        {
            var loza= _repo.PronadjiPoID(lozaId);
            loza.NivoSecera += loza.NivoSecera * procenat / 100;
            _repo.Azuriraj(loza);
            _logger.EvidentirajDogadjaj(TipEvidencije.INFO, $"Promenjen nivo secera loze {lozaId}");
        }

        




        private double RandomSecer()=> Math.Round(15.0 + _random.NextDouble() * 13.0, 2);
    }
}
