namespace ChronIR.IR.Internal
{
    public static class ChronTypes
    {
        public record ChronType(string Value);
        public static Dictionary<string, ChronType> TypeMap = new() { { "object", new("ChronObject") }, { "void", new("ChronVoid") }, { "string", new("const char*") }, { "int", new("int") }, };
        public static string CreateString = "DynString";
        public static string CreateInt = "DynInteger";
        public static string CreateBoolean = "DynBoolean";
        public static string CreateTable = "DynTable";
        public static string CreateNil = "DynNil";
        public static string ObjectCompareEq = "DynObjectCompareEq";
        public static string ObjectCompareNEq = "DynObjectCompareNEq";
        public static string ObjectCompareGrt = "DynObjectCompareGrt";
        public static string ObjectCompareGrtEq = "DynObjectCompareGrtEq";
        public static string ObjectCompareLesstEq = "DynObjectCompareLesstEq";
        public static string ObjectCompareLesst = "DynObjectCompareLesst";
        public static string ObjectCompareOr = "DynObjectCompareOr";
        public static string ObjectCompareAnd = "DynObjectCompareAnd";
        public static string ObjectCompareNot = "DynObjectNot";
        public static string ObjectAdd = "DynObjectAdd";
        public static string ObjectSub = "DynObjectSub";
        public static string ObjectDiv = "DynObjectDiv";
        public static string ObjectMul = "DynObjectMul";
        public static string ObjectModulus = "DynObjectMod";
        public static string GetBooleanFromObject = "GetBoolean";
        public static string IntializeDynamicTable = "InitializeDynamicTable";
        public static string IndexDynamicTable = "IndexDynamicTable";
        public static string SetDynamicTable = "SetDynamicTable";
        public static string GCRelease = "MemoryContext_Release";
        public static int TEMP_VARIABLE = 0;
    }
}
