using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronGreaterT : ChronExpression
    {
        private ChronExpression leftExpr;
        private ChronExpression rightExpr;
        public ChronGreaterT(ChronExpression leftExpr, ChronExpression rightExpr)
        {
            this.leftExpr = leftExpr;
            this.rightExpr = rightExpr;
        }

        public object Read(ChronContext context)
        {
            return $"{ChronTypes.ObjectCompareGrt}({leftExpr.Read(context)},{rightExpr.Read(context)})";
        }
    }
}
