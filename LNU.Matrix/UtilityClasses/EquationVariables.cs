namespace LNU.Matrix.UtilityClasses
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
}
