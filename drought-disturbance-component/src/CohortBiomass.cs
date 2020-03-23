// Authors: Xiaohuan Zhou
// 3/20/2020
// Copy from Extension-ForCS-Succession by Caren Dymond, Sarah Beukema

using Landis.SpatialModeling;
using Landis.Core;
using Landis.Library.BiomassCohorts;
using System.Collections.Generic;
using Landis.Utilities;
using System;

namespace Landis.Extension.DroughtDisturbance
{
    /// <summary>
    /// Calculations for an individual cohort's biomass.
    /// </summary>
    public class CohortBiomass : Landis.Library.BiomassCohorts.ICalculator
    {
        public CohortBiomass()
        {
        }

        /// <summary>
        /// Computes the change in a cohort's biomass due to Annual Net Primary
        /// Productivity (ANPP), age-related mortality (M_AGE), and development-
        /// related mortality (M_BIO).
        /// March 2020: changed to be a constant value 0, which is not called by
        /// Cohort.Initialize().
        /// </summary>
        public int ComputeChange(ICohort cohort, ActiveSite site)
        {
            return 0;
        }

        /// <summary>
        /// Computes the percentage of a cohort's standing biomass that is non-woody.
        /// April 2010: changed to be a constant percentage of foliage, so that the 
        /// calculations of turnover give reasonable numbers.
        /// March 2020: simplify the function.
        /// </summary>

        public Percentage ComputeNonWoodyPercentage(ICohort cohort, ActiveSite site)
        {
            return new Percentage(0.1);
        }
    }
}
