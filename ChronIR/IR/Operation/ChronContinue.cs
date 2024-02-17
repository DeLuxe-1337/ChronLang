using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronContinue : ChronStatement
    {
        public void Write(ChronContext context)
        {
            context.writer.WriteLine("continue;");
        }
    }
}
