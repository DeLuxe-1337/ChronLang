#!/bin/bash

COMPILER_PATH=clang

RUNTIME_FILES="$CHRON_BACKEND/memory.c $CHRON_BACKEND/object.c $CHRON_BACKEND/standard.c"

if [ -f "$CHRON_NAME" ]; then
    rm "$CHRON_NAME"
fi

$COMPILER_PATH -I"$CHRON_BACKEND" -I"$CHRON_WORKING_DIR" -Wno-everything $CHRON_STATIC_LIBS $CHRON_SOURCE_FILE $RUNTIME_FILES -o "$CHRON_NAME"

if [ -f "$CHRON_NAME" ]; then
    "./$CHRON_NAME"
else
    echo "Compilation failed."
fi
