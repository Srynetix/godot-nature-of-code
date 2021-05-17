#!/usr/bin/env python3
#
# Needs `doxygen` and `godot` in PATH,
# or you can use `DOXYGEN_EXE` and `GODOT_EXE` environment variables.

import argparse
import os
import shutil
import subprocess
import sys

BASE_DIR = os.path.dirname(os.path.normpath(os.path.abspath(__file__)))
ANALYZER_ASSEMBLIES = "C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\BuildTools\\Common7\\IDE\\CommonExtensions\\Microsoft\\ManagedLanguages\\VBCSharp\\LanguageServices"

def execute(exe, *args):
    subprocess.call([exe] + list(args))

parser = argparse.ArgumentParser(description="Makefile")
subs = parser.add_subparsers(dest="cmd")
subs.add_parser("format")
subs.add_parser("lint")
subs.add_parser("fix-lint")
subs.add_parser("export")

args = parser.parse_args()
if args.cmd == "format":
    execute("roslynator", "format", "Nature of Code.csproj")
    print("Ok!")

elif args.cmd == "lint":
    execute("roslynator", "analyze", "Nature of Code.csproj", "--analyzer-assemblies", ANALYZER_ASSEMBLIES)

elif args.cmd == "fix-lint":
    execute("roslynator", "fix", "Nature of Code.csproj", "--analyzer-assemblies", ANALYZER_ASSEMBLIES)

elif args.cmd == "export":
    doxygen_exe = os.environ.get("DOXYGEN_EXE", "doxygen")
    godot_exe = os.environ.get("GODOT_EXE", "godot")

    # HTML5
    os.makedirs(os.path.join(BASE_DIR, "exports", "html"), exist_ok=True)
    execute(godot_exe, "--export", "HTML5", os.path.join(BASE_DIR, "exports", "html", "index.html"))
    
    # Android
    os.makedirs(os.path.join(BASE_DIR, "exports", "android"), exist_ok=True)
    execute(godot_exe, "--export-debug", "Android", os.path.join(BASE_DIR, "exports", "android", "GodotNatureOfCode.apk"))
    
    # Windows Desktop
    os.makedirs(os.path.join(BASE_DIR, "exports", "win64"), exist_ok=True)
    execute(godot_exe, "--export", "Windows Desktop", os.path.join(BASE_DIR, "exports", "win64", "GodotNatureOfCode-Win64.exe"))

    # Linux
    os.makedirs(os.path.join(BASE_DIR, "exports", "x11"), exist_ok=True)
    execute(godot_exe, "--export", "Linux/X11", os.path.join(BASE_DIR, "exports", "x11", "GodotNatureOfCode-Linux.run"))

    # Docs
    shutil.rmtree(os.path.join(BASE_DIR, "exports", "docs"), ignore_errors=True)
    execute(doxygen_exe, "Doxyfile")
    print("Ok!")