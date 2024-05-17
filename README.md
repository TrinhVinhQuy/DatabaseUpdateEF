What is the DatabaseUpdateEF Project?
=====================
The DatabaseUpdateEF Project is a open-source project written in .NET Core

Description: Describe the optimization of the performance of statements in data update operations in a .NET project.
Here is a description of the provided APIs in English:

Common Purpose
All the APIs aim to update the salaries of all employees of a specific company by increasing them by 10% and to update the company's salary update timestamp.

API 1: `update-salary-by-company-id`

- **Description**: This API uses Entity Framework to load all the employees of a company into memory, then iterates through each employee to increase their salary by 10%, and saves the changes to the database.
- **Pros**: Easy to understand and maintain, leverages EF's tracking capabilities to automatically update changes.
- **Cons**: Poor performance when there are many employees due to loading all data into memory.

API 2: `update-salary-by-company-id-sql`

- **Description**: This API uses a direct SQL query within Entity Framework to update employee salaries without loading employee data into memory.
- **Pros**: Better performance due to executing a single SQL statement without loading employee data into memory.
- **Cons**: Less flexible and maintains potential risk of SQL injection if not careful.

API 3: `update-salary-by-company-id-dapper`

- **Description**: This API uses Dapper, a micro ORM, to execute the SQL query to update employee salaries and manage transactions.
- **Pros**: Highest performance due to Dapper's lightweight nature, better security than API 2 due to parameterized queries.
- **Cons**: More complex, requires knowledge of Dapper and transaction management.

General Comparison
- **Performance**: API 3 > API 2 > API 1
- **Complexity and Maintainability**: API 1 < API 2 < API 3
- **Security**: API 1 and API 3 are better than API 2 due to minimizing the risk of SQL injection.

Depending on the specific requirements for performance and complexity, you can choose the appropriate API. If performance is the top priority and you can handle the complexity, API 3 with Dapper is the best choice. If you need simplicity and ease of maintenance, API 1 is the suitable option.

## How to use:
- You will need the latest Visual Studio 2022, the latest .NET Core SDK and SQL Server 2019.
- The latest SDK and tools can be downloaded from https://dot.net/core.

To know more about how to setup your enviroment visit the [Microsoft .NET Download Guide](https://www.microsoft.com/net/download)

## Technologies implemented:

- ASP.NET 7.0
- Entity Framework Core 7.0
- SQL Server 2019
- Dapper
- Minimal api
  
## About:
The eProject.Onion Project was developed by [Trịnh Xuân Vinh Quy](https://www.facebook.com/Vhquy).
