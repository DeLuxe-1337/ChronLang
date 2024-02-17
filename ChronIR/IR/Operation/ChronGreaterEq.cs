using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronGreaterEq : ChronExpression
    {
        private ChronInvoke invoke;
        private static ChronFunction compareGreaterEq = new(ChronTypes.ObjectCompareGrtEq, true);
        public ChronGreaterEq(ChronExpression leftExpr, ChronExpression rightExpr)
        {
            invoke = new(compareGreaterEq);
            invoke.AddParameter(leftExpr);
            invoke.AddParameter(rightExpr);
        }

        public object Read(ChronContext context)
        {
            return invoke.Read(context);
        }
    }
}
