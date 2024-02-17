namespace ChronIR.IR.Environment
{
    internal class Scope
    {
        public record ScopeItem(string name, object data);
        private static int CurrentScope = 0;
        public string Name = $"Scope{CurrentScope++}";
        public Scope() { }
        public Scope(string name) { Name = name; }
        private List<ScopeItem> scope = new();
        public void AddToScope(string name, object operation, bool removeExisting = true)
        {
            if (removeExisting)
                RemoveAllWithName(name);

            scope.Add(new(name, operation));
        }
        public void RemoveAllWithName(string name)
        {
            for (int i = 0; i < scope.Count; i++)
            {
                var scopeItem = scope[i];

                if (scopeItem.name == name)
                    scope.RemoveAt(i);
            }
        }
        public void RemoveFromScope(ScopeItem reference) => scope.Remove(reference);
        public List<ScopeItem>.Enumerator GetScope() => scope.GetEnumerator();
        public ScopeItem[] Get(string name)
        {
            List<ScopeItem> request = new();

            foreach (var item in scope)
            {
                if (item.name == name)
                    request.Add(item);
            }

            return request.ToArray();
        }
        public bool Has(string name) => Get(name).Length > 0;
    }
}
