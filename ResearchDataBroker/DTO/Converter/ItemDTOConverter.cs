public static class ItemDTOConverter
{
    public static ItemDTO ConvertToDTO(ItemModel itemModel)
    {
        ItemDTO itemDTO = new ItemDTO
        {
            Id = itemModel.Id,
            Name = itemModel.Name
        };

        foreach (FileModel file in itemModel.Files)
        {
            itemDTO.FileIds.Add(file.Id);
        }

        return itemDTO;
    }
}