# HTTP-5212-RNA-Group4-HospitalProject
A 7 week project creating a new website based on the existing Riverside Healthcare website.

## Concepts Used:

- ASP.NET MVC architecture pattern
- Entity Framework Code-First Migrations to represent the database
- LINQ to perform CRUD operations

## How to Run This Project?

- Clone the repository in Visual Studio
- Open the project folder on your computer (e.g. File Explore for Windows Users)
- Create an <App_Data> folder in the main project folder
- Go back to Visual Studio and open Package Manager Console and run the query to build the database on your local server:
- update-database
- The project should set up
## Database

Below are the tables in the Hosiptal Project.
- Services
- Departments
- Pharmacies
- Prescriptions
- Staff
- Research
- Donar

## Team Members and their input

1- Brian Ssekalegga: Developer for Services and Departments Module
<br>
2 - Austin Caron: Developer for CRUD Pharmacy/Prescriptions, as well as linking staff with pharmacy/prescriptions
<br>
3 - Temi Babalola: Developer for CRUD for Staff and appointments, linked staff to department.
<br>
4 - Akhilender Vallab: Developer for Research and Donar CRUD.
<br>
5 - Siddhant Patel: Developer for HUpdates and Articles, also linked HUpdate to Departments Module.

## Latest additions
1 - Austin Caron: 
<br>
 * Added administrator role/access to Create, Update and Delete features of prescriptions and pharmacies
 * Added scaling visuals for prescriptions and pharmacies
 * Created method of converting military time to 12-hour format with minutes supported for the Pharmacy details
 * Consistently attempted to aid group members in their bugs/issues

