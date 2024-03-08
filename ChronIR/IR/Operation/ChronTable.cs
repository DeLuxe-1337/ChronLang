using ChronIR.IR.Internal;
using ChronIR.IR.Operation.Constants;

namespace ChronIR.IR.Operation
{
    public class ChronTable : ChronExpression, ChronVariableRef
    {
        private ChronInvoke invoke;
        private static ChronFunction createTable = new(ChronTypes.CreateTable, true);
        private List<ChronExpression> initValues;
        public void AddInitialValue(ChronExpression value) => initValues.Add(value);
        public ChronTable()
        {
            initValues = new();
            invoke = new(createTable);
        }
        public object Read(ChronContext context)
        {
            return invoke.Read(context);
        }

        public void VariableCreatedRef(ChronContext context, ChronExpression variable)
        {
            for (int i = 0; i < initValues.Count; i++)
            {
                var value = initValues[i];

                if(value is ChronBindExpression bind)
                {
                    var setter = new ChronTableAccessor(variable, bind.Key);
                    setter.VariableWrite(context, bind.Value);
                }
                else
                {
                    var setter = new ChronTableAccessor(variable, new ChronInt(i));
                    setter.VariableWrite(context, value);
                }
            }
        }
    }
}
