#include "memory.h"
#include <stdio.h>
#include <stdlib.h>

MemoryContext *Context;

MemoryContext *Create_MemoryContext()
{
	MemoryContext *context = (MemoryContext *)malloc(sizeof(MemoryContext));
	if (context != NULL)
	{
		context->size = 0;
		context->capacity = MAX_ALLOCATIONS;
		context->memory = malloc(MAX_ALLOCATIONS * sizeof(ChronObject));

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
		Context = Create_MemoryContext();
	}
}

void MemoryContext_ReleaseAll()
{
	for (size_t i = 0; i < Context->size; ++i)
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
	if (Context->size > Context->capacity)
	{
		Context->capacity *= 2;
		Context->memory = (ChronObject *)realloc(Context->memory, Context->capacity * sizeof(ChronObject));
	}
	Context->memory[Context->size++] = ptr;
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
