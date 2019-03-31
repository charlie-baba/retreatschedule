using System;
using System.IO;
using static System.IO.Directory;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RetreatSchedule.Util;
using RetreatSchedule.Poco;

namespace RetreatSchedule.Controllers
{
    public class UploadFilesController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public UploadFilesController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(FileUpload upload)
        {
            long size = upload.File.Length;
            var error = "";
            var dir = Path.Combine(_hostingEnvironment.WebRootPath, $"uploads\\images\\{upload.Type}\\{DateTime.Now.Year}\\");
            var filePath = $"{dir}{RefGenerator.GetGuid()}.{upload.File.FileName?.Split(".")[1] ?? "jpg"}";
            try
            {
                CreateDirectory(dir);
                if (size > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await upload.File.CopyToAsync(stream);
                    }
                }
            } catch(Exception e) {
                error = e.ToString();
            }

            var relativePath = filePath.Split("wwwroot")[1].Replace("\\", "/");
            return Ok(new { size, relativePath, error });
        }
    }
}