using ChronIR.IR.Internal;
using ChronIR.IR.Operation.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronIR.IR.Operation
{
    public class ChronForTo : ChronStatement
    {
        private ChronStatementBlock block;
        private ChronExpression start;
        private ChronExpression end;
        private string identifier;
        public ChronForTo(string identifier, ChronStatementBlock block, ChronExpression start,  ChronExpression end)
        {
            this.identifier = identifier;
            this.block = block;
            this.start = start;
            this.end = end;
        }
        public void Write(ChronContext context)
        {
            var variable = new ChronVariable(identifier, start);
            var whileLoop = new ChronWhileLoop(new ChronLessT(variable, end), block);

            block.AddStatement(new ChronVariable(identifier, new ChronAdd(variable, new ChronInt(1))));

            variable.Write(context);
            whileLoop.Write(context);
        }
    }
}
