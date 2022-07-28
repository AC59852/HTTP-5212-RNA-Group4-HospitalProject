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
    public class ArticleController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ArticleController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44353/api/");
        }

        // *********** List of all Articles *************
        // *********** List of all Articles *************

        // GET: Article/List

        [HttpGet]
        public ActionResult List()
        {
            string url = "ArticleData/ListArticles";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<ArticleDto> Articles = response.Content.ReadAsAsync<IEnumerable<ArticleDto>>().Result;

            return View(Articles);
        }


        // ************* Detail of article of given ID **********************
        // ************* Detail of article of given ID **********************

        // GET: Article/Details/5

        [HttpGet]
        public ActionResult Details(int id)
        {
            DetailsArticle ViewModel = new DetailsArticle();

            string url = "ArticleData/FindArticle/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ArticleDto Article = response.Content.ReadAsAsync<ArticleDto>().Result;
            ViewModel.SelectedArticle = Article;
            
            return View(ViewModel);
        }


        // ******************* Add a new article *************************
        // ******************* Add a new article *************************

        // GET: Article/AddArticle

        [HttpGet]
        public ActionResult AddArticle()
        {
            // getting list of HUpdates for showing list in dropdown
            string url = "HUpdateData/ListHUpdates";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<HUpdate> HUpdates = response.Content.ReadAsAsync<IEnumerable<HUpdate>>().Result;

            return View(HUpdates);
        }

        // POST: Article/Add
        [HttpPost]
        public ActionResult Add(ArticleDto articleDto)
        {
            string url = "ArticleData/AddArticle";
            string jsonpayload = jss.Serialize(articleDto);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            
            return RedirectToAction("List");

            
        }



        // GET: Article/EditPage/5
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            // our view will also contain list of HUpdates so creating new ViewModel containg ArticleDTO and HUpdate
            EditArticle ViewModel = new EditArticle();

            // finding article of given id
            string url = "ArticleData/FindArticle/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ArticleDto Article = response.Content.ReadAsAsync<ArticleDto>().Result;
            ViewModel.editArticle = Article;

            // getting list of HUpdates for showing list in dropdown
            url = "HUpdateData/ListHUpdates";
            response = client.GetAsync(url).Result;
            IEnumerable<HUpdate> HUpdates = response.Content.ReadAsAsync<IEnumerable<HUpdate>>().Result;
            ViewModel.editHUpdates = HUpdates;

            return View(ViewModel);
        }

        // POST: Article/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ArticleDto articleDto)
        {
            string url = "ArticleData/EditArticle/" + id;
            string jsonpayload = jss.Serialize(articleDto);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("List");
            
            
        }


        // *********** Deleteing an article *******************
        // *********** Deleteing an article *******************

        // GET: Article/DeletePage/5
        [HttpGet]
        public ActionResult DeletePage(int id)
        {
            string url = "ArticleData/FindArticle/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            ArticleDto Article = response.Content.ReadAsAsync<ArticleDto>().Result;

            return View(Article);
        }

        // POST: Article/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, ArticleDto articleDto)
        {
            string url = "ArticleData/DeleteArticle/" + id;


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

        public ActionResult Error()
        {
            return View();
        }
    }
}
