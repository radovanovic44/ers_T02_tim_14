using Domain.Repozitorijumi;
using Domain.Modeli;
using Domain.Enumeracije;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.BazaPodataka;

namespace Database.Repozitorijumi
{
    public class VinoRepozitorijum:IVinoRepozitorijum
    {
        private readonly TabeleBazaPodataka _bazaPodataka;
        public void Dodaj(Vino vino)
        {
            _bazaPodataka.Vina.Add(vino);
        }
        public Vino? PronadjiPoId(Guid id)
        {
            return _bazaPodataka.Vina.FirstOrDefault(v => v.Id == id);
        }
        public IEnumerable<Vino> PronadjiPoKategoriji(KategorijaVina kategorija)
        {
            return _bazaPodataka.Vina.Where(v => v.Kategorija == kategorija);
        }
        public IEnumerable<Vino> PronadjiPoVinovojLozi(Guid vinovaLozaId)
        {
            return _bazaPodataka.Vina.Where(v => v.VinovaLozaId == vinovaLozaId);
        }
        public IEnumerable<Vino> VratiSve()
        {
            return _bazaPodataka.Vina;
        }
    }
}
