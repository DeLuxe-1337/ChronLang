using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronIR.IR.Internal
{
    public interface ChronExpression
    {
        public object Read(ChronContext context);
        public void WriteRead(ChronContext context)
        {
            context.writer.Write(Read(context));
        }
    }
}
