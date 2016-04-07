using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.NetworkOperators;

namespace MachineInspectie.Model
{
    public class Main
    {
        public int id { get; set; }
        public string name { get; set; }

        public Main()
        {
            
        }

        public Main(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
