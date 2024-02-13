// DynObject.c
#include "object.h"
#include "standard.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "gc.h"

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

GC_ITEM* DynInteger(int i)
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

GC_ITEM* DynBoolean(bool boolean) {
  newObject(obj, DynObject);

  _VO_obj->boolean = boolean;
  _VO_obj->str = 0;
  _VO_obj->integer = 0;
  _VO_obj->number = 0.0;
  _VO_obj->type = vboolean;
  _VO_obj->cstruct = 0;

  return GC_obj;
}

// DynObject DynCStructure(void* structure) {
//   DynObject obj = {
//     .boolean = false,
//     .str = NULL,
//     .integer = 0,
//     .number = 0.0,

//     .type = vcstruct,
//     .cstruct = structure
//   };
//   return obj;
// }

// DynObject DynNull() {
//   DynObject obj = {
//     .boolean = false,
//     .str = NULL,

//     .integer = 0,
//     .number = 0.0,
//     .type = vnull,
//     .cstruct = NULL
//   };
//   return obj;
// }

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

// DynObject Clone(DynObject* input) {
//   if (input == NULL) {
//     return DynNull();
//   }

//   DynObject clone;
//   clone.type = input->type;

//   switch (input->type) {
//     case vstring:
//       clone.str = strdup(input->str);
//       if (clone.str == NULL) {
//         return DynNull();
//       }
//       break;

//     case vboolean:
//       clone.boolean = input->boolean;
//       break;

//     case vnumber:
//       clone.number = input->number;
//       break;

//     case vinteger:
//       clone.integer = input->integer;
//       break;

//     case vnull:
//       break;

//     case vdeallocated:
//       clone.type = vdeallocated;
//       break;
//   }

//   return clone;
// }