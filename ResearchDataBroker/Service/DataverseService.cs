// using System.Net.Http.Json;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DataverseService : IDataverseService
{
    // public ServerConfig config = new ServerConfig("https//demo.dataverse.org"); // todo change this
    // private ServerConfig config = new ServerConfig("");

    public async Task<DataverseLatestVersionModel> GetLatestVersion(string url)
    {
        string serverUrl = GetServerUrl(url);
        Console.WriteLine(serverUrl);
        ServerConfig config = new ServerConfig(serverUrl);
        
        // get doi from url
        string persistentId = GetPersistentId(url);

        if (string.IsNullOrEmpty(persistentId))
        {
            // todo throw error
            Console.WriteLine("no persistent id");
            return null;
        }

        // connect to dataverse
        // get dataset
        DataverseResponseModel response = await GetDataverseResponse(persistentId);

        if (response == null)
        {
            // todo throw error
            Console.WriteLine("no response model");
            return null;
        }

        // return latest version (with array of files)
        DataverseLatestVersionModel latestModel = response.Data.LatestVersion;

        if (latestModel == null)
        {
            // TODO throw error
            Console.WriteLine("no latest version");
            return null;
        }

        return latestModel;
    }

    private string GetServerUrl(string url)
    {
        Uri uri = new Uri(url);
        string serverUrl = $"{uri.Scheme}://{uri.Host}";
        return serverUrl;
    }
    private string GetPersistentId(string url)
    {
        // TODO check if draft
        if (!url.Contains("persistentId=") && !url.Contains("&version"))
        {
            // TODO throw error
            Console.WriteLine("wrong url");
            return null;
        }

        int startIndex = url.IndexOf("persistentId=", StringComparison.Ordinal) + "persistentId=".Length;
        int endIndex = url.IndexOf("&version", StringComparison.Ordinal);

        string persistentId = url.Substring(startIndex, endIndex - startIndex);
        // string persistendId = url.Substring(startIndex);

        return persistentId;
    }

    private async Task<DataverseResponseModel> GetDataverseResponse(string persistentId)
    {
        string url = $"datasets/:persistentId/?persistentId={persistentId}";
        var response = await ServerConfig.Client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            // TODO throw error
            Console.WriteLine("no success code");
            return null;
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        var deserializedResponse = JsonConvert.DeserializeObject<DataverseResponseModel>(responseJson);
        
        if (deserializedResponse == null)
        {
            // TODO: throw error
            Console.WriteLine("no deserialised response");
            return null;
        }

        return deserializedResponse;
    }
    
    // public async Task<string> GetDataverseResponse(GetDatasetRequestDTO request)
    // {
    //     string persistendId = await GetPersistentId(request.DatasetUrl);
    //     string version = await GetVersion(request.DatasetUrl);

    //     string response = await GetDataset(persistendId);

    //     if (string.IsNullOrEmpty(response))
    //     {
    //         return "Error getting response";
    //     }

    //     return response;
    // }

    // private async Task<string> GetDataset(string persistentId)
    // {
    //     string url = $"datasets/:persistentId/?persistentId={persistentId}";
    //     var response = await ServerConfig.Client.GetAsync(url);

    //     if (!response.IsSuccessStatusCode)
    //     {
    //         // Console.WriteLine(response);
    //         // string content = await response.Content.ReadAsStringAsync();
    //         // Console.WriteLine(content);
    //         // return true;
    //         return null;
    //     }

    //     return await response.Content.ReadAsStringAsync();
    // }

    // public async Task<DataverseLatestVersionModel> GetLatestVersion(GetDatasetRequestDTO request)
    // {
    //     string persistendId = await GetPersistentId(request.DatasetUrl);

    //     DataverseResponseModel response = await GetDataverseResponse(persistendId);

    //     if (response.Data == null)
    //     {
    //         Console.WriteLine("response is null :(");
    //     }

    //     Console.WriteLine(response.Data);

    //     DataverseLatestVersionModel latestVersion = response.Data.LatestVersion;

    //     if (latestVersion == null)
    //     {
    //         Console.WriteLine("latest version is null :(");
    //         return null;
    //     }

    //     return latestVersion;
    // }

    // private async Task<DataverseResponseModel> GetDataverseResponse(string persistentId)
    // {
    //     string url = $"datasets/:persistentId/?persistentId={persistentId}";
    //     var response = await ServerConfig.Client.GetAsync(url);

    //     if (!response.IsSuccessStatusCode)
    //     {
    //         // TODO: throw error
    //         return null;
    //     }

    //     var json = await response.Content.ReadAsStringAsync();

    //     // fix this !!
        // var deserializedResponse = JsonConvert.DeserializeObject<DataverseResponseModel>(json);
        // if (deserializedResponse == null)
        // {
        //     // TODO: throw error
        //     return null;
        // }

    //     Console.WriteLine(deserializedResponse.Status);
    //     Console.WriteLine(deserializedResponse.Data);

    //     return deserializedResponse;
    // }
}