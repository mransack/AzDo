using System;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

class Program
{
    static async Task Main(string[] args)
    {
        // 1. Get the Key Vault name from an environment variable or configuration
        var keyVaultName = "masskeyvaultci";
        if (string.IsNullOrEmpty(keyVaultName))
        {
            Console.WriteLine("KEY_VAULT_NAME environment variable not set.");
            return;
        }

        // 2. Construct the Key Vault URI
        var kvUri = $"https://{keyVaultName}.vault.azure.net";

        // 3. Create a SecretClient with DefaultAzureCredential
        // DefaultAzureCredential automatically handles authentication for both local dev and production
        var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

        // 4. Specify the name of the secret you want to retrieve
        const string secretName = "demokey"; 

        Console.WriteLine($"Retrieving secret '{secretName}' from {keyVaultName}...");

        try
        {
            // 5. Retrieve the secret
            KeyVaultSecret secret = await client.GetSecretAsync(secretName);

            // 6. Access the secret value
            Console.WriteLine($"Your secret value is: {secret.Value}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving secret: {ex.Message}");
        }
    }
}
