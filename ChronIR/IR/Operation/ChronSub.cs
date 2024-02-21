using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronSub : ChronExpression, ChronAutoRelease
    {
        private ChronInvoke invoke;
        private static ChronFunction binarySub = new(ChronTypes.ObjectSub, true);
        public ChronSub(ChronExpression leftExpr, ChronExpression rightExpr)
        {
            invoke = new(binarySub);
            invoke.AddParameter(leftExpr);
            invoke.AddParameter(rightExpr);
        }

        public object Read(ChronContext context)
        {
            return invoke.Read(context);
        }
    }
}
