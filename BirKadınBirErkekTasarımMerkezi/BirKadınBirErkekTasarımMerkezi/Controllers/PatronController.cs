using BirKadınBirErkekTasarımMerkezi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BirKadınBirErkekTasarımMerkezi.Controllers
{
    [Authorize(Roles = "PATRON")]

    public class PatronController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public PatronController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public IActionResult Panel()

        {
            return View();  
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> KazanclariGor()
        {
            // API endpoint'ine istek atıyoruz
            var client = _clientFactory.CreateClient();
            var response = await client.GetStringAsync("https://localhost:7292/api/randevu/gunluk-kazanc");

            // JSON verisini C# modeline çeviriyoruz
            var kazancListesi = JsonConvert.DeserializeObject<List<KazancModel>>(response);

            // Veriyi View'e gönderiyoruz
            return View(kazancListesi);
        }

    }
}
