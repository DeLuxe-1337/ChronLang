using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronOr : ChronExpression, ChronAutoRelease
    {
        private ChronInvoke invoke;
        private static ChronFunction compareOr = new(ChronTypes.ObjectCompareOr, true);
        public ChronOr(ChronExpression leftExpr, ChronExpression rightExpr)
        {
            invoke = new(compareOr);
            invoke.AddParameter(leftExpr);
            invoke.AddParameter(rightExpr);
        }

        public object Read(ChronContext context)
        {
            return invoke.Read(context);
        }
    }
}
