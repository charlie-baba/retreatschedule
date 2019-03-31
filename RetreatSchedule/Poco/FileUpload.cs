using Microsoft.AspNetCore.Http;

namespace RetreatSchedule.Poco
{
    public class FileUpload
    {
        public IFormFile File { get; set; }

        public string Type { get; set; }
    }
}
