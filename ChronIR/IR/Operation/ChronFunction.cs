using ChronIR.IR.Internal;
using System.Text;

namespace ChronIR.IR.Operation
{
    public class ChronFunction : ChronStatement, ChronInvokable
    {
        public ChronStatementBlock Block;
        public string Name;
        public string ScopeName;
        public bool DoesReturn = false;
        public bool Inline = false;

        private static Dictionary<string, int> DefinedFunctions = new();
        private List<string> parameters = new();
        public ChronFunction(string name, bool define = false) //I do this so,you can easily do extern stuff...
        {
            if (define || name == "main")
                this.Name = name;
            else
                this.Name = $"_F_{name}";

            ScopeName = Name;
        }
        public void SetName(string name) => Name = name;
        public void SetReturn(bool doesReturn) => this.DoesReturn = doesReturn;
        public void AddParameter(string name) => parameters.Add(name);
        public ChronRawText GetParameter(int index) => new(parameters[index]);
        private string FormatParameters()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var parameter in parameters)
            {
                sb.Append($"{ChronTypes.TypeMap["object"].Value} {parameter.ToString()},");
            }
            return sb.ToString().TrimEnd(',');
        }
        public void Write(ChronContext context)
        {
            ChronDefer.IncreaseScope();

            context.env.GetCurrentScope().AddToScope(ScopeName.TrimStart('_', 'F', '_'), this, false);

            {
                if (DefinedFunctions.ContainsKey(Name))
                    DefinedFunctions[Name] += 1;
                else
                    DefinedFunctions[Name] = 0;
                if (DefinedFunctions[Name] > 0)
                {
                    Name = $"{Name}{DefinedFunctions[Name]}";
                }
            }

            context.writer.Write($"{((Block != null && Block.HasStatement<ChronReturn>()) || DoesReturn ? ChronTypes.TypeMap["object"].Value : ChronTypes.TypeMap["void"].Value)} {(Inline ? "inline" : string.Empty)} {Name}({FormatParameters()})");

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
