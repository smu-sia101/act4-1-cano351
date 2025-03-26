using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace PetNameGenerator.Controllers
{
    [ApiController]
    [Route(" [controller]")]
    public class PetNameController : ControllerBase
    {
        private static readonly string[] dogNames = { "Babi", "Maxon", "Charlie", "Cori", "Pingol" };
        private static readonly string[] catNames = { "Tubby", "Pikkie", "Oscar", "Dona", "Kyfi" };
        private static readonly string[] birdNames = { "Berdi", "Ittit", "Shagi", "Dikki", "Buddi" };

        [HttpPost]
        public IActionResult GeneratePetName([FromBody] PetNameRequest request)
        {
            
            if (string.IsNullOrEmpty(request.AnimalType))
                return BadRequest(new { error = "The 'animalType' field is required." });

            string[] names = request.AnimalType.ToLower() switch
            {
                "dog" => dogNames,
                "cat" => catNames,
                "bird" => birdNames,
                _ => null
            };

            if (names == null)
                return BadRequest(new { error = "Invalid animal type. Allowed values: dog, cat, bird." });

         
            if (request.TwoPart.HasValue && !(request.TwoPart.Value is bool))
                return BadRequest(new { error = "The 'twoPart' field must be a boolean (true or false)." });

            Random random = new Random();
            string name = names[random.Next(names.Length)];

            if (request.TwoPart == true)
            {
                string secondName = names[random.Next(names.Length)];
                name += secondName;
            }

            return Ok(new { name });
        }
    }

    public class PetNameRequest
    {
        public string AnimalType { get; set; }
        public bool? TwoPart { get; set; }
    }
}
