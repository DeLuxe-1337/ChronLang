using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronRawText : ChronStatement, ChronExpression
    {
        public string Text = string.Empty;
        public ChronRawText(string text)
        {
            Text = text;
        }

        public object Read(ChronContext context)
        {
            return Text;
        }

        public void Write(ChronContext context)
        {
            context.writer.WriteLine(Text);
        }
    }
}
