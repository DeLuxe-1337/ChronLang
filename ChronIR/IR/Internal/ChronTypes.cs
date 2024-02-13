using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronIR.IR.Internal
{
    public static class ChronTypes
    {
        public record ChronType(string Value);
        public static Dictionary<string, ChronType> TypeMap = new() { { "object", new("ChronGarbageCollectedObject") }, { "void", new("ChronVoid") }, { "string", new("const char*") }, { "int", new("int") }, };
        public static string CreateString = "DynString";
        public static string CreateInt = "DynInteger";
        public static string CreateBoolean = "DynBoolean";
        public static string ObjectCompareEq = "DynObjectCompareEq";
        public static string ObjectCompareNEq = "DynObjectCompareNEq";
        public static string ObjectAdd = "DynObjectAdd";
        public static string ObjectSub = "DynObjectSub";
        public static string ObjectDiv = "DynObjectDiv";
        public static string ObjectMul = "DynObjectMul";
        public static string GetBooleanFromObject = "GetBoolean";
        public static string GCRelease = "GC_Release";
        public static int TEMP_VARIABLE = 0;
    }
}
