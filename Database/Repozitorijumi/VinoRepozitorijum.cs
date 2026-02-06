using Domain.BazaPodataka;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;

namespace Database.Repozitorijumi
{
    public class VinoRepozitorijum : IVinoRepozitorijum
    {
        private readonly IBazaPodataka _baza;

        public VinoRepozitorijum(IBazaPodataka baza)
        {
            _baza = baza;
        }

        public bool Dodaj(Vino vino)
        {
            try
            {
                _baza.Tabele.Vina.Add(vino);
                return _baza.SacuvajPromene();
            }
            catch
            {
                return false;
            }
        }

        public bool Obrisi(Guid id)
        {
            try
            {
                var vino = _baza.Tabele.Vina.FirstOrDefault(v => v.Id == id);
                if (vino == null) return false;

                _baza.Tabele.Vina.Remove(vino);
                return _baza.SacuvajPromene();
            }
            catch
            {
                return false;
            }
        }

        public bool SacuvajIzmene()
        {
            try
            {
                return _baza.SacuvajPromene();
            }
            catch
            {
                return false;
            }
        }

        public Vino? PronadjiPoId(Guid id)
        {
            return _baza.Tabele.Vina.FirstOrDefault(v => v.Id == id);
        }

        public IEnumerable<Vino> VratiSve()
        {
            return _baza.Tabele.Vina;
        }

        public IEnumerable<Vino> PronadjiPoKategoriji(KategorijaVina kategorija)
        {
            return _baza.Tabele.Vina.Where(v => v.Kategorija == kategorija);
        }


    }
}
