using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedResources.Models;

namespace ghettoBasa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private static Random random = new Random();

        // GET: api/Files/5
        [HttpGet("{filename}", Name = "Get")]
        public ActionResult Get(string filename)
        {
            var filePath = Path.Combine("Resources/Uploads", filename);

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "application/force-download", filename);

        }

        // POST: api/Files
        [HttpPost]
        public IActionResult Post([FromForm] Files files )
        {
            var filePath = "Resources/Uploads";

            var file = files.file;

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            if(file != null)
            {
                var fileExt = Path.GetExtension(file.FileName);

                var fname = RandomString(2) + "-" + DateTime.Now.Year + "-" + file.FileName;

                try
                {
                    using (var fs = new FileStream(Path.Combine(filePath, fname), FileMode.Create))
                    {
                        file.CopyTo(fs);
                    }

                    return Ok(fname);
                }
                catch
                {
                    return BadRequest();
                }
            }
            else
            {
                return NoContent();
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
