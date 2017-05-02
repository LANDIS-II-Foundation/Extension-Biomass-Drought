using Landis.Core;
using Landis.SpatialModeling;

using System;
using System.Collections.Generic;
using System.IO;
using Landis.Library.Metadata;


namespace Landis.Extension.DroughtGenerator
{
    public class PlugIn
        :ExtensionMain

    {
        private static readonly bool isDebugEnabled = false; 
        public static readonly ExtensionType Type = new ExtensionType("disturbance:drought");
        public static readonly string ExtensionName = "Drought Generator";

        public static MetadataTable<EventsLog> eventsLog;
        public string varName;
        private double mu;
        private double sigma;
        private static IInputParameters parameters;
        private static ICore modelCore;

        //---------------------------------------------------------------------
        public PlugIn()
            : base(ExtensionName, Type)
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
            parameters = modelCore.Load<IInputParameters>(dataFile, parser);
        }

        //---------------------------------------------------------------------
        public override void Initialize()
        {
            Timestep = parameters.Timestep;
            varName = parameters.VarName;
            mu = parameters.Mu;
            sigma = parameters.Sigma;

            SiteVars.Initialize(varName);

            modelCore.Log.WriteLine("   Opening and Initializing Drought log file \"{0}\"...", parameters.LogFileName);
            MetadataHandler.InitializeMetadata(Timestep, parameters.LogFileName);

            if (isDebugEnabled)
                modelCore.Log.WriteLine("Initialization done");
        }

        //---------------------------------------------------------------------
        ///<summary>
        /// Run the plug-in at a particular timestep.
        ///</summary>
        public override void Run()
        {
            modelCore.Log.WriteLine("   Processing Drought Generator ...");
            SiteVars.DroughtYears.ActiveSiteValues = 0;
            PlugIn.ModelCore.LognormalDistribution.Mu = mu;
            PlugIn.ModelCore.LognormalDistribution.Sigma = sigma;
            double dY = PlugIn.ModelCore.LognormalDistribution.NextDouble();
            if (dY > 10)
                dY = 10;
            // Output carried in SiteVars represents # years drought per decade
            eventsLog.Clear();
            EventsLog el = new EventsLog();
            el.Time = modelCore.CurrentTime;
            el.DroughtYears = dY;

            eventsLog.AddObject(el);
            eventsLog.WriteToFile();

            //  Assign values to sites
            foreach (Site site in modelCore.Landscape.AllSites)
            {
                if (site.IsActive)
                {
                    SiteVars.DroughtYears[site] = dY;
                }
                else
                {
                    SiteVars.DroughtYears[site] = 0;
                }
            }
            
            
        }
    }
}
