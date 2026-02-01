using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.Repozitorijumi
{
    public interface IVinoRepozitorijum
    {
        bool Dodaj(Vino vino);
        Vino? PronadjiPoId(Guid id);
        IEnumerable<Vino> VratiSve();
        IEnumerable<Vino> PronadjiPoKategoriji(KategorijaVina kategorija);
        
    }
}
