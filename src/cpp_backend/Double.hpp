#ifndef DOUBLE_HPP
#define DOUBLE_HPP

#include "Include.hpp"

namespace Object {
    class Double : public Object {
    public:
        double number;

        Double(double d) : Object(vnumber), number(d) {
        }

        bool isEqual(const Object& other) const override {
            if (const Double* derivedOther = dynamic_cast<const Double*>(&other)) {
                return this->number == derivedOther->number;
            }
            if (const Integer* derivedOther = dynamic_cast<const Integer*>(&other)) {
                return this->number == derivedOther->integer;
            }
            return false;
        }

        std::unique_ptr<Object> toString() const override {
            return std::make_unique<String>(std::to_string(number));
        }

        int to_int() const override {
            return number;
        }

        double to_double() const override {
            return number;
        }
    };
}

namespace std {
    template<>
    struct hash<Object::Double> {
        std::size_t operator()(const Object::Double& d) const {
            using std::hash;
            return hash<double>()(d.number);
        }
    };
}

#endif