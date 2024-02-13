using ChronIR.IR.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronIR.IR.Operation
{
    public class ChronIf : ChronStatement
    {
        private ChronExpression condition;
        private ChronStatementBlock trueBlock;
        private ChronStatementBlock? falseBlock;
        public ChronIf(ChronExpression condition, ChronStatementBlock block, ChronStatementBlock? falseBlock)
        {
            this.condition = condition;
            this.trueBlock = block;
            this.falseBlock = falseBlock;
        }

        public void Write(ChronContext context)
        {
            context.writer.WriteLine($"if({ChronTypes.GetBooleanFromObject}({condition.Read(context)})) {{");
            trueBlock.Write(context);
            context.writer.WriteLine("}");

            if(falseBlock != null)
            {
                context.writer.WriteLine("else {");
                falseBlock.Write(context);
                context.writer.WriteLine("}");
            }
        }
    }
}
