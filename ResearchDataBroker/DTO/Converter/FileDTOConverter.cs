using ResearchDataBroker.Models;

public static class FileDTOConverter
{
    public static FileDTO ConvertToDTO(FileModel fileModel)
    {
        FileDTO fileDTO = new FileDTO
        {
            Id = fileModel.Id,
            Filename = fileModel.Filename,
            Link = fileModel.Link,
            DirectoryLabel = fileModel.DirectoryLabel,
            ParentId = fileModel.ParentId
        };

        foreach (ItemModel? item in fileModel.Items)
        {
            fileDTO.ItemNames.Add(item.Name);
        }

        return fileDTO;
    }
}