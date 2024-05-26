#ifndef OBJECT_HPP
#define OBJECT_HPP

enum ObjectType {
    vstring,
    vboolean,
    vnumber,
    vinteger,
    vnull,
    vptr,
    vtable,
    vdeallocated,
    vfunction,
};

#define ConstObject const Object&

class Object {
protected:
    ObjectType type;
public:
    Object() : type(vnull) {}

    Object(ObjectType type) : type(type) {

    }

    ~Object() {
        std::cout << "Deallocate\n";
    }

    virtual int getTypeRaw() const {
        return type;
    }
};

#endif