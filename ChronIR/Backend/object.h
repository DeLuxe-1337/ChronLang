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
  ChronObject value; // error on this line
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
  union
  {
    DynamicTable *table;
    bool boolean;
    char *str;
    int integer;
    double number;
    void *ptr;
  } data;
} DynObject;

typedef struct
{
  void *self;
  ChronObject (*index)(void *, int);
  ChronObject (*value)(void *, int);
  int size;
} Iterator;

void InitializeDynamicTable(DynamicTable *table);
void SetDynamicTable(ChronObject o, ChronObject index, ChronObject value);
ChronObject IndexDynamicTable(ChronObject o, ChronObject index);
ChronObject DynString(const char *str);
ChronObject DynChar(char c);
ChronObject DynInteger(int i);
ChronObject DynBoolean(bool boolean);
ChronObject DynNil();
ChronObject DynTable();
ChronObject DynPointer(void *ptr);

DynObject *GetRef(ChronObject GC);
ChronObject Clone(ChronObject input);

#endif