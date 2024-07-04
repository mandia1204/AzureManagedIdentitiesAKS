// See https://aka.ms/new-console-template for more information
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Data.SqlClient;
using System.Data;

Console.WriteLine("Hello, World!");

void TestDbConnection() {

    Console.WriteLine("Connecting to db...");
    var connString = "Server=tcp:mandiadb.database.windows.net;Authentication=Active Directory Default; Database=mandiatestdb;User Id=7f334dbe-f241-4a1b-a2af-1be292f7868a";

    using (var connection = new SqlConnection(connString))
    {
        connection.Open();
        Console.WriteLine("Connected!");
        var command = new SqlCommand
        {
            Connection = connection,
            CommandType = CommandType.Text,
            CommandText = "SELECT top 1 * FROM dbo.Candidates",
        };
        var row = command.ExecuteReader();
        row.Read();
        var name = row.GetString("name");

        Console.WriteLine($"Name: {name}");
    }
}

void TestKVConnection()
{
    Console.WriteLine("Testing secret...");
    var client = new SecretClient(new Uri("https://mandia2-kv.vault.azure.net/"), new DefaultAzureCredential());
    var val = client.GetSecret("my-secret");
    Console.WriteLine($"secret: {val.Value.Value}");
}

async Task TestStorageAccountConnection()
{
    Console.WriteLine("Downloading a file from storage account...");
    //var tokenCredential = new DefaultAzureCredential();
    //var accessToken = await tokenCredential.GetTokenAsync(
    //    new TokenRequestContext(scopes: new string[] { "https://storage.azure.com/.default" }) { }
    //);
    //var token = accessToken.Token;

    BlobServiceClient blobServiceClient = new(
            new Uri($"https://mandiasa.blob.core.windows.net"),
            new DefaultAzureCredential());

    var containerClient = blobServiceClient.GetBlobContainerClient("files");

    var blobClient = containerClient.GetBlobClient("test.txt");

    BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();
    string blobContents = downloadResult.Content.ToString();

    Console.WriteLine($"File Content: {blobContents}");
}

TestKVConnection();
//TestDbConnection();
await TestStorageAccountConnection();


while (true)
{

}