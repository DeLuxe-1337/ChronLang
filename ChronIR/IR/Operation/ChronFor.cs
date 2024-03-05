using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronFor : ChronStatement
    {
        private ChronTemporaryVariable index;
        private ChronExpression size;
        private ChronStatementBlock block;
        public ChronFor(ChronTemporaryVariable index, ChronExpression size, ChronStatementBlock block)
        {
            this.index = index;
            this.size = size;
            this.block = block;
        }

        public void Write(ChronContext context)
        {
            context.writer.WriteLine($"for({index.EmbedWrite(context)} {index.Read(context)} < {size.Read(context)}; {index.Read(context)}++) {{");
            ChronDefer.IncreaseScope();

            block.Write(context);

            ChronDefer.VisitCurrentScope(context);
            ChronDefer.DecreaseScope();
            context.writer.WriteLine("}");
        }
    }
}
