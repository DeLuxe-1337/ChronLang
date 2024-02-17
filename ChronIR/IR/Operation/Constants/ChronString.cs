﻿using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation.Constants
{
    public class ChronString : ChronExpression, ChronConstant
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
    }
}
