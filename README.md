# JsonInSQL

## Overview

This project demonstrates how to store JSON data in a SQL database and perform basic queries on that data using Entity Framework Core 10.0. It explores options for handling unstructured but searchable data in a relational database, leveraging new features in EF Core 10 for complex types and collections.

## Features

- Stores complex objects (campgrounds and sites) as JSON in a SQLite database
- Demonstrates basic querying and searching of JSON data
- Uses EF Core 10.0's `ComplexCollection` mapping for storing collections as JSON
- Example data generation using [LoremNET](https://www.nuget.org/packages/LoremNET/)

## How It Works

The `Campground` entity contains a collection of `Site` objects. With EF Core 10, the `Sites` collection is mapped to a JSON column in the database using the `ComplexCollection` API:

```csharp
modelBuilder.Entity<Campground>().ComplexCollection(b => b.Sites, b => b.ToJson());
```

This allows you to store and retrieve rich, nested data structures in a single column, while still enabling queries over the JSON content.

## Usage

1. Build and run the project:
	- The app will generate sample campgrounds and sites, and store them in `JsonInSQL-Demo.db`.
2. Explore the database using your favorite SQLite tool.
3. Review `Program.cs` and `DBContext.cs` for examples of data creation and querying.

## Requirements

- .NET 10.0 (Preview)
- Entity Framework Core 10.0 RC1
- SQLite

## References

- [EF Core 10.0 Preview RC1 Release Notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/rc1/efcore.md)

## Next Steps

- Research advanced querying and indexing options for JSON columns (e.g. queries that are not bound to a structured object)
- Explore support for other databases (e.g., SQL Server)
- Add more complex search scenarios
