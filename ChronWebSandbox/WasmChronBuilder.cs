using Antlr4.Runtime;
using ChronIR;
using ChronIR.IR.Operation;
using System.Xml.Linq;

namespace ChronCompiler
{
    public class WasmChronBuilder : Builder
    {
        public static HttpClient Http;
        private static Dictionary<string, string> fetchedSources = new();
        
        private static string Format(string name) => $"{name.Replace('.', '/')}.chron";
        public static async Task FetchSource(string name)
        {
            var src = await Http.GetStringAsync(Format(name));
            fetchedSources[name] = src;
        }
        private static string GetSource(string name)
        {
            return fetchedSources[name];
        }
        public override  void CompileChronScript(string name)
        {
            var source = GetSource(name);

            if (string.IsNullOrEmpty(source))
            {
                throw new FileNotFoundException($"ChronCompiler could not find {source}");
            }

            if (GetInclusion().IsIncluded(name))
                return;

            CompileChronScriptSource(name, source);
            Console.WriteLine($"Compiled {name}");

            GetInclusion().AddInclusion(name);
        }

        public string BuildToString()
        {
            var module = GetModule();
            module.AddStatement(Root);
            module.Write();

            return module.Source();
        }
    }
}
