using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineInspectie.Model
{
    public class Matis: Main
    {
        public int number { get; set; }
        public MatisCategory matisCategory { get; set; }
        public Locatie location { get; set; }

        public Matis()
            :base()
        {
            
        }

        public Matis(int id,int number, string name, MatisCategory matisCategory, Locatie locatie)
            :base(id, name)
        {
            this.number = number;
            this.matisCategory = matisCategory;
            this.location = locatie;
        }

        public string DisplayNaam
        {
            get { return name; }
        }
    }
}
