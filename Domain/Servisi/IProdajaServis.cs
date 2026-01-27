using Domain.Enumeracije;
using Domain.Modeli;
using Domain.PomocneMetode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Servisi
{
    public interface IProdajaServis
    {
        IEnumerable<KatalogVinaStavka> VratiKatalogVina();

       Faktura KreirajFakturu(
       Guid vinoId,
       int kolicina,
       TipProdaje tipProdaje,
       NacinPlacanja nacinPlacanja,
       decimal cenaPoFlasi );
        List<Faktura> VratiSveFakture();
    }
}
