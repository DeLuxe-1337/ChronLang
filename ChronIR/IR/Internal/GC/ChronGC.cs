using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronIR.IR.Internal.GC
{
    public static class ChronGC
    {
        public static bool Enabled = false;
        public static void Retain(ChronContext context, ChronGarbage garbage)
        {
            if (!Enabled) return;

            context.writer?.WriteLine($"GC_Retain({garbage.GC_Reference()});");
        }
        public static void ReleaseAll(ChronContext context)
        {
            if (!Enabled) return;

            context.writer.WriteLine("GC_ReleaseAll();");
        }
    }
}
