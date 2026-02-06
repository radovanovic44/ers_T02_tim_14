using Domain.BazaPodataka;
using Domain.Enumeracije;
using Domain.Modeli;
using System.Collections.Immutable;
using System.Xml.Serialization;

namespace Database.BazaPodataka
{
    public class XMLBazaPodataka : IBazaPodataka
    {
        public TabeleBazaPodataka Tabele { get; set; }
        private const string Putanja = "podaci.xml";

        public XMLBazaPodataka()
        {
            try
            {
                if (File.Exists(Putanja))
                {
                    using FileStream fs = new(Putanja, FileMode.Open);
                    XmlSerializer serializer = new(typeof(TabeleBazaPodataka));
                    Tabele = (TabeleBazaPodataka?)serializer.Deserialize(fs) ?? new();
                }
                else
                {
                    Tabele = new();
                    InicijalniPodaci();
                    SacuvajPromene();
                }
            }
            catch
            {
                Tabele = new();
                InicijalniPodaci();
            }
        }

        public bool SacuvajPromene()
        {
            try
            {
                using FileStream fs = new(Putanja, FileMode.Create);
                XmlSerializer serializer = new(typeof(TabeleBazaPodataka));
                serializer.Serialize(fs, Tabele);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void InicijalniPodaci()
        {
            // Korisnici
            Tabele.Korisnici.Add(new Korisnik("enolog", "1234", "Glavni enolog", TipKorisnika.GLAVNI_ENOLOG));
            Tabele.Korisnici.Add(new Korisnik("kelar", "1234", "Kelar majstor", TipKorisnika.KELAR_MAJSTOR));

            // Vinova loza
            var loza1 = new VinovaLoza("Cabernet", 22.5, 2018, "Toskana", FazaZrelosti.Obrana);
            var loza2 = new VinovaLoza("Chardonnay", 23.2, 2019, "Toskana", FazaZrelosti.Obrana);
            Tabele.Loze.Add(loza1);
            Tabele.Loze.Add(loza2);

            // Vina (minimalno, da postoji katalog)
            //var vino1 = new Vino(Guid.NewGuid(), "Cabernet 0.75", KategorijaVina.Stolno, 0.75, "SER-001", new List<VinovaLoza> { loza1}, DateTime.Now, 30);
            //var vino2 = new Vino(Guid.NewGuid(), "Chardonnay 0.75", KategorijaVina.Kvalitetno, 0.75, "SER-002", new List<VinovaLoza> { loza2}, DateTime.Now, 24);
            //Tabele.Vina.Add(vino1);
            //Tabele.Vina.Add(vino2);

            // Vinski podrum (jedan podrum za demo)
            var podrum = new VinskiPodrum(Guid.NewGuid(), "Glavni vinski podrum", 14, 100);
            Tabele.VinskiPodrum.Add(podrum);

            // Palete (otpremljene) da katalog nije prazan
            //Tabele.Palete.Add(new Paleta($"PL-SEED-{Guid.NewGuid():N}", "Beograd", podrum.Id, vino1, StatusPalete.Otpremljena));
            //Tabele.Palete.Add(new Paleta($"PL-SEED-{Guid.NewGuid():N}", "Novi Sad", podrum.Id, vino2, StatusPalete.Otpremljena));

            // Fakture (prazno na pocetku)
        }
    }
}
