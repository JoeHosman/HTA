﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using HTA.Adventures.BusinessLogic;
using HTA.Adventures.Data.ModelValidation;
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
            //model.SelectableAdventureRegions.Insert(0, new AdventureRegion(new LocationPoint() { Lat = double.MaxValue }, "** Create New Region **", "", ""));
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(AdventureLocationModel model)
        {
            if (ModelState.IsValid)
            {
                // Assign the adventure region to the location object.
                model.Location
                    .Region = _adventureRegionRepository
                                        .GetAdventureRegion(model.Location.Region.Id);

                var locationResponse = _adventureLocationRepository.SaveAdventureLocation(model.Location);

                return View("Details", locationResponse.Location);
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
                else
                {
                    //foreach (var modelStateValue in ViewData.ModelState.Values)
                    //{
                    //    foreach (var error in modelStateValue.Errors)
                    //    {
                    //        // Do something useful with these properties
                    //        var errorMessage = error.ErrorMessage;
                    //        var exception = error.Exception;
                    //    }
                    //}
                    model.SelectableAdventureRegions = _adventureRegionRepository.GetAdventureRegions();
                    return View(model);
                }
            }
        }

        public ActionResult Delete(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}