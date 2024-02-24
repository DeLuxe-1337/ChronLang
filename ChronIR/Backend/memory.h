#ifndef MEMORY_H
#define MEMORY_H

#define MAX_ALLOCATIONS 500
#include <stdio.h>

typedef struct
{
  void *Object;
  void (*deallocate)(void *);
} AllocatedObject;

typedef AllocatedObject* ChronObject;

typedef struct
{
  ChronObject* memory;
  size_t size;
  size_t capacity;
} MemoryContext;


ChronObject MemoryContext_Register(void *object);
void MemoryContext_Release(ChronObject garbage);
void MemoryContext_ReleaseAll();
ChronObject MemoryContext_Malloc(size_t size);

#define newObject(name, type)                                 \
  ChronObject GC_##name = MemoryContext_Malloc(sizeof(type)); \
  type *_VO_##name = GC_##name->Object

#endif