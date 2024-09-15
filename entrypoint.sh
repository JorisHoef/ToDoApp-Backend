#!/bin/bash
set -e

# Run database migrations
dotnet ef database update --project /src/ToDoAppBackend.csproj

# Start the application
exec dotnet ToDoAppBackend.dll
