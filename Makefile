build:
	dotnet build
clean:
	dotnet clean
restore:
	dotnet restore
watch:
	dotnet watch --project ./src/Console/Retryrr.Console.csproj run
run:
	cd src/Console && dotnet run