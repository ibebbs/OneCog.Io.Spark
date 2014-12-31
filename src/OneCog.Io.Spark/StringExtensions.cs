using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark
{
    public static class StringExtensions
    {
        public static Stream ToStream(this string source)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(source);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
