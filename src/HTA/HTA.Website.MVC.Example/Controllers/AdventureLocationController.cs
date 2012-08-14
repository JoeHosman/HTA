using System.Web.Mvc;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using HTA.Website.MVC.Example.API;
using HTA.Website.MVC.Example.Models;

namespace HTA.Website.MVC.Example.Controllers
{
    public class AdventureLocationController : Controller
    {
        private readonly IAdventureLocationRepository _adventureLocationRepository = new APIAdventureLocationProxy();
        private readonly IAdventureRegionRepository _adventureRegionRepository = new APIAdventureRegionProxy();
        public ActionResult Index()
        {
            var locations = _adventureLocationRepository.GetAdventureLocations();
            return View(locations);
        }

        public ActionResult Create()
        {
            var model = new AdventureLocationModel();

            model.SelectableAdventureRegions = _adventureRegionRepository.GetAdventureRegions();
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(AdventureLocationModel model)
        {
            if (ModelState.IsValid)
            {
                var location = _adventureLocationRepository.SaveAdventureReview(model.AdventureLocation);
                return View("Details", location);
            }
            return View(model);
        }

        public ActionResult Details(string id)
        {
            var location = _adventureLocationRepository.GetAdventureLocation(id);
            return View(location);
        }

        public ActionResult Edit(string id)
        {
            var location = _adventureLocationRepository.GetAdventureLocation(id);
            return View(location);
        }

        public ActionResult Delete(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}