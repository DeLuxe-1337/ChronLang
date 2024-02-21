using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronComment : ChronStatement
    {
        private string value;
        public ChronComment(string value)
        {
            this.value = value;
        }
        public void Write(ChronContext context)
        {
            context.writer.WriteLine($"//{value}");
        }
    }
}
