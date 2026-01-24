# Stage 1: The Build Environment (uses the .NET SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the project file and restore all NuGet packages.
# This is done in a separate step to take advantage of Docker's layer caching.
COPY GCP_Pratice.csproj ./
RUN dotnet restore

# Copy the rest of the application's source code.
COPY . ./

# Build and publish the release version of the application.
RUN dotnet publish -c Release -o out

# Stage 2: The Final Production Image (uses the much smaller ASP.NET runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Set the entry point for the container to run the application.
ENTRYPOINT ["dotnet", "GCP_Pratice.dll"]