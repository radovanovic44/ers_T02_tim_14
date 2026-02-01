using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;

namespace Domain.Modeli
{

    public class Vino
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Naziv { get; set; } = string.Empty;
        public KategorijaVina Kategorija { get; set; }
        public double Zapremina { get; set; }          // od 0.75 do 1.5 litara
        public string SifraSerije { get; set; } = string.Empty;
        public List<VinovaLoza> loze { get; set; } = new List<VinovaLoza>();
        public DateTime DatumFlasiranja { get; set; }
        public int KolicinaFlasa {  get; set; }


        public Vino() { }
        public Vino(Guid id, string naziv, KategorijaVina kategorija, double zapremina, string sifraserije, List<VinovaLoza> vinoveLoze, DateTime datumflasiranja, int kolicinaFlasa)
        {

            Id = id;
            Naziv = naziv;
            Kategorija = kategorija;
            Zapremina = zapremina;
            SifraSerije = sifraserije;
            loze = vinoveLoze;
            DatumFlasiranja = datumflasiranja;
            KolicinaFlasa = kolicinaFlasa;

        }
    }

}
