# ControlSystem
This project is created using .NET technology, the aim is to create a software applications which can be used to digitalize activities of an company.

The project herachy is as following:
- Client Folder:
    - Xamarin Forms Application.
    - Application Library (Serve Client Applications).
- Server Folder:
    - Core Folder:
        - Application Library.
        - Domain Library.
    - Infrastructure Folder:
        - Infrastructure Library.
        - Presistence Library.
    - Presentation Folder:
        - Web API ASP.NET Core.
- Shared Folder:
    - Shared Kernel Library.

Business domain groups are planned as following:
1. Territory.
2. Employee.
.
.
.

While constructing, two things were into considration:
1. Unit Testing as much as possible.
2. Authentication and Authorization of users.
