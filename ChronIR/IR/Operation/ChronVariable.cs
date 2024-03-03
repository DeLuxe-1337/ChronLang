using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronVariable : ChronExpression, ChronStatement
    {
        private string _accessor_name;
        private string _name;
        private ChronExpression target;
        private ChronExpression value;
        public static ChronVariable Create(ChronExpression target, ChronExpression value) => new ChronVariable(target, value);
        public ChronVariable(ChronExpression target, ChronExpression value)
        {
            if (target is ChronEnvironmentAccessor accessor)
            {
                this._accessor_name = $"_V_{accessor.Value}";
                this._name = accessor.Value;
            }

            this.target = target;
            this.value = value;
        }
        public object Read(ChronContext context)
        {
            if (target is ChronVariableImpl impl)
            {
                return impl.VariableRead(context);
            }

            return _accessor_name;
        }

        public void Write(ChronContext context)
        {
            if (target is ChronVariableImpl target_impl)
            {
                target_impl.VariableWrite(context, value);

                return;
            }

            if (context.env.FindValueByName(_name) == null)
            {
                context.writer.WriteLine($"void* {_accessor_name} = {value.Read(context)};");
            }
            else
            {
                context.writer.WriteLine($"{_accessor_name} = {value.Read(context)};");
            }

            if (value is ChronVariableRef var_ref)
                var_ref.VariableCreatedRef(context, this);

            context.env.GetCurrentScope().AddToScope(_name, this);
        }
        public void Release(ChronContext context)
        {
            if (target is ChronVariableImpl impl)
                return;

            context.env.GetCurrentScope().RemoveAllWithName(_name);
            new ChronRelease(this).Write(context);
        }
    }
}
