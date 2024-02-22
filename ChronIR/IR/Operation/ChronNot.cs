using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronNot : ChronExpression
    {
        private ChronInvoke invoke;
        private static ChronFunction objectNot = new(ChronTypes.ObjectCompareNot, true);
        public ChronNot(ChronExpression expr)
        {
            invoke = new(objectNot);
            invoke.AddParameter(expr);
        }

        public object Read(ChronContext context)
        {
            return invoke.Read(context);
        }
    }
}
