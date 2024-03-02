cls

set COMPILER_PATH=tcc\tcc
set COMMAND_LINE_ARGS=%*

set SOURCE_FILES=memory.c object.c standard.c

del *.exe

%COMPILER_PATH% %COMMAND_LINE_ARGS% %SOURCE_FILES%

%COMMAND_LINE_ARGS%.exe