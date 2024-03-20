using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronForEach : ChronStatement
    {
        private ChronStatementBlock block;
        private ChronExpression iterator;
        private string index;
        private string value;
        public ChronForEach(string index, string value, ChronStatementBlock block, ChronExpression iterator)
        {
            this.index = index;
            this.value = value;
            this.iterator = iterator;
            this.block = block;
        }
        public void Write(ChronContext context)
        {
            context.writer.WriteLine("{");
            ChronDefer.IncreaseScope();
            context.env.AddScope(new("ForEachBlock"));

            var iter = new ChronTemporaryVariable("ITER_OBJECT", iterator);
            var __iter__ = new ChronTemporaryVariable("ITER", new ChronRawText($"{ChronTypes.GetDynObject}({iter.Read(context)})->data.ptr"), ChronTypes.Iterator);

            iter.Write(context);
            __iter__.Write(context);

            var __i = new ChronTemporaryVariable("i", new ChronRawText("0"), "int");

            var foreachIndex = new ChronVariable(new ChronEnvironmentAccessor(index), new ChronRawText($"{__iter__.Read(context)}->index({__iter__.Read(context)}->self, {__i.Read(context)})"));
            var foreachValue = new ChronVariable(new ChronEnvironmentAccessor(value), new ChronRawText($"{__iter__.Read(context)}->value({__iter__.Read(context)}->self, {__i.Read(context)})"));
            block.PrependStatement(foreachIndex);
            block.PrependStatement(foreachValue);

            var forLoop = new ChronFor(__i, new ChronRawText($"{__iter__.Read(context)}->size"), block);
            forLoop.Write(context);

            ChronDefer.VisitCurrentScope(context);
            ChronDefer.DecreaseScope();

            context.writer.WriteLine("}");
            context.env.RemoveScope();
        }
    }
}
