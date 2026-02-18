using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

string AZURE_STORAGE_CONNECTION_STRING = "";
string blobName = "SampleFile.txt";
string containerName = "";
await CreateBlobContainer(AZURE_STORAGE_CONNECTION_STRING, containerName, blobName);
await ReadAllBlobInContainer(AZURE_STORAGE_CONNECTION_STRING, containerName);
await DownloadBlob(AZURE_STORAGE_CONNECTION_STRING, containerName, blobName);
await DeleteBlob(AZURE_STORAGE_CONNECTION_STRING, containerName, blobName);
// Method to create a blob container and upload a sample blob
async Task CreateBlobContainer(string AZURE_STORAGE_CONNECTION_STRING, string containerName, string blobname)
{
    // Create a BlobServiceClient to interact with the Blob service
    BlobServiceClient blobServiceClient = new BlobServiceClient(AZURE_STORAGE_CONNECTION_STRING);

    // Create the container if it does not exist
    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
    await containerClient.CreateIfNotExistsAsync();
    Console.WriteLine($"Container '{containerName}' created successfully.");
    await containerClient.UploadBlobAsync(blobname, new BinaryData("This is a sample blob content."));
}

// Method to read all blobs in a specified container
async Task ReadAllBlobInContainer(string AZURE_STORAGE_CONNECTION_STRING, string containerName)
{
    // Create a BlobServiceClient to interact with the Blob service
    BlobServiceClient blobServiceClient = new BlobServiceClient(AZURE_STORAGE_CONNECTION_STRING);

    // Get a reference to the container
    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

    // List all blobs in the container
    Console.WriteLine($"Blobs in container '{containerName}':");
    await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
    {
        Console.WriteLine($"- {blobItem.Name}");
    }
}

async Task DownloadBlob(string AZURE_STORAGE_CONNECTION_STRING, string containerName, string blobName)
{
    // Create a BlobServiceClient to interact with the Blob service
    BlobServiceClient blobServiceClient = new BlobServiceClient(AZURE_STORAGE_CONNECTION_STRING);

    // Get a reference to the container
    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

    // Get a reference to the blob
    BlobClient blobClient = containerClient.GetBlobClient(blobName);

    // Download the blob content
    if (await blobClient.ExistsAsync())
    {
        var downloadInfo = await blobClient.DownloadAsync();
        using (var reader = new StreamReader(downloadInfo.Value.Content))
        {
            string content = await reader.ReadToEndAsync();
            Console.WriteLine($"Content of blob '{blobName}': {content}");
        }
    }
    else
    {
        Console.WriteLine($"Blob '{blobName}' does not exist in container '{containerName}'.");
    }
}

async Task DeleteBlob(string AZURE_STORAGE_CONNECTION_STRING, string containerName, string blobName)
{
    // Create a BlobServiceClient to interact with the Blob service
    BlobServiceClient blobServiceClient = new BlobServiceClient(AZURE_STORAGE_CONNECTION_STRING);

    // Get a reference to the container
    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

    // Get a reference to the blob
    BlobClient blobClient = containerClient.GetBlobClient(blobName);

    // Delete the blob if it exists
    if (await blobClient.ExistsAsync())
    {
        await blobClient.DeleteAsync();
        Console.WriteLine($"Blob '{blobName}' deleted successfully from container '{containerName}'.");
    }
    else
    {
        Console.WriteLine($"Blob '{blobName}' does not exist in container '{containerName}'.");
    }
}