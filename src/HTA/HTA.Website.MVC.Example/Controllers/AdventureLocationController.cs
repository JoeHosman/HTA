using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using HTA.Adventures.API.ServiceInterface;
using HTA.Adventures.BusinessLogic;
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
            model.SelectableAdventureRegions.Insert(0, Region.CreateNewRegion);
            //model.SelectableAdventureRegions.Insert(0, new AdventureRegion(new LocationPoint() { Lat = double.MaxValue }, "** Create New Region **", "", ""));
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(AdventureLocationModel model)
        {
            using (var businessValidator = new LocationBusiness())
            {
                IList<ValidationResult> validationErrorResults = new List<ValidationResult>();
                if (businessValidator.Validate(model.Location, validationErrorResults))
                {
                    // Assign the adventure region to the location object.
                    model.Location
                        .Region = _adventureRegionRepository
                            .GetAdventureRegion(model.Location.Region.Id);

                    var locationResponse = _adventureLocationRepository.SaveAdventureLocation(model.Location);

                    return View("Details", locationResponse.Location);
                }

                ErrorUtility.TransformResponseErrors(ModelState, validationErrorResults);

                model.SelectableAdventureRegions = _adventureRegionRepository.GetAdventureRegions();
                model.SelectableAdventureRegions.Insert(0, Region.CreateNewRegion);

            }


            return View(model);
        }

        public ActionResult Details(string id)
        {
            var location = _adventureLocationRepository.GetAdventureLocation(id);
            return View(location.Location);
        }

        public ActionResult Edit(string id)
        {
            var locationResponse = _adventureLocationRepository.GetAdventureLocation(id);

            var model = new AdventureLocationModel()
                            {
                                Location = locationResponse.Location,
                                SelectableAdventureRegions = _adventureRegionRepository.GetAdventureRegions()
                            };
            model.SelectableAdventureRegions.Insert(0, Region.CreateNewRegion);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(AdventureLocationModel model)
        {
            using (var businessValidator = new LocationBusiness())
            {
                IList<ValidationResult> validationErrorResults = new List<ValidationResult>();
                if (businessValidator.Validate(model.Location, validationErrorResults))
                {
                    // Assign the adventure region to the location object.
                    model.Location
                        .Region = _adventureRegionRepository
                                            .GetAdventureRegion(model.Location.Region.Id);

                    var location = _adventureLocationRepository.SaveAdventureLocation(model.Location);
                    return RedirectToAction("Details", location.Location);
                }

                model.SelectableAdventureRegions = _adventureRegionRepository.GetAdventureRegions();
                model.SelectableAdventureRegions.Insert(0, Region.CreateNewRegion);

                ErrorUtility.TransformResponseErrors(ModelState, validationErrorResults);

                return View(model);
            }
        }

        public ActionResult Delete(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}