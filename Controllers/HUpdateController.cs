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
            HUpdateDto SelectedHUpdate = response.Content.ReadAsAsync<HUpdateDto>().Result;
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
        [Authorize(Roles = "Admin")]
        public ActionResult AddHUpdate()
        {
            string url = "DepartmentData/listdepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Department> Departments = response.Content.ReadAsAsync<IEnumerable<Department>>().Result;

            return View(Departments);
        }

        // POST: HUpdate/Add

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public ActionResult Add(HUpdateDto hUpdateDto)
        {
            GetApplicationCookie();
            string url = "HUpdateData/AddHUpdate";
            string jsonpayload = jss.Serialize(hUpdateDto);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("List");
        }


        // ******************** Editing HUpdate of given Id *****************
        // ******************** Editing HUpdate of given Id *****************

        // GET: HUpdate/EditPage/5

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public ActionResult EditPage(int id)
        {
            // our view will also contain list of Departments so creating new ViewModel containg HUpdateDTO and Department
            EditHUpdate ViewModel =  new EditHUpdate();

            // finding HUpdate of given id
            string url = "HUpdateData/FindHUpdate/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            HUpdateDto SelectedHUpdate = response.Content.ReadAsAsync<HUpdateDto>().Result;
            ViewModel.editHUpdate = SelectedHUpdate;

            // getting list of all departments
            url = "DepartmentData/listdepartments";
            response = client.GetAsync(url).Result;
            IEnumerable<Department> Departments = response.Content.ReadAsAsync<IEnumerable<Department>>().Result;
            ViewModel.editDepartments =  Departments;
            

            return View(ViewModel);
        }

        // POST: HUpdate/Edit/5

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public ActionResult Edit(int id, HUpdateDto hUpdateDto)
        {
            GetApplicationCookie();
            string url = "HUpdateData/EditHUpdate/" + id;
            string jsonpayload = jss.Serialize(hUpdateDto);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

                return RedirectToAction("List");

        }


        // *********************** Deleting an HUpdate of given Id *************************
        // *********************** Deleting an HUpdate of given Id ************************* 


        // GET: HUpdate/DeletePage/5

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public ActionResult DeletePage(int id)
        {
            string url = "HUpdateData/FindHUpdate/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            HUpdateDto SelectedHUpdate = response.Content.ReadAsAsync<HUpdateDto>().Result;

            return View(SelectedHUpdate);
        }

        // POST: HUpdate/Delete/5

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public ActionResult Delete(int id, HUpdateDto hUpdateDto)
        {
            GetApplicationCookie();
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
