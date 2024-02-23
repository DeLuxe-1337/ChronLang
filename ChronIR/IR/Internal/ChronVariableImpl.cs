namespace ChronIR.IR.Internal
{
    public interface ChronVariableImpl
    {
        public void VariableWrite(ChronContext context, ChronExpression value);
        public object VariableRead(ChronContext context);
        public void VariableRelease(ChronContext context);
    }
}
