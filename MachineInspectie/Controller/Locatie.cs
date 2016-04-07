using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Web.Http;

namespace MachineInspectie.Controller
{
    public class Locatie
    {
        private readonly Uri _apiLocatie = new Uri("http://vangansewinkel.vanlaer-it.be/api/location");
        private HttpClient _client;
        private string _response;

        public async Task<List<Model.Locatie>> GetListLocation()
        {
            try
            {
                using (_client = new HttpClient())
                {
                    _response = await _client.GetStringAsync(_apiLocatie);
                }

                List<Model.Locatie> LocatieLijst = JsonConvert.DeserializeObject<List<Model.Locatie>>(_response);

                return LocatieLijst;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
