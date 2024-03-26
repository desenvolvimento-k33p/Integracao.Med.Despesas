using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Integracao.Med.Despesas.Services
{
    public abstract class Service_New
    {
        protected StringContent ObterConteudo(object dado)
        {
            string teste = JsonSerializer.Serialize(dado);

            return new StringContent(
                JsonSerializer.Serialize(dado),
                Encoding.UTF8,
                "application/json");
        }

        protected async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            string parada = await responseMessage.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(parada, options);
        }

        //protected async Task<T> DeserializarObjetoResponseTeste<T>(HttpResponseMessage responseMessage)
        //{
        //    var options = new JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true
        //    };

        //    //string parada = await responseMessage.Content.ReadAsStringAsync();
        //    string parada = Resources.Teste;

        //    return JsonSerializer.Deserialize<T>(parada, options);
        //}

        protected virtual bool TratarErrosResponse(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest) return false;

            //response.EnsureSuccessStatusCode();
            return true;
        }

        protected dynamic RetornoOk()
        {
            return true;
        }
    }
}