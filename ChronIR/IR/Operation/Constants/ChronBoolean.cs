using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation.Constants
{
    public class ChronBoolean : ChronExpression, ChronConstant, ChronAutoRelease
    {
        private ChronInvoke invoke;
        private static ChronFunction creqateBoolean = new(ChronTypes.CreateBoolean, true);
        public ChronBoolean(bool value)
        {
            invoke = new(creqateBoolean);
            invoke.AddParameter(new ChronRawText(value.ToString().ToLower()));
        }
        public object Read(ChronContext context)
        {
            return invoke.Read(context);
        }
    }
}
