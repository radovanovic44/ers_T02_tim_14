using Domain.Modeli;

namespace Domain.Repozitorijumi
{
    public interface IVinovaLozaRepozitorijum
    {
        bool Dodaj(VinovaLoza loza);
        bool Azuriraj(VinovaLoza loza);
        VinovaLoza? PronadjiPoID(Guid id);
        IEnumerable<VinovaLoza> VratiSve();
        IEnumerable<VinovaLoza> PronadjiPoNazivu(string naziv);
    }
}
