using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Landis.Library.Metadata;

namespace Landis.Extension.DroughtGenerator
{
    public class EventsLog
    {
        [DataFieldAttribute(Desc = "Time")]
        public int Time { set; get; }

        [DataFieldAttribute(Desc = "Years of Drought Per Decade")]
        public double DroughtYears { set; get; }
    }
}
