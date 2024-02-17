namespace ChronIR.IR.Internal
{
    public interface ChronExpression
    {
        public object Read(ChronContext context);
        public void WriteRead(ChronContext context)
        {
            context.writer.Write(Read(context));
        }
    }
}
