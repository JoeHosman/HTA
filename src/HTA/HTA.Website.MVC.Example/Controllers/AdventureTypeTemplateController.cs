using System.Web.Mvc;
using HTA.Adventures.Models;
using HTA.Adventures.Models.Types;
using HTA.Website.MVC.Example.API;

namespace HTA.Website.MVC.Example.Controllers
{
    public class AdventureTypeTemplateController : Controller
    {
        private static readonly IAdventureTypeTemplateRepository TypeTemplateRepository = new APIAdventureTypeTemplateProxy();

        //
        // GET: /AdventureTypeTemplate/

        public ViewResult Index()
        {
            return View(TypeTemplateRepository.GetTypeTemplateList());
        }

        //
        // GET: /AdventureTypeTemplate/Details/5

        public ViewResult Details(string id)
        {
            AdventureTypeTemplate adventuretypetemplate = TypeTemplateRepository.GetTypeTemplate(id);


            //adventuretypetemplate.DataCards.Add(new AdventureDataCard() { Title = "Before you...", Body = "Do something..." });
            return View(adventuretypetemplate);
        }

        //
        // GET: /AdventureTypeTemplate/Create

        public ActionResult Create()
        {
            var adventureTypeTemplate = new AdventureTypeTemplate();
            for (int i = 0; i < 5; i++)
            {
                adventureTypeTemplate.DataCards.Add(new AdventureDataCard());
            }
            return View(adventureTypeTemplate);
        }

        //
        // POST: /AdventureTypeTemplate/Create

        [HttpPost]
        public ActionResult Create(AdventureTypeTemplate adventureTypeTemplate)
        {
            adventureTypeTemplate.DataCards.RemoveAll(
                                                            d => string.IsNullOrEmpty(d.Title) &&
                                                                string.IsNullOrEmpty(d.Body));

            if (ModelState.IsValid)
            {
                AdventureTypeTemplate template = TypeTemplateRepository.SaveTypeTemplate(adventureTypeTemplate);
                //return View("Details", new { te});
            }

            return View("Index");
        }

        //
        // GET: /AdventureTypeTemplate/Edit/5

        public ActionResult Edit(string id)
        {
            AdventureTypeTemplate adventureTypeTemplate = TypeTemplateRepository.GetTypeTemplate(id);

            for (int i = 0; i < 5; i++)
            {
                adventureTypeTemplate.DataCards.Add(new AdventureDataCard());
            }

            return View(adventureTypeTemplate);
        }

        //
        // POST: /AdventureTypeTemplate/Edit/5

        [HttpPost]
        public ActionResult Edit(AdventureTypeTemplate adventureTypeTemplate)
        {
            adventureTypeTemplate.DataCards.RemoveAll(
                                                           d => string.IsNullOrEmpty(d.Title) &&
                                                               string.IsNullOrEmpty(d.Body));
            if (ModelState.IsValid)
            {
                AdventureTypeTemplate template = TypeTemplateRepository.SaveTypeTemplate(adventureTypeTemplate);
                return View("Details", template.Id);
            }
            return View(adventureTypeTemplate);
        }

        //
        // GET: /AdventureTypeTemplate/Delete/5

        public ActionResult Delete(string id)
        {
            AdventureTypeTemplate adventuretypetemplate = TypeTemplateRepository.GetTypeTemplate(id);
            return View(adventuretypetemplate);
        }

        //
        // POST: /AdventureTypeTemplate/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            TypeTemplateRepository.DeleteTypeTemplate(id);
            return RedirectToAction("Index");
        }
    }
}