namespace ChronIR.IR.Internal
{
    public class ChronConditionalAutoRelease : ChronAutoRelease
    {
        public virtual bool CanAutoRelease(ChronContext context) { return true; }
    }
}