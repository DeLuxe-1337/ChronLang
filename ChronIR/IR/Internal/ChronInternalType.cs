using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronIR.IR.Internal
{
    public enum ChronInternalTypes
    {
        GC_ITEM,
        ObjectRef,
        Object
    }
    public interface ChronInternalType
    {
        public ChronInternalTypes GetInternalType();
    }
}
