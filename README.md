# ApiEndpoint

![ApiEndpoint Icon](./ApiEndpointIcon.png){: width="250px" }

[![GitHub Release](https://img.shields.io/github/v/release/dev-vinz/ApiEndpoint.NET)](https://github.com/dev-vinz/ApiEndpoint.NET/releases/latest) [![GitHub Release Downloads](https://img.shields.io/github/downloads/dev-vinz/ApiEndpoint.NET/latest/total)](https://github.com/dev-vinz/ApiEndpoint.NET/releases/latest) [![GitHub Build, Test, Deploy and Release Action](https://github.com/dev-vinz/ApiEndpoint.NET/actions/workflows/deploy.yml/badge.svg)](https://github.com/dev-vinz/ApiEndpoint.NET/actions/workflows/deploy.yml) [![GitHub License](https://img.shields.io/github/license/dev-vinz/ApiEndpoint.NET)](https://github.com/dev-vinz/ApiEndpoint.NET/blob/main/LICENSE)

## About

This is a simple package that allows you to create a REST API endpoint in your .NET application. It is a lightweight package that is easy to use and can be integrated into your application with minimal effort.

## How to Use

In order to use this package, you have to build your own API endpoint with the built-in `ApiEndpointBuilder` class. Here is an example of how you can create an API endpoint that returns a simple JSON response:

```csharp
using ApiEndpoint;
using System;

class Program
{
    public static async Task Main(string[] args)
    {
        // Create a new API endpoint
        IApiEndpoint endpoint = new ApiEndpointBuilder("https://httpbin.org/")
            .WithBearerTokens("your-bearer-token", "will-go-here") // Add bearer tokens
            .WithDateFormat("yyyy-MM-dd") // Set the date format
            .WithLogger(Console.WriteLine) // Set a logger
            .WithMissingMemberHandling(MissingMemberHandling.Error) // Set the missing member handling
            .WithRequestThrottle(10) // Limit the number of requests per second
            .Build();

        // Create a new request
        IRequest<JsonResponse> request = endpoint.Get<JsonResponse>("/anything")
            .AddParam("key", "value") // Add a query parameter
            .AddParam("key2", "value2"); // Add another query parameter

        // Send the request
        JsonResponse response = await request.ExecuteAsync();

        // Print the response
        Console.WriteLine(response);
    }
}
```

## Main Types

The main types provided by this package are:

-   `IApiEndpoint` - The main interface that represents an API endpoint.
-   `ApiEndpointBuilder` - A builder class that allows you to create an `IApiEndpoint` instance.
-   `IRequest<T>` - An interface that represents a request to an API endpoint.
-   `MessageFormData` - A class that represents form data for a POST request.
-   `DeserializableInternalAttribute` - An attribute that can be used to mark internal properties that should be deserialized.
-   `ApiEndpointException` - An exception that is thrown when an error occurs while executing a request.
-   `ApiEndpointError` - A class that represents an error response from an API endpoint.

## Related Packages

-   JSON Serialization and Deserialization : [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/)
