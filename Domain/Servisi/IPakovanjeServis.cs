using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IPakovanjeServis
    {
    
        IEnumerable<Vino> VratiVinaZaPakovanje(KategorijaVina kategorija);

     
        (bool, Paleta) UpakujVina(Guid vinskiPodrumId, Guid vinoId, int brojFlasa, double zapreminaFlase);

     
        (bool, Paleta) UpakujVina(Guid vinskiPodrumId, KategorijaVina kategorija, int brojFlasa, double zapreminaFlase);

       
        IEnumerable<Paleta> VratiUpakovanePalete(Guid vinskiPodrumId);

      
        bool PosaljiPaletuUSkladiste(Guid vinskiPodrumId, Guid paletaId);

        bool PosaljiPaletuUSkladiste(Guid vinskiPodrumId);
    }
}
