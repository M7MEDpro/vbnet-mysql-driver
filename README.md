# vbnet-mysql-driver

**A beginner-friendly VB.NET MySQL driver (wrapper) for fast CRUD and DataGridView support.**

---

## 🔧 Prerequisites

- **Visual Studio** (2015+ with VB.NET Windows Forms)  
- **.NET Framework** (4.6 or later)  
- **MySQL Server** (Community Edition)  
- **MySql.Data** (NuGet package)

---

## 🚀 Quick Start Guide

1. **Install MySQL Server**  
   - Download the installer: https://dev.mysql.com/downloads/installer/  
   - Choose **Developer Default** and set a **root** password.  
   - Confirm the server is running via MySQL Workbench or the command line.

2. **Download the Driver File**  
   - Visit: https://github.com/M7MEDpro/vbnet-mysql-driver  
   - Click the green **Code** button → **Download ZIP**.  
   - Extract the ZIP to a folder on your PC.

3. **Add DMSQL.vb to Your Project**  
   - In Visual Studio: go to **Project → Add Existing Item...**  
   - Select the `DMSQL.vb` file.

4. **Install MySql.Data NuGet Package**  
   - In Visual Studio: **Tools → NuGet Package Manager → Manage NuGet Packages for Solution**  
   - Search for **MySql.Data**, select the official package, and click **Install**.

---

## 📦 DMSQL.vb: Methods & Usage

| Method                         | Parameters (Simplified)                                     | Description                                                  |
|-------------------------------|--------------------------------------------------------------|--------------------------------------------------------------|
| `Connect(db, ip, port, user, pass)` | Database name, IP (e.g. 127.0.0.1), port (3306), username, password | Connects your app to MySQL                                   |
| `Disconnect()`                | None                                                         | Closes the MySQL connection                                  |
| `CreateTableSimple(name)`     | Table name like "Users"                                     | Creates a table with auto-increment `ID` primary key        |
| `AddFieldSimple(table, field, type)` | Table name, column name, VB type (e.g. "String")      | Adds a new column to the table                              |
| `InsertSimple(table, field, value)` | Table name, column, value                                 | Inserts a new value into a single column                    |
| `UpdateSimple(table, field, value, id)` | Table name, column, new value, record's ID          | Updates a specific cell                                     |
| `DeleteSimple(table, id)`     | Table name, record ID                                       | Deletes a record by `ID`                                    |
| `GetValueSimple(table, field, id)` | Table name, column, record ID                          | Returns one cell's value as a string                        |
| `ViewNoID(table, dgv, searchBox)` | Table name, DataGridView, optional TextBox for search    | Displays a read-only grid without showing the `ID` column  |
| `ViewWithID(table, dgv, searchBox)` | Same as above                                             | Displays read-only grid including `ID`                     |
| `EditNoID(table, dgv, searchBox, saveBtn)` | Editable grid without `ID`, connected to a save button | Allows editing without showing `ID`                          |
| `EditWithID(table, dgv, searchBox, saveBtn)` | Editable grid including `ID`                         | Editable grid with visible `ID`, saves on button click      |

---

## 🧩 Example Usage in Form_Load

```vbnet
' 1) Connect to your database
DMSQL.Connect("testdb", "127.0.0.1", "3306", "root", "password")

' 2) Create a table and add fields
DMSQL.CreateTableSimple("Products")
DMSQL.AddFieldSimple("Products", "Name", "String")
DMSQL.AddFieldSimple("Products", "Price", "Decimal")

' 3) Basic CRUD
DMSQL.InsertSimple("Products", "Name", "Apple")
DMSQL.UpdateSimple("Products", "Price", "1.25", 1)
DMSQL.DeleteSimple("Products", 1)

' 4) Read a field value
Dim productName As String = DMSQL.GetValueSimple("Products", "Name", 1)

' 5) DataGridView (read-only, hide ID)
DMSQL.ViewNoID("Products", DataGridView1, TextBoxSearch)

' 6) DataGridView (editable, show ID)
DMSQL.EditWithID("Products", DataGridView1, TextBoxSearch, ButtonSave)

' 7) Disconnect
DMSQL.Disconnect()
