using Antlr4.Runtime;
using ChronIR;
using ChronIR.IR.Operation;

namespace ChronCompiler
{
    public class Builder
    {
        private static string RootDirectory = AppContext.BaseDirectory;
        private static string WorkingDirectory = Environment.CurrentDirectory;

        public static Dictionary<string, string> AvailableTargets = new();
        public static void InitializeTargets()
        {
            foreach (var target in Directory.GetFiles(Path.Combine(RootDirectory, "Targets")))
            {
                FileInfo file = new(target);
                if (file.Extension == ".bat")
                    AvailableTargets.Add(file.Name.Replace(".bat", "").ToUpper(), file.FullName);
            }
        }
        public string Target;
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

            return File.Exists(currentDirectoryPath) ? currentDirectoryPath :
                   File.Exists(exeDirectoryPath) ? exeDirectoryPath :
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
            module.Compile(AvailableTargets[Target]);
        }
    }
}
