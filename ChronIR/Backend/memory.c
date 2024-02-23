#include "memory.h"
#include <stdio.h>
#include <stdlib.h>

MemoryContext *Context;

MemoryContext *CreateMemoryContext()
{
	MemoryContext *context = (MemoryContext *)malloc(sizeof(MemoryContext));
	if (context != NULL)
	{
		context->allocationCount = 0;
		for (int i = 0; i < MAX_ALLOCATIONS; i++)
		{
			context->allocatedMemory[i] = NULL;
		}
	}
	return context;
}

void MemoryContextCreateIfNull()
{
	if (Context == NULL)
	{
		Context = CreateMemoryContext();
		printf("Set context\n");
	}
}

void MemoryContext_ReleaseAll()
{
	for (size_t i = 0; i < Context->allocationCount; ++i)
	{
		if (Context->allocatedMemory[i] != NULL)
		{
			MemoryContext_Release((ChronObject)Context->allocatedMemory[i]);
			Context->allocatedMemory[i] = NULL; // Set the pointer to NULL after releasing
		}
	}
}

ChronObject MemoryContext_Malloc(size_t size)
{
	MemoryContextCreateIfNull();

	ChronObject ptr = MemoryContext_Register(malloc(size));
	if (Context->allocationCount < MAX_ALLOCATIONS)
	{
		Context->allocatedMemory[Context->allocationCount++] = ptr;
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

ChronObject MemoryContext_Register(void *object)
{
	ChronObject registeredObject = malloc(sizeof(AllocatedObject));
	registeredObject->Object = object;
	registeredObject->deallocate = NULL;

	return registeredObject;
}

void MemoryContext_Release(ChronObject garbage)
{
	if (garbage != NULL)
	{
		if (garbage->deallocate != NULL)
			garbage->deallocate(garbage->Object);

		free(garbage->Object);
		free(garbage);

		Context->allocationCount--;
	}
}
