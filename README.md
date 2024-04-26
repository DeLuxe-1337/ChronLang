# ChronLang - Bootstrapped version 
[Self compilation](#self-compilation)

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

[Legacy Branch](https://github.com/DeLuxe-1337/ChronLang/tree/legacy)  

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
- [Storing and calling functions at runtime](#invoking-functions-at-runtime)
- [Compound assignment operators](#compound-assignment-operators)
- [Comparators](#comparators)
- [Math](#math)
- [Error handling](#error-handling)

## FAQ

#### What is the bootstrapped compiler?
The bootstrapped compiler is the ChronLang compiler written in ChronLang

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
Visit the legacy branch to get the legacy compiler to compile this version...

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

Locally created variables are automatically released at the end of scope.    
Globally created variables must manually be released.  

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

x = 10 // x is a new object
y = 5 // y is a new object

z = x + y // z is a new object

// Note local variables are automatically released

Main :: {
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

Syntax: `{` `expression list` `}`;

// The syntax in bootstrap for tables is different... legacy uses <> while bootstrapped uses {}

You can also initialize a table with key, value pairs for example:  
`T = {"foo" = 5, ...}`

```chron
include core.all

Main :: {
    x = {"Hi", 1, false} // 0 = "Hi", 1 = 1, 2 = false
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
Syntax: `[` `(` `string` (`=` `string`)? `]`
Example:
```chron
["inline"]
Sum :: (a, b) { return a + b }
```
Modifiers can be used in compound for example:  
```chron
["inline", "return", "extern", "native", "name"="..."]
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

## Invoking Functions At Runtime
You can invoke functions at runtime using the `invoke` keyword.  
Syntax: `invoke` `function_sig` `call`  

Here's an example:  
```chron
Sum :: (a, b) {
    return a + b
}

Main :: {
    x = Sum
    PrintLn(invoke Sum x(5, 10))
}
```

You can even store functions in tables; here's an example:  
```chron
Sum :: (a, b) {
    return a + b
}

Main :: {
    x = <"Sum"=Sum>
    PrintLn(invoke Sum x["Sum"](5, 10))
}
```
Here's another example:  
```chron
Sum :: (a, b) {
    return a + b
}

Difference :: (a, b) {
    return a - b;
}

Main :: {
    x = <"Sum"=Sum, "Diff"=Difference>
    PrintLn(invoke Sum x["Sum"](5, 10))
    PrintLn(invoke Sum x["Diff"](5, 10)) // Doesn't matter if you use Sum or Difference as the function sig, as they're both essentially the same sig. (Return and parameters is what is important to a signature)
}
```


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

## Self Compilation
The bootstrapped compiler is self-compilable and passed all tests!

I am very stoked to have been able to reach this milestone.
