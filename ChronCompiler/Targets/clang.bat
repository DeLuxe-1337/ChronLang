@echo off

set COMPILER_PATH=clang.exe

set RUNTIME_FILES=%CHRON_BACKEND%\memory.c %CHRON_BACKEND%\object.c %CHRON_BACKEND%\standard.c

if exist "%CHRON_NAME%.exe" (del %CHRON_NAME%.exe)

%COMPILER_PATH% -I"%CHRON_BACKEND%" -I"%CHRON_WORKING_DIR%" -Wno-everything %CHRON_STATIC_LIBS% %CHRON_SOURCE_FILE% %RUNTIME_FILES% -o %CHRON_NAME%.exe

if exist "%CHRON_NAME%.exe" (
	%CHRON_NAME%.exe
) else (
	echo Compilation failed.
)