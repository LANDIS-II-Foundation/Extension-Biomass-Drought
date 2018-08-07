using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Landis.Core;
using Landis.Library.BiomassCohorts;
using Landis.SpatialModeling;
using Landis.Library.Metadata;

namespace Landis.Extension.DroughtDisturbance
{
    public class PlugIn
        :ExtensionMain
    {
        //private static readonly bool isDebugEnabled = false;
        public static readonly ExtensionType ExtType = new ExtensionType("disturbance:drought");
        public static readonly string ExtensionName = "Drought Disturbance";

        private string varName;
        private double dy_min;
        private double dy_max;
        private string mapNameTemplate;
        public static MetadataTable<EventsLog> eventLog;
        private static IInputParameters parameters;
        private static ICore modelCore;

        //---------------------------------------------------------------------
        public PlugIn()
            : base(ExtensionName, ExtType)
        {
        }

        //---------------------------------------------------------------------
        public static ICore ModelCore
        {
            get
            {
                return modelCore;
            }
        }
        //---------------------------------------------------------------------
        public override void LoadParameters(string dataFile,
                                            ICore mCore)
        {
            modelCore = mCore;
            InputParametersParser parser = new InputParametersParser();
            parameters = Landis.Data.Load<IInputParameters>(dataFile, parser);
        }

        //---------------------------------------------------------------------
        public override void Initialize()
        {
            Timestep = parameters.Timestep;
            varName = parameters.VarName;
            dy_min = parameters.MinDroughtYears;
            dy_max = parameters.MaxDroughtYears;
            mapNameTemplate = parameters.MapNamesTemplate;

            SiteVars.Initialize(varName);
            PartialDisturbance.Initialize();

            modelCore.UI.WriteLine("   Opening and Initializing Drought Disturbance log file \"{0}\"...", parameters.LogFileName);
            MetadataHandler.InitializeMetadata(Timestep, mapNameTemplate, parameters.LogFileName);
        }

        //---------------------------------------------------------------------
        ///<summary>
        /// Run the plug-in at a particular timestep.
        ///</summary>
        public override void Run()
        {
            modelCore.UI.WriteLine("   Processing Drought Disturbance ...");

            double totalRemoved = 0;
            int totalKilled = 0;
            double dy_sum = 0;
            int siteCount = 0;

            double[] removedSpp = new double[PlugIn.ModelCore.Species.Count];
            int[] killedSpp = new int[PlugIn.ModelCore.Species.Count];
            double[] extraRemovedSpp = new double[PlugIn.ModelCore.Species.Count];
            foreach (ISpecies species in PlugIn.ModelCore.Species)
            {
                removedSpp[species.Index] = 0;
                killedSpp[species.Index] = 0;
                extraRemovedSpp[species.Index] = 0;
            }
            
            foreach (ActiveSite site in PlugIn.ModelCore.Landscape)
            {
                double siteBioRemoved = 0;
                double dy = SiteVars.DroughtYears[site];
                if (dy > dy_max)
                    dy = dy_max;
                dy_sum += dy;
                siteCount += 1;
                if (dy > dy_min)
                {
                    
                    // Sort Cohorts be decreasing age
                    List<ICohort> cohortList = new List<ICohort>();
                    foreach (ISpeciesCohorts speciesCohorts in SiteVars.Cohorts[site])
                    {
                        foreach (ICohort cohort in speciesCohorts)
                        {
                            cohortList.Add(cohort);
                        }
                    }
                    cohortList = cohortList.OrderByDescending(x => x.Age).ToList();
                    

                    foreach (ISpecies species in PlugIn.ModelCore.Species)
                    {
                        double bioRemovedSpp = removedSpp[species.Index];
                        int cohortKilledSpp = killedSpp[species.Index];
                        double extraBioRemovedSpp = extraRemovedSpp[species.Index];
                        // Calculate 95% CI of intercept and slope
                        double int_max = parameters.Drought_Y[species] + (1.96 * parameters.Drought_YSE[species]);
                        double int_min = parameters.Drought_Y[species] - (1.96 * parameters.Drought_YSE[species]);
                        double slope_max = parameters.Drought_B[species] + (1.96 * parameters.Drought_BSE[species]);
                        double slope_min = parameters.Drought_B[species] - (1.96 * parameters.Drought_BSE[species]);

                        // Calculate upper and lower CI predicted values
                        double maxPropMort = int_max + slope_max * dy;
                        double minPropMort = int_min + slope_min * dy;
                        double rangePropMort = maxPropMort - minPropMort;

                        int bioRemoved = 0;
                        int cohortsKilled = 0;
                        int extraRemoved = 0;
                        int woodyRemoved = 0;
                        int nonWoodyRemoved = 0;

                        if (rangePropMort > 0)
                        {
                            double Tbiomass = 0;
                            double propLongev = 0;
                            int oldestCohortBio = 0;
                            foreach (ICohort cohort in cohortList)
                                {
                                    if (cohort.Species == species)
                                    {
                                        Tbiomass += cohort.Biomass;
                                        if (((double)cohort.Age / (double)cohort.Species.Longevity) > propLongev)
                                        {
                                            propLongev = (double)cohort.Age / (double)cohort.Species.Longevity;
                                            oldestCohortBio = cohort.Biomass;
                                        }
                                    }
                                }
                            //}
                            if (Tbiomass > 0)
                            {
                                double predictPctMort = (minPropMort + (rangePropMort * propLongev));
                                double backTransPctMort = predictPctMort;
                                double backTrans0 = 0;
                                if (parameters.BackTransform == "EXP")
                                {
                                    // Back-transform - EXP()
                                    backTransPctMort = Math.Exp(predictPctMort);
                                    if (parameters.IntCorrect == "Y")
                                    {
                                        backTrans0 = Math.Exp(parameters.Drought_Y[species]);
                                    }
                                }
                                else if (parameters.BackTransform == "SQUARE")
                                {
                                    //Back-transform - Square
                                    backTransPctMort = Math.Pow(predictPctMort, 2);
                                    if (parameters.IntCorrect == "Y")
                                    {
                                        backTrans0 = Math.Pow(parameters.Drought_Y[species], 2);
                                    }
                                }
                                // Account for timestep
                                double actualPctMort = (backTransPctMort - backTrans0) * Timestep;
                                //double actualPctMort = predictPctMort * Timestep;
                                actualPctMort = Math.Min(actualPctMort, 100);
                                //Convert to proportion
                                double actualPropMort = actualPctMort / 100.0;

                                // Calculate biomass removed
                                bioRemoved = (int)Math.Round(Tbiomass * actualPropMort);

                                int remainBioRem = bioRemoved;

                                //Remove biomass from cohorts, starting with the oldest
                                foreach (ICohort cohort in cohortList)
                                {
                                    if (cohort.Species == species)
                                    {
                                        if (remainBioRem > 0)
                                        {
                                            int nonWoody = cohort.ComputeNonWoodyBiomass(site);
                                            int woody = (cohort.Biomass - nonWoody);
                                            if (cohort.Biomass > remainBioRem)
                                            {
                                                if ((remainBioRem / cohort.Biomass) > 0.9)
                                                {

                                                    extraRemoved = (cohort.Biomass - remainBioRem);
                                                    remainBioRem = cohort.Biomass;
                                                    cohortsKilled++;
                                                }
                                                else
                                                {
                                                    double nonWoodRatio = (double)nonWoody / (double)woody;
                                                    nonWoody = (int)Math.Round(remainBioRem * nonWoodRatio);
                                                    woody = remainBioRem - nonWoody;
                                                }
                                                
                                                PartialDisturbance.RecordBiomassReduction(cohort, remainBioRem);
                                                remainBioRem = 0;
                                                woodyRemoved += woody;
                                                nonWoodyRemoved += nonWoody;
                                            }
                                            else
                                            {
                                                remainBioRem = remainBioRem - cohort.Biomass;
                                                PartialDisturbance.RecordBiomassReduction(cohort, cohort.Biomass);
                                                cohortsKilled++;
                                                woodyRemoved += woody;
                                                nonWoodyRemoved += nonWoody;
                                            }
                                        }
                                    }
                                }
                            }
                            //}
                        }
                        siteBioRemoved += bioRemoved;
                        bioRemovedSpp += bioRemoved;
                        //Landis.Extension.Succession.Biomass.ForestFloor.AddWoody(woodyRemoved, species, site);
                        //Landis.Extension.Succession.Biomass.ForestFloor.AddLitter(nonWoodyRemoved, species, site);
                        cohortKilledSpp += cohortsKilled;
                        extraBioRemovedSpp += extraRemoved;
                        removedSpp[species.Index] = bioRemovedSpp;
                        killedSpp[species.Index] = cohortKilledSpp;
                        extraRemovedSpp[species.Index] = extraBioRemovedSpp;
                        totalKilled += cohortsKilled;

                    }
                    PartialDisturbance.ReduceCohortBiomass(site);
                }
                SiteVars.DroughtBioRemoved[site] = (ushort)siteBioRemoved;
                totalRemoved += siteBioRemoved;
            }
            double avg_dy = dy_sum / siteCount;

            int i = 0;
            eventLog.Clear();
            EventsLog el = new EventsLog();
            el.Time = ModelCore.CurrentTime;
            el.AverageDrought = avg_dy;
            el.BiomassRemoved = new double[PlugIn.ModelCore.Species.Count];
            foreach (ISpecies species in PlugIn.ModelCore.Species)
            {
                el.BiomassRemoved[i] = removedSpp[species.Index];
                i = i + 1;
            }
            i = 0;
            el.TotalBiomassRemoved = totalRemoved;
            el.CohortsKilledSpecies = new double[PlugIn.ModelCore.Species.Count];
            foreach (ISpecies species in PlugIn.ModelCore.Species)
            {
                el.CohortsKilledSpecies[i] = killedSpp[species.Index];
                i = i + 1;
            }
            i = 0;
            el.TotalCohortsKilled = totalKilled;
            el.ExtraRemoved = new double[PlugIn.ModelCore.Species.Count];
            foreach (ISpecies species in PlugIn.ModelCore.Species)
            {
                el.ExtraRemoved[i] = extraRemovedSpp[species.Index];
                i = i + 1;
            }
            eventLog.AddObject(el);
            eventLog.WriteToFile();

            //  Write Biomass Removed map
            string path = MapNames.ReplaceTemplateVars(mapNameTemplate, modelCore.CurrentTime);
            modelCore.UI.WriteLine("   Writing Drought Biomass Removed map to {0} ...", path);
            using (IOutputRaster<ShortPixel> outputRaster = modelCore.CreateRaster<ShortPixel>(path, modelCore.Landscape.Dimensions))
            {
                ShortPixel pixel = outputRaster.BufferPixel;
                foreach (Site site in modelCore.Landscape.AllSites)
                {
                    if (site.IsActive)
                    {
                        pixel.MapCode.Value = (short)(SiteVars.DroughtBioRemoved[site]);

                    }
                    else
                    {
                        //  Inactive site
                        pixel.MapCode.Value = 0;
                    }
                    outputRaster.WriteBufferPixel();
                }
            }
            // Modify establishment
            //if (avg_dy >= dy_min)
            //{
            //    foreach (ISpecies species in PlugIn.ModelCore.Species)
            //    {
            //        double drought_Mod = 1.0;
            //        if (parameters.Drought_Sens[species] == 2)
            //            drought_Mod = 0.5;
            //        else if (parameters.Drought_Sens[species] == 3)
            //            drought_Mod = 0.0;
            //        if (drought_Mod < 1)
            //        {
            //            foreach (IEcoregion ecoregion in PlugIn.ModelCore.Ecoregions)
            //            {
            //                    Landis.Extension.Succession.Biomass.SpeciesData.EstablishModifier[species, ecoregion] *= drought_Mod;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    modelCore.UI.WriteLine("   Drought does not exceed threshold this timestep ...");
            //}

        }
    }
}
