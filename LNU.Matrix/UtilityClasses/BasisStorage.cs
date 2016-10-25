namespace LNU.Matrix.UtilityClasses
{
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

        public override string ToString()
        {
            return FiX.ToString("0.0000") + " " + FiY.ToString("0.0000") + " " + FiZ.ToString("0.0000");
        }
    }
}
