using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineInspectie.Model
{
    public class Matis: Main
    {
        public int Nummer { get; set; }
        public MatisCategory MatisCategory { get; set; }
        public Locatie Locatie { get; set; }

        public Matis()
            :base()
        {
            
        }

        public Matis(int id,int nummer, string naam, MatisCategory matisCategory, Locatie locatie)
            :base(id, naam)
        {
            this.Nummer = nummer;
            this.MatisCategory = matisCategory;
            this.Locatie = locatie;
        }
    }
}
