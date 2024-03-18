using ChronIR.IR.Internal;
using System.Numerics;
using System.Text;

namespace ChronIR.IR.Operation
{
    public class ChronInvoke : ChronConditionalAutoRelease, ChronStatement, ChronExpression
    {
        private ChronInvokable target;
        private ChronExpressionBlock parameters = new();
        public (bool, ChronEnvironmentAccessor) RuntimeInvoke;
        public void AddParameter(ChronExpression chronExpression) => parameters.AddExpression(chronExpression);
        public override bool CanAutoRelease(ChronContext context)
        {
            if (target is ChronInvokableAccessor accessor)
            {
                if (accessor.GetInvokable(context) is ChronNativeFunction)
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
                if(chronExpression is ChronTable) 
                {
                    var tableTemp = new ChronTemporaryVariable("TABLE_CALL_T", chronExpression);
                    tableTemp.Write(ctx);
                    sb.Append($"{tableTemp.Read(ctx)},");
                    continue;
                }

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
            var name = target.GetName(context);
            var parameters = ParametersToString(context);

            if(RuntimeInvoke.Item1)
            {
                var funcRaw = RuntimeInvoke.Item2.GetStatement(context);
                if(funcRaw is ChronFunction func)
                {
                    if (target is ChronEnvironmentAccessor accessor)
                            name = accessor.Read(context).ToString();

                    return $"(({func.Signature})c_pointer({name}))({parameters})";
                }
            }

            return $"{name}({parameters})";
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
