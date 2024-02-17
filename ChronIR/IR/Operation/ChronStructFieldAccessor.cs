using ChronIR.IR.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronIR.IR.Operation
{
    public class ChronStructFieldAccessor : ChronIndexable
    {
        private ChronInvoke invokeSetter;
        private ChronInvoke invokeGetter;
        private ChronStruct Structure;
        public ChronStructFieldAccessor(string Field, ChronStruct structure)
        {
            Structure = structure;

            invokeSetter = new(
                    new ChronFunction($"_SF_{Structure.Name}_Set{Field}", true)
                    );

            invokeGetter = new(
                    new ChronFunction($"_SF_{Structure.Name}_Get{Field}", true)
                    );
        }

        public override void CreateIndexes(ChronContext context, string prefix)
        {
            throw new NotImplementedException();
        }


        public override object Read(ChronContext context)
        {
            invokeGetter.AddParameter(Structure.Parent);
            return invokeGetter.Read(context);
        }

        public override void SetParent(ChronExpression parent)
        {
            Structure.Parent = parent;
        }

        public override void Write(ChronContext context, ChronExpression value)
        {
            invokeSetter.AddParameter(Structure.Parent);
            invokeSetter.AddParameter(value);
            invokeSetter.Write(context);
        }

        public override void Write(ChronContext context)
        {
            throw new NotImplementedException();
        }
    }
}
