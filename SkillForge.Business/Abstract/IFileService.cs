using Microsoft.AspNetCore.Http;

namespace SkillForge.Business
{
    public interface IFileService
    {
        Task<string> GetFileAsync(string path, string controllerName);
        Task<string[]> UploadFileAsync(IFormFile fil,string controllerName);
        Task<bool> DeleteFileAsync(string path, string controllerName);
    }
}