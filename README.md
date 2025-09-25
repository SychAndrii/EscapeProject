# EscapeProject

## Table of Contents

- [EscapeProject](#escapeproject)
  - [Why was this project created?](#why-was-this-project-created)
  - [How does Escape Project work?](#how-does-escape-project-work)
  - [Plans for the future](#plans-for-the-future)
  - [Getting started](#getting-started)

## Why was this project created?

During my self-improvement journey I stumbled upon a couple of problems:

1. I wanted to use a task management app to boost my productivity. However, all existing apps did not click with me. They were either overcomplicated or required a paid subscription. Many apps include features you don’t even need by default, which makes their UI overwhelming.  
2. Using Notepad was inconvenient. It’s not easy to use on your phone and it requires extra effort to locate and update tasks.  

**Escape Project** was created to solve these problems by following a simple philosophy:  
***No extra installations, no extra registrations, no extra features.***

---

## How does Escape Project work?

Escape Project uses **familiar file formats (PDF and Excel)** as the interface for your tasks:

1. **PDF Tasks**  
   - Tasks are exported into a PDF document with checkboxes.  
   - You can tick off completed tasks directly inside the PDF.  
   - PDFs are portable: you can easily copy them to your phone or share with others.  

2. **Excel Tasks**  
   - Tasks are also exported into an Excel spreadsheet.  
   - Each task is placed into a structured table with headers such as *Task, Status, Duration, Time Range*.  
   - For fields that require predefined values (like *Status*), dropdowns are used instead of free text, making updates faster and more consistent.  
   - Since most people are already familiar with Excel, you don’t need to learn a new UI. You can also take advantage of Excel’s built-in features like filtering and sorting.  

Both PDF and Excel formats ensure that your tasks stay **accessible, editable, and easy to share** without requiring proprietary apps or online accounts.  

---

## Plans for the future

1. Add support of sub-tasks
2. Unit test BaseDomain layer.
3. Add cross-platform UI to create tasks instead of manually editing JSON file.

---

## Getting Started

The `docs/` folder contains all project documentation, including:
- Guide on installing and using the app as a CLI tool  
- Advanced documentation that explains the app’s architecture, useful for developers  

[Start exploring the documentation here](docs/README.md)