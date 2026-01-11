using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;

namespace Domain.Modeli
{
   
   

    public class VinovaLoza
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Naziv { get; set; } = string.Empty;
        public double NivoSecera { get; set; } = 0;     //Ovo nam mora biti u opsegu od 15 do 28
        public int GodinaSadnje { get; set; }
        public string Region { get; set; } = string.Empty;
        public FazaZrelosti Faza { get; set; }

        
        public VinovaLoza(Guid id,string naziv,double nivosecera,int godinasadnje,string region,FazaZrelosti faza) {
            Id = id;
            Naziv = naziv;
            NivoSecera = nivosecera;
            GodinaSadnje = godinasadnje;
            Region = region;
            Faza = faza;
        }
    }

}
