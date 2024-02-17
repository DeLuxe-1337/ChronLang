using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronGreaterT : ChronExpression
    {
        private ChronInvoke invoke;
        private static ChronFunction compareGreaterT = new(ChronTypes.ObjectCompareGrt, true);
        public ChronGreaterT(ChronExpression leftExpr, ChronExpression rightExpr)
        {
            invoke = new(compareGreaterT);
            invoke.AddParameter(leftExpr);
            invoke.AddParameter(rightExpr);
        }

        public object Read(ChronContext context)
        {
            return invoke.Read(context);
        }
    }
}
