using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronLessT : ChronExpression
    {
        private ChronInvoke invoke;
        private static ChronFunction compareLessThan = new(ChronTypes.ObjectCompareLesst, true);
        public ChronLessT(ChronExpression leftExpr, ChronExpression rightExpr)
        {
            invoke = new(compareLessThan);
            invoke.AddParameter(leftExpr);
            invoke.AddParameter(rightExpr);
        }

        public object Read(ChronContext context)
        {
            return invoke.Read(context);
        }
    }
}
