#ifndef GC_H
#define GC_H

#include <stdio.h>

typedef struct {
  void* Object;
  int count;
  void (*deallocate)(void*);
} GC_ITEM;

GC_ITEM *GC_Register(void* object);
void GC_Retain(GC_ITEM *garbage);
void GC_Release(GC_ITEM *garbage);
void GC_ReleaseAll();
GC_ITEM* GC_Malloc(size_t size);

#define newObject(name, type) \
GC_ITEM* GC_##name = GC_Malloc(sizeof(type)); \
type* _VO_##name = GC_##name->Object

#endif