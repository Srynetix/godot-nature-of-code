# Nature of Code in Godot

Official book: https://natureofcode.com/book/

Godot Engine: https://godotengine.org

## Summary

Each interactive example and interactive exercise will be implemented with Godot Engine 3.2.3-rc3, using C# for the scripting.

The project contain a launcher, with a **scene explorer** to navigate examples and exercises, and the **ecosystem simulation** scene.

Almost everything is created from scripts.

![screen](./docs/screen.gif)

## Try it online

You can access the HTML5 version on my `github.io` here: [Godot Nature of Code](https://srynetix.github.io/gamedev/godot-nature-of-code/index.html)

You can also access the generated C# documentation here: [C# Documentation](https://srynetix.github.io/gamedev/godot-nature-of-code/docs/index.html)

## Features

- Dynamic scene loader to explore examples and exercises
- Simple reusable primitives are available in the `scripts` folder
  - Examples: Canvas, Attractors, Zones, Springs, Particles, Waves, etc.
- The `Physics Libraries` chapter is using the Godot internal physics engine instead of `Box2D`
- A simple Verlet physics implementation based on `toxiclibs` can be found in `scripts/verletphysics`
- Most C# code is documented with Doxygen, you can use the included Doxyfile to generate documentation
- Drawing is often batched (using GLES2 batching) to ensure correct performance on mobile and web targets

## Roadmap

- [ ] Implement missing examples and exercises.
  - [x] 00. Introduction
  - [x] 01. Vectors
  - [x] 02. Forces
  - [x] 03. Oscillation
  - [x] 04. Particle systems
  - [x] 05. Physics libraries
  - [ ] 06. Autonomous agents
  - [ ] 07. Cellular automata
  - [ ] 08. Fractals
  - [ ] 09. The evolution of code
  - [ ] 10. Neural networks
- [ ] Implement the full Ecosystem project

## Generate documentation

Documentation is generated with Doxygen from C# XML comments.
Use the included Doxyfile to generate HTML documentation in the ../gnoc-html folder.