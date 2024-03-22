using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronTemporaryVariable : ChronStatement, ChronExpression
    {
        private string name;
        private int tempVariableNumber = ChronTypes.TEMP_VARIABLE++;
        private ChronExpression value;
        private string type;
        private static List<ChronTemporaryVariable> Alive = new();
        public ChronTemporaryVariable(string name, ChronExpression value, string type = "void*")
        {
            this.name = $"_TEMP_{name}_{tempVariableNumber}";
            this.value = value;
            this.type = type;
        }
        public string GetName() => name;
        public object Read(ChronContext context)
        {
            return name;
        }
        public string EmbedWrite(ChronContext context) { return $"{type} {name} = {value.Read(context)};"; }
        public string EmbedRewrite(ChronContext context) { return $"{name} = {value.Read(context)};"; }
        public void Undeclare()
        {
            Alive.Remove(this);
        }
        public void Write(ChronContext context)
        {
            if(Alive.Contains(this))
            {
                context.writer.WriteLine(EmbedRewrite(context));
            }
            else
            {
                context.writer.WriteLine(EmbedWrite(context));
                Alive.Add(this);
            }

            if (value is ChronVariableRef var_ref)
                var_ref.VariableCreatedRef(context, this);

        }
    }
}
