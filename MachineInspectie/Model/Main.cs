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
        public int Id { get; set; }
        public string Naam { get; set; }

        public Main()
        {
            
        }

        public Main(int id, string naam)
        {
            this.Id = id;
            this.Naam = naam;
        }
    }
}
