using Landis.SpatialModeling;

namespace Landis.Extension.DroughtGenerator
{
    class SiteVars
    {
        private static ISiteVar<double> droughtYears;

        //---------------------------------------------------------------------
        public static void Initialize(string varName)
        {
            droughtYears = PlugIn.ModelCore.Landscape.NewSiteVar<double>();
          
            
            PlugIn.ModelCore.RegisterSiteVar(SiteVars.DroughtYears, varName);
        }

        //---------------------------------------------------------------------
        public static ISiteVar<double> DroughtYears
        {
            get
            {
                return droughtYears;
            }
        }

    }
}
