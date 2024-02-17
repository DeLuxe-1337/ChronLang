using ChronIR.IR.Environment;
using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronEnvironmentAccessor : ChronExpression, ChronStatement
    {
        private string name;
        public ChronEnvironmentAccessor(string name) => this.name = name;
        private Scope.ScopeItem[] Find(ChronContext ctx)
        {
            var result = ctx.env.FindValueByName(name);
            if (result == null)
                throw new Exception($"Failed to find `{name}` in current scope...");

            return result;
        }
        public object Read(ChronContext context)
        {
            var exp = Find(context).First().data as ChronExpression;
            return exp.Read(context);
        }

        public void Write(ChronContext context)
        {
            var stmt = Find(context).First().data as ChronStatement;
            stmt.Write(context);
        }
    }
}
