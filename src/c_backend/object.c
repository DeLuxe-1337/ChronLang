// object.c
#include "object.h"
#include "standard.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

unsigned long HashString(const char *str)
{
    unsigned long hash = 5381;
    int c;

    while ((c = *str++))
    {
        hash = ((hash << 5) + hash) + c; /* hash * 33 + c */
    }

    return hash;
}

int hash(ChronObject key, int capacity);

int HashPair(Node *node, int capacity)
{
    return hash(node->pair.key, capacity) ^ hash(node->pair.value, capacity);
}

int HashTable(DynamicTable *table)
{
    Node *current = NULL;
    int sum = 0;
    for (int i = 0; i < table->capacity; i++)
    {
        current = table->buckets[i];
        while (current != NULL)
        {
            sum += HashPair(current, table->capacity);
            current = current->next;
        }
    }
    printf("Table hash: %d\n", sum);
    return sum;
}

int hash(ChronObject key, int capacity)
{
    switch (key->type)
    {
    case vstring:
        return HashString(key->data.str) % capacity;
    case vinteger:
        return key->data.integer % capacity;
    case vboolean:
        return key->data.boolean ? 1 : 2;
    case vfunction:
    case vptr:
        return (int)key->data.ptr;
    case vtable:
        return HashTable(key->data.table);
    case vnull:
        return 0;
    }
}

ChronObject HashMapGet(DynamicTable *map, ChronObject key)
{
    int index = hash(key, map->capacity);
    if (index < 0 || index >= map->capacity || map->buckets[index] == NULL)
    {
        return DynNil(); // Index out of bounds or bucket is empty
    }

    Node *current = map->buckets[index];
    while (current != NULL)
    {
        if (GetBoolean(DynObjectCompareEq(current->pair.key, key)))
        {
            return current->pair.value;
        }
        current = current->next;
    }
    return DynNil(); // Key not found
}

void HashMapResize(DynamicTable *map)
{
    int newCapacity = map->capacity * 2;
    Node **newBuckets = (Node **)malloc(sizeof(Node *) * newCapacity);
    memset(newBuckets, 0, sizeof(Node *) * newCapacity);

    // Rehash all elements into the new array
    for (int i = 0; i < map->capacity; i++)
    {
        Node *current = map->buckets[i];
        while (current != NULL)
        {
            Node *next = current->next;
            int index = hash(current->pair.key, newCapacity);
            current->next = newBuckets[index];
            newBuckets[index] = current;
            current = next;
        }
    }

    // Free the old array and update the map's fields
    free(map->buckets);
    map->buckets = newBuckets;
    map->capacity = newCapacity;
}

void HashMapInsert(DynamicTable *map, ChronObject key, ChronObject value)
{
    if ((double)map->size / map->capacity >= LOAD_FACTOR_THRESHOLD)
    {
        HashMapResize(map);
    }

    int index = hash(key, map->capacity);
    KeyValuePair *pair = (KeyValuePair *)malloc(sizeof(KeyValuePair));
    pair->key = &key;
    pair->value = &value;
    Node *node = (Node *)malloc(sizeof(Node));
    node->pair = *pair;
    node->next = NULL;

    Node *current = map->buckets[index];
    Node *prev = NULL;
    while (current != NULL)
    {
        if (GetBoolean(DynObjectCompareEq(current->pair.key, key)))
        {
            MemoryContext_Release(current->pair.value);
            current->pair.value = &value;
            free(pair);
            free(node);
            return; // Exit function as we've replaced the value
        }
        prev = current;
        current = current->next;
    }

    // If key doesn't already exist, add it to the end of the linked list
    if (prev == NULL)
    {
        // No collision, first node at this index
        map->buckets[index] = node;
    }
    else
    {
        // Collision, add node to the end of the linked list
        prev->next = node;
    }
    map->size++;
}

void InitializeDynamicTable(DynamicTable *table)
{
    table->capacity = INITIAL_CAPACITY;
    table->size = 0;
    table->buckets = (Node **)malloc(sizeof(Node *) * table->capacity);
    memset(table->buckets, 0, sizeof(Node *) * table->capacity);
}

void TableInsert(ChronObject tableObject, ChronObject value)
{
    if (tableObject->type != vtable)
    {
        printf("Runtime warning: attempting to set value in table on object that "
               "isn't a table.\n");
        return;
    }

    DynamicTable *table = tableObject->data.table;
    HashMapInsert(table, DynInteger(table->size), value);
}

void SetDynamicTable(ChronObject tableObject, ChronObject index, ChronObject value)
{
    if (tableObject->type != vtable)
    {
        ChronObject result = ToString(tableObject);
        ChronObject i = ToString(index);
        ChronObject v = ToString(value);
        printf("Runtime warning: attempting to set value on Table=%s Index=%s Value=%s\n", c_string(result), c_string(i), c_string(v));
        MemoryContext_Release(result);
        MemoryContext_Release(i);
        MemoryContext_Release(v);
        return;
    }

    DynamicTable *table = tableObject->data.table;

    HashMapInsert(table, Clone(index), Clone(value));
}

ChronObject IndexDynamicTable(ChronObject tableObject, ChronObject index)
{
    if (tableObject->type != vtable)
    {
        ChronObject result = ToString(tableObject);
        ChronObject i = ToString(index);
        printf("Runtime warning: attempting to index on Table=%s Index=%s \n", c_string(result), c_string(i));
        MemoryContext_Release(result);
        MemoryContext_Release(i);
        return DynNil();
    }

    DynamicTable *table = tableObject->data.table;

    return HashMapGet(table, index);
}

void dealloc_string(ChronObject o)
{
    free(o->data.str);
}

ChronObject DynString(const char *str)
{
    newObject(obj);

    GC_obj->data.str = strdup(str);
    GC_obj->type = vstring;

    GC_obj->heap = true;
    GC_obj->deallocate = dealloc_string;

    return GC_obj;
}

ChronObject DynChar(char c)
{
    newObject(obj);

    GC_obj->data.str = malloc(2 * sizeof(char));
    GC_obj->type = vstring;

    GC_obj->data.str[0] = c;
    GC_obj->data.str[1] = '\0';

    GC_obj->deallocate = dealloc_string;
    GC_obj->heap = true;

    return GC_obj;
}

ChronObject DynInteger(int i)
{
    newObject(obj);

    GC_obj->data.integer = i;
    GC_obj->type = vinteger;

    return GC_obj;
}

ChronObject DynNumber(double i)
{
    newObject(obj);

    GC_obj->data.number = i;
    GC_obj->type = vnumber;

    return GC_obj;
}

ChronObject DynBoolean(bool boolean)
{
    newObject(obj);

    GC_obj->data.boolean = boolean;
    GC_obj->type = vboolean;
    return GC_obj;
}

void dealloc_table(ChronObject obj)
{
    DynamicTable *table = obj->data.table;
    for (int i = 0; i < table->capacity; i++)
    {
        Node *current = table->buckets[i];
        while (current != NULL)
        {
            MemoryContext_Release(current->pair.key);
            MemoryContext_Release(current->pair.value);
            current = current->next;
        }
    }
    free(table->buckets);
    table->buckets = NULL;
    table->size = 0;
    table->capacity = 0;
    free(table);
}

ChronObject DynTable()
{
    newObject(obj);

    GC_obj->type = vtable;
    GC_obj->data.table = (DynamicTable *)malloc(sizeof(DynamicTable));

    InitializeDynamicTable(GC_obj->data.table);

    GC_obj->heap = true;

    GC_obj->deallocate = dealloc_table;

    return GC_obj;
}

ChronObject DynNil()
{
    newObject(obj);

    GC_obj->type = vnull;

    return GC_obj;
}

ChronObject DynPointer(void *ptr)
{
    newObject(obj);

    GC_obj->type = vptr;
    GC_obj->data.ptr = ptr;

    return GC_obj;
}

ChronObject DynFunction(void *ptr)
{
    newObject(obj);

    GC_obj->type = vfunction;
    GC_obj->data.ptr = ptr;

    return GC_obj;
}

// DynObject Expect(DynObject input, DynObject errorMessage) {
//     if (input.type == vnull) {
//         Throw(errorMessage);
//     }

// 		return input;
// }

ChronObject Clone(ChronObject target)
{
    ChronObject clone = DynNil();
    clone->type = target->type;

    switch (clone->type)
    {
    case vstring:
        clone->data.str = strdup(target->data.str);
        clone->deallocate = dealloc_string;
        break;
    case vboolean:
        clone->data.boolean = target->data.boolean;
        break;

    case vnumber:
        clone->data.number = target->data.number;
        break;

    case vinteger:
        clone->data.integer = target->data.integer;
        break;
    case vtable:
        clone->data.table = (DynamicTable *)malloc(sizeof(DynamicTable));
        InitializeDynamicTable(clone->data.table);
        clone->deallocate = dealloc_table;

        for (int i = 0; i < target->data.table->capacity; i++)
        {
            Node *current = target->data.table->buckets[i];
            while (current != NULL)
            {
                SetDynamicTable(clone, current->pair.key, current->pair.value);
                current = current->next;
            }
        }

        break;
    case vfunction:
    case vptr:
        clone->data.ptr = target->data.ptr;
        break;
    case vnull:
        break;
    case vdeallocated:
        clone->type = vdeallocated;
        break;
    }

    return clone;
}