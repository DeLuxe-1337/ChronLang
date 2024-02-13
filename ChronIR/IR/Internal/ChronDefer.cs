using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronIR.IR.Internal
{
    public interface ChronDeferer
    {
        void Defer(ChronContext context);
    }
    public static class ChronDefer
    {
        public static List<ChronDeferer> DeferList = new();
        public static List<ChronDeferer>.Enumerator Get() => DeferList.GetEnumerator();
        public static void Add(ChronDeferer de) => DeferList.Add( de);
        public static void Visit(ChronContext context)
        { 
            foreach(var d in DeferList)
            {
                d.Defer(context);
            }
            DeferList.Clear();
        }
    }
}
