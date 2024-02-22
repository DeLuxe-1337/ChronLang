# ChronLang - Legacy Stage

> A fast, compiled, dynamically typed programming language.   

<a href="https://discord.gg/dhvnpnD7sn">
    <img src="https://img.shields.io/discord/1210018824322158602?logo=discord">
  </a>

## Features

- **Dynamically Typed**
- **Natively Compiled**
- **Manual Memory Management** OR **Garbage Collector** (Not Completed, might deprecate as I like the "auto memory release" for basic usage)

## Documentation

- [Hello, world](#hello-world)
- [Functions](#functions)
- [Defer](#defer)
- [Release](#release)
- [If statement](#if)
- [While statement](#while)
- [For to statement](#for-to)
- [Function calling](#function-calling)
- [Compound assignment operators](#compound-assignment-operators)
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

main :: {
  PrintLn("Hello, world")
}
```

## Functions

### Function syntax
Syntax:
`identifier` `::` (`(` `identifier array` `)`)? `statement block`

For context `::` stands for `CONSTANT`  

```chron
Sum :: (a, b) {
  return a + b
}
```
  
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
  Syntax: `if` `expression` `statement array` (`else` `statement block`)?

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
Syntax: `while` `expression` `statement block`

  ```chron
  include core.all

  main :: {
    x = 0

    while x < 5 {
      x = x + 1
    }
  }
  ```

## For To
Syntax: `for` `identifier` `=` `expression` `,` `expression` `statement block`  
```chron
include core.all

main :: {
  for i = 0, 100 {
    PrintLn(i)
  }
}
```

## Function calling
Same as most languages  
`identifier` `(` `expression array` `)`  

## Compound Assignment Operators
Same as most languages  
`expression` `+=` `expression`  
`expression` `-=` `expression`  
`expression` `*=` `expression`  
`expression` `/=` `expression`  
`expression` `%=` `expression`  

## Comparators
Same as most languages  
`expression` (`or` | `||`) `expression`  
`expression` (`and` | `&&`) `expression`  
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
`expression` `%` `expression`  

## Error handling
Still trying to figure out a good design
