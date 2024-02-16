# ChronLang - Legacy Stage

> A fast, compiled, dynamically typed programming language.

## Features

- **Dynamically Typed**
- **Natively Compiled**
- **Manual Memory Management** OR **Garbage Collector**

## Documentation

- [Hello, world](#hello-world)
- [Functions](#functions)
- [Defer](#defer)
- [Release](#release)
- [If statement](#if)
- [While statement](#while)
- [Function calling](#function-calling)
- [Comparators](#comparators)
- [Math](#math)
- [Error handling](#error-handling)

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
Generating C source code thus allows for easy cross-platform with compilers like clang, tcc, gcc, or msvc.

#### What can I do to contribute?
There are many ways you can contribute! You can work on the C backend (most needed), and you can implement features (right now the GC is in need of proper implementation). You can clean up the compiler source. You can extend functionality, you can fix issues, and the list goes on.

## When was this project created?
1/21/24

## Authors

- [@DeLuxe](https://github.com/DeLuxe-1337)


## Examples

## Hello, world
```chron
// Hello, world
include core.all

#gc // Experimental feature
main :: {
  PrintLn("Hello, world")
}
```

```chron
// Hello, world with manual memory management
include core.all

main :: {
  PrintLn("Hello, world") // is the same as PrintLn(release "Hello, world")
}
```

## Functions

### Function syntax
Syntax:
`identifier` `::` (`(` `identifier array` `)`)? `statement array`

For context `::` stands for `CONSTANT`  
  
```chron
  include core.all 

  Foo :: (bar) {
    PrintLn(bar)
  }

  main :: {
    Foo("Hello, world")
  }
```

## Release
Syntax: 
`release` `expression | statement`

- Releases an object from heap
- Release can be used as a statement or expression  

Example of release
```chron
  include core.all

  main :: {
    x = 10 // x is a new object
    y = 5 // y is a new object

    z = x + y // z is a new object

    PrintLn(release z) // Release Z from memory after using it

    release x // Release x from memory
    release y // Release y from memory
  }
```

## Defer
Syntax:
`defer` `statement`

- Defer is used to postpone the execution of a statement until the end of the current scope.

Example of defer
```chron
  include core.all

  main :: {
    defer PrintLn("End of program")

    PrintLn("Hello, world!")
  }
```

## If
  Syntax: `if` `expression` `statement array` (`else` `statement array`)?

  ```chron
    include core.all

    main :: {
      x = true
      y = false

      if x == y {
        ...
      }
      else {
        ...
      }
    }
  ```

## While
Syntax: `while` `expression` `statement array`

  ```chron
    include core.all

    main :: {
      x = 0

      while x < 5 {
        x = x + 1
      }
    }
  ```

## Function calling
`identifier` `(` `expression array` `)`  
Same as most languages  

## Comparators
Same as most languages  
`expression` (`or` | `||`) `expression`  
`expression` (`and` | `and`) `expression`  
`expression` `==` `expression`  
`expression` `!=` `expression`  
`expression` `<=` `expression`  
`expression` `>=` `expression`  
`expression` `>` `expression`  
`expression` `<` `expression`  

## Math
Same as most languages (basic, no standard math library yet)  
`expression` `+` `expression`  
`expression` `-` `expression`  
`expression` `*` `expression`  
`expression` `/` `expression`  

## Error handling
Still trying to figure out a good design