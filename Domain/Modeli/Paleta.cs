using Domain.Enumeracije;

namespace Domain.Modeli
{
    public class Paleta
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Sifra { get; set; } = string.Empty;
        public string AdresaOdredista { get; set; } = string.Empty;
        public Guid VinskiPodrumId { get; set; }

        
        public Vino Vino { get; set; }
        

        public StatusPalete Status { get; set; } = StatusPalete.Upakovana;

        public Paleta() { }

        public Paleta(string sifra, string adresaOdredista, Guid vinskiPodrumId, Vino vino,StatusPalete status)
        {
            Id = Guid.NewGuid();
            Sifra = sifra;
            AdresaOdredista = adresaOdredista;
            VinskiPodrumId = vinskiPodrumId;
            Vino = vino;
            Status = status;
        }
    }
}
