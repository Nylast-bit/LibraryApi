# LibraryApi: backend

API REST en **.NET 10** que actúa como proxy entre el frontend y [FakeRestAPI](https://fakerestapi.azurewebsites.net/index.html), organizada con **Clean Architecture**. No usa base de datos: la capa de datos consume la API externa por HTTP.

## Requisitos

- .NET SDK 10
- Visual Studio 2022 (17.13+), VS Code o Rider

## Instalación y ejecución

```bash
git clone <url-de-este-repo>
cd LibraryApi
dotnet restore
dotnet build
dotnet run --project src/LibraryApi.Api
```

La consola muestra la URL (`Now listening on: http://localhost:5053`). La documentación interactiva está en `http://localhost:5053/scalar`.

La URL de la API externa se configura en `src/LibraryApi.Api/appsettings.json`:

```json
"FakeRestApi": { "BaseUrl": "https://fakerestapi.azurewebsites.net/" }
```

CORS permite el origen `http://localhost:3000` (frontend Next.js).

## Arquitectura

```
src/
├── LibraryApi.Domain          Entidades (Book, Author). No depende de nada.
├── LibraryApi.Application     Interfaces de repositorio, DTOs, mapeos y servicios.
├── LibraryApi.Infrastructure  Repositorios que llaman a FakeRestAPI con HttpClient.
└── LibraryApi.Api             Controllers, configuración y composición (DI).
tests/
└── LibraryApi.Application.Tests
```

Las dependencias apuntan hacia adentro: `Api → Application → Domain` e `Infrastructure → Application`. `Application` define qué necesita (`IBookRepository`, `IAuthorRepository`) e `Infrastructure` lo implementa, de modo que reemplazar FakeRestAPI por una base de datos solo afectaría a `Infrastructure`.

## Endpoints

| Método | Ruta                | Respuesta                             |
| ------ | ------------------- | ------------------------------------- |
| GET    | `/api/books`        | 200 con la lista de libros            |
| GET    | `/api/books/{id}`   | 200 con el libro y sus autores, o 404 |
| POST   | `/api/books`        | 201 con el libro creado               |
| PUT    | `/api/books/{id}`   | 200 con el libro actualizado, o 404   |
| DELETE | `/api/books/{id}`   | 204, o 404                            |
| GET    | `/api/authors`      | 200 con autores y su número de libros |
| GET    | `/api/authors/{id}` | 200, o 404                            |
| POST   | `/api/authors`      | 201 con el autor creado               |
| PUT    | `/api/authors/{id}` | 200, o 404                            |
| DELETE | `/api/authors/{id}` | 204, o 404                            |

## Decisiones técnicas

- **Modelos externos separados del dominio:** `BookApiModel` y `AuthorApiModel` representan el JSON de FakeRestAPI (por ejemplo, `idBook`) y se traducen a las entidades del dominio (`BookId`). Los campos obligatorios que llegan nulos se normalizan para no romper los listados.
- **DTOs por pantalla:** `BookResponse` para la lista, `BookDetailResponse` para el detalle con sus autores, y requests separados para crear y editar (el cliente no envía el `Id`).
- **Detalle de libro con autores:** se obtiene con el endpoint `/api/v1/Authors/authors/books/{idBook}` de FakeRestAPI.
- **Número de libros por autor:** en FakeRestAPI cada registro de autor apunta a un solo libro, por lo que el conteo se calcula agrupando por nombre y apellido y contando los `BookId` distintos. Para un autor individual se consulta el listado completo, ya que la API externa no ofrece un endpoint de conteo.
- **FakeRestAPI no persiste:** POST, PUT y DELETE responden correctamente pero no guardan cambios. La API devuelve lo que responde el servicio externo y el frontend actualiza su estado local.
- **HttpClient tipado** registrado con `AddHttpClient`, `CancellationToken` en todas las capas y mapeo manual con métodos de extensión (sin AutoMapper).

## Ejecutar todo con Docker

Clona ambos repositorios en la misma carpeta:

```bash
git clone https://github.com/Nylast-bit/LibraryApi.git
git clone https://github.com/Nylast-bit/libraryfrontend.git
cd LibraryApi
docker compose up --build
```

- Frontend: http://localhost:3000
- API: http://localhost:5053/api/books
- Documentación: http://localhost:5053/scalar
