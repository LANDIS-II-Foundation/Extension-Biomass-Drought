using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Landis.Library.Metadata;
using Landis.Core;

namespace Landis.Extension.DroughtDisturbance
{
    public class EventsLog
    {

        [DataFieldAttribute(Desc = "Time")]
        public int Time { set; get; }

        [DataFieldAttribute(Desc = "Average Drought Years Per Site")]
        public double AverageDrought { set; get; }

        [DataFieldAttribute(Desc = "Biomass Removed by Species", SppList = true)]
        public double[] BiomassRemoved { set; get; }

        [DataFieldAttribute(Desc = "Total Biomass Removed")]
        public double TotalBiomassRemoved { set; get; }

        [DataFieldAttribute(Desc = "Cohorts Killed by Species", SppList = true)]
        public int[] CohortsKilledSpecies { set; get; }

        [DataFieldAttribute(Desc = "Total Cohorts Killed")]
        public int TotalCohortsKilled { set; get; }

        [DataFieldAttribute(Desc = "Extra Removed by Species", SppList = true)]
        public double[] ExtraRemoved { set; get; }
    }
}
