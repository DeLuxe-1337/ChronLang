using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronTableAccessor : ChronExpression, ChronVariableImpl, ChronAutoRelease
    {
        private ChronExpression table;
        private ChronExpression index;

        private static ChronFunction setDynamicTable = new(ChronTypes.SetDynamicTable, true);
        private static ChronFunction indexDynamicTable = new(ChronTypes.IndexDynamicTable, true);
        private ChronInvoke indexTable;
        private ChronInvoke setTable;
        public ChronTableAccessor(ChronExpression table, ChronExpression index)
        {
            this.table = table;
            this.index = index;
        }

        public object Read(ChronContext context)
        {
            indexTable = new(indexDynamicTable);
            indexTable.AddParameter(table);
            indexTable.AddParameter(index);

            return indexTable.Read(context);
        }

        public void VariableWrite(ChronContext context, ChronExpression value)
        {
            setTable = new(setDynamicTable);
            setTable.AddParameter(table);
            setTable.AddParameter(index);
            setTable.AddParameter(value);

            setTable.Write(context);
        }

        public object VariableRead(ChronContext context)
        {
            return Read(context);
        }

        public void VariableRelease(ChronContext context)
        {

        }
    }
}
