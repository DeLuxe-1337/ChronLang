using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronOr : ChronExpression
    {
        private ChronExpression leftExpr;
        private ChronExpression rightExpr;
        public ChronOr(ChronExpression leftExpr, ChronExpression rightExpr)
        {
            this.leftExpr = leftExpr;
            this.rightExpr = rightExpr;
        }

        public object Read(ChronContext context)
        {
            return $"{ChronTypes.ObjectCompareOr}({leftExpr.Read(context)},{rightExpr.Read(context)})";
        }
    }
}
