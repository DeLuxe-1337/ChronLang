#include "standard.h"
#include "memory.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

ChronObject DynObjectAdd(ChronObject o1, ChronObject o2)
{
	DynObject *left = o1->Object;
	DynObject *right = o2->Object;

	if (left->type != right->type)
	{
		return DynInteger(0);
	}

	if (left->type == vstring)
	{
		int len = strlen(left->str) + strlen(right->str) + 1;
		char *result = (char *)malloc(len * sizeof(char));

		if (result == NULL)
		{
			printf("Memory allocation failed.\n");
			return DynString("Failed to concat");
		}
		strcpy(result, left->str);
		strcat(result, right->str);
		ChronObject ConcatStr = DynString(result);
		free(result);
		return ConcatStr;
	}

	if (left->type == vinteger)
	{
		return DynInteger(left->integer + right->integer);
	}

	return DynInteger(0);
}
ChronObject DynObjectSub(ChronObject o1, ChronObject o2)
{
	DynObject *left = o1->Object;
	DynObject *right = o2->Object;

	if (left->type != right->type)
	{
		return DynInteger(0);
	}

	if (left->type == vinteger)
	{
		return DynInteger(left->integer - right->integer);
	}

	return DynInteger(0);
}
ChronObject DynObjectDiv(ChronObject o1, ChronObject o2)
{
	DynObject *left = o1->Object;
	DynObject *right = o2->Object;

	if (left->type != right->type)
	{
		return DynInteger(0);
	}

	if (left->type == vinteger)
	{
		return DynInteger(left->integer / right->integer);
	}

	return DynInteger(0);
}
ChronObject DynObjectMul(ChronObject o1, ChronObject o2)
{
	DynObject *left = o1->Object;
	DynObject *right = o2->Object;

	if (left->type != right->type)
	{
		return DynInteger(0);
	}

	if (left->type == vinteger)
	{
		return DynInteger(left->integer * right->integer);
	}

	return DynInteger(0);
}
ChronObject DynObjectMod(ChronObject o1, ChronObject o2)
{
	DynObject *left = o1->Object;
	DynObject *right = o2->Object;

	if (left->type != right->type)
	{
		return DynInteger(0);
	}

	if (left->type == vinteger)
	{
		return DynInteger(left->integer % right->integer);
	}

	return DynInteger(0);
}
ChronObject DynObjectCompareGrt(ChronObject o1, ChronObject o2)
{
	DynObject *left = o1->Object;
	DynObject *right = o2->Object;

	if (left->type != right->type)
	{
		return DynBoolean(false);
	}

	if (left->type == vinteger)
	{
		return DynBoolean(left->integer > right->integer);
	}

	return DynBoolean(false);
}
ChronObject DynObjectCompareGrtEq(ChronObject o1, ChronObject o2)
{
	DynObject *left = o1->Object;
	DynObject *right = o2->Object;

	if (left->type != right->type)
	{
		return DynBoolean(false);
	}

	if (left->type == vinteger)
	{
		return DynBoolean(left->integer >= right->integer);
	}

	return DynBoolean(false);
}
ChronObject DynObjectCompareLesstEq(ChronObject o1, ChronObject o2)
{
	DynObject *left = o1->Object;
	DynObject *right = o2->Object;

	if (left->type != right->type)
	{
		return DynBoolean(false);
	}

	if (left->type == vinteger)
	{
		return DynBoolean(left->integer <= right->integer);
	}

	return DynBoolean(false);
}
ChronObject DynObjectCompareLesst(ChronObject o1, ChronObject o2)
{
	DynObject *left = o1->Object;
	DynObject *right = o2->Object;

	if (left->type != right->type)
	{
		return DynBoolean(false);
	}

	if (left->type == vinteger)
	{
		return DynBoolean(left->integer < right->integer);
	}

	return DynBoolean(false);
}
ChronObject DynObjectCompareEq(ChronObject o1, ChronObject o2)
{
	DynObject *left = o1->Object;
	DynObject *right = o2->Object;

	if (left->type != right->type)
	{
		return DynBoolean(false);
	}

	switch (left->type)
	{
	case vstring:
		return DynBoolean(strcmp(left->str, right->str) == 0);
	case vboolean:
		return DynBoolean(left->boolean == right->boolean);
	case vnumber:
		return DynBoolean(left->number == right->number);
	case vinteger:
		return DynBoolean(left->integer == right->integer);
	case vdeallocated:
		return DynBoolean(false);
	default:
		printf("Invalid DynObject type\n");
		return DynBoolean(false);
	}
}

ChronObject DynObjectCompareNEq(ChronObject o1, ChronObject o2)
{
	DynObject *left = o1->Object;
	DynObject *right = o2->Object;

	if (left->type != right->type)
	{
		return DynBoolean(true);
	}

	switch (left->type)
	{
	case vstring:
		return DynBoolean(strcmp(left->str, right->str) != 0);
	case vboolean:
		return DynBoolean(left->boolean != right->boolean);
	case vnumber:
		return DynBoolean(left->number != right->number);
	case vinteger:
		return DynBoolean(left->integer != right->integer);
	case vdeallocated:
		return DynBoolean(false);
	default:
		printf("Invalid DynObject type\n");
		return DynBoolean(false);
	}
}

ChronObject DynObjectCompareOr(ChronObject o1, ChronObject o2)
{
	DynObject *left = o1->Object;
	DynObject *right = o2->Object;

	if (left->type != vboolean && left->type != right->type)
	{
		return DynBoolean(false);
	}

	return DynBoolean(left->boolean || right->boolean);
}
ChronObject DynObjectCompareAnd(ChronObject o1, ChronObject o2)
{
	DynObject *left = o1->Object;
	DynObject *right = o2->Object;

	if (left->type != vboolean && left->type != right->type)
	{
		return DynBoolean(false);
	}

	return DynBoolean(left->boolean && right->boolean);
}
ChronObject DynObjectNot(ChronObject o) {
	DynObject *left = o->Object;

	if (left->type != vboolean)
	{
		return DynBoolean(false);
	}

	return DynBoolean(!left->boolean);
}
DynObject *GetRef(ChronObject GC)
{
	return (DynObject *)GC->Object;
}

DynObject Get(ChronObject GC)
{
	return *(DynObject *)GC->Object;
}

ChronObject TypeOf(ChronObject left)
{
	const char *typeStr;
	DynObject *obj = left->Object;
	switch (obj->type)
	{
	case vdeallocated:
		typeStr = "deallocated";
		break;
	case vstring:
		typeStr = "string";
		break;
	case vboolean:
		typeStr = "boolean";
		break;
	case vnumber:
		typeStr = "number";
		break;
	case vinteger:
		typeStr = "number";
		break;
	case vnull:
		typeStr = "nil";
		break;
	case vtable:
		typeStr = "table";
		break;
	default:
		typeStr = "unknown";
		break;
	}

	return DynString(typeStr);
}

ChronObject ToString(ChronObject item)
{
	DynObject *obj = GetRef(item);
	switch (obj->type)
	{
	case vdeallocated:
		return DynString("Object is deallocated");
	case vstring:
		return DynString(obj->str);
	case vboolean:
		return DynString(obj->boolean ? "true" : "false");
	case vnumber:
	{
		int size = snprintf(NULL, 0, "%f", obj->number) + 1; // Determine the required size
		char *str = (char *)malloc(size * sizeof(char));	 // Dynamically allocate memory
		snprintf(str, size, "%f", obj->number);				 // Convert number to string
		return DynString(str);
	}
	case vinteger:
	{
		int size = snprintf(NULL, 0, "%d", obj->integer) + 1; // Determine the required size
		char *str = (char *)malloc(size * sizeof(char));		// Dynamically allocate memory
		snprintf(str, size, "%d", obj->integer);				// Convert integer to string
		return DynString(str);
	}
	case vtable:
		return DynString("table tostring not supported");
 	case vnull:
		return DynString("nil");
	default:
		return DynString("Invalid object type");
	}
}

void Throw(ChronObject errorMessage)
{
	// Placeholder for text formatting in red
	fprintf(stderr, "\033[31mRuntime Error:\033[0m %s\n", ((DynObject *)errorMessage->Object)->str);
	fprintf(stderr, "Terminated Program\n");
	exit(EXIT_FAILURE);
}

ChronObject ReadLine()
{
	const size_t buffer_size = 1024;
	char *buffer = malloc(buffer_size * sizeof(char));

	if (buffer == NULL)
	{
		Throw(DynString("Failed to allocate memory for buffer.\n"));
	}

	fgets(buffer, buffer_size, stdin);
	size_t len = strlen(buffer);

	// Remove trailing newline character if present
	if (len > 0 && buffer[len - 1] == '\n')
		buffer[len - 1] = '\0';

	return DynString(buffer);
}

bool GetBoolean(ChronObject o)
{
	bool result = ((DynObject *)o->Object)->boolean;
	MemoryContext_Release(o);
	return result;
}

ChronObject CreateMemoryContext() {
	return DynPointer(Create_MemoryContext());
}

ChronObject GetMemoryContext() {
	return DynPointer(Context);
}
void SetMemoryContext(ChronObject o) {
	DynObject* obj = o->Object;
	Context = (MemoryContext*)obj->ptr;
}