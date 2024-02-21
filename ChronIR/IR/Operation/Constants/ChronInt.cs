using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation.Constants
{
    public class ChronInt : ChronExpression, ChronConstant, ChronAutoRelease
    {
        private ChronInvoke invoke;
        private static ChronFunction createInteger = new(ChronTypes.CreateInt, true);
        public ChronInt(int number)
        {
            invoke = new(createInteger);
            invoke.AddParameter(new ChronRawText(number.ToString()));
        }
        public object Read(ChronContext context)
        {
            return invoke.Read(context);
        }
    }
}
