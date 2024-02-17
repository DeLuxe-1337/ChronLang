using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronDiv : ChronExpression
    {
        private ChronInvoke invoke;
        private static ChronFunction binaryDiv = new(ChronTypes.ObjectDiv, true);
        public ChronDiv(ChronExpression leftExpr, ChronExpression rightExpr)
        {
            invoke = new(binaryDiv);
            invoke.AddParameter(leftExpr);
            invoke.AddParameter(rightExpr);
        }

        public object Read(ChronContext context)
        {
            return invoke.Read(context);
        }
    }
}
