using Domain.BazaPodataka;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.SkladistenjeServisi
{
    // Implementacija skladistenja za ulogu GLAVNI_ENOLOG (do 5 paleta, 0.3s po paleti)
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
                _paleteRepo.DodajPaletu(paleta);
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

        public bool ImaNaStanju(Guid vinoId)
        {
            return _tabele.Palete.Any(p => p.Status == StatusPalete.Otpremljena && p.Vino.Id == vinoId);
        }
    }
}
