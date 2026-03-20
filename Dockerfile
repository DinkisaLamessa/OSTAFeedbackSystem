# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file(s)
COPY ["YourProject.csproj", "YourProject/"]
RUN dotnet restore "YourProject/YourProject.csproj"

# Copy all source files and build
COPY . .
WORKDIR "/src/YourProject"
RUN dotnet publish "YourProject.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy published application
COPY --from=build /app/publish .

# Configure the entry point
ENTRYPOINT ["dotnet", "YourProject.dll"]
