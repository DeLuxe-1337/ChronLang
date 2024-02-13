using ChronIR;
using ChronIR.IR.Internal;
using ChronIR.IR.Operation;
using ChronIR.IR.Operation.Constants;

ChronContext moduleContext = new("MyProgram");
ChronModule module = new(moduleContext);
module.SetupChronRuntime();

ChronFunction Print = new("Print", true);

ChronFunction printLine = new("Println");
{ // printLine function

    printLine.AddParameter("message");

    {
        var printCall = new ChronInvoke(Print);
        printCall.AddParameter(printLine.GetParameter(0));
        printLine.Block.AddStatement(printCall);
    }

    {
        var printCall = new ChronInvoke(Print);
        printCall.AddParameter(new ChronString("\\n"));
        printLine.Block.AddStatement(printCall);
    }

    module.AddStatement(printLine);
}

ChronFunction mainFunc = new("main");
{ // Main function

    var printCall = new ChronInvoke(printLine);
    mainFunc.Block.AddStatement(new ChronVariable("myMessage", new ChronString("Hello, world")));
    printCall.AddParameter(new ChronEnvironmentAccessor("myMessage"));

    mainFunc.Block.AddStatement(printCall);

    module.AddStatement(mainFunc);
}

module.AddStatement(new ChronRawText("void main() { _F_main(); }"));

module.Write();
Console.WriteLine($"\t------>\tCompiling and executing\t<------");
module.Source(ChronModuleCompile.Compile);