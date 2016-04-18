using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Newtonsoft.Json;
using MachineInspectionLibrary;

namespace MachineInspectie.Dal
{
    public class Matis
    {
        private readonly Uri _apiMatis = new Uri("http://vangansewinkel.vanlaer-it.be/api/matis?location=");
        private HttpClient _client;
        private string _response;



        public async Task<List<MachineInspectionLibrary.Matis>> GetMatisByLocation(string locatie)
        {
            try
            {
                Uri apiMatis = new Uri(_apiMatis.ToString() + locatie);
                
                using (_client = new HttpClient())
                {
                    _response = await _client.GetStringAsync(apiMatis);
                }
                List<MachineInspectionLibrary.Matis> listByLocation = JsonConvert.DeserializeObject<MatisWrapper>(_response).data;
                return listByLocation;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
