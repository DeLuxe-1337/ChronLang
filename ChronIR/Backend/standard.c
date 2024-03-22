#include "standard.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

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
		int len = strlen(left->data.str) + strlen(right->data.str) + 1;
		char *result = (char *)malloc(len * sizeof(char));

		if (result == NULL)
		{
			printf("Memory allocation failed.\n");
			return DynString("Failed to concat");
		}
		strcpy(result, left->data.str);
		strcat(result, right->data.str);
		ChronObject ConcatStr = DynString(result);
		free(result);
		return ConcatStr;
	}

	if (left->type == vinteger)
	{
		return DynInteger(left->data.integer + right->data.integer);
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
		return DynInteger(left->data.integer - right->data.integer);
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
		return DynInteger(left->data.integer / right->data.integer);
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
		return DynInteger(left->data.integer * right->data.integer);
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
		return DynInteger(left->data.integer % right->data.integer);
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
		return DynBoolean(left->data.integer > right->data.integer);
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
		return DynBoolean(left->data.integer >= right->data.integer);
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
		return DynBoolean(left->data.integer <= right->data.integer);
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
		return DynBoolean(left->data.integer < right->data.integer);
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
		return DynBoolean(strcmp(left->data.str, right->data.str) == 0);
	case vboolean:
		return DynBoolean(left->data.boolean == right->data.boolean);
	case vnumber:
		return DynBoolean(left->data.number == right->data.number);
	case vinteger:
		return DynBoolean(left->data.integer == right->data.integer);
	case vfunction:
	case vptr:
		return DynBoolean(left->data.ptr == right->data.ptr);
	case vdeallocated:
		return DynBoolean(false);
	default:
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
		return DynBoolean(strcmp(left->data.str, right->data.str) != 0);
	case vboolean:
		return DynBoolean(left->data.boolean != right->data.boolean);
	case vnumber:
		return DynBoolean(left->data.number != right->data.number);
	case vinteger:
		return DynBoolean(left->data.integer != right->data.integer);
	case vfunction:
	case vptr:
		return DynBoolean(left->data.ptr == right->data.ptr);
	case vdeallocated:
		return DynBoolean(false);
	default:
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

	return DynBoolean(left->data.boolean || right->data.boolean);
}
ChronObject DynObjectCompareAnd(ChronObject o1, ChronObject o2)
{
	DynObject *left = o1->Object;
	DynObject *right = o2->Object;

	if (left->type != vboolean && left->type != right->type)
	{
		return DynBoolean(false);
	}

	return DynBoolean(left->data.boolean && right->data.boolean);
}
ChronObject DynObjectNot(ChronObject o)
{
	DynObject *left = o->Object;

	if (left->type != vboolean)
	{
		return DynBoolean(false);
	}

	return DynBoolean(!left->data.boolean);
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
	case vptr:
		typeStr = "pointer";
		break;
	case vfunction:
		typeStr = "function";
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
		return DynString(obj->data.str);
	case vboolean:
		return DynString(obj->data.boolean ? "true" : "false");
	case vnumber:
	{
		int size = snprintf(NULL, 0, "%f", obj->data.number) + 1; // Determine the required size
		char *str = (char *)malloc(size * sizeof(char));		  // Dynamically allocate memory
		snprintf(str, size, "%f", obj->data.number);			  // Convert number to string
		return DynString(str);
	}
	case vinteger:
	{
		int size = snprintf(NULL, 0, "%d", obj->data.integer) + 1; // Determine the required size
		char *str = (char *)malloc(size * sizeof(char));		   // Dynamically allocate memory
		snprintf(str, size, "%d", obj->data.integer);			   // Convert integer to string
		return DynString(str);
	}
	case vtable:
	{
		DynamicTable *table = obj->data.table;

		/*
			The following was written and compiled by ChronLang (besides the loop, no way to iter tables without an index as of now)
		*/

		ChronObject _V_str = DynString("<\n");

		for (int i = 0; i < table->size; i++)
		{
			ChronObject _TEMP_V0 = _V_str;
			ChronObject _TEMP_V8 = DynString("\t[");
			_V_str = DynObjectAdd(_TEMP_V0, _TEMP_V8);
			ChronObject _TEMP_V1 = _V_str;
			ChronObject _TEMP_V9 = ToString(table->pairs[i].key);
			_V_str = DynObjectAdd(_TEMP_V1, _TEMP_V9);
			ChronObject _TEMP_V2 = _V_str;
			ChronObject _TEMP_V10 = DynString("] = ");
			_V_str = DynObjectAdd(_TEMP_V2, _TEMP_V10);
			ChronObject _TEMP_V3 = _V_str;
			ChronObject _TEMP_V11 = ToString(table->pairs[i].value);
			_V_str = DynObjectAdd(_TEMP_V3, _TEMP_V11);
			ChronObject _TEMP_V4 = _V_str;
			ChronObject _TEMP_V12 = DynString(",\n");
			_V_str = DynObjectAdd(_TEMP_V4, _TEMP_V12);
			MemoryContext_Release(_TEMP_V0);
			MemoryContext_Release(_TEMP_V8);
			MemoryContext_Release(_TEMP_V1);
			MemoryContext_Release(_TEMP_V9);
			MemoryContext_Release(_TEMP_V2);
			MemoryContext_Release(_TEMP_V10);
			MemoryContext_Release(_TEMP_V3);
			MemoryContext_Release(_TEMP_V11);
			MemoryContext_Release(_TEMP_V4);
			MemoryContext_Release(_TEMP_V12);
		}
		ChronObject _TEMP_V5 = _V_str;
		ChronObject _TEMP_V13 = DynString(">");
		_V_str = DynObjectAdd(_TEMP_V5, _TEMP_V13);
		MemoryContext_Release(_TEMP_V5);
		MemoryContext_Release(_TEMP_V13);

		return _V_str;
	}
	case vptr:
		return DynString("pointer");
	case vfunction:
		return DynString("function");
	case vnull:
		return DynString("nil");
	default:
		return DynString("Invalid object type");
	}
}

void Throw(ChronObject errorMessage)
{
	// Placeholder for text formatting in red
	fprintf(stderr, "\033[31mRuntime Error:\033[0m %s\n", ((DynObject *)errorMessage->Object)->data.str);
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
	bool result = ((DynObject *)o->Object)->data.boolean;
	MemoryContext_Release(o);
	return result;
}

ChronObject CreateMemoryContext()
{
	return DynPointer(Create_MemoryContext());
}

ChronObject GetMemoryContext()
{
	return DynPointer(Context);
}
void SetMemoryContext(ChronObject o)
{
	DynObject *obj = o->Object;
	Context = (MemoryContext *)obj->data.ptr;
}

void ReleaseMemoryContext(ChronObject o)
{
	DynObject *obj = o->Object;
	MemoryContext *ctx = obj->data.ptr;
	MemoryContext_ReleaseContext(ctx);
}

int c_int(ChronObject o)
{
	DynObject *obj = o->Object;
	return obj->data.integer;
}
const char *c_string(ChronObject o)
{
	DynObject *obj = o->Object;
	return obj->data.str;
}
bool c_bool(ChronObject o)
{
	DynObject *obj = o->Object;
	return obj->data.boolean;
}

void *c_pointer(ChronObject o)
{
	DynObject *obj = o->Object;
	return obj->data.ptr;
}

ChronObject TableSizeOf(ChronObject o)
{
	DynObject *obj = o->Object;
	DynamicTable *table = obj->data.table;

	return DynInteger(table->size);
}

void dealloc_iter(void *o)
{
	DynObject *obj = o;
	Iterator *iter = obj->data.ptr;
	free(iter);
}

ChronObject TableIterIndex(void *self, int index)
{
	DynamicTable *table = self;
	return Clone(table->pairs[index].key);
}

ChronObject TableIterValue(void *self, int index)
{
	DynamicTable *table = self;
	return Clone(table->pairs[index].value);
}

ChronObject TableIter(ChronObject o)
{
	DynObject *object = o->Object;
	DynamicTable *table = object->data.table;

	Iterator *iter = malloc(sizeof(Iterator));
	iter->self = table;
	iter->size = table->size;
	iter->index = TableIterIndex;
	iter->value = TableIterValue;

	ChronObject iterPointer = DynPointer(iter);
	iterPointer->deallocate = dealloc_iter;

	return iterPointer;
}

ChronObject StringIterIndex(void *self, int index)
{
	return DynInteger(index);
}

ChronObject StringIterValue(void *self, int index)
{
	const char *str = self;
	return DynChar(str[index]);
}

ChronObject StringIter(ChronObject o)
{
	DynObject *object = o->Object;
	const char *str = object->data.str;

	Iterator *iter = malloc(sizeof(Iterator));
	iter->self = str;
	iter->size = (int)strlen(str);
	iter->index = StringIterIndex;
	iter->value = StringIterValue;

	ChronObject iterPointer = DynPointer(iter);
	iterPointer->deallocate = dealloc_iter;

	return iterPointer;
}

ChronObject StringContains(ChronObject source, ChronObject target)
{
	char *found = strstr(c_string(source), c_string(target));
	if (found != NULL)
		return DynBoolean(true);

	return DynBoolean(false);
}

ChronObject StringIsAlpha(ChronObject source)
{
	const char *str = c_string(source);
	for (size_t i = 0; i < strlen(str); i++)
	{
		char check = str[i];
		if (isalpha(check) == false)
			return DynBoolean(false);
	}

	return DynBoolean(true);
}

ChronObject StringIsAlphaNumeric(ChronObject source)
{
	const char *str = c_string(source);
	for (size_t i = 0; i < strlen(str); i++)
	{
		char check = str[i];
		if (isalnum(check) == false)
			return DynBoolean(false);
	}

	return DynBoolean(true);
}

ChronObject StringIsNumeric(ChronObject source)
{
	const char *str = c_string(source);
	for (size_t i = 0; i < strlen(str); i++)
	{
		char check = str[i];
		if (isdigit(check) == false)
			return DynBoolean(false);
	}

	return DynBoolean(true);
}

ChronObject StringIsWhitespace(ChronObject source)
{
	const char *str = c_string(source);
	for (size_t i = 0; i < strlen(str); i++)
	{
		char check = str[i];
		if (isspace(check) == false)
			return DynBoolean(false);
	}

	return DynBoolean(true);
}

ChronObject StringLength(ChronObject source)
{
	return DynInteger(strlen(c_string(source)));
}

ChronObject StringIndex(ChronObject source, ChronObject index)
{
	char x = c_string(source)[c_int(index)];
	return DynString(&x);
}

int c_object_type(ChronObject o)
{
	DynObject *object = o->Object;
	return object->type;
}

int c_table_size(ChronObject o)
{
	DynObject *object = o->Object;
	DynamicTable *table = object->data.table;
	return table->size;
}

ChronObject c_table_key(int index, ChronObject o)
{
	DynObject *object = o->Object;
	DynamicTable *table = object->data.table;
	ChronObject key = table->pairs[index].key;
	return key;
}

ChronObject c_table_value(int index, ChronObject o)
{
	DynObject *object = o->Object;
	DynamicTable *table = object->data.table;
	ChronObject value = table->pairs[index].value;
	return value;
}