# Using Escape Project as a CLI tool

EscapeProject is distributed as ready-to-use installers for Windows and Linux.  
Follow the steps below to set it up and run the CLI tool.

---

[← Back to Documentation Index](./README.md)

## Table of Contents

- [Installation](#installation)
- [Runner Script](#runner-script)
- [Supported Commands](#supported-commands)
  - [generate-excel](#generate-excel)
  - [generate-pdf](#generate-pdf)
- [Task JSON Format](#task-json-format)
  - [Task Groups](#task-groups)
  - [Task Fields](#task-fields)
  - [Validation Rules](#validation-rules)
  - [Normalization](#normalization)
  - [Example JSON](#example-json)

---

### Installation

1. Go to the [latest release](https://github.com/SychAndrii/EscapeProject/releases).  
2. Download the installer archive for your operating system:
   - **Windows:** `windows-installer.zip`
   - **Linux:** `linux-installer.zip`
3. Extract the contents of the archive to a folder of your choice.

---

### Runner Script

1. Navigate to the extracted folder.  
2. Run the provided **runner script** to start the app:
   - On **Linux:** `./escape-run.sh`
   - On **Windows:** `escape-run.bat`
3. **Important:** Do **not** run the executable directly. It must be started via the runner script for everything to work correctly.  
4. To see the list of available commands, run:
   ```bash
   ./escape-run.sh -h   # Linux
   escape-run.bat -h    # Windows
   ```
   This will display all available CLI commands and options.

### Supported Commands

By default, EscapeProject comes with a sample JSON file located at **`Task/tasks.json`**.  
You can use this file right away to test the CLI without creating your own task set.  

---

#### `generate-excel`

Exports your tasks into an **Excel spreadsheet** with structured columns (*Task, Status, Duration, Time Range*).  
Dropdowns are used for fields like *Status* to keep task management simple and consistent.

**Usage:**
```bash
escape-run.bat generate-excel [OPTIONS]    # Windows
./escape-run.sh generate-excel [OPTIONS]   # Linux
```

**Options:**
- `-h, --help` – Prints help information  
- `-t, --tasks <PATH>` – Path to the tasks JSON file  
- `-d, --directory <DIR>` – Output directory for generated Excel files (default: `./TaskPlans`)  

**Example:**
```bash
escape-run.bat generate-excel -t Task/tasks.json -d ./OutputPlans   # Windows
./escape-run.sh generate-excel -t Task/tasks.json -d ./OutputPlans  # Linux
```

---

#### `generate-pdf`

Exports your tasks into a **PDF document** with checkboxes, so you can mark tasks complete directly in the file.  
Portable and easy to share across devices.

**Usage:**
```bash
escape-run.bat generate-pdf [OPTIONS] # Windows
./escape-run.sh generate-pdf [OPTIONS] # Linux
```


**Options:**
- `-h, --help` – Prints help information  
- `-t, --tasks <PATH>` – Path to the tasks JSON file  
- `-d, --directory <DIR>` – Output directory for generated PDF files (default: `./TaskPlans`)  

**Example:**
```bash
escape-run.bat generate-pdf -t Task/tasks.json -d ./OutputPlans
./escape-run.sh generate-pdf -t Task/tasks.json -d ./OutputPlans
```

---

With these commands you can quickly generate either **Excel** or **PDF task plans** from your JSON file.  
Start with the provided `Task/tasks.json` to try it out, then create your own task sets for daily use.

---
### Task JSON Format

EscapeProject uses a JSON file to define your tasks.  
Each JSON file is parsed into **task groups**, where the group name is the key (e.g., `Monday`, `Work`, `Personal`).  
Every task group must contain **at least one task**.

---

1. **Task Groups**  
   - Keys (e.g., `"Monday"`, `"Work"`, `"Personal"`) are the names of groups.  
   - Each group must have at least one task.  
   - A group **cannot contain other groups** (no nesting).  
   - Tasks must always be defined **inside a group** (no top-level tasks allowed).  

2. **Task Fields**
   - `name` *(string, required)* – A short description of the task.  
   - `from` *(DateTime, accepts null)* – Start time in strict format `yyyy-MM-dd'T'HH:mm:ss`.  
   - `until` *(DateTime, accepts null)* – End time in strict format `yyyy-MM-dd'T'HH:mm:ss`.  

3. **Validation Rules**
   - If both `from` and `until` are provided, `from` must be **earlier** than `until`.

4. **Normalization**
   - Task names are normalized (trimmed, no empty strings).  
   - Invalid or empty names will cause a parsing error.  

---

#### Example JSON

Below is a valid `tasks.json` file with multiple groups:

```json
{
    "Work": [
        {
            "name": "Team Meeting",
            "from": "2025-09-23T10:00:00",
            "until": "2025-09-23T11:00:00"
        },
        {
            "name": "Code Review",
            "from": "2025-09-23T13:00:00",
            "until": "2025-09-23T14:30:00"
        },
        {
            "name": "Write Documentation",
            "from": null,
            "until": null
        }
    ],
    "Personal": [
        {
            "name": "Grocery Shopping",
            "from": "2025-09-23T18:00:00",
            "until": null
        },
        {
            "name": "Dinner with Friends",
            "from": "2025-09-23T19:30:00",
            "until": "2025-09-23T21:00:00"
        }
    ],
    "Learning": [
        {
            "name": "Read Philosophy Book",
            "from": "2025-09-23T21:30:00",
            "until": null
        },
        {
            "name": "Online Course: Distributed Systems",
            "from": "2025-09-23T22:00:00",
            "until": "2025-09-23T23:30:00"
        }
    ]
}
```

With this format, you can create as many groups as you like (Weekdays, Chores, Hobbies) and EscapeProject will automatically parse and validate them into task plans.
