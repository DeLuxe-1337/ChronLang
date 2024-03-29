﻿using ChronIR.IR.Internal;

namespace ChronIR.IR.Operation
{
    public class ChronStatementBlock : ChronStatement
    {
        internal List<ChronStatement> block = new();
        public void AddStatement(ChronStatement stmt) => block.Add(stmt);
        public void PrependStatement(ChronStatement stmt) => block.Insert(0, stmt);
        public ChronStatement PopStatement()
        {
            var stmt = block[block.Count - 1];
            block.Remove(stmt);
            return stmt;
        }
        public bool HasAnyStatements() => block.Any();
        public bool HasStatement<T>() where T : ChronStatement => block.Any((statement) => statement.GetType() == typeof(T));

        public void Write(ChronContext context)
        {
            foreach (ChronStatement stmt in block)
                stmt.Write(context);
        }
    }
}
