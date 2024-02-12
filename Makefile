build:
	dotnet build
clean:
	dotnet clean
restore:
	dotnet restore
watch:
	dotnet watch --project ./src/Console/Retryrr.Console.csproj run
run:
	dotnet run --project src/Console/Retryrr.Console.csproj
docker-build:
	docker buildx build -t retryrr -f Dockerfile . --no-cache
docker-run:
	docker run -it --rm retryrr --publish-all --name retryrr-dev