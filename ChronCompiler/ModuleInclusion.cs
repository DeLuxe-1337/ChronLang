using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronCompiler
{
    public class ModuleInclusion
    {
        private Dictionary<string, bool> modules = new();

        public bool IsIncluded(string name) => modules.ContainsKey(name);
        public void AddInclusion(string name) => modules[name] = true;
    }
}
