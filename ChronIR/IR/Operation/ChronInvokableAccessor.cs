using ChronIR.IR.Environment;
using ChronIR.IR.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronIR.IR.Operation
{
    public class ChronInvokableAccessor : ChronInvokable
    {
        private string name;
        private int parameterCount = 0;
        public ChronInvokableAccessor(string name, int parameterCount = 0)
        {
            this.name = name;
            this.parameterCount = parameterCount;
        }
        private Scope.ScopeItem[] Find(ChronContext ctx)
        {
            var result = ctx.env.FindValueByName(name);
            if (result == null)
            {
                Console.WriteLine($"Failed to find `{name}` in current scope...");
            }

            return result;
        }
        public string GetName(ChronContext context)
        {
            Scope.ScopeItem[] results = Find(context);

            foreach(var scopeItem in results)
            {
                if(scopeItem.data is ChronInvokable invoke)
                {
                    if(invoke.ParameterCount() == parameterCount)
                        return invoke.GetName(context);
                }
            }

            //if (.First().data is ChronInvokable invoke)
            //    return invoke.GetName(context);

            return "CHRON_ENV_ACCESSOR_NO_NAME_FOR_INVOKABLE";
        }

        int ChronInvokable.ParameterCount()
        {
            return parameterCount;
        }
    }
}
