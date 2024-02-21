﻿using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronReturn : ChronStatement
    {
        private ChronExpression returnExpression;
        public ChronReturn(ChronExpression expr) { this.returnExpression = expr; }
        public void Write(ChronContext context)
        {
            var expression = returnExpression.Read(context);

            ChronDefer.VisitCurrentScope(context);

            context.writer.WriteLine($"return {expression};");
        }
    }
}
