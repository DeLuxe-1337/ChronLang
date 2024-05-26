#ifndef STANDARD_HPP
#define STANDARD_HPP

#include "Include.hpp"
#include <iostream>

void print(Object* object) {
    switch (object->getTypeRaw()) {
    case vstring: {
        StringObject* String = static_cast<StringObject*>(object);
        std::cout << String->str;
        break;
    }
    case vinteger: {
        IntegerObject* Integer = static_cast<IntegerObject*>(object);
        std::cout << Integer->integer;
        break;
    }
    default: {
        std::cout << "Unknown type";
        break;
    }
    }
}


void print_line(Object* object) {
	print(object);
	std::cout << "\n";
}

#endif