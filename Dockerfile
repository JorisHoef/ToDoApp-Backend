# Use the .NET SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["ToDoAppBackend.csproj", "./"]
RUN dotnet restore
RUN dotnet tool install --global dotnet-ef

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

# Copy the HTTPS certificate into the container
COPY ./https_certificate.pfx /https_certificate.pfx

# Set environment variables for the HTTPS certificate
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https_certificate.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__KeyPassword=jemoeder1

# Set the entry point to run the app
ENTRYPOINT ["dotnet", "ToDoAppBackend.dll"]