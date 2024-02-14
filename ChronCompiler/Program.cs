using ChronCompiler;

class Entry
{
    static void Main(string[] args)
    {
        Dictionary<string, string> parameters = new() { { "source", "main.chron" }, { "name", "MyProgram" } };
        foreach (var param in args)
        {
            if (param.StartsWith("--"))
            {
                var split = param.Replace("--", "").Split('=');
                parameters[split[0]] = split[1];
            }
            if (param.EndsWith(".chron"))
            {
                parameters["source"] = param;
            }
        }

        Builder builder = new Builder();
        builder.Create(parameters["name"]);

        builder.CompileChronScript(parameters["source"].Replace(".chron", ""));

        builder.Build();
    }
}