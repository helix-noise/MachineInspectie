﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Newtonsoft.Json;
using MachineInspectionLibrary;

namespace MachineInspectie.Controller
{
    public class Matis
    {
        private readonly Uri _apiMatis = new Uri("http://vangansewinkel.vanlaer-it.be/api/matis?location=");
        private HttpClient _client;
        private string _response;



        public async Task<List<MachineInspectionLibrary.Matis>> GetMatisByLocation(string Locatie)
        {
            try
            {
                Uri locatie = new Uri(_apiMatis.ToString() + Locatie);
                List<MachineInspectionLibrary.Matis> ListByLocation = new List<MachineInspectionLibrary.Matis>();
                using (_client = new HttpClient())
                {
                    _response = await _client.GetStringAsync(locatie);
                }
                ListByLocation = JsonConvert.DeserializeObject<List<MachineInspectionLibrary.Matis>>(_response);
                return ListByLocation;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
