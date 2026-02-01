using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.PomocneMetode
{
    public static class GeneratorSifreVina
    {
        public static string GenerisiSifruVina(Guid Id)
        {
            string Id_naziv = Id.ToString();
            string godina = DateTime.Now.Year.ToString();

            string sifraVina = $"VN-{godina}-{Id_naziv}";
            return sifraVina;
        }
    }
}
