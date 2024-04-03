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

            var value = new ChronTemporaryVariable("WHILE_TMP", condition);

            context.writer.WriteLine($"while(1) {{");

            ChronDefer.IncreaseScope();

            // Condition
            value.Write(context);

            // Evaluate condition
            context.writer.WriteLine($"if({ChronTypes.GetBooleanFromObject}({value.Read(context)}) == false) {{");

            // Clean up condition
            ChronDefer.VisitCurrentScope(context);

            // Exit while loop
            context.writer.WriteLine("break;\n}");

            block.Write(context);

            ChronDefer.VisitCurrentScope(context);
            ChronDefer.DecreaseScope();

            value.Undeclare();

            context.writer.WriteLine("}");

            ChronDefer.VisitCurrentScope(context);
            ChronDefer.DecreaseScope();
            context.env.RemoveScope();
        }
    }
}
