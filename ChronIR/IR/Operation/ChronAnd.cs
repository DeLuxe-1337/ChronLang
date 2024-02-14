using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronAnd : ChronExpression
    {
        private ChronExpression leftExpr;
        private ChronExpression rightExpr;
        public ChronAnd(ChronExpression leftExpr, ChronExpression rightExpr)
        {
            this.leftExpr = leftExpr;
            this.rightExpr = rightExpr;
        }

        public object Read(ChronContext context)
        {
            return $"{ChronTypes.ObjectCompareAnd}({leftExpr.Read(context)},{rightExpr.Read(context)})";
        }
    }
}
