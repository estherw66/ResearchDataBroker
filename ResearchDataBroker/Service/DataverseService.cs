// using System.Net.Http.Json;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DataverseService : IDataverseService
{
    public ServerConfig config = new ServerConfig("https://demo.dataverse.org"); // change this

    public async Task<DataverseLatestVersionModel> GetLatestVersion(string url)
    {
        // get doi from url
        string persistentId = await GetPersistentId(url);

        if (string.IsNullOrEmpty(persistentId))
        {
            // throw error
            Console.WriteLine("no persistent id");
            return null;
        }

        // connect to dataverse
        // get dataset
        DataverseResponseModel response = await GetDataverseResponse(persistentId);

        if (response == null)
        {
            // throw error
            Console.WriteLine("no response model");
            return null;
        }

        // return latest version (with array of files)
        DataverseLatestVersionModel latestModel = response.Data.LatestVersion;

        if (latestModel == null)
        {
            // throw error
            Console.WriteLine("no latest version");
            return null;
        }

        return latestModel;
    }
    
    private async Task<string> GetPersistentId(string url)
    {
        // check if draft
        if (!url.Contains("persistentId=") && !url.Contains("&version"))
        {
            // throw error
            Console.WriteLine("wrong url");
            return null;
        }

        int startIndex = url.IndexOf("persistentId=") + "persistentId=".Length;
        int endIndex = url.IndexOf("&version");

        string persistendId = url.Substring(startIndex, endIndex - startIndex);
        // string persistendId = url.Substring(startIndex);
        Console.WriteLine(persistendId);

        return persistendId;
    }

    private async Task<DataverseResponseModel> GetDataverseResponse(string persistentId)
    {
        string url = $"datasets/:persistentId/?persistentId={persistentId}";
        var response = await ServerConfig.Client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            // throw error
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