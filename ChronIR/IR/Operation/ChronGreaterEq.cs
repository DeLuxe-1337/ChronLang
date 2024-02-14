using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronGreaterEq : ChronExpression
    {
        private ChronExpression leftExpr;
        private ChronExpression rightExpr;
        public ChronGreaterEq(ChronExpression leftExpr, ChronExpression rightExpr)
        {
            this.leftExpr = leftExpr;
            this.rightExpr = rightExpr;
        }

        public object Read(ChronContext context)
        {
            return $"{ChronTypes.ObjectCompareGrtEq}({leftExpr.Read(context)},{rightExpr.Read(context)})";
        }
    }
}
