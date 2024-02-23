#include "gc.h"
#include <stdio.h>
#include <stdlib.h>

#define MAX_ALLOCATIONS 1000

void *allocatedMemory[MAX_ALLOCATIONS];
size_t allocationCount = 0;

void GC_ReleaseAll()
{
	for (size_t i = 0; i < allocationCount; ++i)
	{
		if (allocatedMemory[i] != NULL)
		{
			GC_Release((GC_ITEM *)allocatedMemory[i]);
			allocatedMemory[i] = NULL; // Set the pointer to NULL after releasing
		}
	}
}

GC_ITEM *GC_Malloc(size_t size)
{
	GC_ITEM *ptr = GC_Register(malloc(size));
	if (allocationCount < MAX_ALLOCATIONS)
	{
		allocatedMemory[allocationCount++] = ptr;
	}
	else
	{
		// Handle the case when the array is full (you might want to extend it
		// dynamically).
		fprintf(stderr, "Error: Maximum number of allocations reached\n");
		free(ptr); // Free the memory to avoid leaks.
		return NULL;
	}
	return ptr;
}

GC_ITEM *GC_Register(void *object)
{
	GC_ITEM *registeredObject = malloc(sizeof(GC_ITEM));
	registeredObject->Object = object;
	registeredObject->count = 1;

	return registeredObject;
}

void GC_Retain(GC_ITEM *garbage)
{
	if (garbage != NULL)
	{
		garbage->count++;
	}
}

void GC_Release(GC_ITEM *garbage)
{
	if (garbage != NULL)
	{
		garbage->count--;
		if (garbage->count == 0)
		{
			if(garbage->deallocate != NULL)
				garbage->deallocate(garbage->Object);

			free(garbage->Object);
			free(garbage);

			allocationCount--;

			#if CHRON_DEBUG == 1
			printf("\tGC: Collected Garbage\n");
			#endif
		}
	}
}
