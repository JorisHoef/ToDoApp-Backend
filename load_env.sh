#!/bin/bash

# Load the .env file and export the variables to the shell session
if [ -f .env ]; then
  # Export each line from the .env file
  export $(grep -v '^#' .env | xargs)
  echo "Environment variables loaded."
else
  echo ".env file not found."
fi
