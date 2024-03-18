using ChronIR.IR.Environment;
using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronEnvironmentAccessor : ChronExpression, ChronStatement, ChronInvokable
    {
        private ChronInvokableAccessor Invokable;

        public string Value;
        public ChronEnvironmentAccessor(string name) => this.Value = name;
        public ChronEnvironmentAccessor(string name, int param)
        {
            this.Value = name;
            Invokable = new(name, param);
        }
        private Scope.ScopeItem[] Find(ChronContext ctx)
        {
            var result = ctx.env.FindValueByName(Value);
            if (result == null)
                throw new Exception($"Failed to find `{Value}` in current scope...");

            return result;
        }
        public ChronExpression GetExpression(ChronContext ctx)
        {
            return Find(ctx).First().data as ChronExpression;
        }
        public ChronStatement GetStatement(ChronContext ctx)
        {
            return Find(ctx).First().data as ChronStatement;
        }
        public object Read(ChronContext context)
        {
            var exp = GetExpression(context);
            return exp.Read(context);
        }

        public void Write(ChronContext context)
        {
            var stmt = GetStatement(context);
            stmt.Write(context);
        }

        public string GetName(ChronContext context)
        {
            return Invokable.GetName(context);
        }

        public int ParameterCount()
        {
            return Invokable.ParameterCount();
        }
    }
}
