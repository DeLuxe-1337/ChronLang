using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronEqual : ChronExpression, ChronAutoRelease
    {
        private ChronInvoke invoke;
        private static ChronFunction compareEq = new(ChronTypes.ObjectCompareEq, true);
        public ChronEqual(ChronExpression leftExpr, ChronExpression rightExpr)
        {
            invoke = new(compareEq);
            invoke.AddParameter(leftExpr);
            invoke.AddParameter(rightExpr);
        }

        public object Read(ChronContext context)
        {
            return invoke.Read(context);
        }
    }
}
