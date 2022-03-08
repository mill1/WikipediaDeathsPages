using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace WikipediaDeathsPages.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssemblyController : ControllerBase
    {
        private readonly ILogger<AssemblyController> logger;
        public readonly Assembly ExecutingAssembly;

        public AssemblyController(ILogger<AssemblyController> logger)
        {
            this.logger = logger;
            ExecutingAssembly = Assembly.GetExecutingAssembly();
        }

        [HttpGet]
        public ContentResult GetAssemblyInfo()
        {
            var assemblyName = ExecutingAssembly.GetName();

            return Content($"{assemblyName.Name} version: { assemblyName.Version}");
        }

        [HttpGet("property/{property}")]
        public IActionResult GetPropertyValue(string property)
        {
            try
            {
                var assemblyName = ExecutingAssembly.GetName();

                var value = GetAssemblyPropertyValue(assemblyName, property);

                return Ok(new { name = property, value = value });
            }
            catch (Exception e)
            {
                string message = $"Getting the property value failed. Property: {property}. Exception: {e}";
                logger.LogError($"{message}", e);
                return BadRequest(message);
            }
        }

        private string GetAssemblyPropertyValue(AssemblyName assemblyName, string property)
        {
            try
            {
                if (property.Contains("CodeBase"))
                    return "not available";
                else
                    return assemblyName.GetType().GetProperty(property).GetValue(assemblyName, null).ToString();
            }
            catch (Exception)
            {
                return $"Unknown assembly property: {property}";
            }
        }
    }
}
