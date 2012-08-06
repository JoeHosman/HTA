using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using HTA.Website.MVC.Example.Models;

namespace HTA.Website.MVC.Example.Controllers
{
    public class AdventureReviewController : Controller
    {
        private static readonly IAdventureTypeRepository TypeRepository = new APIAdventureTypeProxy();
        //
        // GET: /AdventureReview/

        public ViewResult Index()
        {
            return View();
        }

        //
        // GET: /AdventureReview/Details/5

        public ViewResult Details(string id)
        {
            AdventureReview adventurereview = null;
            return View(adventurereview);
        }

        //
        // GET: /AdventureReview/Create

        public ActionResult Create()
        {
            var model = new AdventureReviewModel();

            model.SelectableTypes = TypeRepository.GetAdventureTypes();
            return View(model);
        }

        //
        // POST: /AdventureReview/Create

        [HttpPost]
        public ActionResult Create(FormCollection formCollection, AdventureReviewModel adventurereview)
        {
            if (ModelState.IsValid)
            {
                foreach (string key in formCollection.Keys)
                {

                }
                return RedirectToAction("Index");
            }

            return View(adventurereview);
        }

        //
        // GET: /AdventureReview/Edit/5

        public ActionResult Edit(string id)
        {
            AdventureReview adventurereview = null;
            return View(adventurereview);
        }

        //
        // POST: /AdventureReview/Edit/5

        [HttpPost]
        public ActionResult Edit(AdventureReview adventurereview)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("Index");
            }
            return View(adventurereview);
        }

        //
        // GET: /AdventureReview/Delete/5

        public ActionResult Delete(string id)
        {
            AdventureReview adventurereview = null;
            return View(adventurereview);
        }

        //
        // POST: /AdventureReview/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            AdventureReview adventurereview = null;
            return RedirectToAction("Index");
        }
    }

    public class AdventureReviewModel
    {
        public AdventureReview Review { get; set; }
        public IEnumerable<AdventureType> SelectableTypes { get; set; }

        public string AdventureTypeId { get; set; }
    }
}