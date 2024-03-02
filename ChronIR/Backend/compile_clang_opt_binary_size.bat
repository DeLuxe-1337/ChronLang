@echo off

set COMPILER_PATH=clang.exe
set COMMAND_LINE_ARGS=%*

set SOURCE_FILES=Backend\memory.c Backend\object.c Backend\standard.c

del %COMMAND_LINE_ARGS%.exe

%COMPILER_PATH% -Os -ffunction-sections -fdata-sections -Wl %COMMAND_LINE_ARGS% %SOURCE_FILES% -o %COMMAND_LINE_ARGS%.exe

%COMMAND_LINE_ARGS%.exe