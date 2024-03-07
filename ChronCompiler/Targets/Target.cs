namespace ChronCompiler.Targets
{
    public class Target
    {
        public string Name;
        public string Path;

        private static List<Target> targets = new();
        public static Target GetTarget(string name) => targets.Find(x => x.Name == name);
        public static void AddTarget(string name, string path) => targets.Add(new() { Name = name, Path = path });
    }
}
