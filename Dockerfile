# Use the .NET SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["ToDoAppBackend.csproj", "./"]
RUN dotnet restore

# Copy the rest of the code and build
COPY . ./
RUN dotnet publish "ToDoAppBackend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the ASP.NET Core runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy the published app from the build stage
COPY --from=build /app/publish .

# Set the entry point to run the app
ENTRYPOINT ["dotnet", "ToDoAppBackend.dll"]