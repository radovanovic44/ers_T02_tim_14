using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.Repozitorijumi
{
    public interface IVinoRepozitorijum
    {
        bool Dodaj(Vino vino);
        bool Obrisi(Guid id);
        Vino? PronadjiPoId(Guid id);
        IEnumerable<Vino> VratiSve();
        IEnumerable<Vino> PronadjiPoKategoriji(KategorijaVina kategorija);
        bool SacuvajIzmene();

    }
}
