# The Nature of Code in Godot

**Official book website**: https://natureofcode.com/book/

**Godot Engine website**: https://godotengine.org

## :page_facing_up: Summary

Each interactive example and interactive exercise will be implemented with **Godot Engine 3.2.3 stable**, using **C#** for the scripting.

The project contain a **launcher**, with a **scene explorer** to navigate examples and exercises, and the **ecosystem simulation** scene.

Almost everything is created from scripts.

![screen](./docs/screen.gif)

## :joystick: Try it online

You can access the HTML5 version on my `github.io` here: [Godot Nature of Code](https://srynetix.github.io/gamedev/godot-nature-of-code/index.html)  

## :sparkles: Features

- :joystick: Dynamic scene loader to explore examples and exercises
- :art: Simple reusable primitives are available in the `scripts` folder
- :alembic: A simple custom Verlet physics implementation based on `toxiclibs` can be found in `scripts/verletphysics`
- :zap: Drawing is often batched (using GLES2 batching) to ensure correct performance on mobile and web targets
- :book: Most C# code is documented with Doxygen, you can use the included Doxyfile to generate documentation

## :book: Documentation

Documentation is generated with Doxygen from C# XML comments.  
Use the included Doxyfile to generate HTML documentation in the `exports/docs` folder.  
You can also access the generated C# documentation here: [C# Documentation](https://srynetix.github.io/gamedev/godot-nature-of-code/docs/annotated.html)

## :date: Roadmap

- [ ] Implement missing examples and exercises.
  - [x] 00. Introduction
  - [x] 01. Vectors
  - [x] 02. Forces
  - [x] 03. Oscillation
  - [x] 04. Particle systems
  - [x] 05. Physics libraries
  - [x] 06. Autonomous agents
  - [x] 07. Cellular automata
  - [ ] 08. Fractals
  - [ ] 09. The evolution of code
  - [ ] 10. Neural networks
- [ ] Implement the full Ecosystem project

## :mage: Contributing

- *File rules*: An .editorconfig file is provided.    
- *Formatting*: This project use `roslynator` to automatically format the C# code, without specific configuration.
- *Lint*: I am using the Roslynator VSCode extension. I am still looking for a command-line version with the same hints.

You can use the Python 3 `Makefile.py` to format or export the project and its documentation.  
It needs `doxygen` and `godot` in `PATH`, or you can use `DOXYGEN_EXE` and `GODOT_EXE` environment variables.

If you want to contribute, do not hesitate to create Pull Requests or file Issues on GitHub :wink:. 