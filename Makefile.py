#!/usr/bin/env python
#
# Needs `doxygen` and `godot` in PATH,
# or you can use `DOXYGEN_EXE` and `GODOT_EXE` environment variables.

import argparse
import os
import shutil
import subprocess
import sys

BASE_DIR = os.path.dirname(os.path.normpath(os.path.abspath(__file__)))

def execute(exe, *args):
    subprocess.call([exe] + list(args))

parser = argparse.ArgumentParser(description="Makefile")
subs = parser.add_subparsers(dest="cmd")
subs.add_parser("format")
subs.add_parser("export")

args = parser.parse_args()
if args.cmd == "format":
    execute("roslynator", "format", "Nature of Code.csproj")
    print("Ok!")

elif args.cmd == "export":
    shutil.rmtree(os.path.join(BASE_DIR, "exports", "docs"), ignore_errors=True)
    execute(os.environ.get("GODOT_EXE", "godot"), "--export", "HTML5", os.path.join(BASE_DIR, "exports", "index.html"))
    execute(os.environ.get("DOXYGEN_EXE", "doxygen"), "Doxyfile")
    print("Ok!")