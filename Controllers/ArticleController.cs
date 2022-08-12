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
            // for authentication and authorization
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44353/api/");
        }

        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.

            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
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
        [Authorize(Roles ="Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult Add(ArticleDto articleDto)
        {
            GetApplicationCookie();
            string url = "ArticleData/AddArticle";
            string jsonpayload = jss.Serialize(articleDto);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            
            return RedirectToAction("List");

            
        }


        // ******************* Edit article *********************
        // ******************* Edit article *********************

        // GET: Article/EditPage/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, ArticleDto articleDto, HttpPostedFileBase ArticlePic)
        {
            GetApplicationCookie();
            string url = "ArticleData/EditArticle/" + id;
            string jsonpayload = jss.Serialize(articleDto);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            // handling if image has been added, if added then only we will run below method
            // also if incase image uploading will take time, till then our text data would be uploaded
            if(response.IsSuccessStatusCode && ArticlePic != null)
            {
                url = "ArticleData/UploadArticlePic/" + id;

                // as it is multiform data (same as we did in form)
                MultipartFormDataContent incomingcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(ArticlePic.InputStream);
                incomingcontent.Add(imagecontent, "ArticlePic", ArticlePic.FileName);
                response = client.PostAsync(url, incomingcontent).Result;

                return RedirectToAction("List");
            }

            return RedirectToAction("List");
            
            
        }


        // *********** Deleteing an article *******************
        // *********** Deleteing an article *******************

        // GET: Article/DeletePage/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeletePage(int id)
        {
            string url = "ArticleData/FindArticle/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            ArticleDto Article = response.Content.ReadAsAsync<ArticleDto>().Result;

            return View(Article);
        }

        // POST: Article/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, ArticleDto articleDto)
        {
            GetApplicationCookie();
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
