# WeeControl
Please follow the following steps when adding new feature.

## 1. Run whole project unit tests and ensure all passes.

## 2. In project "Common.BoundedContext" navigate to the required context or create new context.

### To create new context, you will need to create the following folders:
1. BaseObjects (optional abstracted classes contains common properties between DTOs and DBOs).
2. DataTransferObjects (inherent from BaseObjects).
3. Operations (interfaces for common transactions between frontend and backend and ApiRouteLink).

## 3. Create checklist of each operation in "Common.BoundedContext.Context.Operations.".

## 4. In project "Backend.WebApi" create the necessary controller and with all data.

### a. Ensure that frontend and backend agrees about data types in controller using swagger.

### b. Create the necessary unit tests in "Backend.WebApi.Test.Functional" against bad data and from security POV.

### c. Ensure that all unit tests pass.

## 5. In project "backend.Application" create the necessary commands and queries in corresponding BoundedContext folder.

## 6. Create necessary units tests in "Backend.Application.Test".
### a) Tests must ensure that business logic is satisfied and acceptable on each command and query.
### b) Now all unit tests must fail.

## 7. Create Necessary domain objects in project "Backend.Domain".

## 8. Create Necessary persistence classes in project "Backend.Persistence".

## 9. Implement commands and queries handlers in project "Backend.Application".

## 10. Ensure that all units tests in project "Backend.Application.Test" pass.

## 11. Run whole project unit tests and ensure that all passes.

## 12. Commit.





