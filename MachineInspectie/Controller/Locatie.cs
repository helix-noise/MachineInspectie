using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Web.Http;
using MachineInspectionLibrary;

namespace MachineInspectie.Controller
{
    public class Locatie
    {
        private readonly Uri _apiLocatie = new Uri("http://vangansewinkel.vanlaer-it.be/api/location");
        private readonly string _apiLocatieString = "http://vangansewinkel.vanlaer-it.be/api/location";
        private HttpClient _client;
        private string _response { get; set; }
        private HttpWebRequest _webRequest;

        public async Task<string> GetApiLocation()
        {
            using (_client = new HttpClient())
            {
                _response = await _client.GetStringAsync(_apiLocatie);
            }
            return _response;
        }

        public async Task<List<Location>> LocationList()
        {
            using (_client = new HttpClient())
            {
                HttpResponseMessage response = await _client.GetAsync(_apiLocatie);
                response.EnsureSuccessStatusCode();
            }


            //await GetApiLocation();
            //List<Location> Location = JsonConvert.DeserializeObject<List<Location>>(_response);
            //return Location;

            return new List<Location>();
        } 

        public async Task<List<MachineInspectionLibrary.Location>> GetListLocation()
        {
            try
            {
                using (_client = new HttpClient())
                {
                    _response = await _client.GetStringAsync(_apiLocatie);
                }

                List<MachineInspectionLibrary.Location> LocatieLijst = JsonConvert.DeserializeObject<List<MachineInspectionLibrary.Location>>(_response);

                return LocatieLijst;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //public List<MachineInspectionLibrary.Location> GetListLocation()
        //{
        //    try
        //    {
        //        _webRequest = (HttpWebRequest)WebRequest.Create(_apiLocatie);
        //        _webRequest.Accept = "application/json";
        //        _webRequest.Method = HttpMethod.Get.ToString();
        //        var response = _webRequest.GetResponseAsync().Result;
        //        using (var sr = new StreamReader(response.GetResponseStream()))
        //        {
        //            _response = sr.ReadToEnd();
        //        }
        //        List<MachineInspectionLibrary.Location> LocatieLijst = JsonConvert.DeserializeObject<List<MachineInspectionLibrary.Location>>(_response);

        //        return LocatieLijst;
        //    }
        //    catch (Exception)
        //    {

        //        return new List<MachineInspectionLibrary.Location>();
        //    }
        //}


    }
}
