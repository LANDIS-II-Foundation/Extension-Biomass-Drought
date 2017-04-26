
using System.Collections.Generic;

using Edu.Wisc.Forest.Flel.Util;

namespace Landis.Extension.DroughtGenerator
{
    /// <summary>
    /// Parameters for the plug-in.
    /// </summary>
    public interface IInputParameters
    {
        int Timestep { get; set; }
        string VarName { get; set; }
        double Mu { get; set; }
        double Sigma { get; set; }
        string LogFileName { get; set; }
    }

    class InputParameters
        : IInputParameters
    {
        private int timestep;
        private string varName;
        private double mu;
        private double sigma;
        private string logFileName;

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
        /// <summary>
        /// Name of variable.
        /// </summary>
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
        public double Mu
        {
            get
            {
                return mu;
            }
            set
            {
                mu = value;
            }
        }

        //---------------------------------------------------------------------
        public double Sigma
        {
            get
            {
                return sigma;
            }
            set
            {
                sigma = value;
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
    }
}
