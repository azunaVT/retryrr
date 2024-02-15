FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /App
COPY --from=build /App/out .
EXPOSE 8888
RUN useradd retryrr
USER retryrr
ENTRYPOINT ["dotnet", "Retryrr.Console.dll"]