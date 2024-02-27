using System.Text;

namespace ChronIR
{
    internal class Writer
    {
        public StringBuilder StringBuilder { get; set; } = new();
        public string WritePath;
        public Writer(string path = "build.chron.c")
        {
            WritePath = path;
        }
        public void Write(string a) => StringBuilder.Append(a);
        public void Write(object a) => StringBuilder.Append(a);
        public void WriteLine(string a) => StringBuilder.AppendLine(a);
        public void End()
        {

        }
    }
}
