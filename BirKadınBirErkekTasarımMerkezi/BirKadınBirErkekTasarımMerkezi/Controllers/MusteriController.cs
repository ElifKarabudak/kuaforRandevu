using Newtonsoft.Json;
using System.Security.Claims;
using RestSharp;
using BirKadınBirErkekTasarımMerkezi.Data;
using BirKadınBirErkekTasarımMerkezi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace BirKadınBirErkekTasarımMerkezi.Controllers
{
    [Authorize]
    public class MusteriController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string OpenAIUrl = "https://api.openai.com/v1/chat/completions";
        private readonly string _apiKey;

        public MusteriController(ApplicationDbContext context, IOptions<OpenAISettings> openAISettings)
        {
            _apiKey = openAISettings.Value.ApiKey;
            _context = context;
        }

        public async Task<IActionResult> Randevularim()
        {
            var girisyapan = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationDbContext = _context.randevus
                                               .Include(r => r.Islemler)
                                               .Include(r => r.Ustalar)
                                               .Where(x => x.kullaniciID == girisyapan);
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaçStiliOnerisiAl(IFormFile foto)
        {
            if (foto == null || foto.Length == 0)
            {
                ModelState.AddModelError("", "Lütfen bir fotoğraf yükleyin Fotoğrafsız nasıl analiz yapalım ? :D.");
                return View("Index");
            }

            // Fotoğrafı Base64 formatına dönüştür
            var base64Foto = FotoBase64eDonustur(foto);

            // OpenAI API'ye istek yap
            var prompt = "Bu fotoğraftaki kişi için saç kesim stili ve saç rengi öner. A";
            var response = await OpenAIyeIstekGonder(base64Foto, prompt);

            // View'e sonucu gönder
            ViewBag.Oneriler = response;
            return View("Index");
        }

        private string FotoBase64eDonustur(IFormFile foto)
        {
            using var ms = new MemoryStream();
            foto.CopyTo(ms);
            var dosyaByte = ms.ToArray();
            return Convert.ToBase64String(dosyaByte);
        }

        private async Task<string> OpenAIyeIstekGonder(string base64Foto, string prompt)
        {
            var client = new RestClient(OpenAIUrl);
            var request = new RestRequest(OpenAIUrl, RestSharp.Method.Post);

            // Header bilgileri
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            request.AddHeader("Content-Type", "application/json");

            // API'ye gönderilecek veriler
            var body = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = new object[]
                        {
                            new { type = "text", text = prompt },
                            new { type = "image_url", image_url = new { url = $"data:image/jpeg;base64,{base64Foto}" } }
                        }
                    }
                },
                max_tokens = 500
            };

            // JSON body ekle
            request.AddJsonBody(JsonConvert.SerializeObject(body));

            // API isteğini gönder
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);
                return jsonResponse.choices[0].message.content;
            }
            else
            {
                return "Başaramadık abi .";
            }
        }
    }
}
