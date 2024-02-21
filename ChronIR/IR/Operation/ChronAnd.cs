using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronAnd : ChronExpression, ChronAutoRelease
    {
        private ChronInvoke invoke;
        private static ChronFunction compareAnd = new(ChronTypes.ObjectCompareAnd, true);
        public ChronAnd(ChronExpression leftExpr, ChronExpression rightExpr)
        {
            invoke = new(compareAnd);
            invoke.AddParameter(leftExpr);
            invoke.AddParameter(rightExpr);
        }

        public object Read(ChronContext context)
        {
            return invoke.Read(context);
        }
    }
}
