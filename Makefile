ifeq ($(OS),Windows_NT)
    CLEAR = cls
else
    CLEAR = clear
endif


test:
	${CLEAR}
	cd src/LibrarySystem.Tests && dotnet test

build:
	${CLEAR}
	cd src && dotnet build

run:
	${CLEAR}
	cd src/LibrarySystem.Presentation && dotnet run