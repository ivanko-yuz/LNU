using System.Collections.Generic;
using NMMP.Triangulation;

namespace LNU.Matrix.UtilityClasses
{
    public class Constants
    {
        public double Beta { get; private set; }
        public double Sigma { get; private set; }
        public double UcCof { get; private set; }
        public List<SerializableLine> Segments { get; set; }
        public Constants(double beta, double sigma, double ucCof)
        {
            Beta = beta;
            Sigma = sigma;
            UcCof = ucCof;
        }
    }
}
