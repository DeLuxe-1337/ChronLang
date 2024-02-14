using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronLessT : ChronExpression
    {
        private ChronExpression leftExpr;
        private ChronExpression rightExpr;
        public ChronLessT(ChronExpression leftExpr, ChronExpression rightExpr)
        {
            this.leftExpr = leftExpr;
            this.rightExpr = rightExpr;
        }

        public object Read(ChronContext context)
        {
            return $"{ChronTypes.ObjectCompareLesst}({leftExpr.Read(context)},{rightExpr.Read(context)})";
        }
    }
}
