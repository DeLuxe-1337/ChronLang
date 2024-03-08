using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronBindExpression : ChronExpression
    {
        public ChronExpression Key;
        public ChronExpression Value;
        public ChronBindExpression(ChronExpression key, ChronExpression value)
        {
            Key = key;
            Value = value;
        }

        public object Read(ChronContext context) => null;
    }
}
