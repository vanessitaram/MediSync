using System.Text;
using System.Text.Json;

namespace MediSync.Services
{
    public class IAService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public IAService(IConfiguration configuration)
        {
            _apiKey = configuration["OpenAI:ApiKey"] ?? throw new Exception("Falta API Key de OpenAI");
            _httpClient = new HttpClient();
        }

        public async Task<string> GenerarPrediagnosticoAsync(string sintomas)
        {
            string prompt = $@"
Eres un asistente clínico especializado en apoyo diagnóstico dentro del sistema médico MediSync. 
Tu función es analizar los síntomas descritos por el paciente y generar un informe dirigido al médico tratante. 

**Instrucciones del análisis:**
1. Describe brevemente los síntomas principales.
2. Identifica las posibles causas o patologías relacionadas con base en el texto.
3. Evalúa si los síntomas podrían implicar una condición leve, moderada o grave.
4. Si hay riesgo evidente o combinación de síntomas que sugieran urgencia, indícalo claramente en un tono profesional.
5. Proporciona recomendaciones médicas contextuales para orientar al médico (p. ej., posibles estudios, factores a revisar, seguimiento, etc.).

**Restricciones estrictas:**
- No indiques que el paciente debe acudir con un especialista, médico o institución: ya se encuentra bajo supervisión médica.
- No inventes información ajena a los síntomas o patologías médicas (sin teorías, emociones, ni suposiciones).
- No des consejos de automedicación ni de tratamiento directo.
- Usa terminología clínica, objetiva y profesional.
- Responde en formato claro, con encabezados y listas ordenadas si es necesario.

Ahora analiza los síntomas proporcionados por el paciente y genera el prediagnóstico clínico:
---
Síntomas: {sintomas}
---";

            var body = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system", content = "Eres un asistente clínico de apoyo diagnóstico en MediSync, especializado en análisis de síntomas para médicos." },
                    new { role = "user", content = prompt }
                },
                temperature = 0.6
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            return doc.RootElement
                      .GetProperty("choices")[0]
                      .GetProperty("message")
                      .GetProperty("content")
                      .GetString()?
                      .Trim()
                   ?? "No se pudo generar el prediagnóstico.";
        }
    }
}
