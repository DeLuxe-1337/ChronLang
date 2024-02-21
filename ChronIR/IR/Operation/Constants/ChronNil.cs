using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation.Constants
{
    public class ChronNil : ChronExpression, ChronConstant, ChronAutoRelease
    {
        private ChronInvoke invoke;
        private static ChronFunction createNil = new(ChronTypes.CreateNil, true);
        public ChronNil()
        {
            invoke = new(createNil);
        }
        public object Read(ChronContext context)
        {
            return invoke.Read(context);
        }
    }
}
