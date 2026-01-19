# 🎽 Sports Jersey Management System

A robust **ASP.NET MVC 5** application designed to manage sports team inventories. This project demonstrates a **Code First** approach using **Entity Framework 6**, featuring complex relationships, asynchronous programming, and secure data handling.

## 🚀 Project Overview

This application manages the lifecycle of sports jerseys, from team association to stock management. It implements a strict **One-to-Many** relational model across three layers of data:
1.  **Lookup:** Teams (Categories)
2.  **Master:** Jersey Definitions (Product Info)
3.  **Detail:** Jersey Stock (Inventory/Sizes)

The system is built to handle high-reliability data entry using SQL Transactions and Stored Procedures within a Code First context.

## 🛠️ Tech Stack

* **Framework:** ASP.NET MVC 5 (.NET Framework 4.8)
* **Language:** C#
* **ORM:** Entity Framework 6 (**Code First Workflow**)
* **Database:** SQL Server (LocalDB / Express)
* **Frontend:** HTML5, CSS3, Bootstrap, Razor View Engine
* **Tools:** Visual Studio 2022

## ✨ Key Features

* **Code First Architecture:** Database schema is generated and managed entirely through C# POCO classes and Migrations.
* **Complex Data Relationships:**
    * **Team:** *(Lookup Table)* Stores team names and country.
    * **Jersey:** *(Master Table)* Links to Teams; stores season, price, and home/away status.
    * **JerseyStock:** *(Detail Table)* Tracks quantity and sizes for specific jerseys.
* **Advanced Data Handling:**
    * **Stored Procedures:** Custom SQL logic integrated into EF for complex fetch operations.
    * **ACID Transactions:** Ensures data integrity when saving Master and Detail records simultaneously.
    * **Asynchronous Programming:** `async/await` implementation for non-blocking database calls.
* **Authentication:** Secure user login and registration system.

## 📂 Database Schema (Code First Models)

The project relies on three primary domain models:

| Model | Type | Description |
| :--- | :--- | :--- |
| **Team.cs** | Lookup | The root category (e.g., "Real Madrid", "Bangladesh"). |
| **Jersey.cs** | Master | The product definition. Has a Foreign Key to `Team`. |
| **JerseyStock.cs** | Detail | The inventory unit. Has a Foreign Key to `Jersey`. |

## ⚙️ How to Run

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/your-username/SportsJerseyManagement.git](https://github.com/your-username/SportsJerseyManagement.git)
    ```
2.  **Open in Visual Studio:**
    Open the `.sln` file in Visual Studio 2022.
3.  **Restore Packages:**
    Right-click the solution in Solution Explorer -> **Restore NuGet Packages**.
4.  **Update Database (Crucial for Code First):**
    * Go to **Tools** > **NuGet Package Manager** > **Package Manager Console**.
    * Run the command:
        ```powershell
        Update-Database
        ```
    * *This will generate the SQL database and apply all migrations.*
5.  **Run the Application:**
    Press `F5` to start the server.

## 📸 Screenshots

*(Add your screenshots here later, e.g., the Master-Detail Entry Form)*

---
*Created by [Your Name]*