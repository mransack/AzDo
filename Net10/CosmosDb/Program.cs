using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Azure.Identity;
using Microsoft.Azure.Cosmos;

DefaultAzureCredential credential = new ();

CosmosClient client = new ("AccountEndpoint=https://<RESOURCE_NAME>.documents.azure.com:443/;AccountKey=YOUR_KEY;");
Database database =  client.GetDatabase("mydb");

Microsoft.Azure.Cosmos.Container container = database.GetContainer("dbcontainer");

Product product = new (
    id: Guid.NewGuid().ToString(),
    name: "Widget",
    type: "Gadget",
    quantity: 100,
    price: 9.99m,
    clearance: false
);
Console.WriteLine("Adding new product entry: "+ product.id);
ItemResponse<Product> response = await container.UpsertItemAsync<Product>(product, new PartitionKey(product.type));
Console.WriteLine($"Created item with id: {response.Resource.id} and status code: {response.StatusCode}");

ItemResponse<Product> readResponse = await container.ReadItemAsync<Product>(product.id, new PartitionKey(product.type));
Console.WriteLine($"Read item with id: {readResponse.Resource.id} and status code: {readResponse.StatusCode}");

public record Product
(
    string id, 
    string name,
    string type,
    int quantity,
    decimal price,
    bool clearance
);