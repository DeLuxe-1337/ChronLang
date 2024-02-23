// DynObject.h
#ifndef DynObject_H
#define DynObject_H

#include <stdbool.h>
#include "gc.h"
#include "malloc.h"

typedef enum
{
  vstring,
  vboolean,
  vnumber,
  vinteger,
  vnull,
  vcstruct,
  vtable,
  vdeallocated
} DynObjectType;

typedef struct
{
  GC_ITEM *key;
  GC_ITEM *value;
} TableKeyValuePair;

typedef struct
{
  TableKeyValuePair *pairs;
  size_t size;
  size_t capacity;
} DynamicTable;

typedef struct
{
  bool boolean;
  char *str;
  int integer;
  double number;
  DynObjectType type;
  void *cstruct;
  DynamicTable* table;
} DynObject;

void InitializeDynamicTable(DynamicTable * table);
void SetDynamicTable(GC_ITEM *o, GC_ITEM *index, GC_ITEM *value);
GC_ITEM* IndexDynamicTable(GC_ITEM* o, GC_ITEM* index);
GC_ITEM *DynString(const char *str);
GC_ITEM *DynInteger(int i);
GC_ITEM *DynBoolean(bool boolean);
GC_ITEM *DynNil();
GC_ITEM *DynTable();
DynObject SetDynObjectType(DynObject *DynObject, DynObjectType type);
DynObject Expect(DynObject input, DynObject errorMessage);
GC_ITEM* Clone(GC_ITEM *input);

#endif