using ChronIR.IR.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronIR.IR.Operation
{
    public class ChronWhileLoop : ChronStatement
    {
        private ChronExpression condition;
        private ChronStatementBlock block;
        public ChronWhileLoop(ChronExpression condition, ChronStatementBlock block)
        {
            this.condition = condition;
            this.block = block;
        }

        public void Write(ChronContext context)
        {
            context.writer.WriteLine($"while({ChronTypes.GetBooleanFromObject}({condition.Read(context)})) {{");
            block.Write(context);
            context.writer.WriteLine("}");
        }
    }
}
