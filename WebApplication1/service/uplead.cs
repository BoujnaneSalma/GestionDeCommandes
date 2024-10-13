using static System.Net.Mime.MediaTypeNames;

namespace WebApplication1.service
{
    public class uplead
    {

   
    public async Task<string> UploadImageAsync(IFormFile image, string uploadsFolder)
    {

        var randomFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
        var filePath = Path.Combine(uploadsFolder, randomFileName);
            
            
            using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }

        return randomFileName;
    } 
    }
}
