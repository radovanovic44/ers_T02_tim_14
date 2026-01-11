using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repozitorijumi
{
    public interface IVinovaLozaRepozitorijum
    {
        void Dodaj(VinovaLoza loza);

        VinovaLoza? PronadjiPoID(Guid id);

        IEnumerable<VinovaLoza> VratiSve();

        IEnumerable<VinovaLoza> PronadjiPoNazivu(string naziv);
    }
}
