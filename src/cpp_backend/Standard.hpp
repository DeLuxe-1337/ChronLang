#ifndef STANDARD_HPP
#define STANDARD_HPP

#include "Include.hpp"

void print(Object::Object* object) {
	auto stringObjectPointer = object->toString();
	Object::String* stringObject = dynamic_cast<Object::String*>(stringObjectPointer.get());
	if (stringObject) {
		std::cout << stringObject->str;
	}
	else {
		std::cout << "Failed to print\n";
	}
}


void print_line(Object::Object* object) {
	print(object);
	std::cout << "\n";
}

#endif