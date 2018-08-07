using Landis.SpatialModeling;
using Landis.Library.BiomassCohorts;
using Landis.Core;
using System.Collections.Generic;
using Landis.Utilities;
//using Landis.Library.Succession;

namespace Landis.Extension.DroughtDisturbance
{
    public class SpeciesData
    {
        public static Landis.Library.Parameters.Species.AuxParm<double> Drought_Y;
        public static Landis.Library.Parameters.Species.AuxParm<double> Drought_YSE;
        public static Landis.Library.Parameters.Species.AuxParm<double> Drought_B;
        public static Landis.Library.Parameters.Species.AuxParm<double> Drought_BSE;
        public static Landis.Library.Parameters.Species.AuxParm<int> Drought_Sens;

        //---------------------------------------------------------------------
        public static void Initialize(IInputParameters parameters)
        {
            //ChangeParameters(parameters);
            Drought_Y = parameters.Drought_Y;
            Drought_YSE = parameters.Drought_YSE;
            Drought_B = parameters.Drought_B;
            Drought_BSE = parameters.Drought_BSE;
            Drought_Sens = parameters.Drought_Sens;
        }
    }
}
