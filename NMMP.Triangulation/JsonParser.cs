using System.Collections.Generic;
using Newtonsoft.Json;

namespace NMMP.Triangulation
{
    public static class JsonParser
    {
        public static void Write<T>(List<T> data, string path)
        {
            System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(data));
        }
    }
}
