#!/bin/bash

COMPILER_PATH=tcc
RUNTIME_FILES="Backend/memory.c Backend/object.c Backend/standard.c"

if [ -f "$CHRON_NAME" ]; then
    rm "$CHRON_NAME"
fi

$COMPILER_PATH "$CHRON_SOURCE_FILE" $RUNTIME_FILES -o "$CHRON_NAME"

if [ -f "$CHRON_NAME" ]; then
    "./$CHRON_NAME"
else
    echo "Compilation failed."
fi