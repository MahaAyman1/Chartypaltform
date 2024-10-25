namespace Chartypaltform.Service
{
    public static class FileUploadHelper
    {
        public static async Task<string> HandleFileUpload(IFormFile file, string uploadsFolder)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/" + uniqueFileName;
        }
    }


}
