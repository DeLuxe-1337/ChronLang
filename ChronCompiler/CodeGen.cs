﻿using Antlr4.Runtime.Misc;
using ChronIR;
using ChronIR.IR.Internal;
using ChronIR.IR.Operation;
using ChronIR.IR.Operation.Constants;

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
        public override object VisitForeach([NotNull] ChronParser.ForeachContext context)
        {
            BlockStack.Peek().AddStatement(new ChronForEach(context.index.Text, context.value.Text, Visit(context.block()) as ChronStatementBlock, Visit(context.iter) as ChronExpression));

            return null;
        }
        public override object VisitLinkStatic([NotNull] ChronParser.LinkStaticContext context)
        {
            switch (Builder.SelectedTarget)
            {
                case "TCC":
                    {
                        Console.WriteLine("TCC not supported for static linking! Switching target to CLANG!");
                        Builder.SelectedTarget = "CLANG";
                        break;
                    }
            }

            ChronModule.AddStaticLink(context.STRING().GetText().Trim('"'));

            return null;
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
        public override object VisitFor([NotNull] ChronParser.ForContext context)
        {
            var start = Visit(context.start) as ChronExpression;
            var end = Visit(context.end) as ChronExpression;

            BlockStack.Peek().AddStatement(new ChronForTo(context.IDENTIFIER().GetText(), Visit(context.block()) as ChronStatementBlock, start, end));

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

            if (context.ifElse() != null)
            {
                falseBlock = Visit(context.ifElse().block()) as ChronStatementBlock;
            }

            BlockStack.Peek().AddStatement(new ChronIf(Visit(context.expression()) as ChronExpression, Visit(context.block()) as ChronStatementBlock, falseBlock));

            return null;
        }
        public override object VisitRelease([NotNull] ChronParser.ReleaseContext context)
        {
            foreach(var expression in context.expression())
            {
                BlockStack.Peek().AddStatement(
                new ChronRelease(Visit(expression) as ChronExpression)
                );
            }

            return null;
        }
        public override object VisitReleaseExpr([NotNull] ChronParser.ReleaseExprContext context)
        {
            return new ChronRelease(Visit(context.expression()) as ChronExpression);
        }
        public override object VisitImport_function([NotNull] ChronParser.Import_functionContext context)
        {
            var Import = new ChronImport(context.IDENTIFIER().GetText());

            foreach (var p in context.import_functionParameters().IDENTIFIER())
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
            var identifier = Visit(context.expression(0)) as ChronExpression;
            var value = Visit(context.expression(1)) as ChronExpression;

            switch (context.op.Text)
            {
                case "+=":
                    value = new ChronAdd(identifier, value);
                    break;
                case "-=":
                    value = new ChronSub(identifier, value);
                    break;
                case "*=":
                    value = new ChronMul(identifier, value);
                    break;
                case "/=":
                    value = new ChronDiv(identifier, value);
                    break;
                case "%=":
                    value = new ChronModulus(identifier, value);
                    break;
            }

            var variable = new ChronVariable(
                    identifier,
                    value);

            if(context.modifiers() != null)
            {
                var modifiers = Visit(context.modifiers()) as Dictionary<string, string>;

                foreach(var modifier in modifiers)
                {
                    switch(modifier.Key)
                    {
                        case "type":
                            {
                                variable.DefaultType = modifier.Value;
                                break;
                            }
                    }
                }
            }

            BlockStack.Peek().AddStatement(variable);

            return null;
        }
        public override object VisitBindExpr([NotNull] ChronParser.BindExprContext context)
        {
            return new ChronBindExpression(Visit(context.expression(0)) as ChronExpression, Visit(context.expression(1)) as ChronExpression);
        }
        public override object VisitTableExpr([NotNull] ChronParser.TableExprContext context)
        {
            var table = new ChronTable();

            if (context.expression() != null)
            {
                foreach (var value in context.expression())
                {
                    if (Visit(value) is ChronExpression expr)
                        table.AddInitialValue(expr);
                }
            }

            return table;
        }
        public override object VisitTableIndexExpr([NotNull] ChronParser.TableIndexExprContext context)
        {
            var table = Visit(context.expression(0)) as ChronExpression;
            var index = Visit(context.expression(1)) as ChronExpression;

            return new ChronTableAccessor(table, index);
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
            if (context.STRING() != null)
            {
                var str = context.STRING().GetText();
                str = str.Substring(1);
                str = str.Substring(0, str.Length - 1);
                return new ChronString(str);
            }

            if (context.NUMBER() != null)
            {
                return new ChronInt(int.Parse(context.NUMBER().GetText()));
            }

            if (context.BOOLEAN() != null)
            {
                return new ChronBoolean(context.BOOLEAN().GetText() == "true" ? true : false);
            }

            if (context.NIL() != null)
            {
                return new ChronNil();
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
                case "%":
                    return new ChronModulus(left, right);
            }

            return base.VisitBinaryExpr(context);
        }
        public override object VisitNotExpr([NotNull] ChronParser.NotExprContext context)
        {
            return new ChronNot(Visit(context.expression()) as ChronExpression);
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
        private ChronInvoke GenerateCall(ChronParser.CallInvokeContext callInvoke, [NotNull] ChronParser.ExpressionContext expression, [NotNull]ChronParser.CallArgsContext args)
        {
            ChronExpression callee = Visit(expression) as ChronExpression;

            if (callee is ChronEnvironmentAccessor accessor)
                    callee = new ChronEnvironmentAccessor(accessor.Value, args.expression().Length);

            if (callee is ChronInvokable inv)
            {
                ChronInvoke invoke = new(inv);

                if(callInvoke != null)
                {
                    invoke.RuntimeInvoke.Item1 = true;
                    invoke.RuntimeInvoke.Item2 = new ChronEnvironmentAccessor(callInvoke.IDENTIFIER().GetText());
                }

                foreach (var expr in args.expression())
                {
                    invoke.AddParameter(Visit(expr) as ChronExpression);
                }

                return invoke;
            }
            return null;
        }
        public override object VisitCallInvokeExpr([NotNull] ChronParser.CallInvokeExprContext context)
        {
            return GenerateCall(context.callInvoke(), context.expression(), context.callArgs());
        }
        public override object VisitCall([NotNull] ChronParser.CallContext context)
        {
            BlockStack.Peek().AddStatement(GenerateCall(context.callInvoke(), context.expression(), context.callArgs()));

            return null;
        }
        public override object VisitCallExpr([NotNull] ChronParser.CallExprContext context)
        {
            return GenerateCall(null, context.expression(), context.callArgs());
        }
        public override object VisitForeign_c([NotNull] ChronParser.Foreign_cContext context)
        {
            BlockStack.Peek().AddStatement(new ChronRawText(context.TICK_BLOCK().GetText().TrimStart('`').TrimEnd('`')));

            return null;
        }
        public override object VisitModifiers([NotNull] ChronParser.ModifiersContext context)
        {
            Dictionary<string, string> modifiers = new();

            foreach(var modifier in context.modifier())
            {
                modifiers.Add(modifier.STRING(0).GetText().TrimStart('"').TrimEnd('"'), modifier.STRING(1) != null ? modifier.STRING(1).GetText().TrimStart('"').TrimEnd('"') : string.Empty);
            }

            return modifiers;
        }
        private void ApplyFunctionModifiers(Dictionary<string, string> modifiers, ChronInvokable func)
        {
            foreach (var modifier in modifiers)
            {
                string value = modifier.Value;

                switch (modifier.Key)
                {
                    case "parameters":
                        {
                            if (func is ChronNativeFunction function)
                            {
                                function.SetParameters(value);
                            }
                            break;
                        }
                    case "inline":
                        {
                            {
                                if (func is ChronFunction function)
                                    function.Inline = true;
                            }
                            {
                                if (func is ChronNativeFunction function)
                                    function.Inline = true;
                            }
                            break;
                        }
                    case "extern":
                        {
                            {
                                if (func is ChronFunction function)
                                    function.External = true;
                            }
                            {
                                if (func is ChronNativeFunction function)
                                    function.External = true;
                            }
                            break;
                        }
                    case "return":
                        {
                            {
                                if (func is ChronFunction function)
                                    function.SetReturn(true);
                            }
                            {
                                if (func is ChronNativeFunction function)
                                    function.SetReturn(value);
                            }
                            break;
                        }
                    case "name":
                        {
                            {
                                if (func is ChronFunction function)
                                    function.ScopeName = value;
                            }
                            {
                                if (func is ChronNativeFunction function)
                                    function.ScopeName = value;
                            }
                            break;
                        }
                }
            }
        }
        private ChronFunction GenerateFunction(ChronParser.FunctionContext context)
        {
            string FunctionName = context.IDENTIFIER().GetText();

            ChronFunction function = new(FunctionName, context.functionForceName() != null);

            if (context.functionBlock() != null && Visit(context.functionBlock()) is ChronStatementBlock block)
                function.Block = block;

            if (context.functionParameters() != null)
            {
                foreach (var p in context.functionParameters().IDENTIFIER())
                {
                    function.AddParameter(p.GetText());
                }
            }

            if(context.modifiers() != null)
            ApplyFunctionModifiers(Visit(context.modifiers()) as Dictionary<string, string>, function);

            return function;
        }
        private ChronNativeFunction GenerateNativeFunction(ChronParser.FunctionContext context)
        {
            string FunctionName = context.IDENTIFIER().GetText();

            ChronNativeFunction function = new(FunctionName);

            if (context.functionBlock() != null && Visit(context.functionBlock()) is ChronStatementBlock block)
                function.Block = block;

            if (context.functionParameters() != null)
            {
                foreach (var p in context.functionParameters().IDENTIFIER())
                {
                    function.AddParameter(p.GetText());
                }
            }

            if (context.modifiers() != null)
                ApplyFunctionModifiers(Visit(context.modifiers()) as Dictionary<string, string>, function);

            return function;
        }
        public override object VisitFunction([NotNull] ChronParser.FunctionContext context)
        {
            if(context.modifiers() != null)
            {
                var modifiers = Visit(context.modifiers()) as Dictionary<string, string>;
                foreach (var modifier in modifiers)
                {
                    if (modifier.Key == "native")
                    {
                        BlockStack.Peek().AddStatement(GenerateNativeFunction(context));
                        return null;
                    }
                }
            }

            BlockStack.Peek().AddStatement(GenerateFunction(context));

            return null;
        }
    }
}
