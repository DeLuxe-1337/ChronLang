using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronExpressionBlock
    {
        private List<ChronExpression> Block = new();
        public void AddExpression(ChronExpression stmt) => Block.Add(stmt);
        public bool HasAnyExpressions() => Block.Any();
        public List<ChronExpression> GetParameters() => Block;
    }
}
