using ChronCompiler;

class Entry
{
    static void Main(string[] args)
    {
        Builder.InitializeTargets();

        Dictionary<string, string> parameters = new() { { "source", "main" }, { "target", "tcc" } };
        foreach (var param in args)
        {
            if (param.StartsWith("--"))
            {
                var split = param.Replace("--", "").Split('=');
                parameters[split[0]] = split[1];
            }
            if (param.EndsWith(".chron"))
            {
                parameters["source"] = param.Replace(".chron", "");
            }
        }

        Builder builder = new Builder();

        if (!parameters.ContainsKey("name"))
            parameters["name"] = parameters["source"];

        builder.Create(parameters["name"]);
        builder.Target = (parameters["target"].ToUpper());

        builder.CompileChronScript(parameters["source"]);

        builder.Build();
    }
}