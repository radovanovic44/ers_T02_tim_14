using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Modeli;
using Domain.BazaPodataka;
using Domain.Repozitorijumi;

namespace Database.Repozitorijumi
{
    public class VinovaLozaRepozitorijum: IVinovaLozaRepozitorijum
    {
        private readonly TabeleBazaPodataka _bazaPodataka;
        public VinovaLozaRepozitorijum(TabeleBazaPodataka bazaPodataka)
        {
            _bazaPodataka = bazaPodataka;
        }
        public void Dodaj(VinovaLoza loza)
        {
            _bazaPodataka.Loze.Add(loza);
        }
        public void Azuriraj(VinovaLoza loza)
        {
            var postojecaLoza = _bazaPodataka.Loze
                .FirstOrDefault(l => l.Id == loza.Id);

            if (postojecaLoza == null)
                throw new InvalidOperationException($"Loza sa ID {loza.Id} ne postoji.");

            postojecaLoza.Naziv = loza.Naziv;
            postojecaLoza.NivoSecera = loza.NivoSecera;
            postojecaLoza.GodinaSadnje = loza.GodinaSadnje;
            postojecaLoza.Region = loza.Region;
            postojecaLoza.Faza = loza.Faza;
        }



        public VinovaLoza? PronadjiPoID(Guid id)
        {
            return _bazaPodataka.Loze.FirstOrDefault(l => l.Id == id);
        }
        public IEnumerable<VinovaLoza> VratiSve()
        {
            return _bazaPodataka.Loze;
        }
        public IEnumerable<VinovaLoza> PronadjiPoNazivu(string naziv)
        {
            return _bazaPodataka.Loze.Where(l => l.Naziv == naziv);
        }
           

    }
}
