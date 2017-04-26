using Landis.SpatialModeling;
using Landis.Library.BiomassCohorts;

namespace Landis.Extension.DroughtDisturbance
{
    class SiteVars
    {
        private static ISiteVar<ushort> droughtBioRemoved;
        private static ISiteVar<double> droughtYears;
        private static ISiteVar<ISiteCohorts> biomassCohorts;

        //---------------------------------------------------------------------
        public static void Initialize(string varName)
        {
            biomassCohorts = PlugIn.ModelCore.GetSiteVar<ISiteCohorts>("Succession.BiomassCohorts");
            droughtBioRemoved = PlugIn.ModelCore.Landscape.NewSiteVar<ushort>();
            droughtYears = PlugIn.ModelCore.GetSiteVar<double>(varName);
            if (droughtYears == null)
            {
                string mesg = string.Format("VariableName '{0}' does not match any variable name provided by the Drought Generator Extension.",varName);
                throw new System.ApplicationException(mesg);
            }
        }

        //---------------------------------------------------------------------
        public static ISiteVar<ushort> DroughtBioRemoved
        {
            get
            {
                return droughtBioRemoved;
            }
        }

        //---------------------------------------------------------------------
        public static ISiteVar<double> DroughtYears
        {
            get
            {
                return droughtYears;
            }
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Biomass cohorts at each site.
        /// </summary>
        public static ISiteVar<ISiteCohorts> Cohorts
        {
            get
            {
                return biomassCohorts;
            }
            set
            {
                biomassCohorts = value;
            }
        }

        //---------------------------------------------------------------------

    }
}
