// DynObject.c
#include "object.h"
#include "standard.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "gc.h"

void InitializeDynamicTable(DynamicTable *table)
{
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
  DynObject *tableObject = o->Object;
  DynamicTable *table = tableObject->table;

  if (table->size >= table->capacity)
  {
    table->capacity += 1;
    TableKeyValuePair *new_pairs = (TableKeyValuePair *)realloc(table->pairs, table->capacity * sizeof(TableKeyValuePair));
    if (new_pairs == NULL)
    {
      fprintf(stderr, "Memory reallocation failed\n");
      return; // Return without modifying the table
    }
    table->pairs = new_pairs;
  }

  table->pairs[table->size].key = Clone(index);
  if (table->pairs[table->size].key != NULL)
  {
    table->pairs[table->size].value = Clone(value);
    table->size++;
  }
  else
  {
    printf("Table key is null\n");
  }
}

GC_ITEM *IndexDynamicTable(GC_ITEM *o, GC_ITEM *index)
{
  DynObject *tableObject = o->Object;
  DynamicTable *table = tableObject->table;

  for (size_t i = 0; i < table->size; i++)
  {
    if (GetBoolean(DynObjectCompareEq(table->pairs[i].key, index)))
    {
      return Clone(table->pairs[i].value);
    }
  }
  return DynNil();
}

GC_ITEM *DynString(const char *str)
{
  newObject(obj, DynObject);

  _VO_obj->boolean = false;
  _VO_obj->str = strdup(str);
  _VO_obj->integer = 0;
  _VO_obj->number = 0.0;
  _VO_obj->type = vstring;
  _VO_obj->cstruct = 0;

  return GC_obj;
}

GC_ITEM *DynInteger(int i)
{
  newObject(obj, DynObject);

  _VO_obj->boolean = false;
  _VO_obj->str = 0;
  _VO_obj->integer = i;
  _VO_obj->number = 0.0;
  _VO_obj->type = vinteger;
  _VO_obj->cstruct = 0;

  return GC_obj;
}

GC_ITEM *DynBoolean(bool boolean)
{
  newObject(obj, DynObject);

  _VO_obj->boolean = boolean;
  _VO_obj->str = 0;
  _VO_obj->integer = 0;
  _VO_obj->number = 0.0;
  _VO_obj->type = vboolean;
  _VO_obj->cstruct = 0;

  return GC_obj;
}

GC_ITEM *DynTable()
{
  newObject(obj, DynObject);

  _VO_obj->boolean = 0;
  _VO_obj->str = 0;
  _VO_obj->integer = 0;
  _VO_obj->number = 0.0;
  _VO_obj->type = vtable;
  _VO_obj->cstruct = 0;
  _VO_obj->table = (DynamicTable *)GC_Malloc(sizeof(DynamicTable))->Object;

  InitializeDynamicTable(_VO_obj->table);

  return GC_obj;
}

GC_ITEM *DynNil()
{
  newObject(obj, DynObject);

  _VO_obj->boolean = 0;
  _VO_obj->str = 0;
  _VO_obj->integer = 0;
  _VO_obj->number = 0.0;
  _VO_obj->type = vnull;
  _VO_obj->cstruct = 0;

  return GC_obj;
}

DynObject SetDynObjectType(DynObject *DynObject, DynObjectType type)
{
  DynObject->type = type;
  return *DynObject;
}

void Deallocate(DynObject *DynObject)
{
  if (DynObject == NULL)
  {
    return;
  }

  if (DynObject->type == vstring)
  {
    free(DynObject->str);
    DynObject->str = NULL;
  }

  DynObject->type = vdeallocated;
}

void DynObjectFree(DynObject *DynObject)
{
  Deallocate(DynObject);

  free(DynObject);
}

// DynObject Expect(DynObject input, DynObject errorMessage) {
//     if (input.type == vnull) {
//         Throw(errorMessage);
//     }

// 		return input;
// }

GC_ITEM * Clone(GC_ITEM * input) {
  if (input == NULL) {
    return DynNil();
  }

  DynObject *target = input->Object;

  GC_ITEM * cloneObject = DynNil();
  DynObject* clone = cloneObject->Object;
  clone->type = target->type;

  switch (clone->type) {
    case vstring:
      clone->str = strdup(target->str);
      break;
    case vboolean:
      clone->boolean = target->boolean;
      break;

    case vnumber:
      clone->number = target->number;
      break;

    case vinteger:
      clone->integer = target->integer;
      break;

    case vnull:
      break;
    case vdeallocated:
      clone->type = vdeallocated;
      break;
  }

  return cloneObject;
}