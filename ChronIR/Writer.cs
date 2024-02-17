namespace ChronIR
{
    internal class Writer : StreamWriter
    {
        public string WritePath;
        public Writer(string path = "build.chron.c") : base(path)
        {
            WritePath = path;
        }

        public void End()
        {
            this.Close();
            this.Dispose();
        }
    }
}
