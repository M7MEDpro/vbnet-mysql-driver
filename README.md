# vbnet-mysql-driver

**A beginner-friendly VB.NET MySQL driver (wrapper) for fast CRUD and DataGridView support.**

---

## 📚 Project Background

This project was created by a student at the **Egyptian Chinese University** during their freshman year.

While working on a VB.NET Windows Forms project, I needed to connect and interact with a MySQL database. But I couldn’t find a simple, beginner-friendly driver that handled basic tasks like CRUD and DataGridView linking without writing lots of repetitive SQL code.

So I built `DMSQL.vb` — a lightweight, reusable class that makes MySQL interaction in VB.NET easy and readable. Whether you're building a student management system, inventory app, or sales tool — this driver helps you focus on features, not syntax.

If you're learning VB.NET and need a fast way to work with MySQL, this tool is for you.

---

## 🔧 Prerequisites

- **Visual Studio** (2015 or newer with VB.NET Windows Forms)
- **.NET Framework** (4.6 or later)
- **MySQL Server** (Community Edition)
- **MySql.Data** (via NuGet)

---

## 🚀 Quick Start Guide

1. **Install MySQL Server**
   - Download: https://dev.mysql.com/downloads/installer
   - Choose **Developer Default**
   - Set a **root password**
   - Confirm it’s running via MySQL Workbench or terminal

2. **Download the Driver File**
   - Visit: https://github.com/M7MEDpro/vbnet-mysql-driver
   - Click **Code → Download ZIP**
   - Extract and locate `DMSQL.vb`

3. **Add DMSQL.vb to Your Project**
   - In Visual Studio: `Project → Add Existing Item...`
   - Select `DMSQL.vb`

4. **Install MySql.Data (NuGet)**
   - `Tools → NuGet Package Manager → Manage NuGet Packages for Solution`
   - Search **MySql.Data**, select it, then click **Install**

---

## 📦 DMSQL.vb: Methods & Usage

| Method                            | Parameters                                                 | Description                                                    |
|----------------------------------|-------------------------------------------------------------|----------------------------------------------------------------|
| `Connect(db, ip, port, user, pass)` | Database, IP, port, username, password                    | Connects to MySQL database                                    |
| `Disconnect()`                   | None                                                       | Closes the MySQL connection                                    |
| `CreateTableSimple(name)`        | Table name (e.g. "Users")                                  | Creates a table with `ID` as auto-increment primary key        |
| `AddFieldSimple(table, field, type)` | Table name, field name, VB type (e.g. "String")         | Adds a new column to the table                                |
| `InsertSimple(table, field, value)` | Table, column, value                                    | Inserts a value into a column                                  |
| `UpdateSimple(table, field, value, id)` | Table, column, value, record ID                     | Updates a specific row by ID                                   |
| `DeleteSimple(table, id)`        | Table name, record ID                                      | Deletes a row using its ID                                     |
| `GetValueSimple(table, field, id)` | Table, column, ID                                       | Returns one cell's value as string                             |
| `ViewNoID(table, dgv, searchBox)` | Table, DataGridView, optional TextBox for search         | Displays read-only grid without showing `ID`                   |
| `ViewWithID(table, dgv, searchBox)` | Same as above                                           | Displays read-only grid with `ID` column                       |
| `EditNoID(table, dgv, searchBox, saveBtn)` | Table, DataGridView, search box, save button        | Editable grid (no `ID` shown)                                  |
| `EditWithID(table, dgv, searchBox, saveBtn)` | Same as above                                      | Editable grid with `ID`, saved via button                      |

---

## 🧩 Example Usage in Form_Load

```vbnet
' 1) Connect to your database
DMSQL.Connect("testdb", "127.0.0.1", "3306", "root", "password")

' 2) Create a table and add fields
DMSQL.CreateTableSimple("Products")
DMSQL.AddFieldSimple("Products", "Name", "String")
DMSQL.AddFieldSimple("Products", "Price", "Decimal")

' 3) Insert, Update, Delete
DMSQL.InsertSimple("Products", "Name", "Apple")
DMSQL.UpdateSimple("Products", "Price", "1.25", 1)
DMSQL.DeleteSimple("Products", 1)

' 4) Read value from a cell
Dim productName As String = DMSQL.GetValueSimple("Products", "Name", 1)

' 5) Show table in read-only DataGridView (hide ID)
DMSQL.ViewNoID("Products", DataGridView1, TextBoxSearch)

' 6) Editable grid with Save button (includes ID)
DMSQL.EditWithID("Products", DataGridView1, TextBoxSearch, ButtonSave)

' 7) Disconnect when closing
DMSQL.Disconnect()
