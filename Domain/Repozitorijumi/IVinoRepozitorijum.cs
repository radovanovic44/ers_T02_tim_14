using Domain.Enumeracije;
using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repozitorijumi
{
    public interface IVinoRepozitorijum
    {
        void Dodaj(Vino vino);

        Vino? PronadjiPoId(Guid id);

        IEnumerable<Vino> VratiSve();

        IEnumerable<Vino> PronadjiPoKategoriji(KategorijaVina kategorija);

        IEnumerable<Vino> PronadjiPoVinovojLozi(Guid vinovaLozaId);
    }

}
