all: help

help:
	@echo "Command list:"
	@echo "  - format"
	@echo "  - build"

format:
	@dotnet format 'Nature of Code.csproj'

build:
	@dotnet build