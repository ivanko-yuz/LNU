using TriangleNet.Geometry;

namespace LNU.Matrix
{
    public class EquationVariables
    {
        public EquationVariables(double a, double b, double c)
        {
            A = a;
            B = b;
            C = c;
        }

        public double B { get; set; }
        public double C { get; set; }
        public double A { get; set; }
    }


    public class EquationStorage
    {
        public EquationStorage()
        {
            X = new EquationVariables(0, 0, 0);
            Y = new EquationVariables(0, 0, 0);
            Z = new EquationVariables(0, 0, 0);
        }
        public EquationVariables X { get; set; }
        public EquationVariables Y { get; set; }
        public EquationVariables Z { get; set; }
    }

    public class BasisStorage
    {
        public BasisStorage(double fiX, double fiY, double fiZ)
        {
            FiX = fiX;
            FiY = fiY;
            FiZ = fiZ;
        }

        public BasisStorage()
        {

        }

        public double FiX { get; set; }
        public double FiY { get; set; }
        public double FiZ { get; set; }
    }
}
