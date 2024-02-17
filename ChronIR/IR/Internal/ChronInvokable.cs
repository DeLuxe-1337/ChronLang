namespace ChronIR.IR.Internal
{
    public interface ChronInvokable
    {
        public string GetName(ChronContext context);
        public int ParameterCount();
    }
}
