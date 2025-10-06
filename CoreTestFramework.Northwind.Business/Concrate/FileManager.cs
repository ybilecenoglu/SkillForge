
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CoreTestFramework.Northwind.Business
{

    public class FileManager : IFileService
    {
        private readonly string _storagePath;
        private readonly string uploadYil;
        private readonly string uploadAy;
        private readonly string uploadGun;
        public FileManager(IConfiguration configuration)
        {
            uploadYil = DateTime.Now.ToString("yyyy");
            uploadAy = DateTime.Now.ToString("MM");
            uploadGun = DateTime.Now.ToString("dd");
            _storagePath = Path.Combine(configuration["FileStoragePath"]);
        }
        public async Task<bool> DeleteFileAsync(string path, string controllerName)
        {
            try
            {
                var filePath = Path.Combine(_storagePath, controllerName, uploadYil, uploadAy, uploadGun, path);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return await Task.FromResult(true);
                }
                else
                    return await Task.FromResult(false);

            }
            catch (System.Exception ex)
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<string> GetFileAsync(string fileName, string controllerName)
        {
            try
            {
                var filePath = Path.Combine(_storagePath, controllerName, uploadYil, uploadAy, uploadGun, fileName);
                if (File.Exists(filePath))
                {
                    filePath = filePath.Replace(_storagePath, "");
                    return await Task.FromResult($"/Uploads/{filePath}");
                }
                else
                    return await Task.FromResult(string.Empty);
            }
            catch (System.Exception)
            {
                return await Task.FromResult(string.Empty);
            }

        }

        public async Task<string[]> UploadFileAsync(IFormFile file, string controllerName)
        {
            try
            {
                string[] paths = new string[2];
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(_storagePath, controllerName, uploadYil, uploadAy, uploadGun);
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                filePath = Path.Combine(filePath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                paths[0] = $"/Uploads/{filePath.Replace(_storagePath, "")}";
                paths[1] = fileName;
                return await Task.FromResult(paths);

            }
            catch (System.Exception ex)
            {
                return null;
            }

        }
    }
}