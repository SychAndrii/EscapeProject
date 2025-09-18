# EscapeProject

## Why this project is created?

During my self-improvement journey I stumbled upon a couple of problems: 

1. I want to use a task management app to boost my productivity. However, all existing apps do not click with me. They are either overcomplicated or require a paid subscription. A lot of apps out there include the features that you do not even need by default, which makes their UI too overwhelming.
2. Using Notepad is inconvenient. Notepad is not easy to use on your phone and it requires you to spend quite some time to locate and find the task you want to update. 

**Escape Project** is created to solve the problems I came across by following this simple philosophy: ***No extra installations, no extra registrations, no extra features***.

1. The UI of Escape Project is very simple, it uses PDF files to construct the UI for your tasks. Chances are, you have already interacted with PDF format. You don't have to learn user interface from scratch, which enhances user experience. Besides, this app has MIT license, which means you are free to modify it and distribute it however you like.
2. Since PDF files are used for displaying the tasks, you will easily be able to save your them and send the file to your phone and keep the copy of your tasks on another device. Due to the usage of checkboxes for marking the completed tasks, Escape Project is much easier to deal with than a regular notepad.

## Plans for the future

1. Allow to store your tasks in an excel file.
2. Add console UI to show analytics about your performance.
3. Add desktop UI to show analystics about your performance.

## Requirements
.NET Core 8.0

## What you need to know to get started
**Task/tasks.json** file contains a json file that shows the structure that your tasks are supposed to follow. If you run the **Program.cs** file, you will be able to generate the **file.pdf** inside on of the **bin** folders.
If you want to change the tasks that are generated, simply change the json file.
