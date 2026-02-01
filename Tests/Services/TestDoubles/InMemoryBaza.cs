using Domain.BazaPodataka;

namespace Tests.TestDoubles
{
    public class InMemoryBaza : IBazaPodataka
    {
        public TabeleBazaPodataka Tabele { get; set; } = new();

        public bool SacuvajPromene() => true;
    }
}
