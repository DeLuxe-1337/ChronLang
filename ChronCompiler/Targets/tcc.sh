#!/bin/bash

COMPILER_PATH=tcc
RUNTIME_FILES="$CHRON_BACKEND/memory.c $CHRON_BACKEND/object.c $CHRON_BACKEND/standard.c"

if [ -f "$CHRON_NAME" ]; then
    rm "$CHRON_NAME"
fi

$COMPILER_PATH "$CHRON_SOURCE_FILE" $RUNTIME_FILES -o "$CHRON_NAME"

if [ -f "$CHRON_NAME" ]; then
    "./$CHRON_NAME"
else
    echo "Compilation failed."
fi