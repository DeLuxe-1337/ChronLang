using ChronIR.IR.Internal;

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
            context.env.AddScope(new("WhileBlock"));
            ChronDefer.IncreaseScope();

            if(condition is ChronRelease releaseExpr)
            {
                condition = releaseExpr.Expression;
            }

            context.writer.WriteLine($"while({ChronTypes.GetBooleanFromObject}({condition.Read(context)})) {{");

            ChronDefer.IncreaseScope();

            block.Write(context);

            ChronDefer.VisitCurrentScope(context);
            ChronDefer.DecreaseScope();

            context.writer.WriteLine("}");

            ChronDefer.VisitCurrentScope(context);
            ChronDefer.DecreaseScope();
            context.env.RemoveScope();
        }
    }
}
