using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repozitorijumi
{
    public interface IFakturaRepozitorijum
    {
        void Dodaj(Faktura faktura);
        List<Faktura> VratiSve();

        Faktura? PronadjipoID(Guid Id);    
    }
}
