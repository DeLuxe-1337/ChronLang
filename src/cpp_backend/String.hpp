#ifndef STRING_HPP
#define STRING_HPP

#include "Include.hpp"

namespace Object {
    class String : public Object {
    public:
        std::string str;

        String(const std::string& s) : Object(vstring), str(s) {
        }

        bool isEqual(const Object& other) const override {
            const String* derivedOther = dynamic_cast<const String*>(&other);
            if (derivedOther) {
                return this->str == derivedOther->str;
            }
            return false;
        }

        std::unique_ptr<Object> toString() const override {
            return std::make_unique<String>(str);
        }
    };
}
#endif