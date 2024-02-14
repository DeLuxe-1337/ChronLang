# ChronLang - Legacy Compiler

A fast, dynamically typed programming language.

## Features

- **Dynamically Typed**
- **Natively Compiled**
- **Manual Memory Management** OR **Garbage Collector**

## Examples

```chron
// Hello, world
include core.all

#gc // Very experimental feature
!main :: {
  PrintLn("Hello, world")
}
```

```chron
// Hello, world with manual memory management
include core.all

!main :: {
  PrintLn(release "Hello, world")
}
```

## FAQ

#### What is the legacy compiler?
The legacy compiler is the initial version of ChronLang. It will shed the "legacy" label once it is bootstrapped and reaches a stable state.

#### How mature is this programming language?
ChronLang is currently in its early stages of development and may not be suitable for production-level projects.

#### Is the syntax subject to change?
Yes, the syntax of ChronLang may undergo changes as the language evolves.

#### Why does the (ChronLang) source compiler have some unconventional code sometimes?
As a solo developer on this project, I occasionally prioritize functionality over code aesthetics. If you notice any areas for improvement and would like to contribute, your input and contributions are highly valued and encouraged!

#### Will there be bugs?
Probably, as I mentioned above this is a solo project at the moment!

#### How does this compiler achieve native compilation?
By generating C source code, thus allows for easy cross-platform with compilers like clang, tcc, gcc, or msvc.

#### What can I do to contribute?
There's many ways you can contribute! You can work on the C backend (most needed), you can implement features (right now the GC is in need of proper implementation). You can clean up the compiler source. You can extend functionality, you can fix issues, the list goes on.

## When was this project created?
1/21/24

## Authors

- [@DeLuxe](https://github.com/DeLuxe-1337)
