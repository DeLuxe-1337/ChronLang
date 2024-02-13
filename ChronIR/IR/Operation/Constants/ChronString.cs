using ChronIR.IR.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronIR.IR.Operation.Constants
{
    public class ChronString : ChronExpression, ChronInternalType
    {
        private ChronInvoke invoke;
        private static ChronFunction createString = new(ChronTypes.CreateString, true);
        public ChronString(string text)
        {
            invoke = new(createString);
            invoke.AddParameter(new ChronRawText($"\"{text}\""));
        }
        public object Read(ChronContext context)
        {
            return invoke.Read(context);
        }

        public ChronInternalTypes GetInternalType() => ChronInternalTypes.Object;

        //public void Write(ChronContext context)
        //{
        //    context.writer.Write($"{Read(context)};");
        //}
    }
}
