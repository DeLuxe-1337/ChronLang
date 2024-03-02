@echo off

set COMPILER_PATH=clang.exe
set COMMAND_LINE_ARGS=%*

if not exist "%COMPILER_PATH%" (
	rem For some reason it can't find clang.exe from path? had to do this hacky work around
	set COMPILER_PATH="C:\Program Files\LLVM\bin\clang.exe"
)

set SOURCE_FILES=Backend\memory.c Backend\object.c Backend\standard.c

del compiled.exe

%COMPILER_PATH% %COMMAND_LINE_ARGS% %SOURCE_FILES% -o compiled.exe

compiled.exe