using ChronIR.IR.Environment;
using ChronIR.IR.Internal;
using System;

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
        public object GetObject(ChronContext context)
        {
            return Find(context).First().data;
        }
        public object Read(ChronContext context)
        {
            var exp = GetObject(context) as ChronExpression;
            return exp.Read(context);
        }

        public void Write(ChronContext context)
        {
            var stmt = GetObject(context) as ChronStatement;
            stmt.Write(context);
        }
    }
}
