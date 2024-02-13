using Antlr4.Runtime;
using ChronIR;
using ChronIR.IR.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronCompiler
{
    public class Builder
    {
        private ChronContext moduleContext;
        private ChronModule module;
        private ModuleInclusion inclusion = new();
        public ModuleInclusion GetInclusion() => inclusion;
        public ChronContext GetCtx() => moduleContext;
        public ChronModule GetModule() => module;
        public ChronStatementBlock Root = new();
        public void Create(string name)
        {
            moduleContext = new(name);
            module = new(moduleContext);
            module.SetupChronRuntime();
        }
        public void CompileChronScript(string name) 
        {
            if (inclusion.IsIncluded(name))
                return;

            var input = File.ReadAllText($"{name.Replace('.', '/')}.chron");

            var inputStream = new AntlrInputStream(input);
            var lexer = new ChronLexer(inputStream);
            var CTS = new CommonTokenStream(lexer);
            var parser = new ChronParser(CTS);
            var context = parser.program();

            var visitor = new CodeGen(name, this);
            visitor.Visit(context);

            inclusion.AddInclusion(name);
        }
        public void Build()
        {
            module.AddStatement(Root);
            module.Write();
            Console.WriteLine($"\t------>\tCompiling and executing\t<------");
            module.Source(ChronModuleCompile.Compile);
        }
    }
}
