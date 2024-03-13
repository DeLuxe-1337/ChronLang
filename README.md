# ChronLang - Legacy Stage

> A fast, compiled, dynamically typed programming language.   

<p float="left">
<a href="https://discord.gg/dhvnpnD7sn">
    <img src="https://img.shields.io/discord/1210018824322158602?logo=discord"></img>
  </a>

<img src="https://img.shields.io/github/commit-activity/m/DeLuxe-1337/ChronLang"></img>
</p>

## Features

- **Dynamically Typed**
- **Natively Compiled**
- **Manual Memory Management**

## Documentation

- [Hello, world](#hello-world)
- [Functions](#functions)
- [Variables](#variables)
- [Defer](#defer)
- [Release](#release)
- [If statement](#if)
- [While statement](#while)
- [For to statement](#for-to)
- [Foreach statement](#for-each)
- [Tables](#tables)
- [Include](#include)
- [Memory management](#memory-management)
- [Memory context handling](#memory-context-handling)
- [Function attributes](#function-attributes)
- [Function returning](#function-returning)
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
There are many ways you can contribute! You can work on the C backend, and you can implement features. You can clean up the compiler source. You can extend functionality, you can fix issues, and the list goes on.

#### How to compile?
```bat
git clone https://github.com/DeLuxe-1337/ChronLang
cd ChronLang
dotnet build
```
You can also use [Dotnet Native AOT](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot) if you wish.

#### Target Audience
The target audience is anyone who enjoys dynamically typed, lightweight, minimalistic, fast, and compiled languages. 

#### Goal
The goal of this programming language is to compile a dynamically typed nature. We often encounter dynamically typed languages that are interpreted, not true compilation.

## When was this project created?
1/21/24

## Authors

- [@DeLuxe](https://github.com/DeLuxe-1337)


## Examples

## Hello, world
```chron
// Hello, world
include core.all

Main :: {
  PrintLn("Hello, world")
}
```

## Functions

### Function syntax
Syntax:
`identifier` `::` (`(` `identifier array` `)`)? `statement block`

For context `::` signifies `constant`
All functions are considered `constant`/`::`

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

Main :: {
  Foo("Hello, world")
}
```

## Variables
Syntax: `identifier` `=` `expression`  

Variables can be used globally and locally...

```chron
global_variable = 100

Main :: {
    local_variable = 50
    PrintLn(global_variable)
    PrintLn(local_variable)
}
```

## Release
Syntax: `release` `expression`
- Releases an object from heap
- Release can be used as a statement or expression

Using release as a statement allows for compound releasing.  
`release x, y, z`

Example of release
```chron
include core.all

Main :: {
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
`defer` `expression`
- Defer is used as a statement

- Defer is used to postpone the execution of a statement until the end of the current scope.

Example of defer
```chron
include core.all

Main :: {
  defer PrintLn("End of program")

  PrintLn("Hello, world!")
}
```

## If
  Syntax: `if` `expression` `statement array` (`else` `statement block`)?

  ```chron
  include core.all

  Main :: {
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

  Main :: {
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

Main :: {
  for i = 0, 100 {
    PrintLn(i)
  }
}
```

## For Each
Syntax: `foreach` index=`identifier` `,` value=`identifier` `in` `iter` `statement block`  
```chron
include core.all

Main :: {
    x = <nil, true, 69, "Hello, world", false>

    foreach index, value in Table.Iter(x) {
        PrintLn(index)
        PrintLn(value)
    }
}
```

## Tables

Syntax: `<` `expression list` `>` //Subject to change;  

You can also initialize a table with key, value pairs for example:  
`T = <"foo" = 5, ...>`

```chron
include core.all

Main :: {
    x = <"Hi", 1, false> // 0 = "Hi", 1 = 1, 2 = false
    x[false] = true
    x["wow!"] = 100

    PrintLn(x[0])
    PrintLn(x[false])
    PrintLn(x["wow!"])
}
```

There's functions you can use for tables...  
`Table.SizeOf :: (table)` will return the length of the table  
`Table.Iter :: (table)` will return an iterable object for the table  

## Include
Syntax: `include` `identifier`  
For example `include core.all` will include `core/all.chron`  
You can write your own global "packages" if you store them in the compiler directory.

## Memory Management
At the moment you have to manually `release` variables.
Check out [Memory Context Handling](#memory-context-handling) to view how to use contexts

## Memory Context Handling
// Most of this might change, so it could be subject to change...  
The following functions are from memory.chron  
`Memory.CreateContext` -> Creates a new context    
`Memory.GetContext` -> Returns the current context in use     
`Memory.SetContext :: (obj)` -> Sets the current context in use   
`Memory.ReleaseAll` -> Releases everything from current context  
`Memory.Release :: (object)` -> Releases a specific object  

The following example will demonstrate using contexts:  
```chron
include core.all

Main :: {
    oldContext = Memory.GetContext() // Store current context

    x = "X variable!" // Create the variable on oldContext

    newContext = Memory.CreateContext() // Create a new context
    Memory.SetContext(newContext) // Set the current context to our new context

    PrintLn("Hello, from the new context!"); // This will allocate, and deallocate the string
    y = "Bye!" // This variable is allocated on the new context
    
    Memory.ReleaseContext(newContext) // Release everything from newContext as well as the MemoryContext itself
    Memory.SetContext(oldContext) // Return the context back to our old state

    PrintLn(y) // Not valid
    PrintLn(x) // Valid
}
```

## Function attributes
Syntax: `$` `(` `string` (`=` `string`)? `)`
Example:
```chron
$("inline")
Sum :: (a, b) { return a + b }
```

The built in attributes there are:
- `inline` -> inlines a function
- `extern` -> declares a function as extern
- `return`=?... -> declares a function to return; used if there's no function body and you want it to return (or for native function)
- `name`=... -> override the scope name for a function
- `parameters`=... -> overrides a functions parameters; currently used to interop with native functions (subject to change)
- `native` -> declares a function to be native (which means it's basically just declared like a regular c function)

## Function returning
Same as most langauges
`return` `expression`

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
