using Antlr4.Runtime;
using ChronCompiler.Targets;
using ChronIR;
using ChronIR.IR.Operation;

namespace ChronCompiler
{
    public class Builder
    {
        private static string RootDirectory = AppContext.BaseDirectory;
        private static string WorkingDirectory = Environment.CurrentDirectory;
        public static void InitializeTargets()
        {
            foreach (var target in Directory.GetFiles(Path.Combine(RootDirectory, "Targets")))
            {
                FileInfo file = new(target);
#if LINUX
                if (file.Extension == ".sh")
                    Target.AddTarget(file.Name.Replace(".sh", "").ToUpper(), file.FullName);
#else
                if (file.Extension == ".bat")
                    Target.AddTarget(file.Name.Replace(".bat", "").ToUpper(), file.FullName);
#endif
            }
        }
        public string SelectedTarget;
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
        private string? GetFilePath(string name)
        {
            string currentDirectoryPath = Path.Combine(WorkingDirectory, name);
            string exeDirectoryPath = Path.Combine(RootDirectory, name);

            return File.Exists(exeDirectoryPath) ? exeDirectoryPath : 
                File.Exists(currentDirectoryPath) ? currentDirectoryPath :
                   null;
        }
        public void CompileChronScript(string name)
        {
            var sourcePath = GetFilePath($"{name.Replace('.', '/')}.chron");

            if (string.IsNullOrEmpty(sourcePath))
            {
                throw new FileNotFoundException($"ChronCompiler could not find {sourcePath}");
            }

            if (inclusion.IsIncluded(name))
                return;

            var input = File.ReadAllText(sourcePath);

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
            TargetCompiler.CompileTarget(module, Target.GetTarget(SelectedTarget));
        }
    }
}
