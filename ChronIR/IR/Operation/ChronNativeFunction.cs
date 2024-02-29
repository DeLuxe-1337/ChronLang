using ChronIR.IR.Internal;
using System.Text;

namespace ChronIR.IR.Operation
{
    public class ChronNativeFunction : ChronStatement, ChronInvokable
    {
        public ChronStatementBlock Block;
        public string Name;
        public string ScopeName;
        public string ReturnValue = "void";
        public bool Inline = false;
        private List<string> parameters = new();
        private string Parameters;
        public ChronNativeFunction(string name) //I do this so,you can easily do extern stuff...
        {
            this.Name = name;
            this.ScopeName = name;
        }
        public void SetParameters(string parameters) => Parameters = parameters;
        public void SetName(string name) => Name = name;
        public void SetReturn(string returnValue) => this.ReturnValue = returnValue;
        public void AddParameter(string name) => parameters.Add(name);
        public ChronRawText GetParameter(int index) => new(parameters[index]);
        public void Write(ChronContext context)
        {
            ChronDefer.IncreaseScope();

            context.env.GetCurrentScope().AddToScope(ScopeName, this, false);

            Name = ChronTypes.DefineFunction(Name);

            context.writer.Write($"{ReturnValue} {(Inline ? "inline" : string.Empty)} {Name}({Parameters})");

            for (int i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];
                context.env.GetCurrentScope().AddToScope(parameter, GetParameter(i));
            }

            if (Block != null)
            {
                context.writer.WriteLine("{");
                Block.Write(context);

                ChronDefer.VisitCurrentScope(context);
                ChronDefer.DecreaseScope();

                context.writer.WriteLine("}");
            }
            else
                context.writer.WriteLine(";");

            foreach (var p in parameters)
            {
                context.env.GetCurrentScope().RemoveAllWithName(p);
            }
        }

        public string GetName(ChronContext context)
        {
            return Name;
        }

        public int ParameterCount()
        {
            return parameters.Count;
        }
    }
}
