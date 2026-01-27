using Domain.BazaPodataka;
using Domain.Repozitorijumi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.BazaPodataka;
using Domain.Modeli;

namespace Database.Repozitorijumi
{
    public class FakturaRepozitorijum: IFakturaRepozitorijum
    {
        private readonly TabeleBazaPodataka _bazaPodataka;
        public FakturaRepozitorijum(TabeleBazaPodataka bazaPodataka)
        {
            _bazaPodataka = bazaPodataka;
        }
        public void Dodaj(Faktura faktura)
        {
            _bazaPodataka.Fakture.Add(faktura);
        }
        public List<Faktura> VratiSve()
        {
            return _bazaPodataka.Fakture;
        }
        public Faktura? PronadjipoID(Guid Id)
        {
            return _bazaPodataka.Fakture.FirstOrDefault(f => f.Id == Id);
        }



    }
}
