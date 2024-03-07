@echo off

rem Batch is weird, ChatGPT wrote this part because I couldn't do `if exist emsdk_env.bat (...)`

call emsdk_env.bat >nul 2>&1

if errorlevel 1 (
	if not exist "emsdk" (
		git clone https://github.com/emscripten-core/emsdk.git
		cd emsdk
		call emsdk.bat install latest
		call emsdk.bat activate latest
		cd ..
	)

	call emsdk\emsdk_env
)

set COMPILER_PATH=emcc

set RUNTIME_FILES=%CHRON_BACKEND%\memory.c %CHRON_BACKEND%\object.c %CHRON_BACKEND%\standard.c

if "%CHRON_JS_LIBRARY%" == "" ( 
	set CHRON_JS_LIBRARY="wasmLibrary.js" 
)

%COMPILER_PATH% -s LINKABLE=1 -s EXPORT_ALL=1 -s EXPORTED_RUNTIME_METHODS=ccall,cwrap %CHRON_SOURCE_FILE% %RUNTIME_FILES% -o %CHRON_NAME%.js --js-library %CHRON_JS_LIBRARY%
