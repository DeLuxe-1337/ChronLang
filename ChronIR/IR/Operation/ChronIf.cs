using ChronIR.IR.Internal;

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
            context.env.AddScope(new("IfBlock"));
            context.writer.WriteLine($"if({ChronTypes.GetBooleanFromObject}({condition.Read(context)})) {{");

            ChronDefer.IncreaseScope();
            context.env.AddScope(new("IfTrueBlock"));

            trueBlock.Write(context);

            ChronDefer.VisitCurrentScope(context);
            ChronDefer.DecreaseScope();
            context.env.RemoveScope();

            context.writer.WriteLine("}");

            if (falseBlock != null)
            {
                context.writer.WriteLine("else {");
                ChronDefer.IncreaseScope();
                context.env.AddScope(new("IfFalseBlock"));

                falseBlock.Write(context);

                ChronDefer.VisitCurrentScope(context);
                ChronDefer.DecreaseScope();
                context.env.RemoveScope();

                context.writer.WriteLine("}");
            }

            ChronDefer.VisitCurrentScope(context);
            ChronDefer.DecreaseScope();
            context.env.RemoveScope();
        }
    }
}
