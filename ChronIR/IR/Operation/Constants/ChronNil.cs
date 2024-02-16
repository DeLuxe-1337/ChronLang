using ChronIR.IR.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronIR.IR.Operation.Constants
{
    public class ChronNil : ChronExpression, ChronConstant
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
