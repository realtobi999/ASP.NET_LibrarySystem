.PHONY: test

test:
	cd src/LibrarySystem.Tests && dotnet test

build:
	cd src && dotnet build

run:
	cd src && dotnet run