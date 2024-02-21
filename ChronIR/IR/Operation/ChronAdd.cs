using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronAdd : ChronExpression, ChronAutoRelease
    {
        private ChronInvoke invoke;
        private static ChronFunction binaryAdd = new(ChronTypes.ObjectAdd, true);
        public ChronAdd(ChronExpression leftExpr, ChronExpression rightExpr)
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
