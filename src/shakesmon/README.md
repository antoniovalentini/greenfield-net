# Shakesmon

A simple web app that adds a Shakespeare flavor to pokemon descriptions. It exposes a REST API that, given a Pokemon name, returns its Shakespearean description.

It relies on two platforms:
- [PokeApi](https://pokeapi.co/) to fetch pokemons' description
- [FunTranslations](https://funtranslations.com/api/shakespeare) to translate an english text into the Shakespeare version.

## How to run it

### Standalone
Requirements:
- [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1)

Clone the project on your local machine, navigate to the Shakesmon folder, open the command line and type:
```
dotnet run --project ./src/Avalentini.Shakesmon.Api/Avalentini.Shakesmon.Api.csproj
```
To test the application, open your browser and navigate to:
```
https://localhost:5001/api/pokemon/bulbasaur
```

You should see a response like the one below:
```JSON
{
  "name": "bulbasaur",
  "description": "Bulbasaur can beest seen napping in bright sunlight. Thither is a seed on its back. By soaking up the travelling lamp’s rays, the seed grows progressively larger."
}
```

### Docker
Requirements:
- [Docker Engine](https://docs.docker.com/engine/install/)

Clone the project on your local machine, open the terminal, navigate to the project "src" folder and run the following commands:

```
sudo docker build -t shakesmon .
sudo docker run -d -p 5000:80 -p 5001:443 --name shakesmon shakesmon
```

If everything went smoothly, open your browser and navigate to http://localhost:5000/api/pokemon/bulbasaur or https://localhost:5001/api/pokemon/bulbasaur in order to test the application.

## Solution Details
The solution is composed of:
- an API project (ASP.NET Core 3.1 web app)
- a CORE project (.NET Standard 2.1 plus few nuget dependencies)
- Unit and Integration test projects

### API
The API project simply exposes one RESTful endpoint. Usually, we tend to keep this as "business-logic free" as possible. In an ideal world, we should be able to swap APIs technology without having to rewrite so much code.

The project is **SWAGGER** enabled. You can use the built-in Swagger UI to test the endpoint by navigating the '/swagger/' path. You can also generate a client by following the OpenAPI Specification standard.

### Core
The CORE project aims to keep business logic away from the "presentation" layer. This must be considered only a first step that can enable a future clean architecture implementation. A nice improvement could be to move interface implementations to a separate project (Infrastructure?) in order to keep the Core (which becomes Domain/Application) business-logic oriented and free from nuget dependencies.

I like to structure Domain projects in a way that is clear for the developer which are the available functionalities (Feature folder) and then have a separate folder with interfaces for all the external dependencies (Services).

The task we're trying to accomplish is not so complex, so for the moment, I'd prefer to keep everything in the same Core project in favor of simplicity and code readability. I'm a fan of the "premature optimization is the root of all evil" rule.

### Tests
The solution provides a starting set of tests:
- Api.IntegrationTests: a set of integration tests to make sure the api endpoint works properly with all the real external dependencies
- Core.IntegrationTests: a set of integration tests to make sure each external dependency works properly
- Core.UnitTests: a set of isolated tests to make sure domain logics acts properly

## Extras
Given the limitations due to the free plan of the 3rd party api platforms, it was necessary to add a **caching system**. Translated descriptions are cached using a simple in-memory cache provided out-of-the-box by microsoft. [You can read more here](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-3.1).

## Nice to Have
A list of nice to have features for the future:
- circuit breaking pattern for external services
- health checks or status pages to make sure dependencies are up and running
- allow the user to send proper API KEYS for each external service in order to use paid plans
- separate domain interfaces and actual implementations into different projects
- setup a proper CI/CD pipeline

## LICENSE
Shakesmon is released under the MIT license.

Favicon provided by [draseart](http://www.iconarchive.com/artist/draseart.html) through [IconArchive](http://www.iconarchive.com/show/dumper-icons-by-draseart/PokeBall-icon.html).
