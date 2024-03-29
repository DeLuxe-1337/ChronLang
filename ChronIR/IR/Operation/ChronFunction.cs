﻿using ChronIR.IR.Internal;
using System.Text;

namespace ChronIR.IR.Operation
{
    public class ChronFunction : ChronStatement, ChronInvokable, ChronExpression
    {
        public ChronStatementBlock Block;
        public string Name;
        public string ScopeName;
        public bool DoesReturn = false;
        public bool Inline = false;
        public bool External = false;
        private List<string> parameters = new();
        private static ChronFunction createFunction = new(ChronTypes.CreateFunction, true);
        public ChronFunction(string name, bool define = false) //I do this so,you can easily do extern stuff...
        {
            if (define)
                this.Name = name;
            else if(name.ToLower() == "main")
                this.Name = "main";
            else
                this.Name = $"_F_{name}";

            ScopeName = name;

            this.Name = Name.Replace(".", "_");

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
        private string GenerateReturn()
        {
            return $"{((Block != null && Block.HasStatement<ChronReturn>()) || DoesReturn ? ChronTypes.TypeMap["object"].Value : ChronTypes.TypeMap["void"].Value)}";
        }
        private string GenerateFunctionNameParam(string name)
        {
            return $"{name}({FormatParameters()})";
        }
        private string GenerateFunctionHeader(string name)
        {
            return $"{(External ? "extern" : string.Empty)} {GenerateReturn()} {(Inline ? "inline" : string.Empty)} {GenerateFunctionNameParam(Name)}";
        }
        private string GenerateFunctionSignature(string name)
        {
            return $"typedef {GenerateReturn()} (*{name})({FormatParameters()});";
        }
        public string Signature => Name + "Sig";
        public void ForwardDeclare(ChronContext context)
        {
            Name = ChronTypes.DefineFunction(Name);
            context.writer.WriteLine(GenerateFunctionSignature(Name + "Sig"));
            context.writer.WriteLine($"{GenerateFunctionHeader(Name)};");
            context.env.GetCurrentScope().AddToScope(ScopeName, this, false);
        }
        public void Write(ChronContext context)
        {
            if(Name == "main")
            {
                var initialize = ChronVariable.WriteGlobalInitializerFunction(context);
                Block.PrependStatement(new ChronInvoke(new ChronFunction(initialize, true)));
            }

            context.env.AddScope(new("FunctionBlock"));
            ChronDefer.IncreaseScope();

            context.writer.Write(GenerateFunctionHeader(Name));

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

            context.env.RemoveScope();
        }

        public string GetName(ChronContext context)
        {
            return Name;
        }

        public int ParameterCount()
        {
            return parameters.Count;
        }

        public object Read(ChronContext context)
        {
            ChronInvoke invoke = new(createFunction);
            invoke.AddParameter(new ChronRawText(Name));
            return new ChronRelease(invoke).Read(context);
        }
    }
}
