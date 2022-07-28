using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HTTP_5212_RNA_Group4_HospitalProject.Models;

namespace HTTP_5212_RNA_Group4_HospitalProject.Controllers
{
    public class ArticleDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // ************************* Listing all articles *******************************
        // ************************* Listing all articles *******************************


        /// <summary>
        /// Returns all Articles present in hospital database
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// DATA: List of all Articles and information related to them
        /// </returns>
        /// <example>
        /// GET: api/ArticleData/ListArticles       
        /// </example>
        
        [HttpGet]
        [Route("api/ArticleData/ListArticles")]
        [ResponseType(typeof(ArticleDto))]
        public IEnumerable<ArticleDto> ListArticles()
        {
            List<Article> Articles = db.Articles.ToList();
            List<ArticleDto> articleDtos = new List<ArticleDto>();

            Articles.ForEach(a => articleDtos.Add(new ArticleDto()
            {
                ArticleID = a.ArticleID,
                ArticleTitle = a.ArticleTitle,
                ArticleContent = a.ArticleContent,

                // info related to Update
                HUpdateId = a.HUpdate.HUpdateId,
                HUpdateTitle = a.HUpdate.HUpdateTitle,
                HUpdateType = a.HUpdate.HUpdateType                
               
            }));

            return articleDtos;
        }

        // ********************* Get details of Article of given id *************************
        // ********************* Get details of Article of given id *************************

        /// <summary>
        /// Returns an Article of given id present in hospital database and its infromation
        /// </summary>
        /// <param name="id">id for Article which needs to be find</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// DATA:  all information of Article with given id
        /// </returns>
        /// <example>
        /// GET: api/ArticleData/FindArticle/5     
        /// </example>
        
        [HttpGet]
        [ResponseType(typeof(Article))]
        [Route("api/ArticleData/FindArticle/{id}")]
        public IHttpActionResult GetArticle(int id)
        {
            Article article = db.Articles.Find(id);
            ArticleDto articleDto = new ArticleDto()
            {
                ArticleID = article.ArticleID,
                ArticleTitle = article.ArticleTitle,
                ArticleContent = article.ArticleContent,

                // info related to Update
                HUpdateId = article.HUpdate.HUpdateId,
                HUpdateTitle = article.HUpdate.HUpdateTitle,
                HUpdateType = article.HUpdate.HUpdateType

            };

            if (article == null)
            {
                return NotFound();
            }

            return Ok(articleDto);
        }



        //**************** List of article for given Events id *************
        //**************** List of article for given Events id *************


        /// <summary>
        /// Returns all Articles which are related to update or event of given id
        /// </summary>
        /// <param name="id">id of update or event for which articles needs to be fetched</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// DATA: List of all Articles, which are related to given HUpdate
        /// </returns>
        /// <example>
        /// GET : api/ArticleData/ListArticleForHUpdate/1       
        /// </example>
        
        [HttpGet]
        [Route("api/ArticleData/ListArticlesForHUpdate/{id}")]
        public IEnumerable<ArticleDto> ListArticlesForHUpdate(int id)
        {
            List<Article> Articles = db.Articles.Where(a => a.HUpdateId == id).ToList();
            List<ArticleDto> articleDtos = new List<ArticleDto>();

            Articles.ForEach(a => articleDtos.Add(new ArticleDto()
            {
                ArticleID = a.ArticleID,
                ArticleTitle = a.ArticleTitle,
                ArticleContent = a.ArticleContent
            }));

            Debug.WriteLine("My second error messa", articleDtos);

            return articleDtos;
        }


        //****************** Updating an article of given id **********************
        //****************** Updating an article of given id ********************** 

        /// <summary>
        /// Perform an edit on Article data of given id present in database
        /// </summary>
        /// <param name="id">id of Article on which edit is being performed</param>
        /// <param name="article">an Article object containing edited infromation</param>
        /// <returns>
        /// HEADER: 200 (OK) or Bad request (500) etc
        /// DATA: No data is returned
        /// </returns>
        /// <example>
        /// POST: api/ArticleData/EditArticle/5       
        /// </example>
       
        [HttpPost]
        [Route("api/ArticleData/EditArticle/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult EditArticle(int id, Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != article.ArticleID)
            {
                return BadRequest();
            }

            db.Entry(article).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // ************** adding an Article ******************
        // ************** adding an Article ******************

        /// <summary>
        /// adds a new article with its information in database
        /// </summary>
        /// <param name="article">an Article object containing new information of new article</param>
        /// <returns>
        /// HEADER: 200 (OK) or Bad request (500) etc
        /// DATA: no data is returned
        /// </returns>
        /// <example>
        /// POST: api/ArticleData/AddArticle   
        /// </example>

        [HttpPost]
        [Route("api/ArticleData/AddArticle")]
        [ResponseType(typeof(Article))]
        public IHttpActionResult AddArticle(Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Articles.Add(article);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = article.ArticleID }, article);
        }


        //*************** Deleting an Article of a given id **********************
        //*************** Deleting an Article of a given id **********************


        /// <summary>
        /// Deletes all information of Article of given id
        /// </summary>
        /// <param name="id">id of Article which needs to be deleted</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// DATA: no data is returned
        /// </returns>
        /// <example>
        /// POST: api/ArticleData/DeleteArticle      
        /// </example>
        
        [HttpPost]
        [Route("api/ArticleData/DeleteArticle/{id}")]
        [ResponseType(typeof(Article))]
        public IHttpActionResult DeleteArticle(int id)
        {
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return NotFound();
            }

            db.Articles.Remove(article);
            db.SaveChanges();

            return Ok(article);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ArticleExists(int id)
        {
            return db.Articles.Count(e => e.ArticleID == id) > 0;
        }
    }
}