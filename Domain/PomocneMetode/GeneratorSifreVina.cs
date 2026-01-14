using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.PomocneMetode
{
    public class GeneratorSifreVina
    {
        public static string GenerisiSifruVina(Guid IDvina)
        {
            string Id_naziv=IDvina.ToString();
            string godina = datumFlasiranja.Year.ToString();
            
            string sifraVina = $"VN-{godina}-{Id_naziv}";
            return sifraVina;
        }
    }
}
