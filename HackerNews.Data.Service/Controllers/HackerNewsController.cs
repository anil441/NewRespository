using HackerNews.Data.Abstract.Abstract;
using HackerNews.Data.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace HackerNews.Data.Service.Controllers
{
    [Produces("application/json")]
    //[Route("api/[controller]")]
    [Route("Phytel/PatientData")]
    [ApiController]
    public class HackerNewsController : ControllerBase
    {
        private IMemoryCache _cache;

        private readonly IHackerNewsRepository _repo;

        public HackerNewsController(IMemoryCache cache, IHackerNewsRepository repository)
        {
            this._cache = cache;
            this._repo = repository;
        }

        [HttpGet("GetStories")]
        public async Task<List<HackerNewsStory>> Index()
        {
            List<HackerNewsStory> stories = new List<HackerNewsStory>();

            var response = await _repo.BestStoriesAsync();
            if (response.IsSuccessStatusCode)
            {
                var storiesResponse = response.Content.ReadAsStringAsync().Result;
                var bestIds = JsonConvert.DeserializeObject<List<int>>(storiesResponse);

                var tasks = bestIds.Select(GetStoryAsync);
                stories = (await Task.WhenAll(tasks)).OrderByDescending(x => x.Score).ToList();
            
            }
            return stories;
        }

        private async Task<HackerNewsStory> GetStoryAsync(int storyId)
        {
            return await _cache.GetOrCreateAsync<HackerNewsStory>(storyId,
                async cacheEntry =>
                {
                    HackerNewsStory story = new HackerNewsStory();

                    var response = await _repo.GetStoryByIdAsync(storyId);
                    if (response.IsSuccessStatusCode)
                    {
                        var storyResponse = response.Content.ReadAsStringAsync().Result;
                        story = JsonConvert.DeserializeObject<HackerNewsStory>(storyResponse);
                    }

                    return story;
                });
        }
    }
}
