#!/bin/bash

COMPILER_PATH=tcc
RUNTIME_FILES="memory.c object.c standard.c"

if [ -f "$CHRON_NAME.exe" ]; then
    rm "$CHRON_NAME.exe"
fi

$COMPILER_PATH "$CHRON_SOURCE_FILE" $RUNTIME_FILES -o "$CHRON_NAME.exe"

if [ -f "$CHRON_NAME.exe" ]; then
    "./$CHRON_NAME.exe"
else
    echo "Compilation failed."
fi