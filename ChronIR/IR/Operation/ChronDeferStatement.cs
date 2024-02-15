using ChronIR.IR.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronIR.IR.Operation
{
    public class ChronDeferStatement : ChronStatement, ChronDeferer
    {
        private ChronStatement deferStatement;
        public ChronDeferStatement(ChronStatement deferStatement)
        {
            this.deferStatement = deferStatement;
        }

        public void Defer(ChronContext context)
        {
            deferStatement.Write(context);
        }

        public void Write(ChronContext context)
        {
            ChronDefer.Add(this);
        }
    }
}
