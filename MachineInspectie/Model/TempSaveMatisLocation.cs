using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineInspectionLibrary;

namespace MachineInspectie.Model
{
    public class TempSaveMatisLocation
    {
        public Matis Matis { get; set; }
        public Location Location { get; set; }

        public TempSaveMatisLocation()
        {
            
        }

        public TempSaveMatisLocation(Matis matis, Location location)
        {
            this.Matis = matis;
            this.Location = location;
        }
    }
}
