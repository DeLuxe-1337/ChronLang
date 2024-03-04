﻿using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronRelease : ChronStatement, ChronExpression, ChronDeferer
    {
        private ChronExpression expression;
        private string variableReferenceName = $"_TEMP_V{ChronTypes.TEMP_VARIABLE++}";
        public ChronRelease(ChronExpression expr) { this.expression = expr; }

        public void Defer(ChronContext context)
        {
            context.writer.WriteLine($"{ChronTypes.GCRelease}({variableReferenceName});");
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
            context.writer.WriteLine($"void* {variableReferenceName} = {expression.Read(context)};");
            ChronDefer.Add(this);
            return variableReferenceName;
        }

        public void Write(ChronContext context)
        {
            context.writer.WriteLine($"{ChronTypes.GCRelease}({expression.Read(context)});");
        }
    }
}
