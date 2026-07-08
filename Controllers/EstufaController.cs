using Microsoft.AspNetCore.Mvc;
using Firebase.Database;
using Firebase.Database.Query;

namespace EstufaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstufaController : ControllerBase
    {
        private readonly string _firebaseUrl = "https://estufaapi-73d82-default-rtdb.firebaseio.com/";
        private readonly FirebaseClient _firebaseClient;

        public EstufaController()
        {
            _firebaseClient = new FirebaseClient(_firebaseUrl);
        }

        [HttpPost("leitura")]
        public async Task<IActionResult> ReceberLeitura([FromBody] Leitura novaLeitura)
        {
            novaLeitura.DataHora = DateTime.Now;

            await _firebaseClient
                .Child("leituras")
                .PostAsync(novaLeitura);

            if (novaLeitura.Temperatura > 30 && novaLeitura.Umidade < 50)
            {
                var novoAlerta = new Alerta
                {
                    Temperatura = novaLeitura.Temperatura,
                    Umidade = novaLeitura.Umidade,
                    Mensagem = "Alerta de ressecamento! Temperatura alta e baixa umidade.",
                    DataHora = DateTime.Now
                };

                await _firebaseClient
                    .Child("alertas")
                    .PostAsync(novoAlerta);

                return Ok(new { msg = "Leitura registrada e ALERTA de perigo gerado!", alerta = true });
            }

            return Ok(new { msg = "Leitura registrada com sucesso.", alerta = false });
        }

        [HttpGet("alertas-criticos")]
        public async Task<IActionResult> ObterAlertasUltimas24Horas()
        {
            var todosAlertas = await _firebaseClient
                .Child("alertas")
                .OnceAsync<Alerta>();

            var limiteTempo = DateTime.Now.AddHours(-24);

            var alertasRecentes = todosAlertas
                .Select(item => item.Object)
                .Where(alerta => alerta.DataHora >= limiteTempo)
                .OrderByDescending(alerta => alerta.DataHora)
                .ToList();

            return Ok(alertasRecentes);
        }
    }
}