using Domain.Modeli;

namespace Domain.Servisi
{
    public interface ISkladistenjeServis
    {
        bool PrimiPaletu(Paleta paleta);
        List<Paleta> IsporuciPalete(int brojPaleta);

      
        bool ImaNaStanju(Guid vinoId);
    }
}
