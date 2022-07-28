using HTTP_5212_RNA_Group4_HospitalProject.Models;
using HTTP_5212_RNA_Group4_HospitalProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HTTP_5212_RNA_Group4_HospitalProject.Controllers
{
    public class HUpdateController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static HUpdateController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44353/api/");
        }

        // ********************* List of all HUpdates **********************
        // ********************* List of all HUpdates **********************

        // GET: HUpdate/List

        [HttpGet]
        public ActionResult List()
        {
            string url = "HUpdateData/ListHUpdates";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<HUpdate> HUpdates = response.Content.ReadAsAsync<IEnumerable<HUpdate>>().Result;

            return View(HUpdates);
        }

        // *********************** getting detail of HUpdate of given Id *****************
        // *********************** getting detail of HUpdate of given Id *****************

        // GET: HUpdate/Details/5

        [HttpGet]
        public ActionResult Details(int id)
        {
            DetailsHUpdate ViewModel = new DetailsHUpdate();

            // getting details of Update
            string url = "HUpdateData/FindHUpdate/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            HUpdate SelectedHUpdate = response.Content.ReadAsAsync<HUpdate>().Result;
            ViewModel.SelectedUpdate = SelectedHUpdate;

            // getting list of all articles for this Update
            url = "ArticleData/ListArticlesForHUpdate/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ArticleDto> articleDtos = response.Content.ReadAsAsync<IEnumerable<ArticleDto>>().Result;
            ViewModel.ArticlesForHUpdate = articleDtos;

            return View(ViewModel);
        }

        // ************* Adding New Update ****************
        // ************* Adding New Update ****************

        // GET: HUpdate/AddHUpdate

        [HttpGet]
        public ActionResult AddHUpdate()
        {
            return View();
        }

        // POST: HUpdate/Add

        [HttpPost]
        public ActionResult Add(HUpdate hUpdate)
        {
            string url = "HUpdateData/AddHUpdate";

            string jsonpayload = jss.Serialize(hUpdate);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("List");
        }


        // ******************** Editing HUpdate of given Id *****************
        // ******************** Editing HUpdate of given Id *****************

        // GET: HUpdate/EditPage/5

        [HttpGet]
        public ActionResult EditPage(int id)
        {
            string url = "HUpdateData/FindHUpdate/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            HUpdate SelectedHUpdate = response.Content.ReadAsAsync<HUpdate>().Result;

            return View(SelectedHUpdate);
        }

        // POST: HUpdate/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, HUpdate hUpdate)
        {
            string url = "HUpdateData/EditHUpdate/" + id;
            string jsonpayload = jss.Serialize(hUpdate);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

                return RedirectToAction("List");

        }


        // *********************** Deleting an HUpdate of given Id *************************
        // *********************** Deleting an HUpdate of given Id ************************* 


        // GET: HUpdate/DeletePage/5

        [HttpGet]
        public ActionResult DeletePage(int id)
        {
            string url = "HUpdateData/FindHUpdate/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            HUpdate SelectedHUpdate = response.Content.ReadAsAsync<HUpdate>().Result;

            return View(SelectedHUpdate);
        }

        // POST: HUpdate/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, HUpdate hUpdate)
        {
            string url = "HUpdateData/DeleteHUpdate/" + id;

            HttpContent content = new StringContent("");

            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsJsonAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
