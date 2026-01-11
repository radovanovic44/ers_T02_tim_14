using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Modeli;

namespace Domain.Servisi
{
    public interface ISkladistenjeServis
    {
        void PrimiPaletu(Paleta paleta);
        List<Paleta> IsporuciPalete(int brojPaleta);
    }
}
