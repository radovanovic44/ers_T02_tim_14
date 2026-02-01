namespace Domain.Enumeracije
{
    public enum StatusPalete
    {
        Aktivna,            // aktivna i koristi se za pakovanje
        Upakovana,  // puna i spremna da se otpremi iz podruma
        Otpremljena,        // napustila vinski podrum
        Otvorena,           // otvorena za dodatno pakovanje / izmene
        Uklonjena           // izbacena iz upotrebe
    }
}