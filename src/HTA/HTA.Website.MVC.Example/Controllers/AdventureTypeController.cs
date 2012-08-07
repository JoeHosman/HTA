using System.Collections.Generic;
using System.Web.Mvc;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using HTA.Website.MVC.Example.API;
using HTA.Website.MVC.Example.Models;
using System.Linq;

namespace HTA.Website.MVC.Example.Controllers
{
    public class AdventureTypeController : Controller
    {
        private static readonly IAdventureTypeRepository TypeRepository = new APIAdventureTypeProxy();
        private static readonly IAdventureTypeTemplateRepository TypeTemplateRepository = new APIAdventureTypeTemplateProxy();

        //
        // GET: /AdventureType/

        public ViewResult Index()
        {
            return View(TypeRepository.GetAdventureTypes());
        }

        //
        // GET: /AdventureType/Details/5

        public ViewResult Details(string id)
        {
            AdventureType adventuretype = TypeRepository.GetAdventureType(id);

            if (adventuretype == null)
                return View("NotFound");
            return View(adventuretype);
        }

        //
        // GET: /AdventureType/Create

        public ActionResult Create()
        {
            //ViewBag.AdventureTypeTemplates = TypeTemplateRepository.GetTypeTemplateList();
            var model = new AdventureTypeModel {TemplateList = TypeTemplateRepository.GetTypeTemplateList()};
            return View(model);
        }

        //
        // POST: /AdventureType/Create

        [HttpPost]
        public ActionResult Create(AdventureTypeModel model)
        {
            if (ModelState.IsValid)
            {
                var templates = TypeTemplateRepository.GetTypeTemplateList();
                var selectedTemplates = templates.Where(t => model.SelectedTemplates.Contains(t.Id)).ToList();

                model.AdventureType.DataCardTemplates = selectedTemplates;

                return View("Details", TypeRepository.SaveAdventureType(model.AdventureType));
            }
            ViewBag.AdventureTypeTemplates = TypeTemplateRepository.GetTypeTemplateList();
            return View(model);
        }

        //
        // GET: /AdventureType/Edit/5

        public ActionResult Edit(string id)
        {
            AdventureType adventuretype = TypeRepository.GetAdventureType(id);

            if (adventuretype == null)
                return View("NotFound");

            var model = new AdventureTypeModel(adventuretype)
                            {TemplateList = TypeTemplateRepository.GetTypeTemplateList()};


            return View(model);
        }

        //
        // POST: /AdventureType/Edit/5

        [HttpPost]
        public ActionResult Edit(AdventureTypeModel model)
        {
            if (ModelState.IsValid)
            {
                var templates = TypeTemplateRepository.GetTypeTemplateList();
                var selectedTemplates = templates.Where(t => model.SelectedTemplates.Contains(t.Id)).ToList();

                model.AdventureType.DataCardTemplates = selectedTemplates;
                return View("Details", TypeRepository.SaveAdventureType(model.AdventureType));
            }
            model.TemplateList = TypeTemplateRepository.GetTypeTemplateList();
            return View(model);
        }

        //
        // GET: /AdventureType/Delete/5

        public ActionResult Delete(string id)
        {
            AdventureType adventuretype = TypeRepository.GetAdventureType(id);
            return View(adventuretype);
        }

        //
        // POST: /AdventureType/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            return RedirectToAction("Index");
        }

        public ActionResult Templates(string id)
        {
            AdventureType adventureType = TypeRepository.GetAdventureType(id);

            if (adventureType == null)
                return View("NotFound");

            var model = new UpdateTypeTemplatesModel
                            {
                                AdventureTypeId = adventureType.Id,
                                TemplateList = TypeTemplateRepository.GetTypeTemplateList()
                            };


            return View(model);
        }

        [HttpPost]
        public ActionResult Templates(UpdateTypeTemplatesModel request)
        {
            AdventureType adventureType = TypeRepository.GetAdventureType(request.AdventureTypeId);

            if (adventureType == null)
                return View("NotFound");

            var templates = TypeTemplateRepository.GetTypeTemplateList();
            var selectedTemplates = templates.Where(t => request.TemplatesSelected.Contains(t.Id)).ToList();

            adventureType.DataCardTemplates = selectedTemplates;

            TypeRepository.SaveAdventureType(adventureType);

            return View(request.AdventureTypeId);
        }
    }

    public class UpdateTypeTemplatesModel
    {
        public string AdventureTypeId { get; set; }
        public IEnumerable<string> TemplatesSelected { get; set; }
        public IEnumerable<AdventureTypeTemplate> TemplateList { get; set; }
    }
}