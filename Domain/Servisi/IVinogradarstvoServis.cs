using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IVinogradarstvoServis
    {
        VinovaLoza PosadiNovuLozu(string naziv, int godinaSadnje, string region);

       
        bool PromeniNivoSecera(Guid lozaId, double procenat);

        List<VinovaLoza> OberiLoze(string nazivSorte, int kolicina);

       
        VinovaLoza PosadiKompenzacionuLozu(double visakSecera);
    }
}
