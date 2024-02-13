using ChronIR.IR.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronIR.IR.Operation
{
    public class ChronContinue : ChronStatement
    {
        public void Write(ChronContext context)
        {
            context.writer.WriteLine("continue;");
        }
    }
}
