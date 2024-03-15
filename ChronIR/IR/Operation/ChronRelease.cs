﻿using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronRelease : ChronStatement, ChronExpression, ChronDeferer
    {
        public bool AddToDeferQueue = true;
        private ChronExpression expression;
        private ChronTemporaryVariable expressionStore;
        private static ChronFunction MemoryRelease = new(ChronTypes.GCRelease, true);
        public ChronRelease(ChronExpression expr) 
        { 
            this.expression = expr;
            this.expressionStore = new("RELEASE_TMP", this.expression);
        }

        public void Defer(ChronContext context)
        {
            ChronInvoke invoke = new(MemoryRelease);
            invoke.AddParameter(expressionStore);
            invoke.Write(context);
        }
        public object Read(ChronContext context)
        {
            if (expression is ChronConditionalAutoRelease condRelease)
            {
                if (!condRelease.CanAutoRelease(context))
                {
                    return expression.Read(context);
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
            invoke.AddParameter(expression);
            invoke.Write(context);
        }
    }
}
