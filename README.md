# Sale Prototype API

## How to startup the project

In the root directory of this repository, do as describe below:
 1. Run the command to `docker-compose up -d` startup database in docker
 2. Run the command to install/update dotnet ef core cli: `dotnet tool install --global dotnet-ef`
 3. Run the command to apply database migrations: `dotnet ef database update --project .\SaleApiPrototype\SaleApiPrototype.Infra.Database\SaleApiPrototype.Infra.Database.csproj --startup-project .\SaleApiPrototype\SaleApiPrototype.Api\SaleApiPrototype.Api.csproj`
 4. Run the command to startup application: `dotnet run --project .\SaleApiPrototype\SaleApiPrototype.Api\SaleApiPrototype.Api.csproj`
 5. Open any browser and navigate to `http://localhost:5084/swagger/index.html`
 6. Enjoy!