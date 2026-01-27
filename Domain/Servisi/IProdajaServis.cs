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
    }
}
