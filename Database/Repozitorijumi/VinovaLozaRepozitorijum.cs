using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;

namespace Database.Repozitorijumi
{
    public class VinovaLozaRepozitorijum : IVinovaLozaRepozitorijum
    {
        private readonly IBazaPodataka _baza;

        public VinovaLozaRepozitorijum(IBazaPodataka baza)
        {
            _baza = baza;
        }

        public bool Dodaj(VinovaLoza loza)
        {
            try
            {
                _baza.Tabele.Loze.Add(loza);
                return _baza.SacuvajPromene();
            }
            catch
            {
                return false;
            }
        }

        public bool Azuriraj(VinovaLoza loza)
        {
            try
            {
                var postojeca = _baza.Tabele.Loze.FirstOrDefault(l => l.Id == loza.Id);
                if (postojeca == null) return false;

                postojeca.Naziv = loza.Naziv;
                postojeca.NivoSecera = loza.NivoSecera;
                postojeca.GodinaSadnje = loza.GodinaSadnje;
                postojeca.Region = loza.Region;
                postojeca.Faza = loza.Faza;

                return _baza.SacuvajPromene();
            }
            catch
            {
                return false;
            }
        }

        public VinovaLoza? PronadjiPoID(Guid id)
        {
            return _baza.Tabele.Loze.FirstOrDefault(l => l.Id == id);
        }

        public IEnumerable<VinovaLoza> VratiSve()
        {
            return _baza.Tabele.Loze;
        }

        public IEnumerable<VinovaLoza> PronadjiPoNazivu(string naziv)
        {
            return _baza.Tabele.Loze.Where(l => l.Naziv == naziv);
        }
    }
}
