#ifndef INTEGER_HPP
#define INTEGER_HPP

#include "Include.hpp"

namespace Object {
    class Integer : public Object {
    public:
        int integer;

        Integer(int i) : Object(vinteger), integer(i) {
        }

        bool isEqual(const Object& other) const override {
            const Integer* derivedOther = dynamic_cast<const Integer*>(&other);
            if (derivedOther) {
                return this->integer == derivedOther->integer;
            }
            return false;
        }

        std::unique_ptr<Object> toString() const override {
            return std::make_unique<String>(std::to_string(integer));
        }

        int to_int() const override {
            return integer;
        }
    };
}

namespace std {
    template<>
    struct hash<Object::Integer> {
        std::size_t operator()(const Object::Integer& i) const {
            using std::hash;
            return hash<int>()(i.integer);
        }
    };
}

#endif