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
const char* c_string(ChronObject o);
bool c_bool(ChronObject o);

#define ref(o) (&o)
#define deref(o) (*o)

#endif