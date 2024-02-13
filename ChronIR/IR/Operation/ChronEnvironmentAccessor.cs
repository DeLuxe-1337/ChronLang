﻿using ChronIR.IR.Environment;
using ChronIR.IR.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronIR.IR.Operation
{
    public class ChronEnvironmentAccessor : ChronExpression, ChronStatement
    {
        private string name;
        public ChronEnvironmentAccessor(string name) => this.name = name;
        private Scope.ScopeItem[] Find(ChronContext ctx)
        {
            var result = ctx.env.FindValueByName(name);
            if(result == null)
            {
                Console.WriteLine($"Failed to find `{name}` in current scope...");
            }

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

        //public string GetName(ChronContext context)
        //{
        //    if (Find(context).First().data is ChronInvokable invoke)
        //        return invoke.GetName(context);

        //    return "CHRON_ENV_ACCESSOR_NO_NAME_FOR_INVOKABLE";
        //}

        //public int ParameterCount()
        //{
        //    return -1;
        //}
    }
}
