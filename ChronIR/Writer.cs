using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronIR
{
    internal class Writer : StreamWriter
    {
        public string WritePath;
        public Writer(string path = "build.chron.c") : base(path)
        {
            WritePath = path;
        }

        public void End()
        {
            this.Close();
            this.Dispose();
        }
    }
}
