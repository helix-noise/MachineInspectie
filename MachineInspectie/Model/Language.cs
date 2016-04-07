using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineInspectie.Model;

namespace MachineInspectie
{
    public class Language: Main
    {
        public string Locale { get; set; }
        public string Question { get; set; }

        public Language()
            :base()
        {
            
        }

        public Language(int id, string naam, string locale, string question)
            :base(id, naam)
        {
            this.Locale = locale;
            this.Question = question;
        }
    }
}
