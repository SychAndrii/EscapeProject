# Architecture of Escape Project

[← Back to Documentation Index](../README.md)

EscapeProject is built using Clean Architecture principles, making the codebase highly modular, maintainable, and extensible.  
This approach was chosen to enable two main goals:  
- **Flexibility in output formats** – new file formats for task plans can be added with minimal effort.  
- **Flexibility in interfaces** – new ways of interacting with the app (CLI tool, desktop app, web server, etc.) can be introduced without disrupting existing functionality.

---

## Table of Contents

- [High level description of modules](#high-level-description-of-modules)

---

### High level description of modules

![High Level Diagram](high_level_diagram.png)

---
