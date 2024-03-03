using ChronIR.IR.Internal;
using System.Text;

namespace ChronIR.IR.Operation
{
    public class ChronInvoke : ChronConditionalAutoRelease, ChronStatement, ChronExpression
    {
        private ChronInvokable target;
        private ChronExpressionBlock parameters = new();
        public void AddParameter(ChronExpression chronExpression) => parameters.AddExpression(chronExpression);
        public override bool CanAutoRelease(ChronContext context)
        {
            if (target is ChronInvokableAccessor accessor)
            {
                if(accessor.GetInvokable(context) is ChronNativeFunction)
                    return false;
            }

            return true;
        }
        public ChronInvoke(ChronInvokable invokeTarget)
        {
            this.target = invokeTarget;
        }
        private string ParametersToString(ChronContext ctx)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ChronExpression chronExpression in parameters.GetParameters())
            {
                if ((chronExpression is ChronConstant || chronExpression is ChronAutoRelease))
                {
                    sb.Append($"{new ChronRelease(chronExpression).Read(ctx)},");
                }
                else
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
            context.writer.WriteLine($"{value};");

            ChronDefer.VisitCurrentScope(context);
            ChronDefer.DecreaseScope();
        }
    }
}
