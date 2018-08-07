
using System.Diagnostics;
using Landis.Core;
using Landis.Utilities;
using Landis.Library.Parameters;


namespace Landis.Extension.DroughtDisturbance
{
    /// <summary>
    /// Parameters for the plug-in.
    /// </summary>
    public interface IInputParameters
    {
        int Timestep { get; set; }
        string VarName { get; set; }
        double MinDroughtYears { get; set; }
        double MaxDroughtYears { get; set; }
        string BackTransform { get; set; }
        string IntCorrect { get; set; }
        string MapNamesTemplate { get; set; }
        string LogFileName { get; set; }
        Landis.Library.Parameters.Species.AuxParm<double> Drought_Y { get; }
        Landis.Library.Parameters.Species.AuxParm<double> Drought_YSE { get; }
        Landis.Library.Parameters.Species.AuxParm<double> Drought_B { get; }
        Landis.Library.Parameters.Species.AuxParm<double> Drought_BSE { get; }
        Landis.Library.Parameters.Species.AuxParm<int> Drought_Sens { get; }
        
    }

    class InputParameters
        : IInputParameters
    {
        private int timestep;
        private string varName;
        private double minDroughtYears;
        private double maxDroughtYears;
        private string backTransform;
        private string intCorrect; 
        private string mapNamesTemplate;
        private string logFileName;
        private Landis.Library.Parameters.Species.AuxParm<double> drought_Y;
        private Landis.Library.Parameters.Species.AuxParm<double> drought_YSE;
        private Landis.Library.Parameters.Species.AuxParm<double> drought_B;
        private Landis.Library.Parameters.Species.AuxParm<double> drought_BSE;
        private Landis.Library.Parameters.Species.AuxParm<int> drought_Sens;
        //---------------------------------------------------------------------
        /// <summary>
        /// Timestep (years)
        /// </summary>
        public int Timestep
        {
            get
            {
                return timestep;
            }
            set
            {
                if (value < 0)
                    throw new InputValueException(value.ToString(),
                                                  "Value must be = or > 0.");
                timestep = value;
            }
        }
        //---------------------------------------------------------------------
       
        public string VarName
        {
            get
            {
                return varName;
            }
            set
            {
                varName = value;
            }
        }
        //---------------------------------------------------------------------
        public double MinDroughtYears
        {
            get
            {
                return minDroughtYears;
            }
            set
            {
                minDroughtYears = value;
            }
        }
        //---------------------------------------------------------------------
        public double MaxDroughtYears
        {
            get
            {
                return maxDroughtYears;
            }
            set
            {
                if (value <= minDroughtYears)
                    throw new InputValueException(value.ToString(), "MaxDroughtVar must be greater than MinDroughtVar");
                maxDroughtYears = value;
            }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// BackTransformation
        /// </summary>
        public string BackTransform
        {
            get
            {
                return backTransform;
            }
            set
            {
                if ((value != "NONE") && (value != "EXP") && (value != "SQUARE"))
                    throw new InputValueException(value.ToString(),
                                                  "Value must be NONE, EXP or SQUARE.");
                backTransform = value;
            }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// InterceptCorrection
        /// </summary>
        public string IntCorrect
        {
            get
            {
                return intCorrect;
            }
            set
            {
                if ((value != "Y") && (value != "N"))
                    throw new InputValueException(value.ToString(),
                                                  "Value must be Y or N.");
                intCorrect = value;
            }
        }
        //---------------------------------------------------------------------
        /// <summary>
        /// Template for the filenames for output maps.
        /// </summary>
        public string MapNamesTemplate
        {
            get
            {
                return mapNamesTemplate;
            }
            set
            {
                MapNames.CheckTemplateVars(value);
                mapNamesTemplate = value;
            }
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Name of log file.
        /// </summary>
        public string LogFileName
        {
            get
            {
                return logFileName;
            }
            set
            {
                // FIXME: check for null or empty path (value);
                logFileName = value;
            }
        }

        //---------------------------------------------------------------------
        public Landis.Library.Parameters.Species.AuxParm<double> Drought_Y
        {
            get
            {
                return drought_Y;
            }
        }

        //---------------------------------------------------------------------
        public Landis.Library.Parameters.Species.AuxParm<double> Drought_YSE
        {
            get
            {
                return drought_YSE;
            }
        }

        //---------------------------------------------------------------------
        public Landis.Library.Parameters.Species.AuxParm<double> Drought_B
        {
            get
            {
                return drought_B;
            }
        }

        //---------------------------------------------------------------------
        public Landis.Library.Parameters.Species.AuxParm<double> Drought_BSE
        {
            get
            {
                return drought_BSE;
            }
        }

        //---------------------------------------------------------------------
        public Landis.Library.Parameters.Species.AuxParm<int> Drought_Sens
        {
            get
            {
                return drought_Sens;
            }
        }
        
        //---------------------------------------------------------------------
        public void SetDrought_Y(ISpecies species,
                                     InputValue<double> newValue)
        {
            Debug.Assert(species != null);
            drought_Y[species] = newValue.CheckInRange(-100.0, 100.0);
        }

        //---------------------------------------------------------------------
        public void SetDrought_YSE(ISpecies species,
                                     InputValue<double> newValue)
        {
            Debug.Assert(species != null);
            drought_YSE[species] = newValue.CheckInRange(-100.0, 100.0);
        }

        //---------------------------------------------------------------------
        public void SetDrought_B(ISpecies species,
                                     InputValue<double> newValue)
        {
            Debug.Assert(species != null);
            drought_B[species] = newValue.CheckInRange(-100.0, 100.0);
        }

        //---------------------------------------------------------------------
        public void SetDrought_BSE(ISpecies species,
                                     InputValue<double> newValue)
        {
            Debug.Assert(species != null);
            drought_BSE[species] = newValue.CheckInRange(-100.0, 100.0);
        }

        //---------------------------------------------------------------------
        public void SetDrought_Sens(ISpecies species,
                                     InputValue<double> newValue)
        {
            Debug.Assert(species != null);
            drought_Sens[species] = (int)newValue.CheckInRange(1, 3);
        }
        //---------------------------------------------------------------------
        public InputParameters()
        {
            drought_Y = new Landis.Library.Parameters.Species.AuxParm<double>(PlugIn.ModelCore.Species);
            drought_YSE = new Landis.Library.Parameters.Species.AuxParm<double>(PlugIn.ModelCore.Species);
            drought_B = new Landis.Library.Parameters.Species.AuxParm<double>(PlugIn.ModelCore.Species);
            drought_BSE = new Landis.Library.Parameters.Species.AuxParm<double>(PlugIn.ModelCore.Species);
            drought_Sens = new Landis.Library.Parameters.Species.AuxParm<int>(PlugIn.ModelCore.Species);
        }
    }
}
