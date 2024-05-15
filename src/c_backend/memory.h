#ifndef MEMORY_H
#define MEMORY_H

#define MAX_ALLOCATIONS 500
#include <stdio.h>
#include <stdbool.h>

typedef struct DynObject DynObject;

typedef enum
{
  vstring,
  vboolean,
  vnumber,
  vinteger,
  vnull,
  vptr,
  vtable,
  vdeallocated,
  vfunction,
} DynObjectType;

typedef struct
{
  DynObject *key;
  DynObject *value;
} KeyValuePair;

typedef struct Node
{
  KeyValuePair pair;
  struct Node *next;
} Node;

typedef struct
{
  Node **buckets;
  int capacity;
  int size;
} DynamicTable;

typedef struct
{
  void *self;
  DynObject* (*index)(void *, int);
  DynObject* (*value)(void *, int);
  int size;
} Iterator;

typedef struct DynObject
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
  void (*deallocate)(DynObject*);
  size_t index;
  bool heap;
} DynObject;

typedef DynObject* ChronObject;

typedef struct
{
  DynObject *memory;
  size_t size;
  size_t capacity;
} MemoryContext;

extern MemoryContext *Context;

MemoryContext *Create_MemoryContext();
DynObject MemoryContext_Register(void *object);
void MemoryContext_Release(ChronObject garbage);
void MemoryContext_ReleaseAll();
ChronObject MemoryContext_Malloc(size_t size);
void MemoryContext_ReleaseContext(MemoryContext *ctx);
ChronObject MemoryContext_Push(DynObject object);

#define newAllocatedObject(name, type)                        \
  ChronObject GC_##name = MemoryContext_Malloc(sizeof(type)); \

#define newObject(name)    \
  DynObject VO_##name;         \
  VO_##name.heap = false; \
  VO_##name.deallocate = NULL; \
  ChronObject GC_##name = MemoryContext_Push(VO_##name); \

#endif