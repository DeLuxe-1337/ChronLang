using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronMul : ChronExpression, ChronAutoRelease
    {
        private ChronInvoke invoke;
        private static ChronFunction binaryMul = new(ChronTypes.ObjectMul, true);
        public ChronMul(ChronExpression leftExpr, ChronExpression rightExpr)
        {
            invoke = new(binaryMul);
            invoke.AddParameter(leftExpr);
            invoke.AddParameter(rightExpr);
        }

        public object Read(ChronContext context)
        {
            return invoke.Read(context);
        }
    }
}
