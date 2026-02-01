using Domain.Modeli;

namespace Domain.Servisi
{
    public interface ISkladistenjeServis
    {
        bool PrimiPaletu(Paleta paleta);
        List<Paleta> IsporuciPalete(int brojPaleta);

        // da li postoji bar jedno vino sa datim ID u skladistu
        bool ImaNaStanju(Guid vinoId);
    }
}
