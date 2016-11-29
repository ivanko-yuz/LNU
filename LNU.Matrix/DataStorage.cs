using Newtonsoft.Json;
using NMMP.Triangulation;
using System.Collections.Generic;
using TriangleNet.Geometry;


namespace LNU.Matrix
{
    public class DataStorage
    {
        public List<Vertex> CT { get; set; }
        public List<List<Vertex>> NT { get; set; }
        public List<List<SerializableLine>> NTG { get; set; }
        public List<int> Figure { get; set; }

        public DataStorage()
        {
            this.CT = new List<Vertex>();                  //точки
            this.NT = new List<List<Vertex>>();            //трикутники
            this.NTG = new List<List<SerializableLine>>(); //сегменти
            this.Figure = new List<int>();
            ReadJson();

        }

        private void ReadJson()
        {
            CT = JsonConvert.DeserializeObject<List<Vertex>>(System.IO.File.ReadAllText(@"C:\Users\vanuy\Desktop\Lnu.Triangulation\NMMP.Triangulation\bin\Debug\CT.json"));
            NT = JsonConvert.DeserializeObject<List<List<Vertex>>>(System.IO.File.ReadAllText(@"C:\Users\vanuy\Desktop\Lnu.Triangulation\NMMP.Triangulation\bin\Debug\NT.json"));
            NTG = JsonConvert.DeserializeObject<List<List<SerializableLine>>>(System.IO.File.ReadAllText(@"C:\Users\vanuy\Desktop\Lnu.Triangulation\NMMP.Triangulation\bin\Debug\NTG.json"));
            Figure = JsonConvert.DeserializeObject<List<int>>(System.IO.File.ReadAllText(@"C:\Users\vanuy\Desktop\Lnu.Triangulation\NMMP.Triangulation\bin\Debug\figure.json"));
        }
    }
}
