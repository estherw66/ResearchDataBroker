// using System.Net.Http.Json;
using System.Text.Json;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DataverseService : IDataverseService
{
    public async Task<DataverseLatestVersionModel> GetLatestVersion(string url)
    {
        string serverUrl = GetServerUrl(url);
        // Console.WriteLine(serverUrl);
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

    public async Task<string> GetItemFromXml(string url)
    {
        var response = await ServerConfig.Client.GetAsync(url);
        Stream data = await response.Content.ReadAsStreamAsync();

        XDocument xDocument = XDocument.Load(data);

        IEnumerable<XElement> xElements = xDocument.Root.Elements().Where(x => x.Name.LocalName == "object");

        string name;
        foreach (var element in xElements)
        {
            name = element.Elements().FirstOrDefault(x => x.Name.LocalName == "name").ToString();
            name = name.Replace("<name>", "").Replace("</name>", "");
            return name;
        }

        return null;
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
        // string persistentId = url.Substring(startIndex);

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
}