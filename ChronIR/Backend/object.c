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

void dealloc_string(void* o)
{
  DynObject* obj = o;
  free(obj->str);
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

  GC_obj->deallocate = dealloc_string;

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

void dealloc_table(void *o)
{
  printf("Delloacted table\n");
  DynObject* obj = o;
  DynamicTable *table = obj->table;
  for (size_t i = 0; i < table->size; i++)
  {
    GC_Release(table->pairs[i].key);
    GC_Release(table->pairs[i].value);
  }
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

  GC_obj->deallocate = dealloc_table;

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

// DynObject Expect(DynObject input, DynObject errorMessage) {
//     if (input.type == vnull) {
//         Throw(errorMessage);
//     }

// 		return input;
// }

GC_ITEM *Clone(GC_ITEM *input)
{
  if (input == NULL)
  {
    return DynNil();
  }

  DynObject *target = input->Object;

  GC_ITEM *cloneObject = DynNil();
  DynObject *clone = cloneObject->Object;
  clone->type = target->type;

  switch (clone->type)
  {
  case vstring:
    clone->str = strdup(target->str);
    cloneObject->deallocate = dealloc_string;
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