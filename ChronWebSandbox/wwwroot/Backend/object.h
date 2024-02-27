// object.h
#ifndef OBJECT_H
#define OBJECT_H

#include <stdbool.h>
#include "memory.h"
#include "standard.h"

typedef enum
{
  vstring,
  vboolean,
  vnumber,
  vinteger,
  vnull,
  vptr,
  vtable,
  vdeallocated
} DynObjectType;

typedef struct
{
  ChronObject key;
  ChronObject value; //error on this line
} TableKeyValuePair;

typedef struct
{
  TableKeyValuePair *pairs;
  size_t size;
  size_t capacity;
} DynamicTable;

typedef struct
{
  DynObjectType type;
  DynamicTable* table;
  bool boolean;
  char *str;
  int integer;
  double number;
  void * ptr;
} DynObject;

void InitializeDynamicTable(DynamicTable * table);
void SetDynamicTable(ChronObject o, ChronObject index, ChronObject value);
ChronObject IndexDynamicTable(ChronObject o, ChronObject index);
ChronObject DynString(const char *str);
ChronObject DynInteger(int i);
ChronObject DynBoolean(bool boolean);
ChronObject DynNil();
ChronObject DynTable();
ChronObject DynPointer(void* ptr);
DynObject SetDynObjectType(DynObject *DynObject, DynObjectType type);
DynObject Expect(DynObject input, DynObject errorMessage);
ChronObject Clone(ChronObject input);

#endif