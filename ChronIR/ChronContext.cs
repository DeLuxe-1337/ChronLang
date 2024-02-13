using ChronIR.IR.Environment;

namespace ChronIR
{
    public enum BuildModeOption
    {
        Release,
        Debug
    }
    public class ChronContext
    {
        private static int ModuleNumber = 0;

        internal Writer writer;
        internal IR.Environment.Environment env = new();

        public string Name = $"module{++ModuleNumber}";
        public BuildModeOption BuildMode = BuildModeOption.Debug;
        public ChronContext(string name)
        {
            this.Name = name;
        }

        public ChronContext()
        {
        }
        internal void End()
        {
            writer.End();
        }
    }
}
