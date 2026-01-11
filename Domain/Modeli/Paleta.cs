using System;
using Domain.Enumeracije;

namespace Domain.Modeli
{
    public class Paleta
    {
        // Jedinstveni identifikator palete
        public Guid Id { get; set; } = Guid.NewGuid();

        // Sifra palete
        public string Sifra { get; set; } = string.Empty;

        // Adresa odredista
        public string AdresaOdredista { get; set; } = string.Empty;

        // Id vinskog podruma u kom se paleta nalazi
        public Guid VinskiPodrumId { get; set; }

        // Id vina koje je upakovano u paletu
        public Guid VinoId { get; set; }

        // Trenutni broj pakovanja (sanduk / kutija) na paleti
        public int TrenutniBrojPakovanja { get; set; }

        // Status palete
        public StatusPalete Status { get; set; } = StatusPalete.Aktivna;

        public Paleta() { }

        public Paleta(
            string sifra,
            string adresaOdredista,
            Guid vinskiPodrumId,
            Guid vinoId,
            int trenutniBrojPakovanja,
            StatusPalete status = StatusPalete.Aktivna)
        {
            Sifra = sifra;
            AdresaOdredista = adresaOdredista;
            VinskiPodrumId = vinskiPodrumId;
            VinoId = vinoId;
            TrenutniBrojPakovanja = trenutniBrojPakovanja;
            Status = status;
        }
    }
}