using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronRelease : ChronStatement, ChronExpression, ChronDeferer
    {
        public bool AddToDeferQueue = true;
        public ChronExpression Expression;
        private ChronTemporaryVariable expressionStore;
        private static ChronFunction MemoryRelease = new(ChronTypes.GCRelease, true);
        public ChronRelease(ChronExpression expr) 
        { 
            this.Expression = expr;
            this.expressionStore = new("RELEASE_TMP", this.Expression);
        }

        public void Defer(ChronContext context)
        {
            ChronInvoke invoke = new(MemoryRelease);
            invoke.AddParameter(expressionStore);
            invoke.Write(context);
        }
        public object Read(ChronContext context)
        {
            if (Expression is ChronConditionalAutoRelease condRelease)
            {
                if (!condRelease.CanAutoRelease(context))
                {
                    return Expression.Read(context);
                }
            }

            if (AddToDeferQueue)
                ChronDefer.Add(this);

            expressionStore.Write(context);
            return expressionStore.Read(context);
        }

        public void Write(ChronContext context)
        {
            ChronInvoke invoke = new(MemoryRelease);
            invoke.AddParameter(Expression);
            invoke.Write(context);
        }
    }
}
