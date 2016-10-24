using LNU.Matrix.UtilityClasses;
using TriangleNet.Geometry;

namespace LNU.Matrix
{
    public static class MatrixKeConstants
    {
        public static EquationStorage getConstants(Vertex x, Vertex y, Vertex z)
        {
            EquationStorage item = new EquationStorage();
            item.X.B = y.Y - z.Y;
            item.X.C = z.X - y.X;
            item.X.A = 0;

            item.Y.B = z.Y - x.Y;
            item.Y.C = x.X - z.X;
            item.Y.A = 0;

            item.Z.B = x.Y - y.Y;
            item.Z.C = y.X - x.X;
            item.Z.A = 0;

            return item;
        }
    }
}
