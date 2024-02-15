using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChronIR.IR.Internal;
using ChronIR.IR.Internal.GC;

namespace ChronIR.IR.Operation
{
    public class ChronInvoke : ChronStatement, ChronExpression
    {
        private ChronInvokable target;
        private ChronExpressionBlock parameters = new();
        public void AddParameter(ChronExpression chronExpression) => parameters.AddExpression(chronExpression);
        public ChronInvoke(ChronInvokable invokeTarget) 
        { 
            this.target = invokeTarget;
        }
        private string ParametersToString(ChronContext ctx)
        {
            StringBuilder sb = new StringBuilder();
            foreach(ChronExpression chronExpression in parameters.GetParameters())
            {
                sb.Append($"{chronExpression.Read(ctx)},");
            }
            return sb.ToString().TrimEnd(',');
        }
        public object Read(ChronContext context)
        {
            return $"{target.GetName(context)}({ParametersToString(context)})";
        }

        public void Write(ChronContext context)
        {
            ChronDefer.IncreaseScope();

            var value = Read(context);
            ChronGC.ReleaseAll(context);
            context.writer.WriteLine($"{value};");

            ChronDefer.VisitCurrentScope(context);
            ChronDefer.DecreaseScope();
        }
    }
}
