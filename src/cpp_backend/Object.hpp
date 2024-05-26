#ifndef OBJECT_HPP
#define OBJECT_HPP

enum ObjectType {
    vstring,
    vchar,
    vboolean,
    vnumber,
    vinteger,
    vnull,
    vptr,
    vtable,
    vdeallocated,
    vfunction,
};

namespace Object {

    class Object {
    protected:
        ObjectType type;
    public:
        Object() : type(vnull) {}

        Object(ObjectType type) : type(type) {

        }

        ~Object() {
        }

        virtual int getTypeRaw() const {
            return type;
        }

        virtual bool isEqual(const Object& other) const = 0;

        virtual bool operator==(const Object& other) const {
            return this->isEqual(other);
        }

        virtual std::unique_ptr<Object> toString() const = 0;
    };

}

#endif