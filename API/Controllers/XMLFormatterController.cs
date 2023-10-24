using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Text;
using System.Xml.Linq;
using System;
using System.Xml;
using API.Models.Helpers.XMLFormatter;
using System.Text.Json.Nodes;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class XMLFormatterController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<XMLFormatterController> _logger;

        public XMLFormatterController(ILogger<XMLFormatterController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetXMLFormatterFile")]
        public IEnumerable<XMLFormatter> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new XMLFormatter
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public IActionResult Post([FromForm] IFormFile file)
        {
            Console.WriteLine("I am now processing");

            if (file != null && file.Length > 0)
            {
                try
                {
                    using var stream = new MemoryStream();
                    XMLFormatterHelper helper = new XMLFormatterHelper();
                    file.CopyTo(stream);
                    byte[] fileBytes = stream.ToArray();
                    string xmlString = Encoding.UTF8.GetString(fileBytes);
                    //XElement xml = XElement.Parse(xmlString);
                    JsonObject json = new JsonObject();

                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc.LoadXml(xmlString);

                    helper.XmlNodeToJSON(doc, json);
                    //XAttribute productionDateAttribute = xml.Attribute("productionDate");
                    Console.Write(json);




                    // Return a success response
                    return Ok("File uploaded successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while processing the file: " + ex.Message);
                    // Return an error response
                    return StatusCode(500, "Internal Server Error");
                }
            }
            else
            {
                // Return a bad request response
                return BadRequest("No file was uploaded.");
            }
        }

    }
}