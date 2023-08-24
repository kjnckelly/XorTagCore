using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XorTag.Domain;

namespace XorTag.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MapController : ControllerBase
    {
        private readonly MapImageBuilder mapImageBuilder;
        private readonly ILogger<MapController> logger;

        public MapController(MapImageBuilder mapImageBuilder, ILogger<MapController> logger)
        {
            this.mapImageBuilder = mapImageBuilder;
            this.logger = logger;
        }

        [HttpGet("/map")]
        [ResponseCache(Duration = 1)]
        public IActionResult Get()
        {
            return File(mapImageBuilder.BuildImage(), "image/png");
        }
    }
}