using Antlr4.Runtime;
using ChronIR;
using ChronIR.IR.Operation;
using System.Xml.Linq;

namespace ChronCompiler
{
    public abstract class Builder
    {
        private static string RootDirectory = System.IO.Directory.GetCurrentDirectory();
        private static string WorkingDirectory = Environment.CurrentDirectory;

        internal ChronContext moduleContext;
        internal ChronModule module;
        internal ModuleInclusion inclusion = new();
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
        public void CompileChronScriptSource(string name, string source)
        {
            var inputStream = new AntlrInputStream(source);
            var lexer = new ChronLexer(inputStream);
            var CTS = new CommonTokenStream(lexer);
            var parser = new ChronParser(CTS);
            var context = parser.program();

            var visitor = new CodeGen(name, this);
            visitor.Visit(context);
        }
        public abstract void CompileChronScript(string name);
    }
}
