@echo off

set COMPILER_PATH=tcc.exe

set RUNTIME_FILES=Backend\memory.c Backend\object.c Backend\standard.c

%COMPILER_PATH% %CHRON_SOURCE_FILE% %RUNTIME_FILES% -o %CHRON_NAME%.exe