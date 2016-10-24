namespace LNU.Matrix.UtilityClasses
{
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
}
