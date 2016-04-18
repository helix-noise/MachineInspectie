using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
//using Windows.Web.Http;
using MachineInspectionLibrary;

namespace MachineInspectie.Dal
{
    public class Locatie
    {
        private readonly Uri _apiLocatie = new Uri("http://vangansewinkel.vanlaer-it.be/api/location");
        //private readonly string apiLocatie = "http://vangansewinkel.vanlaer-it.be/api/location";
        private HttpClient _client;
        private string _response { get; set; }
        //public List<Location> LocationList { get; set; }

        //public async Task<string> GetApiLocation()
        //{
        //    //using (_client = new HttpClient())
        //    //{
        //    //    _response = await _client.GetStringAsync(_apiLocatie);
        //    //}
        //    //return _response;
        //    _client = new HttpClient();
        //    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiLocatie);
        //    HttpResponseMessage response = await _client.SendAsync(request);
        //    string returnString = await response.Content.ReadAsStringAsync();
        //    return returnString;
        //}

        //public async void Get()
        //{
        //    HttpClient client = new HttpClient();
        //    string response = await client.GetStringAsync(_apiLocatie);
        //    List<Location> obj = JsonConvert.DeserializeObject<List<Location>>(response);
        //    if (obj != null)
        //    {
        //        LocationList = obj;
        //    }
        //    else
        //    {
        //        LocationList = new List<Location>();
        //    }
        //}

        //public async Task<LocationWrapper> LocationList()
        //{

        //    //List<Datum> location = JsonConvert.DeserializeObject<List<Datum>>(await GetApiLocation());
        //    //return location;

        //    var location = JsonConvert.DeserializeObject<LocationWrapper>(await GetApiLocation());
        //    return location;
        //}

        public async Task<List<MachineInspectionLibrary.Location>> GetListLocation()
        {
            try
            {
                using (_client = new HttpClient())
                {
                    _response = await _client.GetStringAsync(_apiLocatie);
                }

                List<Location> locatieLijst = JsonConvert.DeserializeObject<LocationWrapper>(_response).data;

                return locatieLijst;
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
