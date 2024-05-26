#ifndef INTEGER_HPP
#define INTEGER_HPP

#include "Object.hpp"

class IntegerObject : public Object {
public:
    int integer;

    IntegerObject(int i) : Object(vinteger), integer(i) {
    }
};

#endif