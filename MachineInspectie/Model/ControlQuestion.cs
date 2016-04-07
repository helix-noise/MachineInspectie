using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineInspectie.Model;

namespace MachineInspectie
{
    public class ControlQuestion
    {
        public int Id { get; set; }
        public MatisCategory MatisCategory { get; set; }
        public double Weight { get; set; }
        public List<Locatie> Translations { get; set; }

        public ControlQuestion()
        {
            
        }

        public ControlQuestion(int id, MatisCategory matisCategory, double weight, List<Locatie> translations)
        {
            this.Id = id;
            this.MatisCategory = matisCategory;
            this.Weight = weight;
            this.Translations = translations;
        }
    }
}
