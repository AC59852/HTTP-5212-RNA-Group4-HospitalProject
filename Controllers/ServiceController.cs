using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTP_5212_RNA_Group4_HospitalProject.Models;
using HTTP_5212_RNA_Group4_HospitalProject.Models.ViewModels;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Diagnostics;


namespace HTTP_5212_RNA_Group4_HospitalProject.Controllers
{
    public class ServiceController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ServiceController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44353/api/");
        }

        // GET: Service
        public ActionResult List()
        {
            string url = "Servicedata/listservices";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ServiceDto> services = response.Content.ReadAsAsync<IEnumerable<ServiceDto>>().Result;

            return View(services);
        }

        // GET: Service/Details/5
        public ActionResult Details(int id)
        {
            DetailsService ViewModel = new DetailsService();

            string url = "Servicedata/findService/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ServiceDto SelectedService = response.Content.ReadAsAsync<ServiceDto>().Result;

            ViewModel.SelectedService = SelectedService;

            return View(ViewModel);
        }

        // GET: Service/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Service/Create
        [HttpPost]
        public ActionResult Create(Service Service)
        {
            string url = "Servicedata/addService";

            string jsonpayload = jss.Serialize(Service);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

           
            return RedirectToAction("List");
        }

        // GET: Service/Edit/5
        public ActionResult Edit(int id)
        {
            DetailsService ViewModel = new DetailsService();

            string url = "Servicedata/findService/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ServiceDto SelectedService = response.Content.ReadAsAsync<ServiceDto>().Result;

            ViewModel.SelectedService = SelectedService;

            return View(ViewModel);
        }

        // POST: Service/Update/5
        [HttpPost]
        public ActionResult Update(int id, Service service, HttpPostedFileBase ServiceImage)
        {
            
            string url = "servicedata/updateservice/" + id;
            string jsonpayload = jss.Serialize(service);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
           

            //update request is successful, and we have image data
            if (response.IsSuccessStatusCode && ServiceImage != null)
            {

                //Updating the service image as a separate request
                //Send over image data for player
                url = "ServiceData/UploadServiceImage/" + id;

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(ServiceImage.InputStream);
                requestcontent.Add(imagecontent, "ServiceImage", ServiceImage.FileName);
                response = client.PostAsync(url, requestcontent).Result;

                return RedirectToAction("/Details/" + id);
            }
            else if (response.IsSuccessStatusCode)
            {
                //No image upload, but update still successful
                return RedirectToAction("/Details/" + id);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Service/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "Servicedata/findService/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ServiceDto selectedservice = response.Content.ReadAsAsync<ServiceDto>().Result;
            return View(selectedservice);
        }

        // POST: Service/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {

            string url = "Servicedata/deleteservice/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
