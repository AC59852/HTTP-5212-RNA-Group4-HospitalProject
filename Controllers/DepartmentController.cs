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
    
    public class DepartmentController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DepartmentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44353/api/");
        }

        // GET: Department
        public ActionResult List()
        {
            string url = "departmentdata/listdepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<DepartmentDto> Departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;

            return View(Departments);
        }

        // GET: Department/Details/5
        public ActionResult Details(int id)
        {
            DetailsDepartment ViewModel = new DetailsDepartment();

            string url = "Departmentdata/findDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;

            ViewModel.SelectedDepartment = SelectedDepartment;
           
            return View(ViewModel);
        }

        //GET: Department/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        public ActionResult Create(Department Department)
        {
            string url = "Departmentdata/addDepartment";

            string jsonpayload = jss.Serialize(Department);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("List");
        }

        // GET: Department/Edit/5
        public ActionResult Edit(int id)
        {
            DetailsDepartment ViewModel = new DetailsDepartment();

            string url = "Departmentdata/findDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;

            ViewModel.SelectedDepartment = SelectedDepartment;

            return View(ViewModel);
        }

        // POST: Department/Update/5
        [HttpPost]
        public ActionResult Update(int id, Department Department)
        {
            string url = "Departmentdata/updateDepartment/" + id;
            string jsonpayload = jss.Serialize(Department);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("/Details/" + Department.DepartmentID);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "Departmentdata/findDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DepartmentDto selectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;

            return View(selectedDepartment);
        }

        // POST: Anime/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "Departmentdata/deleteDepartment/" + id;
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
    }

}