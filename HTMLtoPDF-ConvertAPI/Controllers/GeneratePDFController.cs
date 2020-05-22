using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;

namespace HTMLtoPDF_ConvertAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneratePDFController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private INodeServices _nodeServices;
        public GeneratePDFController(IHostingEnvironment hostingEnvironment, INodeServices nodeServices)
        {
            _hostingEnvironment = hostingEnvironment;
            _nodeServices = nodeServices;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromServices] INodeServices nodeServices)
        {
            string filename = @"GeneratePDF.pdf";
            //var sb = new StringBuilder();
            var html = @"
                        <html>
                            <head>
                            </head>
                            <body style='padding-left:10px'>

                                <p>HTML to PDF convert</p>   
                                <br><p>PDF Genetation</p>
                        </body>
                        </html>
                        ";
            try
            {
                var htmlContent = html;

                var webRoot = _hostingEnvironment.WebRootPath;
                var result = await _nodeServices.InvokeAsync<byte[]>(webRoot + "/pdf", htmlContent);

                HttpContext.Response.ContentType = "application/pdf";

                var filePath = webRoot + "/Downloads" + $@"/{ filename}";


                bool fileExists = (System.IO.File.Exists(filePath) ? true : false);

                if (fileExists == true)
                {
                    Random random = new Random();
                    var randomNum = random.Next(99999).ToString();

                    filename = randomNum + filename;
                    filePath = webRoot + "/Downloads" + $@"/{ filename}";

                }
                FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite);
                fileStream.Write(result, 0, result.Length);
                fileStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
            }

            return Ok(true);
        }
    }
}