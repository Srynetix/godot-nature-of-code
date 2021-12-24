MSBUILD_PATH := "C:\\Program Files\\dotnet\\sdk\\3.1.416"
ANALYZER_ASSEMBLIES := "C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\BuildTools\\Common7\\IDE\\CommonExtensions\\Microsoft\\ManagedLanguages\\VBCSharp\\LanguageServices"

_all:
    @just --list

# Format code
fmt:
    roslynator format "Nature of Code.csproj" --msbuild-path "{{ MSBUILD_PATH }}"

# Lint code
lint:
    roslynator analyze "Nature of Code.csproj" --msbuild-path "{{ MSBUILD_PATH }}" --analyzer-assemblies "{{ ANALYZER_ASSEMBLIES }}"

# Lint code with autofix when possible
fix-lint:
    roslynator fix "Nature of Code.csproj" --msbuild-path "{{ MSBUILD_PATH }}" --analyzer-assemblies "{{ ANALYZER_ASSEMBLIES }}"