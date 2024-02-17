using ChronIR.IR.Internal;
using System;

namespace ChronIR.IR.Operation
{
    public class ChronStruct : ChronIndexable, ChronAlloca
    {
        public string Name;
        public string RawCName;
        public List<string> Fields = new();
        public void AddField(string fieldName) =>   Fields.Add(fieldName);
        public ChronStruct(string name) { this.Name = name; }
        public ChronExpression Parent;
        private void GenerateSetField(ChronContext context, string fieldName) {
            context.writer.WriteLine($"{ChronTypes.TypeMap["void"]} _SF_{Name}_Set{fieldName}({RawCName}* this, {ChronTypes.TypeMap["object"]} value) {{ this->{fieldName} = value; }}");
        }
        private void GenerateGetField(ChronContext context, string fieldName) {
            context.writer.WriteLine($"{ChronTypes.TypeMap["object"]} _SF_{Name}_Get{fieldName}({RawCName}* this) {{ return this->{fieldName}; }}");
        }
        private void GenerateNewAlloca(ChronContext context)
        {
            context.writer.WriteLine($"{RawCName}* _new_{Name}() {{\n{RawCName}* x = {ChronTypes.GCMalloc}(sizeof({RawCName}));");
            foreach(var field in Fields)
            {
                context.writer.WriteLine($"x->{field} = {ChronTypes.CreateNil}();");
            }
            context.writer.WriteLine("return x;\n}");
        }
        public override void Write(ChronContext context)
        {
            RawCName = $"_S_{Name}";

            context.writer.WriteLine($"typedef struct {{");

            foreach(var field in Fields)
            {
                context.writer.WriteLine($"{ChronTypes.TypeMap["object"]} {field};");
            }

            context.writer.WriteLine($"}} {RawCName};");

            foreach(var field in Fields)
            {
                GenerateSetField(context, field);
                GenerateGetField(context, field);
            }

            GenerateNewAlloca(context);

            context.env.GetCurrentScope().AddToScope(Name, this);
        }

        public override object Read(ChronContext context)
        {
            return RawCName;
        }

        public override void CreateIndexes(ChronContext context, string prefix)
        {
            foreach(var field in Fields)
            {
                context.env.GetCurrentScope().AddToScope($"{prefix}.{field}", new ChronStructFieldAccessor(field, this), true);
            }
        }

        public override void SetParent(ChronExpression parent)
        {
            throw new NotImplementedException();
        }

        public override void Write(ChronContext context, ChronExpression value)
        {
            throw new NotImplementedException();
        }

        public ChronFunction GetAllocateFunction()
        {
            return new($"_new_{Name}", true);
        }
    }
}
