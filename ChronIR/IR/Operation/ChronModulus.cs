using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronModulus : ChronExpression
    {
        private ChronInvoke invoke;
        private static ChronFunction binaryAdd = new(ChronTypes.ObjectModulus, true);
        public ChronModulus(ChronExpression leftExpr, ChronExpression rightExpr)
        {
            invoke = new(binaryAdd);
            invoke.AddParameter(leftExpr);
            invoke.AddParameter(rightExpr);
        }

        public object Read(ChronContext context)
        {
            return invoke.Read(context);
        }
    }
}
