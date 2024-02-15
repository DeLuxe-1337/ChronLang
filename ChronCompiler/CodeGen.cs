using Antlr4.Runtime.Misc;
using ChronIR;
using ChronIR.IR.Internal;
using ChronIR.IR.Operation;
using ChronIR.IR.Operation.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronCompiler
{
    public class CodeGen : ChronBaseVisitor<object>
    {
        private string ModuleName;
        private ChronContext Context;
        private ChronModule Module;
        private Builder Builder;
        public Stack<ChronStatementBlock> BlockStack = new();
        public CodeGen(string moduleName, Builder builder)
        {
            this.Builder = builder;

            this.ModuleName = moduleName;
            this.Context = builder.GetCtx();
            this.Module = builder.GetModule();
            BlockStack.Push(builder.Root);
        }
        public override object VisitDefer([NotNull] ChronParser.DeferContext context)
        {
            Visit(context.statement());
            BlockStack.Peek().AddStatement(new ChronDeferStatement(
                    BlockStack.Peek().PopStatement()
                ));

            return null;
        }
        public override object VisitContinue([NotNull] ChronParser.ContinueContext context)
        {
            BlockStack.Peek().AddStatement(new ChronContinue());

            return null;
        }
        public override object VisitBreak([NotNull] ChronParser.BreakContext context)
        {
            BlockStack.Peek().AddStatement(new ChronBreak());

            return null;
        }
        public override object VisitWhile([NotNull] ChronParser.WhileContext context)
        {
            BlockStack.Peek().AddStatement(new ChronWhileLoop(Visit(context.expression()) as ChronExpression, Visit(context.block()) as ChronStatementBlock));

            return null;
        }
        public override object VisitIf([NotNull] ChronParser.IfContext context)
        {
            ChronStatementBlock? falseBlock = default;

            if(context.ifElse() != null)
            {
                falseBlock = Visit(context.ifElse().block()) as ChronStatementBlock;
            }

            BlockStack.Peek().AddStatement(new ChronIf(Visit(context.expression()) as ChronExpression, Visit(context.block()) as ChronStatementBlock, falseBlock));

            return null;
        }
        public override object VisitRelease([NotNull] ChronParser.ReleaseContext context)
        {
            BlockStack.Peek().AddStatement(
                new ChronRelease(Visit(context.expression()) as ChronExpression)
                );

            return null;
        }
        public override object VisitReleaseExpr([NotNull] ChronParser.ReleaseExprContext context)
        {
            return new ChronRelease(Visit(context.expression()) as ChronExpression);
        }
        public override object VisitImport_function([NotNull] ChronParser.Import_functionContext context)
        {
            var Import = new ChronImport(context.IDENTIFIER().GetText());

            foreach(var p in context.import_functionParameters().IDENTIFIER())
            {
                Import.AddParameter(ChronTypes.TypeMap[p.GetText()]);
            }

            BlockStack.Peek().AddStatement(
                Import
                );

            return null;
        }
        public override object VisitImport_stmt([NotNull] ChronParser.Import_stmtContext context)
        {
            if (Visit(context.import_block()) is ChronStatementBlock block)
                BlockStack.Peek().AddStatement(block);

            return null;
        }
        public override object VisitVariable([NotNull] ChronParser.VariableContext context)
        {
            BlockStack.Peek().AddStatement(
                new ChronVariable(
                    context.IDENTIFIER().GetText(), 
                    Visit(context.expression()) as ChronExpression)
                );

            return null;
        }
        public override object VisitReturn([NotNull] ChronParser.ReturnContext context)
        {
            BlockStack.Peek().AddStatement(
                new ChronReturn(
                    Visit(context.expression()) as ChronExpression
                    )
                );

            return null;
        }
        public override object VisitInclude_module([NotNull] ChronParser.Include_moduleContext context)
        {
            Builder.CompileChronScript(context.IDENTIFIER().GetText());

            return null;
        }
        public override object VisitBlock([NotNull] ChronParser.BlockContext context)
        {
            BlockStack.Push(new());

            VisitChildren(context);

            return BlockStack.Pop();
        }
        public override object VisitConstant([NotNull] ChronParser.ConstantContext context)
        {
            if(context.STRING() != null)
            {
                return new ChronString(context.STRING().GetText().TrimStart('"').TrimEnd('"'));
            }

            if(context.NUMBER() != null)
            {
                return new ChronInt(int.Parse(context.NUMBER().GetText()));
            }

            if(context.BOOLEAN() != null)
            {
                return new ChronBoolean(context.BOOLEAN().GetText() == "true" ? true : false);
            }

            return base.VisitConstant(context);
        }
        public override object VisitBinaryExpr([NotNull] ChronParser.BinaryExprContext context)
        {
            var left = Visit(context.expression().First()) as ChronExpression;
            var right = Visit(context.expression()[1]) as ChronExpression;
            switch (context.op.Text)
            {
                case "+":
                    return new ChronAdd(left, right);
                case "-":
                    return new ChronSub(left, right);
                case "*":
                    return new ChronMul(left, right);
                case "/":
                    return new ChronDiv(left, right);
            }

            return base.VisitBinaryExpr(context);
        }
        public override object VisitComparatorExpr([NotNull] ChronParser.ComparatorExprContext context)
        {
            var left = Visit(context.expression().First()) as ChronExpression; 
            var right = Visit(context.expression()[1]) as ChronExpression;
            switch (context.op.Text)
            {
                case "==":
                    return new ChronEqual(left, right);
                case "!=":
                    return new ChronNotEqual(left, right);
                case "&&":
                case "and":
                    return new ChronAnd(left, right);
                case "||":
                case "or":
                    return new ChronOr(left, right);
                case ">":
                    return new ChronGreaterT(left, right);
                case ">=":
                    return new ChronGreaterEq(left, right);
                case "<":
                    return new ChronLessT(left, right);
                case "<=":
                    return new ChronLessEq(left, right);
            }

            return base.VisitComparatorExpr(context);
        }
        public override object VisitEvaluateExpr([NotNull] ChronParser.EvaluateExprContext context)
        {
            return Visit(context.expression()) as ChronExpression;
        }
        public override object VisitIDENTIFIERExpr([NotNull] ChronParser.IDENTIFIERExprContext context)
        {
            return new ChronEnvironmentAccessor(context.IDENTIFIER().GetText());
        }
        private ChronInvoke GenerateCall([NotNull] ChronParser.CallContext context)
        {
            string callee = context.IDENTIFIER().GetText();
            ChronInvoke invoke = new(new ChronInvokableAccessor(callee, context.callArgs().expression().Length));
            foreach (var expr in context.callArgs().expression())
            {
                invoke.AddParameter(Visit(expr) as ChronExpression);
            }

            return invoke;
        }
        public override object VisitCall([NotNull] ChronParser.CallContext context)
        {
            BlockStack.Peek().AddStatement(GenerateCall(context));

            return null;
        }
        public override object VisitCallExpr([NotNull] ChronParser.CallExprContext context)
        {
            return GenerateCall(context.call());
        }
        public override object VisitForeign_c([NotNull] ChronParser.Foreign_cContext context)
        {
            BlockStack.Peek().AddStatement(new ChronRawText(context.TICK_BLOCK().GetText().TrimStart('`').TrimEnd('`')));

            return null;
        }
        public override object VisitFunction([NotNull] ChronParser.FunctionContext context)
        {
            string FunctionName = context.IDENTIFIER().GetText();

            ChronFunction function = new(FunctionName, context.functionForceName() != null);
            function.SetGc(false);

            if (context.functionBlock() != null && Visit(context.functionBlock()) is ChronStatementBlock block)
                function.Block = block;

            if(context.functionParameters() != null)
            {
                foreach(var p in context.functionParameters().IDENTIFIER())
                {
                    function.AddParameter(p.GetText());
                }
            }

            if (context.functionForceGc() != null)
                function.SetGc(true);

            if (context.functionForceReturn() != null)
                function.SetReturn(true);

            if(context.functionRename() != null)
            {
                function.ScopeName = context.functionRename().IDENTIFIER().GetText();
            }

            BlockStack.Peek().AddStatement(function);

            return null;
        }
    }
}
