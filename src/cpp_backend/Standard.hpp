#ifndef STANDARD_HPP
#define STANDARD_HPP

#include "Include.hpp"
#include <iostream>

void print(const Object& object) {
    switch (object.getTypeRaw()) {
    case vstring: {
        const StringObject& String = static_cast<const StringObject&>(object);
        std::cout << String.str;
        break;
    }
    case vinteger: {
        const IntegerObject& Integer = static_cast<const IntegerObject&>(object);
        std::cout << Integer.integer;
        break;
    }
    default: {
        std::cout << "Unknown type";
        break;
    }
    }
}


void print_line(const Object& object) {
	print(object);
	std::cout << "\n";
}

#endif