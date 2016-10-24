using System;
using TriangleNet.Geometry;

namespace NMMP.Triangulation
{
    [Serializable]
    public class SerializableLine
    {
        public Vertex Vertex1 { get; set; }
        public Vertex Vertex2 { get; set; }

        public SerializableLine(Vertex vertex1, Vertex vertex2)
        {
            this.Vertex1 = vertex1;
            this.Vertex2 = vertex2;
        }
    }
}