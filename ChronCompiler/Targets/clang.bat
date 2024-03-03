@echo off

set COMPILER_PATH=clang.exe

set RUNTIME_FILES=Backend\memory.c Backend\object.c Backend\standard.c

%COMPILER_PATH% -Wno-everything %CHRON_SOURCE_FILE% %RUNTIME_FILES% -o %CHRON_NAME%.exe