#ifndef POINTER_HPP
#define POINTER_HPP

#include "Include.hpp"

namespace Object {
    class Pointer : public Object {
    public:
        void* pointer;

        Pointer(void* ptr) : Object(vptr), pointer(ptr) {
        }

        ~Pointer() override
        {
            delete pointer;
        }

        bool isEqual(const Object& other) const override {
            const Pointer* derivedOther = dynamic_cast<const Pointer*>(&other);
            if (derivedOther) {
                return this->pointer == derivedOther->pointer;
            }
            return false;
        }

        std::unique_ptr<Object> toString() const override {
            std::stringstream ss;
            ss << "pointer %" << this->pointer;
            return std::make_unique<String>(ss.str());
        }

        void* to_pointer() const override {
            return pointer;
        }
    };
}
#endif