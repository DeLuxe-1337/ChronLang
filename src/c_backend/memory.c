#include "memory.h"
#include <stdio.h>
#include <stdlib.h>

MemoryContext *Context;

typedef struct
{
	size_t *data;
	size_t top;
	size_t capacity;
} IndexStack;

IndexStack *CreateIndexStack(size_t capacity)
{
	IndexStack *stack = malloc(sizeof(IndexStack));
	if (stack != NULL)
	{
		stack->data = malloc(capacity * sizeof(size_t));
		if (stack->data == NULL)
		{
			free(stack);
			return NULL;
		}
		stack->top = 0;
		stack->capacity = capacity;
	}
	return stack;
}

void PushIndex(IndexStack *stack, size_t index)
{
	if (stack->top < stack->capacity)
		stack->data[stack->top++] = index;
}

size_t PopIndex(IndexStack *stack)
{
	if (stack->top > 0)
		return stack->data[--stack->top];
	else
		return -1; // Indicates empty stack
}

void DestroyIndexStack(IndexStack *stack)
{
	free(stack->data);
	free(stack);
}

IndexStack *released_indices;

MemoryContext *Create_MemoryContext()
{
#if CHRON_DEBUG
	printf("\t>Memory context created\n");
#endif

	MemoryContext *context = (MemoryContext *)malloc(sizeof(MemoryContext));
	if (context != NULL)
	{
		context->size = 0;
		context->capacity = MAX_ALLOCATIONS;
		context->memory = malloc(MAX_ALLOCATIONS * sizeof(ChronObject));
	}
	return context;
}

void MemoryContextCreateIfNull()
{
	if (Context == NULL)
	{
		released_indices = CreateIndexStack(MAX_ALLOCATIONS);
		Context = Create_MemoryContext();
	}
}

void MemoryContext_ReleaseAll()
{
	for (size_t i = 0; i < Context->size; ++i)
	{
		DynObject object = Context->memory[i];
		if (object.deallocate != NULL || object.type == vptr)
		{
			MemoryContext_Release(&object);
		}
	}
}

ChronObject MemoryContext_Push(DynObject object)
{
	MemoryContextCreateIfNull();
	size_t index = PopIndex(released_indices);

	if (index != -1)
	{
		object.index = index;
		Context->memory[index] = object;
		printf("Reusing index %d\n", index);
		return &Context->memory[index];
	}

	if (Context->size >= Context->capacity)
	{
		printf("MemoryContext expansion required...\n");
		size_t new_capacity = Context->capacity * 2;
		ChronObject *new_memory = (ChronObject *)malloc(new_capacity * sizeof(ChronObject));
		if (new_memory == NULL)
		{
			// Handle allocation failure
			return NULL;
		}
		memcpy(new_memory, Context->memory, Context->capacity * sizeof(ChronObject));
		free(Context->memory);
		Context->memory = new_memory;
		Context->capacity = new_capacity;
	}
	object.index = Context->size;
	Context->memory[Context->size++] = object;
	return &Context->memory[Context->size - 1];
}

ChronObject MemoryContext_Malloc(size_t size)
{
	MemoryContextCreateIfNull();

	DynObject result = MemoryContext_Register(malloc(size));
	return MemoryContext_Push(result);
}

DynObject MemoryContext_Register(void *object)
{
	MemoryContextCreateIfNull();

	DynObject result;
	result.data.ptr = object;
	result.type = vptr;
	result.heap = true;
	result.deallocate = NULL;

	return result;
}

void MemoryContext_ReleaseContext(MemoryContext *ctx)
{
	for (size_t i = 0; i < ctx->size; i++)
	{
		DynObject object = ctx->memory[i];
		if (object.deallocate != NULL || object.type == vptr)
		{
			MemoryContext_Release(&object);
		}
	}
	free(ctx->memory);
}

void MemoryContext_Release(ChronObject garbage)
{
	if (garbage->heap == true && garbage->deallocate != NULL)
	{
		printf("Deallocate\n");
		garbage->deallocate(garbage);
	}

	if (garbage->type == vptr)
	{
		printf("Free ptr\n");
		free(garbage->data.ptr);
	}

	PushIndex(released_indices, garbage->index);
}
