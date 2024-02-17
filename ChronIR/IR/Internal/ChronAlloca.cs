using ChronIR.IR.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronIR.IR.Internal
{
    public interface ChronAlloca
    {
        public ChronFunction GetAllocateFunction();
    }
}
