// object.h
#ifndef OBJECT_H
#define OBJECT_H

#include <stdbool.h>
#include "memory.h"
#include "standard.h"

#define INITIAL_CAPACITY 100
#define LOAD_FACTOR_THRESHOLD 0.75

void InitializeDynamicTable(DynamicTable* table);
void SetDynamicTable(ChronObject o, ChronObject index, ChronObject value);
ChronObject IndexDynamicTable(ChronObject o, ChronObject index);
ChronObject DynString(const char* str);
ChronObject DynChar(char c);
ChronObject DynInteger(int i);
ChronObject DynNumber(double i);
ChronObject DynBoolean(bool boolean);
ChronObject DynNil();
ChronObject DynTable();
ChronObject DynPointer(void* ptr);
ChronObject DynFunction(void* ptr);

ChronObject Clone(ChronObject input);

#endif