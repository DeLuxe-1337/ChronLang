@echo off

set COMPILER_PATH=clang.exe

set RUNTIME_FILES=Backend\memory.c Backend\object.c Backend\standard.c

if exist "%CHRON_NAME%.exe" (del %CHRON_NAME%.exe)

%COMPILER_PATH% -Wno-everything %CHRON_STATIC_LIBS% %CHRON_SOURCE_FILE% %RUNTIME_FILES% -o %CHRON_NAME%.exe

if exist "%CHRON_NAME%.exe" (
	%CHRON_NAME%.exe
) else (
	echo Compilation failed.
)