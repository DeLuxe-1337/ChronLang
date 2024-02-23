#include "memory.h"
#include <stdio.h>
#include <stdlib.h>

MemoryContext *Context;

MemoryContext *CreateMemoryContext()
{
	MemoryContext *context = (MemoryContext *)malloc(sizeof(MemoryContext));
	if (context != NULL)
	{
		context->size = 0;
		context->capacity = MAX_ALLOCATIONS;
		for (int i = 0; i < MAX_ALLOCATIONS; i++)
		{
			context->memory[i] = NULL;
		}
	}
	return context;
}

void MemoryContextCreateIfNull()
{
	if (Context == NULL)
	{
		Context = CreateMemoryContext();
	}
}

void MemoryContext_ReleaseAll()
{
	for (size_t i = 0; i < Context->memory; ++i)
	{
		if (Context->memory[i] != NULL)
		{
			MemoryContext_Release((ChronObject)Context->memory[i]);
			Context->memory[i] = NULL; // Set the pointer to NULL after releasing
		}
	}
}

ChronObject MemoryContext_Malloc(size_t size)
{
	MemoryContextCreateIfNull();

	ChronObject ptr = MemoryContext_Register(malloc(size));
	if (Context->size < Context->capacity)
	{
		Context->memory[Context->size++] = ptr;
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

		Context->size--;
	}
}
