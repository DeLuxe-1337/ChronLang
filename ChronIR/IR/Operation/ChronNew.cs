using ChronIR.IR.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronIR.IR.Operation
{
    public class ChronNew : ChronIndexable
    {
        private ChronInvoke invoke;
        private ChronExpression allocatedObject;
        public ChronNew(ChronExpression allocateObject)
        {
            this.allocatedObject = allocateObject;
        }
        private void SetInvoke(ChronContext context)
        {
            if (allocatedObject is ChronEnvironmentAccessor accessor)
            {
                if (accessor.GetObject(context) is ChronAlloca alloca)
                {
                    invoke = new(alloca.GetAllocateFunction());
                }
            }
        }
        public override void CreateIndexes(ChronContext context, string prefix)
        {
            if (allocatedObject is ChronEnvironmentAccessor accessor)
            {
                if (accessor.GetObject(context) is ChronIndexable index)
                {
                    index.CreateIndexes(context, prefix);
                }
            }
        }

        public override object Read(ChronContext context)
        {
            SetInvoke(context);
            return invoke.Read(context);
        }

        public override void SetParent(ChronExpression parent)
        {
            throw new NotImplementedException();
        }

        public override void Write(ChronContext context, ChronExpression value)
        {
            throw new NotImplementedException();
        }

        public override void Write(ChronContext context)
        {
            throw new NotImplementedException();
        }
    }
}
