using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Modeli;

namespace Domain.Modeli
{
    public class VinskiPodrum
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Naziv { get; set; } = string.Empty;

        public int Temperatura {  get; set; } = 0;

        public int MaxBrojPaleta { get; set; } = 0;

        public VinskiPodrum() { }

        public VinskiPodrum(Guid id, string naziv, int temperatura, int maxBrojPaleta)
        {
            Id = id;
            Naziv = naziv;
            Temperatura = temperatura;
            MaxBrojPaleta = maxBrojPaleta;
        }
    }
}
