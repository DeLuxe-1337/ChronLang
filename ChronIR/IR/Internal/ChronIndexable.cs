using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronIR.IR.Internal
{
    public abstract class ChronIndexable : ChronExpression, ChronStatement
    {
        public abstract void CreateIndexes(ChronContext context, string prefix);
        public abstract object Read(ChronContext context);
        public abstract void SetParent(ChronExpression parent);
        public abstract void Write(ChronContext context, ChronExpression value);
        public abstract void Write(ChronContext context);
    }
}
