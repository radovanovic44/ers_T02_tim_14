using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Servisi
{
    public interface IVinogradarstvoServis
    {
        VinovaLoza PosadiNovuLozu(string naziv, int godinaSadnje, string region);
        void promeniNivoSecera(Guid lozaid, double procenat);
        List<VinovaLoza> oberiLoze(string NazivSorte, int kolicina);

        VinovaLoza PosadiKompenzacionuLozu(double visakSecera);
    }
}
