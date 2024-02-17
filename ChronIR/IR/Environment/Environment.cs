using static ChronIR.IR.Environment.Scope;

namespace ChronIR.IR.Environment
{
    internal class Environment
    {
        private Stack<Scope> scopes = new();
        public void AddScope(Scope scope) => scopes.Push(scope);
        public Scope RemoveScope() => scopes.Pop();
        public Stack<Scope>.Enumerator GetScopes() => scopes.GetEnumerator();
        public Scope GetCurrentScope() => scopes.Peek();
        public ScopeItem[] FindValueByName(string name)
        {
            foreach (Scope scope in scopes)
            {
                if (scope.Has(name)) return scope.Get(name);
            }

            return null;
        }
    }
}
