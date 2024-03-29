﻿using ChronIR.IR.Internal;
using ChronIR.IR.Operation;

namespace ChronIR
{
    public enum ChronModuleCompile
    {
        Compile
    }
    public class ChronModule
    {
        private static Dictionary<string, string> StaticallyLink = new();
        public static void AddStaticLink(string link) => StaticallyLink[link] = $"{link}";
        private ChronContext CurrentContext;
        private ChronContext Context;
        private List<ChronStatement> Statements = new();
        public ChronContext GetContext() => CurrentContext;
        public Dictionary<string, string> GetStaticLink() => StaticallyLink;
        public ChronModule(ChronContext context)
        {
            Context = context;

            CurrentContext = context;
            Initialize();
        }

        public void DefineCompilerInfo(string name, string value)
        {
            CurrentContext.writer.WriteLine($"#define {name} {value}");
        }
        public void DefineInclusion(string name)
        {
            CurrentContext.writer.WriteLine($"#include \"{name}\"");
        }
        public void SetupChronRuntime()
        {
            DefineInclusion("Backend/include.h");
        }
        internal void Initialize()
        {
            CurrentContext.env.AddScope(new("Global"));

            CurrentContext.writer = new Writer($"{CurrentContext.Name}.chron.c");

            CurrentContext.writer.WriteLine($"/*\n\n\tThis is a ChronScript module generated by ChronIR(0.1)\n" +
                $"\t->\tModule Name: {CurrentContext.Name}\n" +
                $"\t->\tBuild Mode: {CurrentContext.BuildMode} \n" +
                $"\n*/");
        }
        public void AddStatement(ChronStatement statement) => Statements.Add(statement);
        public void Write()
        {
            try
            {
                //Forward declare all functions
                foreach (var statement in Statements)
                {
                    if (statement is ChronStatementBlock block)
                    {
                        foreach (var stmt in block.block)
                            if (stmt is ChronFunction func)
                                func.ForwardDeclare(CurrentContext);
                    }
                }

                foreach (var statement in Statements)
                {
                    statement.Write(CurrentContext);
                }
                CurrentContext.End();
            }
            catch
            {
                CurrentContext.End();
            }
        }
    }
}
