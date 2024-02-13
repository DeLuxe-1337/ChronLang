using ChronCompiler;

Builder builder = new Builder();
builder.Create("MyProgram");

builder.CompileChronScript("main");

builder.Build();