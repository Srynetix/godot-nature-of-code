all: help

help:
	@echo "Command list:"
	@echo "  - format"
	@echo "  - build"

format:
	@roslynator format 'Nature of Code.csproj'

build:
	@dotnet build