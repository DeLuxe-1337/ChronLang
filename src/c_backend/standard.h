// standard.h
#ifndef STANDARD_H
#define STANDARD_H

#include "object.h"

ChronObject DynObjectCompareGrt(ChronObject left, ChronObject right);
ChronObject DynObjectCompareGrtEq(ChronObject left, ChronObject right);
ChronObject DynObjectCompareLesstEq(ChronObject left, ChronObject right);
ChronObject DynObjectCompareLesst(ChronObject left, ChronObject right);
ChronObject DynObjectCompareEq(ChronObject left, ChronObject right);
ChronObject DynObjectCompareOr(ChronObject left, ChronObject right);
ChronObject DynObjectCompareAnd(ChronObject left, ChronObject right);
ChronObject DynObjectCompareNEq(ChronObject left, ChronObject right);
ChronObject DynObjectAdd(ChronObject left, ChronObject right);
ChronObject DynObjectSub(ChronObject left, ChronObject right);
ChronObject DynObjectDiv(ChronObject left, ChronObject right);
ChronObject DynObjectMul(ChronObject left, ChronObject right);
ChronObject DynObjectMod(ChronObject left, ChronObject right);
ChronObject DynObjectNot(ChronObject o);
ChronObject DynObjectNegative(ChronObject o);
ChronObject TypeOf(ChronObject obj);
ChronObject Add(ChronObject left, ChronObject right);
ChronObject ToString(ChronObject input);
ChronObject ReadLine();
ChronObject CreateMemoryContext();
ChronObject GetMemoryContext();

void ReleaseMemoryContext(ChronObject o);
void SetMemoryContext();
void Throw(ChronObject errorMessage);
bool GetBoolean(ChronObject o);
int c_int(ChronObject o);
const char *c_string(ChronObject o);
bool c_bool(ChronObject o);
void* c_pointer(ChronObject o);
int c_object_type(ChronObject o);
int c_table_size(ChronObject o);
ChronObject c_table_key(int index, ChronObject o);
ChronObject c_table_value(int index, ChronObject o);

/*
    Table standard
*/

ChronObject TableSizeOf(ChronObject table);
ChronObject TableIter(ChronObject o);

/*
    String standard
*/
ChronObject StringIter(ChronObject o);
ChronObject StringContains(ChronObject source, ChronObject target);
ChronObject StringLength(ChronObject source);
ChronObject StringIsAlpha(ChronObject source);
ChronObject StringIsAlphaNumeric(ChronObject source);
ChronObject StringIsNumeric(ChronObject source);
ChronObject StringIsWhitespace(ChronObject source);
ChronObject StringIndex(ChronObject source, ChronObject index);
ChronObject StringReplace(ChronObject input, ChronObject sub, ChronObject replace);

#define ref(o) (&o)
#define deref(o) (*o)

#endif