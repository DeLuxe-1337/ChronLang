using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronNotEqual : ChronExpression, ChronAutoRelease
    {
        private ChronInvoke invoke;
        private static ChronFunction compareNotEqual = new(ChronTypes.ObjectCompareNEq, true);
        public ChronNotEqual(ChronExpression leftExpr, ChronExpression rightExpr)
        {
            invoke = new(compareNotEqual);
            invoke.AddParameter(leftExpr);
            invoke.AddParameter(rightExpr);
        }

        public object Read(ChronContext context)
        {
            return invoke.Read(context);
        }
    }
}
