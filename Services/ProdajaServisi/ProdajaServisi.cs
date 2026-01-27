using Domain.Enumeracije;
using Domain.PomocneMetode;
using Domain.Repozitorijumi;
using Domain.Servisi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ProdajaServisi
{
    public class ProdajaServisi : IProdajaServis
    {
        private readonly IVinoRepozitorijum _vinoRepo;
        private readonly ISkladistenjeServis _skladisteServis;
        private readonly ILoggerServis _logger; 

        public ProdajaServisi(
            IVinoRepozitorijum vinoRepo,
            ISkladistenjeServis skladisteServis,
            ILoggerServis logger 
        )
        {
            _vinoRepo = vinoRepo;
            _skladisteServis = skladisteServis;
            _logger = logger;
        }

        public IEnumerable<KatalogVinaStavka> VratiKatalogVina()
        {
            var katalog = _vinoRepo.VratiSve()
                .Where(v => _skladisteServis.ImaNaStanju(v.Id))
                .Select(v => new KatalogVinaStavka
                {
                    VinoId = v.Id,
                    Naziv = v.Naziv,
                    Kategorija = v.Kategorija,
                    Zapremina = v.Zapremina
                })
                .OrderBy(x => x.Kategorija)
                .ThenBy(x => x.Naziv)
                .ToList();

            _logger?.EvidentirajDogadjaj(TipEvidencije.INFO,"Prikazan katalog vina.");

            return katalog;
        }
    }
}

