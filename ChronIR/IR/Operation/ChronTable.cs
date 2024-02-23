using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronTable : ChronExpression
    {
        private ChronInvoke invoke;
        private static ChronFunction createTable = new(ChronTypes.CreateTable, true);
        public ChronTable()
        {
            invoke = new(createTable);
        }
        public object Read(ChronContext context)
        {
            return invoke.Read(context);
        }
    }
}
