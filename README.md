# The Nature of Code in Godot

**Official book website**: https://natureofcode.com/book/

**Godot Engine website**: https://godotengine.org

## :page_facing_up: Summary

Each interactive example and interactive exercise will be implemented with **Godot Engine 3.2.2 stable**, using **C#** for the scripting.

The project contain a **launcher**, with a **scene explorer** to navigate examples and exercises, and the **ecosystem simulation** scene.

Almost everything is created from scripts.

![screen](./docs/screen.gif)

## :joystick: Try it online

You can access the HTML5 version on my `github.io` here: [Godot Nature of Code](https://srynetix.github.io/gamedev/godot-nature-of-code/index.html)  

## :sparkles: Features

- :joystick: Dynamic scene loader to explore examples and exercises
- :art: Simple reusable primitives are available in the `scripts` folder
- :gear: The `Physics Libraries` chapter is using the Godot internal physics engine instead of `Box2D`
- :alembic: A simple custom Verlet physics implementation based on `toxiclibs` can be found in `scripts/verletphysics`
- :zap: Drawing is often batched (using GLES2 batching) to ensure correct performance on mobile and web targets
- :book: Most C# code is documented with Doxygen, you can use the included Doxyfile to generate documentation

## :book: Documentation

Documentation is generated with Doxygen from C# XML comments.  
Use the included Doxyfile to generate HTML documentation in the `../gnoc-html` folder.  
You can also access the generated C# documentation here: [C# Documentation](https://srynetix.github.io/gamedev/godot-nature-of-code/docs/annotated.html)

## :date: Roadmap

- [ ] :joystick: Implement missing examples and exercises.
  - [x] 00. Introduction
  - [x] 01. Vectors
  - [x] 02. Forces
  - [x] 03. Oscillation
  - [x] 04. Particle systems
  - [x] 05. Physics libraries
  - [-] 06. Autonomous agents
  - [ ] 07. Cellular automata
  - [ ] 08. Fractals
  - [ ] 09. The evolution of code
  - [ ] 10. Neural networks
- [ ] :bug: Implement the full Ecosystem project

## :mage: Contributing

- :memo: File rules: An .editorconfig file is provided.    
- :pencil2: Formatting: This project use `dotnet-format` to automatically format the C# code, without specific configuration.
- :dark_sunglasses: Lint: I am still looking for a command-line utility to lint the code (like `flake8` in the Python world or `clippy` in the Rust world).

If you want to contribute, do not hesitate to create Pull Requests or file Issues on GitHub :octocat: :wink:. 