using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronLessEq : ChronExpression, ChronAutoRelease
    {
        private ChronInvoke invoke;
        private static ChronFunction compareLessThanEq = new(ChronTypes.ObjectCompareLesstEq, true);
        public ChronLessEq(ChronExpression leftExpr, ChronExpression rightExpr)
        {
            invoke = new(compareLessThanEq);
            invoke.AddParameter(leftExpr);
            invoke.AddParameter(rightExpr);
        }

        public object Read(ChronContext context)
        {
            return invoke.Read(context);
        }
    }
}
