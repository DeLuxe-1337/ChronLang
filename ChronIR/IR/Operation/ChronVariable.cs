using ChronIR.IR.Internal;
using ChronIR.IR.Internal.GC;
using System;
using System.Security.Cryptography;

namespace ChronIR.IR.Operation
{
    public class ChronVariable : ChronGarbage, ChronExpression, ChronStatement
    {
        private string _accessor_name;
        private string name;
        private ChronExpression value;
        public static ChronVariable Create(string name, ChronExpression value) => new ChronVariable(name, value);
        public ChronVariable(string name, ChronExpression value)
        {
            this.name = name;
            this._accessor_name = $"_V_{name}";
            this.value = value;
        }
        private ChronStatement GetIndexable(ChronContext context)
        {
            ChronEnvironmentAccessor accessor = new(name);
            return accessor.GetObject(context) as ChronStatement;
        }
        private void SetIndexableParent(ChronContext context)
        {
            if (GetIndexable(context) is ChronIndexable varOverride)
            {
                varOverride.SetParent(this);
            }
        }
        public object Read(ChronContext context)
        {
            SetIndexableParent(context);
            ChronGC.Retain(context, this);
            return _accessor_name.Split('.')[0];
        }

        public void Write(ChronContext context)
        {
            ChronGC.ReleaseAll(context);

            if (context.env.FindValueByName(name) == null)
            {
                var exp = value.Read(context);

                if(value is ChronIndexable indexable)
                {
                    indexable.CreateIndexes(context, name);
                }

                context.writer.WriteLine($"{ChronTypes.TypeMap["object"].Value} {_accessor_name} = {exp};");
            }
            else
            {
                if(GetIndexable(context) is ChronIndexable varOverride)
                {
                    varOverride.SetParent(this);
                    varOverride.Write(context, value);
                    return;
                }

                context.writer.WriteLine($"{_accessor_name} = {value.Read(context)};");
            }

            context.env.GetCurrentScope().AddToScope(name, this);
        }

        public override string GC_Reference()
        {
            return _accessor_name;
        }
    }
}
