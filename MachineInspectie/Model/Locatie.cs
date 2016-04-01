using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineInspectie
{
    class Locatie
    {
        public int Id { get; set; }
        public int PostCode { get; set; }
        public string Naam { get; set; }

        public Locatie()
        {
            
        }

        public Locatie(int id, int postCode, string naam)
        {
            Id = id;
            PostCode = postCode;
            Naam = naam;
        }


    }
}
