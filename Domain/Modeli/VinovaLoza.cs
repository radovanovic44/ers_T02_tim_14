using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
   
    public enum FazaZrelosti
    {
        Posadjena,
        Cveta,
        Zrenje,
        SpremnaZaBerbu,
        Obrana
    }

    public class VinovaLoza
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Naziv { get; set; } = string.Empty;
        public double NivoSecera { get; set; }         //Ovo nam mora biti u opsegu od 15 do 28
        public int GodinaSadnje { get; set; }
        public string Region { get; set; } = string.Empty;
        public FazaZrelosti Faza { get; set; }

        
        public VinovaLoza() { }
    }

}
