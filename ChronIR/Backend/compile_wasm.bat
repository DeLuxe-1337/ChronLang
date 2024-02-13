@echo off

call emsdk\emsdk_env

set COMPILER_PATH=emcc
set COMMAND_LINE_ARGS=%*

set SOURCE_FILES=Backend\gc.c Backend\object.c Backend\standard.c

%COMPILER_PATH% %COMMAND_LINE_ARGS%.c %SOURCE_FILES% -o %COMMAND_LINE_ARGS%.html