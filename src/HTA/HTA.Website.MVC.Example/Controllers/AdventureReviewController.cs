using System;
using System.Linq;
using System.Web.Mvc;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using HTA.Website.MVC.Example.API;
using HTA.Website.MVC.Example.Models;

namespace HTA.Website.MVC.Example.Controllers
{
    public class AdventureReviewController : Controller
    {
        private static readonly IAdventureLocationRepository AdventureLocationRepository = new APIAdventureLocationProxy();
        private static readonly IAdventureTypeRepository AdventureTypeRepository = new APIAdventureTypeProxy();
        private static readonly IAdventureReviewRepository AdventureReviewRepository = new APIAdventureReviewProxy();
        //
        // GET: /AdventureReview/

        public ViewResult Index()
        {
            return View(AdventureReviewRepository.GetAdventureReviews());
        }

        //
        // GET: /AdventureReview/Details/5

        public ViewResult Details(string id)
        {
            AdventureReview adventurereview = AdventureReviewRepository.GetAdventureReviewById(id);
            return View(adventurereview);
        }

        //
        // GET: /AdventureReview/Create

        public ActionResult Create()
        {
            var model = new AdventureReviewModel();

            var adventureTypeList = AdventureTypeRepository.GetAdventureTypes();
            //adventureTypeList.Insert(0, new AdventureType(){Name = "-Select Adventure Type-"});
            model.SelectableTypes = adventureTypeList;

            return View(model);
        }

        //
        // POST: /AdventureReview/Create

        [HttpPost]
        public ActionResult Create(AdventureReviewModel adventurereview, FormCollection formCollection)
        {
            var adventureTypeList = AdventureTypeRepository.GetAdventureTypes();

            var adventureType = adventureTypeList.FirstOrDefault(t => t.Id == adventurereview.AdventureTypeId);

            adventurereview.Review.AdventureType = adventureType;

            adventurereview.Review.AdventureDate = DateTime.UtcNow;
            adventurereview.Review.AdventureDuration = new TimeSpan(1, 0, 0);

            //if (ModelState.IsValid)
            {
                GetDataCardsFromFormCollection(adventurereview, formCollection);

                AdventureLocation location = adventurereview.Review.AdventureLocation;
                var geoPoint = new GeoPoint() { Lat = 39.1042, Lon = -94.5745 };
                if (null == location || string.IsNullOrEmpty(location.Id))
                {
                    //var adventureLat = Convert.ToDouble(adventurereview.AdventureLat);
                    //var adventureLon = Convert.ToDouble(adventurereview.AdventureLon);
                    location = new AdventureLocation(geoPoint, "LOCATION_NAME");
                    adventurereview.Review.AdventureLocation =
                        AdventureLocationRepository.SaveAdventureLocation(location);
                }
                else
                {
                    adventurereview.Review.AdventureLocation =
                        AdventureLocationRepository.GetAdventureLocation(location.Id);


                }



                var review = AdventureReviewRepository.SaveAdventureReview(adventurereview.Review);

                return RedirectToAction("Details", new { review.Id });
            }

            return View(adventurereview);
        }

        private static void GetDataCardsFromFormCollection(AdventureReviewModel adventurereview, FormCollection formCollection)
        {
            var regionId =
                    formCollection.GetValue("nearByRegionDD").AttemptedValue;
            
            if (!string.IsNullOrEmpty(regionId))
            {
                adventurereview.Review.AdventureLocation.Region.Id = regionId;
            }

            var locationId =
                    formCollection.GetValue("nearByLocationDD").AttemptedValue;

            if (!string.IsNullOrEmpty(locationId))
            {
                adventurereview.Review.AdventureLocation.Id = locationId;
            }

            for (int i = 0; i < adventurereview.DataCardCount; i++)
            {
                var title =
                    formCollection.GetValue(string.Format("dataCardTitle{0}", i)).AttemptedValue;

                var body = formCollection.GetValue(string.Format("dataCardBody{0}", i)).AttemptedValue;

                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(body))
                {
                    adventurereview.Review.DataCards.Add(new AdventureDataCard { Title = title, Body = body });
                }
            }
        }

        //
        // GET: /AdventureReview/Edit/5

        public ActionResult Edit(string id)
        {
            AdventureReview adventurereview = AdventureReviewRepository.GetAdventureReviewById(id);
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
            return RedirectToAction("Index");
        }
    }
}