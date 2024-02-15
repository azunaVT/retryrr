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
dbuild:
	docker buildx build -t retryrr -f Dockerfile . --no-cache
drun:
	docker run -it --rm -p 8888:8888 --name retryrr-dev retryrr