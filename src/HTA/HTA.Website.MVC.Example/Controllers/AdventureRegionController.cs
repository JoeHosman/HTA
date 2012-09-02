using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
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
            var region = _adventureRegionRepository.GetAdventureRegion(id);
            var regionLocations = _adventureLocationRepository.GetRegionAdventureLocations(id);

            var model = new AdventureRegionAdventureLocationsModel();
            model.Region = region;
            model.Locations = regionLocations;

            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Region model)
        {
            if (ModelState.IsValid)
            {
                model = _adventureRegionRepository.SaveAdventureRegion(model);

                var item = new AdventureRegionAdventureLocationsModel() { Region = model, Locations = new List<Location>() };
                return View("Details", item);
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

        public ActionResult Edit(string id)
        {
            var region = _adventureRegionRepository.GetAdventureRegion(id);
            var regionLocations = _adventureLocationRepository.GetRegionAdventureLocations(id);

            var model = new AdventureRegionAdventureLocationsModel();
            model.Region = region;
            model.SelectedLocations = regionLocations.Select(s => s.Id).ToList();
            model.Locations = _adventureLocationRepository.GetAdventureLocations();
            // i dont think we need to add one, just do it by default.
            //model.Locations.Insert(0, new AdventureLocation(new LocationPoint() { Lat = double.MaxValue, Lon = double.MaxValue }, "Create New", string.Empty, string.Empty));

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(AdventureRegionAdventureLocationsModel model)
        {
            if (ModelState.IsValid)
            {
                var locations = new List<Location>();
                foreach (var locationId in model.SelectedLocations)
                {
                    // Get existing location object
                    var locationResponse = _adventureLocationRepository.GetAdventureLocation(locationId);

                    // set location.region to new region
                    locationResponse.Location.Region = model.Region;

                    // save in API and get response
                    locationResponse = _adventureLocationRepository.SaveAdventureLocation(locationResponse.Location);

                    // add response to this response.locations
                    locations.Add(locationResponse.Location);
                }

                var region = model.Region;

                model.Region = _adventureRegionRepository.SaveAdventureRegion(region);
                model.Locations = locations;
                return View("Details", model);
            }

            model.Locations = _adventureLocationRepository.GetAdventureLocations();
            return View(model);
        }
    }
}