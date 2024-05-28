#ifndef BOOLEAN_HPP
#define BOOLEAN_HPP

#include "Include.hpp"

namespace Object {
    class Boolean : public Object {
    public:
        bool boolean;

        Boolean(bool b) : Object(vboolean), boolean(b) {
        }

        bool isEqual(const Object& other) const override {
            const Boolean* derivedOther = dynamic_cast<const Boolean*>(&other);
            if (derivedOther) {
                return this->boolean == derivedOther->boolean;
            }
            return false;
        }

        std::unique_ptr<Object> toString() const override {
            return std::make_unique<String>(this->boolean == true ? "true" : "false");
        }

        bool to_bool() const override {
            return boolean;
        }
    };
}

namespace std {
    template<>
    struct hash<Object::Boolean> {
        std::size_t operator()(const Object::Boolean& b) const {
            using std::hash;
            return hash<bool>()(b.boolean);
        }
    };
}

#endif