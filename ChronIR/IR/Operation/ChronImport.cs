using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronImport : ChronStatement, ChronInvokable
    {
        public bool DefineExtern = false;
        public string Return = ChronTypes.TypeMap["void"].Value;
        public List<ChronTypes.ChronType> Parameters = new();
        public string Name;
        public string RawCName;
        private static Dictionary<string, int> definedFunctions = new();
        private Dictionary<ChronTypes.ChronType, string> chronTypeConversion = new() { { ChronTypes.TypeMap["string"], "str" }, { ChronTypes.TypeMap["int"], "integer" }, };
        public void AddParameter(ChronTypes.ChronType parameter) => Parameters.Add(parameter);
        public ChronImport(string name)
        {
            this.Name = name;
            RawCName = name;
        }
        private void DefineExternal(ChronContext context, List<ChronTypes.ChronType> type)
        {
            context.writer.WriteLine($"extern {Return} _E_{RawCName}({string.Join(",", type)});");
        }
        private void DefineOriginal(ChronContext context, List<string> type)
        {
            context.writer.WriteLine($"{Return} _I_{Name}({string.Join(",", type)})");
        }
        public void Write(ChronContext context)
        {
            context.env.GetCurrentScope().AddToScope(Name, this, false);

            {
                if (definedFunctions.ContainsKey(Name))
                    definedFunctions[Name] += 1;
                else
                    definedFunctions[Name] = 0;

                if (definedFunctions[Name] > 0)
                {
                    Name = $"{Name}{definedFunctions[Name]}";
                }
            }

            if(DefineExtern)
            {
                DefineExternal(context, Parameters);
                RawCName = $"_E_{RawCName}";
            }

            {
                List<string> param = new();

                int param_count = 0;
                foreach (var p in Parameters)
                {
                    param.Add($"{ChronTypes.TypeMap["object"]} p{param_count++}");
                }

                DefineOriginal(context, param);
            }

            context.writer.WriteLine("{");

            {
                int param_count = 0;
                foreach (var p in Parameters)
                {
                    context.writer.WriteLine($"DynObject* o{param_count} = p{param_count++}->Object;");
                }
            }

            {
                int param_count = 0;
                foreach (var p in Parameters)
                {
                    context.writer.WriteLine($"{p.Value} r{param_count} = o{param_count++}->{chronTypeConversion[p]};");
                }
            }

            context.writer.Write($"{(Return == "void" ? "return" : "")} {RawCName}(");
            List<string> build = new();
            for (int i = 0; i < Parameters.Count; i++)
            {
                build.Add($"r{i}");
            }
            context.writer.Write(string.Join(",", build));
            context.writer.WriteLine(");");

            context.writer.WriteLine("}");
        }

        public string GetName(ChronContext context)
        {
            return $"_I_{Name}";
        }

        public int ParameterCount()
        {
            return Parameters.Count;
        }
    }
}
