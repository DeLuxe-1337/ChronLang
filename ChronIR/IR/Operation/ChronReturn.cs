using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronReturn : ChronStatement
    {
        private ChronExpression returnExpression;
        public ChronReturn(ChronExpression expr) { this.returnExpression = expr; }
        public void Write(ChronContext context)
        {
            var tmpReturn = new ChronTemporaryVariable("RETURN_TMP", returnExpression);
            tmpReturn.Write(context);

            var expression = tmpReturn.Read(context);

            ChronDefer.VisitCurrentScope(context);

            context.writer.WriteLine($"return {expression};");
        }
    }
}
