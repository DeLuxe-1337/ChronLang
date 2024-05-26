#ifndef STRING_HPP
#define STRING_HPP

#include "Object.hpp"
#include <string>

class StringObject : public Object {
public:
    std::string str;

    StringObject(const std::string& s) : Object(vstring), str(s) {
    }
};

#endif