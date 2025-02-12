# Use ASP.NET Core runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ExpenseService/ExpenseService.csproj", "ExpenseService/"]
RUN dotnet restore "ExpenseService/ExpenseService.csproj"
COPY . .
WORKDIR "/src/ExpenseService"
RUN dotnet build "ExpenseService.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "ExpenseService.csproj" -c Release -o /app/publish

# Final stage: Create runtime container
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpenseService.dll"]
