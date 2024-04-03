using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronBreak : ChronStatement
    {
        public void Write(ChronContext context)
        {
            ChronDefer.VisitCurrentScope(context);

            context.writer.WriteLine("break;");
        }
    }
}
