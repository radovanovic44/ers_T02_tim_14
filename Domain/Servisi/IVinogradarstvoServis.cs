using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IVinogradarstvoServis
    {
        VinovaLoza PosadiNovuLozu(string naziv, int godinaSadnje, string region);

        // promena nivoa secera za procenat (npr. 10 znaci +10%)
        bool PromeniNivoSecera(Guid lozaId, double procenat);

        List<VinovaLoza> OberiLoze(string nazivSorte, int kolicina);

        // pomocna metoda koju poziva servis fermentacije u slucaju prekomernog secera
        VinovaLoza PosadiKompenzacionuLozu(double visakSecera);
    }
}
