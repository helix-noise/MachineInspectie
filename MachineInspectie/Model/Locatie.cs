using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineInspectie.Model;

namespace MachineInspectie
{
    public class Locatie: Main
    {
        public int PostCode { get; set; }

        private string _displayNaam;

        public string DisplayNaam
        {
            get { return Naam + " " + PostCode; }
        }


        public Locatie()
            :base()
        {
            
        }

        public Locatie(int id, int postCode, string naam)
            :base(id, naam)
        {
            this.PostCode = postCode;
            this.Naam = naam;
        }


    }
}
