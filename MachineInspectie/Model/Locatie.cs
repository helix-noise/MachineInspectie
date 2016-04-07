using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineInspectie.Model;

namespace MachineInspectie.Model
{
    public class Locatie: Main
    {
        public int postalCode { get; set; }

        public string DisplayNaam
        {
            get { return name + " " + postalCode; }
        }


        public Locatie()
            :base()
        {
            
        }

        public Locatie(int id, int postalCode, string name)
            :base(id, name)
        {
            this.postalCode = postalCode;
            this.name = name;
        }
    }
}
