namespace ChronIR.IR.Internal
{
    internal interface ChronDeferer
    {
        void Defer(ChronContext context);
    }
    internal static class ChronDefer
    {
        private static Dictionary<int, List<ChronDeferer>> DeferDict = new();
        private static int ScopeLevel = -1;
        public static void IncreaseScope()
        {
            ScopeLevel++;
            DeferDict[ScopeLevel] = new List<ChronDeferer>();
        }
        public static void DecreaseScope()
        {
            DeferDict[ScopeLevel] = null;
            ScopeLevel--;
        }
        public static void Remove(ChronDeferer deferer)
        {
            DeferDict[ScopeLevel].Remove(deferer);
        }
        public static void Add(ChronDeferer deferer)
        {
            if (DeferDict.TryGetValue(ScopeLevel, out List<ChronDeferer> deferList))
            {
                deferList.Add(deferer);
            }
        }
        public static void VisitCurrentScope(ChronContext context)
        {
            if (DeferDict.TryGetValue(ScopeLevel, out List<ChronDeferer> deferList))
            {
                foreach (var deferer in deferList)
                    deferer.Defer(context);

                deferList.Clear();
            }
        }
    }
}
