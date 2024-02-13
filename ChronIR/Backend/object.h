// DynObject.h
#ifndef DynObject_H
#define DynObject_H

#include <stdbool.h>
#include "gc.h"

typedef enum {
  vstring,
  vboolean,
  vnumber,
  vinteger,
  vnull,
  vcstruct,
  vdeallocated
} DynObjectType;

typedef struct {
  bool boolean;
  char* str;
  int integer;
  double number;
  DynObjectType type;
  void* cstruct;
} DynObject;

GC_ITEM* DynString(const char* str);
GC_ITEM* DynInteger(int i);
GC_ITEM* DynBoolean(bool boolean);
GC_ITEM* DynCStructure(void* structure);
GC_ITEM* DynNull();
DynObject SetDynObjectType(DynObject* DynObject, DynObjectType type);
DynObject Expect(DynObject input, DynObject errorMessage);
DynObject Clone(DynObject* input);

#endif