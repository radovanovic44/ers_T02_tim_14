using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Domain.BazaPodataka;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.SkladistenjeServisi
{

    public class VinskiPodrumServis : ISkladistenjeServis
    {
        private readonly IPaleteRepozitorijum _paleteRepo;
        private readonly ILoggerServis _logger;
        private readonly TabeleBazaPodataka _tabele;

        public VinskiPodrumServis(IPaleteRepozitorijum paleteRepo, ILoggerServis logger, TabeleBazaPodataka tabele)
        {
            _paleteRepo = paleteRepo;
            _logger = logger;
            _tabele = tabele;
        }

        public bool PrimiPaletu(Paleta paleta)
        {
            try
            {
                // Paleta je vec dodata prilikom pakovanja; ovde ne smemo praviti duplikat.
                // Ako iz nekog razloga nije u repozitorijumu, dodaj je jednom.
                var postojeca = _paleteRepo.PronadjiPaletuPoSifri(paleta.Sifra);
                if (postojeca == null)
                {
                    _paleteRepo.DodajPaletu(paleta);
                }
                _logger.EvidentirajDogadjaj(TipEvidencije.INFO, $"Paleta {paleta.Sifra} primljena u skladiste.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Greska prijema palete: {ex.Message}");
                return false;
            }
        }

        public List<Paleta> IsporuciPalete(int brojPaleta)
        {
            var isporucene = new List<Paleta>();
            int max = Math.Min(brojPaleta, 5);

            foreach (var paleta in _paleteRepo.SvePalete())
            {
                if (isporucene.Count == max) break;

                if (paleta.Status == StatusPalete.Otpremljena)
                {
                    Thread.Sleep(300); // 0.3s
                    isporucene.Add(paleta);
                }
            }

            _logger.EvidentirajDogadjaj(TipEvidencije.INFO, $"Vinski podrum isporucio {isporucene.Count} paleta.");
            return isporucene;
        }


        public List<Paleta> IsporuciPaleteZaVino(Guid vinoId, int brojPaleta)
        {
            var isporucene = new List<Paleta>();
            int max = Math.Min(brojPaleta, 5);

            foreach (var paleta in _paleteRepo.SvePalete())
            {
                if (isporucene.Count == max) break;

                if (paleta.Status == StatusPalete.Otpremljena && paleta.Vino.Id == vinoId)
                {
                    Thread.Sleep(300); // 0.3s
                    isporucene.Add(paleta);
                }
            }

            _logger.EvidentirajDogadjaj(TipEvidencije.INFO, $"Vinski podrum isporucio {isporucene.Count} paleta za vino {vinoId}.");
            return isporucene;
        }

        public int DostupnoFlasa(Guid vinoId)
        {
            // Saberi stvaran broj flasa na paletama (ne pretpostavljaj uvek 24)
            return _tabele.Palete
                .Where(p => p.Status == StatusPalete.Otpremljena && p.Vino.Id == vinoId)
                .Sum(p => p.Vino.KolicinaFlasa);
        }

        public bool ImaNaStanju(Guid vinoId)
        {
            return _tabele.Palete.Any(p => p.Status == StatusPalete.Otpremljena && p.Vino.Id == vinoId);
        }
    }
}