// standard.h
#ifndef STANDARD_H
#define STANDARD_H

#include "object.h"

GC_ITEM *DynObjectCompareEq(GC_ITEM *left, GC_ITEM *right);
GC_ITEM *DynObjectCompareNEq(GC_ITEM *left, GC_ITEM *right);
GC_ITEM *DynObjectAdd(GC_ITEM *left, GC_ITEM *right);
GC_ITEM *DynObjectSub(GC_ITEM *left, GC_ITEM *right);
GC_ITEM *DynObjectDiv(GC_ITEM *left, GC_ITEM *right);
GC_ITEM *DynObjectMul(GC_ITEM *left, GC_ITEM *right);
GC_ITEM *TypeOf(GC_ITEM *obj);
GC_ITEM *Add(GC_ITEM *left, GC_ITEM *right);
GC_ITEM *ToString(GC_ITEM *input);
GC_ITEM *ReadLine();
void Throw(GC_ITEM *errorMessage);
bool GetBoolean(GC_ITEM *o);

#define ref(o) (&o)
#define deref(o) (*o)

#endif