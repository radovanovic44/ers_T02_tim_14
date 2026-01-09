using Domain.BazaPodataka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                    Tabele = (TabeleBazaPodataka?)
                        serializer.Deserialize(fs) ?? new();
                }

                else
                 Tabele = new(); 
            }


            catch
            {
                Tabele = new();
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

    }
}

    

