.PHONY: test

test:
	clear
	cd src/LibrarySystem.Tests && dotnet test

build:
	cd src && dotnet build

run:
	cd src/LibrarySystem.Presentation && dotnet run