using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronLessEq : ChronExpression
    {
        private ChronExpression leftExpr;
        private ChronExpression rightExpr;
        public ChronLessEq(ChronExpression leftExpr, ChronExpression rightExpr)
        {
            this.leftExpr = leftExpr;
            this.rightExpr = rightExpr;
        }

        public object Read(ChronContext context)
        {
            return $"{ChronTypes.ObjectCompareLesstEq}({leftExpr.Read(context)},{rightExpr.Read(context)})";
        }
    }
}
