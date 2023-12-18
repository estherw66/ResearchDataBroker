using Moq;
using ResearchDataBroker.Data;
using ResearchDataBroker.Models;
using ResearchDataBroker.Service;

namespace ResearchDataBroker.Tests.Service;

[TestFixture]
public class TestIndexService
{
    private IndexService _indexService;
    
    private Mock<IDataverseService> _dataverseService;
    private Mock<IFilesRepository> _filesRepository;
    private Mock<IItemRepository> _itemRepository;
    
    [SetUp]
    public void SetUp()
    {
        _dataverseService = new Mock<IDataverseService>();
        _filesRepository = new Mock<IFilesRepository>();
        _itemRepository = new Mock<IItemRepository>();

        _indexService = new IndexService(_dataverseService.Object, _itemRepository.Object,  _filesRepository.Object);
    }

    [Test]
    public async Task GetItems_ShouldReturnListOfItemDTOs()
    {
        // arrange
        var files = new List<FileModel>
        {
            new FileModel
            {
                Id = 1,
                DirectoryLabel = "directory/label",
                Filename = "filename.jpg",
                Link = "itemlink.com"
            }
        };

        var items = new HashSet<ItemModel>
        {
            new ItemModel
            {
                Id = 1,
                Name = "Item 1",
                Files = files
            },
            new ItemModel
            {
                Id = 2,
                Name = "Item 2",
                Files = files
            }
        };

        var expectedItemDTOs = new HashSet<ItemDTO>
        {
            new ItemDTO
            {
                Id = 1,
                Name = "Item 1",
                FileIds = new List<int>
                {
                    1
                }
            },
            new ItemDTO
            {
                Id = 2,
                Name = "Item 2",
                FileIds = new List<int>
                {
                    1
                }
            }
        };

        var expectedResponse = new GetItemsResponseDTO
        {
            Items = expectedItemDTOs
        };

        _itemRepository.Setup(itemRepo => itemRepo.GetItems()).ReturnsAsync(items);
        
        // act
        var actualResponse = await _indexService.GetItems();

        // assert
        Assert.IsInstanceOf<GetItemsResponseDTO>(actualResponse);
        Assert.IsNotNull(actualResponse.Items);

        for (int i = 0; i < expectedResponse.Items.Count; i++)
        {
            Assert.That(actualResponse.Items.ToList()[i].Id, Is.EqualTo(expectedResponse.Items.ToList()[i].Id));
            Assert.That(actualResponse.Items.ToList()[i].Name, Is.EqualTo(expectedResponse.Items.ToList()[i].Name));            Assert.That(actualResponse.Items.ToList()[i].Id, Is.EqualTo(expectedResponse.Items.ToList()[i].Id));
            Assert.That(actualResponse.Items.ToList()[i].FileIds, Is.EqualTo(expectedResponse.Items.ToList()[i].FileIds));
        }
        
        // _guestRepositoryMock.Verify(repo => repo.FindAll(), Times.Once);
        _itemRepository.Verify(itemRepo => itemRepo.GetItems(), Times.Once);
    }
    
    [Test]
    public async Task GetFiles_ShouldReturnListOfFileDTOs()
    {
        // arrange
        var items = new HashSet<ItemModel>
        {
            new ItemModel
            {
                Id = 1,
                Name = "Item 1"
            }
        };

        var files = new HashSet<FileModel>
        {
            new FileModel
            {
                Id = 1,
                DirectoryLabel = "directory/label",
                Filename = "filename_1.jpg",
                Link = "filelink.com",
                Items = items
            },
            new FileModel
            {
                Id = 2,
                DirectoryLabel = "directory/label",
                Filename = "filename_2.jpg",
                Link = "filelink.com",
                Items = items
            }
        };

        var fileDTOs = new HashSet<FileDTO>
        {
            new()
            {
                Id = 1,
                DirectoryLabel = "directory/label",
                Filename = "filename_1.jpg",
                Link = "filelink.com",
                ItemNames = new List<string>
                {
                    "Item 1"
                }
            },
            new()
            {
                Id = 2,
                DirectoryLabel = "directory/label",
                Filename = "filename_2.jpg",
                Link = "filelink.com",
                ItemNames = new List<string>
                {
                    "Item 1"
                }
            }
        };

        var expectedResponse = new GetFilesResponseDTO()
        {
            Files = fileDTOs
        };

        _filesRepository.Setup(fileRepo => fileRepo.GetFiles()).ReturnsAsync(files);
        
        // act
        var actualResponse = await _indexService.GetFiles();

        // assert
        Assert.IsInstanceOf<GetFilesResponseDTO>(actualResponse);
        Assert.IsNotNull(actualResponse.Files);

        for (int i = 0; i < expectedResponse.Files.Count; i++)
        {
            Assert.That(actualResponse.Files.ToList()[i].Id, Is.EqualTo(expectedResponse.Files.ToList()[i].Id));
            Assert.That(actualResponse.Files.ToList()[i].Filename, Is.EqualTo(expectedResponse.Files.ToList()[i].Filename));
            Assert.That(actualResponse.Files.ToList()[i].Link, Is.EqualTo(expectedResponse.Files.ToList()[i].Link));
            Assert.That(actualResponse.Files.ToList()[i].DirectoryLabel, Is.EqualTo(expectedResponse.Files.ToList()[i].DirectoryLabel));
            Assert.That(actualResponse.Files.ToList()[i].ParentId, Is.EqualTo(expectedResponse.Files.ToList()[i].ParentId));
            Assert.That(actualResponse.Files.ToList()[i].ItemNames, Is.EqualTo(expectedResponse.Files.ToList()[i].ItemNames));
        }
        
        // _guestRepositoryMock.Verify(repo => repo.FindAll(), Times.Once);
        _filesRepository.Verify(fileRepo => fileRepo.GetFiles(), Times.Once);
    }

    [Test]
    public void IndexDataset_ShouldSaveFileAndItemAndReturnItemDTOs()
    {
        
    }
}