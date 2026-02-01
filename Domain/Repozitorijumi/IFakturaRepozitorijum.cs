using Domain.Modeli;

namespace Domain.Repozitorijumi
{
    public interface IFakturaRepozitorijum
    {
        bool Dodaj(Faktura faktura);
        IEnumerable<Faktura> VratiSve();
    }
}
