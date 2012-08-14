using System.Web.Mvc;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using HTA.Website.MVC.Example.API;
using HTA.Website.MVC.Example.Models;

namespace HTA.Website.MVC.Example.Controllers
{
    public class AdventureRegionController : Controller
    {
        private readonly IAdventureRegionRepository _adventureRegionRepository = new APIAdventureRegionProxy();
        private readonly IAdventureLocationRepository _adventureLocationRepository = new APIAdventureLocationProxy();

        public ActionResult Index()
        {
            var regions = _adventureRegionRepository.GetAdventureRegions();

            return View(regions);
        }

        public ActionResult Details(string id)
        {
            AdventureRegion region = _adventureRegionRepository.GetAdventureRegion(id);
            return View(region);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(AdventureRegion model)
        {
            if (ModelState.IsValid)
            {
                model = _adventureRegionRepository.SaveAdventureRegion(model);
                return View("Details", model);
            }
            return View(model);
        }

        public ActionResult Locations(string id)
        {
            var region = _adventureRegionRepository.GetAdventureRegion(id);
            var regionLocations = _adventureLocationRepository.GetRegionAdventureLocations(id);

            var model = new AdventureRegionAdventureLocationsModel();
            model.Region = region;
            model.Locations = regionLocations;

            return View(model);
        }
    }
}