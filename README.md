# WeeControl
## Project Overview
This project is created as a milestone to develop and customize a software application using .NET technologies to solve a business need.

### Philosophy:
* Continuous refactoring to increase code readability and performance.
* Separation of concerns by applying SOLID principles to the limit before creating too much abstraction.
* Functional and Unit testing are important during development.

### Technologies:
* .NET 6 for Server API.
* Blazor WebAssembly as front end.
* Entity Framework Core as database persistence.

### Patterns:
* CQRS using Mediator in backend side.

## Copyright and Licensing
This is open source project, while you need to read and accept each nuget package imported into this project.

## Configuration Instructions
In order to use this application you need to do the following:

1. Edit values in \"/src/WebApi/appsettings.json\" including:
    1. Change "UseInMemoryDb" to false if you didn't want to use in memory database.
    2. Database connection string (recommended to use postgres as backend database).
    3. <del>File storage connection string, you can use your own server directory</del>.
3. Edit values in \"/src/user/Wasm/wwwroot/appsettings.json\" including:
    1. Api base address of WebApi server.
4. Run WebApi server.
5. Run Wasm (Blazor App).
6. ...

## Installation Instructions
This project code depend on many nuget packages, so ensure to restore the packages before running the servers.

Note that this project is **currently under heavy development** so if you faced any issues try to manually delete the database then the application will create new one with the default values, or migrate the database.

## Operating Instructions
1. Ensure that both API and Blazor servers are running.
2. On Blazor webpage using credentials:
   Username : admin
   Password : admin
3. If you face any issues try to migrate the db or drop the old database then try again.

[comment]: <> (<del>“What is this? Where does this go?” Now is the time to demystify any assumptions around how to use your project.</del>)

[comment]: <> (## A list of files included)

[comment]: <> (<del>Contingent upon how large your source code is, you may opt to not include the file tree, however you can still explain how to traverse through your code. For example, how is your code modularized? Did you use the MVC &#40;Model, View, Controller&#41; method? Did you use a Router system? Just a few questions to consider when detailing your file structure.</del>)


## Contact
You can contact me on my email at <sayed.hussein@gmx.com>.

## Known Bugs
Still this project is in its initial developing stage, lots of bugs are expected at the current time, I wish that the project will be ready soon to be released in alpha stage to identfy bugs.

[comment]: <> (## Troubleshooting)

[comment]: <> (<del>In this section you will be able to highlight how your users can become troubleshooting masters for common issues encountered on your project.</del>)

## Credits and Acknowledgments
Appreciate the opensoure community for inspiring and supporting such project:

* The Markdown Guide: [Markdown Guide](https://www.markdownguide.org) and how to create a README.md file on [Medium](https://medium.com/@latoyazamill/how-to-create-a-readme-md-file-37cffa2d7ab4).
* JWT authentication: [aspnet-core-3-jwt-authentication-api](https://github.com/cornflourblue/aspnet-core-3-jwt-authentication-api).
* Clean architecture pattern: [CleanArchitecture By Steve Smith](https://github.com/ardalis/CleanArchitecture), [CleanArchitecture by Jason Taylor](https://github.com/jasontaylordev/CleanArchitecture) and [NorthwindTraders](https://github.com/jasontaylordev/NorthwindTraders).
* The youtube channels [IAmTimCorey](https://www.youtube.com/user/IAmTimCorey), [Nick Chapsas](https://www.youtube.com/c/Elfocrash) and [VoidRealms](https://www.youtube.com/channel/UCYP0nk48grsMwO3iL8YaAKA) gave the courage to start getting a workable programming application.
* Jetbrains for approving licence to use [Rider](https://www.jetbrains.com/rider/) to develop this opensource project.

[//]: # (**<mark>List not Completed Yet</mark>**)

[comment]: <> (## A changelog &#40;usually for programmers&#41;)

[comment]: <> (<del>A changelog is a chronological list of all notable changes made to a project such as: records of changes such as bug fixes, new features, improvements, new frameworks or libraries used, and etc.</del>)

[comment]: <> (## A news section &#40;usually for users&#41;)

[comment]: <> (<del>If your project is live and in production and you are receiving feedback from users, this is a great place to let them know, “Hey, we hear you, we appreciate you, and because of your feedback here are the most recent changes, updates, and new features made.”</del>)
