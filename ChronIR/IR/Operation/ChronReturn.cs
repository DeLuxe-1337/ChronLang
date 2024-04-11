using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronReturn : ChronStatement
    {
        private ChronExpression returnExpression;
        public ChronReturn(ChronExpression expr) { this.returnExpression = expr; }
        public void Write(ChronContext context)
        {
            if(returnExpression != null)
            {
                var tmpReturn = new ChronTemporaryVariable("RETURN_TMP", returnExpression);
                tmpReturn.Write(context);

                var expression = tmpReturn.Read(context);

                // Only do this for the defer check
                if (returnExpression is ChronEnvironmentAccessor accessor)
                    returnExpression = accessor.GetExpression(context);

                if (returnExpression is ChronDeferer defer)
                    ChronDefer.Remove(defer);

                ChronDefer.VisitCurrentScope(context);

                context.writer.WriteLine($"return {expression};");
            } 
            else
                context.writer.WriteLine($"return;");
        }
    }
}
