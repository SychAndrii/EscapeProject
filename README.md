# EscapeProject

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

1. Rewrite README to serve as the foundation of comprehensive documentation.  
2. Allow this project to be used as a CLI tool.  
3. Add an installer to generate executables for the first release.  

---

## Requirements
- .NET 8.0

---

## Getting Started

- The **EscapeProjectInfrastructure/Task/tasks.json** file contains the structure of your tasks.  
- Running **EscapeProjectComposition/Program.cs** generates two files inside the build output folder:  
  - **EscapeProjectComposition/bin/Debug/net8.0** (if running in Debug mode)  
  - **EscapeProjectComposition/bin/Release/net8.0** (if running in Release mode)  

  The generated files are:  
  - **file.pdf** → A portable PDF version of your tasks (with checkboxes).  
  - **file.xlsx** → An Excel version of your tasks (with structured columns and dropdowns).  

- To modify tasks, simply edit the JSON file and re-run the program.
