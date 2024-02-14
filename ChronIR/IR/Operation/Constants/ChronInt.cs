using ChronIR.IR.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronIR.IR.Operation.Constants
{
    public class ChronInt : ChronExpression, ChronConstant
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
