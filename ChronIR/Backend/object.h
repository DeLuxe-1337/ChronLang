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
  varray,
  vdeallocated
} DynObjectType;

typedef struct
{
  bool boolean;
  char *str;
  int integer;
  double number;
  DynObjectType type;
  void *cstruct;
  void *table;
} DynObject;

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

void InitializeDynamicTable(GC_ITEM *o)
{
  DynamicTable* table = (DynamicTable*)o->Object;

  table->pairs = (TableKeyValuePair *)malloc(1 * sizeof(TableKeyValuePair));
  if (table->pairs == NULL)
  {
    fprintf(stderr, "Memory allocation failed\n");
  }
  table->size = 0;
  table->capacity = 1;
}

void SetDynamicTable(GC_ITEM *o, GC_ITEM *index, GC_ITEM *value)
{  
  DynamicTable* table = (DynamicTable*)o->Object;
}

GC_ITEM *DynString(const char *str);
GC_ITEM *DynInteger(int i);
GC_ITEM *DynBoolean(bool boolean);
GC_ITEM *DynNil();
DynObject SetDynObjectType(DynObject *DynObject, DynObjectType type);
DynObject Expect(DynObject input, DynObject errorMessage);
DynObject Clone(DynObject *input);

#endif