@echo off

if not exist "emsdk" (
	git clone https://github.com/emscripten-core/emsdk.git
	cd emsdk
	call emsdk.bat install latest
	call emsdk.bat activate latest
	cd ..
)

call emsdk\emsdk_env

set COMPILER_PATH=emcc

set RUNTIME_FILES=%CHRON_BACKEND%\memory.c %CHRON_BACKEND%\object.c %CHRON_BACKEND%\standard.c

%COMPILER_PATH% %CHRON_SOURCE_FILE% %RUNTIME_FILES%