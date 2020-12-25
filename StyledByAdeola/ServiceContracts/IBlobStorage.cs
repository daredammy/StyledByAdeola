using System.IO;
using System.Threading.Tasks;
using StyledByAdeola.Models;
namespace StyledByAdeola.ServiceContracts
{
    public interface IBlobStorage
    {
        Task UploadFileToStorage(BlobContainers containerName, string fileName, Stream fileStream);
    }
}
