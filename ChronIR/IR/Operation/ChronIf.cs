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
            ChronDefer.IncreaseScope();

            context.writer.WriteLine($"if({ChronTypes.GetBooleanFromObject}({condition.Read(context)})) {{");

            ChronDefer.IncreaseScope();

            trueBlock.Write(context);

            ChronDefer.VisitCurrentScope(context);
            ChronDefer.DecreaseScope();

            context.writer.WriteLine("}");

            if(falseBlock != null)
            {
                context.writer.WriteLine("else {");
                ChronDefer.IncreaseScope();

                falseBlock.Write(context);

                ChronDefer.VisitCurrentScope(context);
                ChronDefer.DecreaseScope();

                context.writer.WriteLine("}");
            }

            ChronDefer.VisitCurrentScope(context);
            ChronDefer.DecreaseScope();
        }
    }
}
