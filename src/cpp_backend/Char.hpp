#ifndef CHAR_HPP
#define CHAR_HPP

#include "Include.hpp"

namespace Object {
    class Char : public Object {
    public:
        char character;

        Char(char c) : Object(vchar), character(c) {
        }

        bool isEqual(const Object& other) const override {
            const Char* derivedOther = dynamic_cast<const Char*>(&other);
            if (derivedOther) {
                return this->character == derivedOther->character;
            }
            return false;
        }

        std::unique_ptr<Object> toString() const override {
            return std::make_unique<String>(std::to_string(character));
        }
    };
}

#endif