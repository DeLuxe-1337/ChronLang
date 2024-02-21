using ChronIR.IR.Internal;
using ChronIR.IR.Internal.GC;

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
        public object Read(ChronContext context)
        {
            ChronGC.Retain(context, this);
            return _accessor_name;
        }

        public void Write(ChronContext context)
        {
            ChronGC.ReleaseAll(context);

            if (context.env.FindValueByName(name) == null)
            {
                context.writer.WriteLine($"{ChronTypes.TypeMap["object"].Value} {_accessor_name} = {value.Read(context)};");
            }
            else
            {
                context.writer.WriteLine($"{_accessor_name} = {value.Read(context)};");
            }

            context.env.GetCurrentScope().AddToScope(name, this);
        }
        public void Release(ChronContext context)
        {
            context.env.GetCurrentScope().RemoveAllWithName(name);
            new ChronRelease(this).Write(context);
        }
        public override string GC_Reference()
        {
            return _accessor_name;
        }
    }
}
