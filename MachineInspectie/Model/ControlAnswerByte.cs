using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace MachineInspectie.Model
{
    public class ControlAnswerByte
    {
        public int controlQuestionId { get; set; }
        public string comment { get; set; }
        public List<byte[]> images { get; set; }
        public bool testOk { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }

        public ControlAnswerByte()
        {

        }

        public ControlAnswerByte(int controlQuestionId, string comment, List<byte[]> images, bool testOk, DateTime startTime, DateTime endTime)
        {
            this.controlQuestionId = controlQuestionId;
            this.comment = comment;
            this.images = images;
            this.testOk = testOk;
            this.startTime = startTime;
            this.endTime = endTime;
        }
    }
}
