using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoListInfrastructure.DTOs;
using ToDoListInfrastructure.Models.Services;
using ToDoListInfrastructure.Models.ViewModels.ToDoEntry;

namespace ToDoList.Controllers
{
    public class ToDoEntryController : Controller
    {
        private readonly IToDoEntryService service;
        private readonly UserManager<IdentityUser> userManager;

        private readonly int pageSize = 4;

        public ToDoEntryController(IToDoEntryService service, UserManager<IdentityUser> userManager)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service), "Given service is null.");
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager), "Given manager is null.");
        }

        // GET: ToDoEntryController
        [Authorize]
        public IActionResult Index(Guid Id, int listPage = 1, bool hideCompleted = true)
        {
            ViewBag.ToDoListId = Id;
            var toDoEntriesCollection = this.service.ReadToDoEntriesByToDoListId(Id, listPage, pageSize, hideCompleted);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView(toDoEntriesCollection);
            }

            return View(toDoEntriesCollection);
        }

        // GET: ToDoEntryController/Details/5
        public IActionResult Details(Guid Id, Guid tdlId)
        {
            var toDoEntryDetails = this.service.ReadToDoEntry(Id);
            ViewBag.ToDoListId = tdlId;

            return View(toDoEntryDetails);
        }

        // GET: ToDoEntryController/Create
        public IActionResult Create(Guid toDoListId)
        {
            ViewBag.ToDoListId = toDoListId;
            return View();
        }

        // POST: ToDoEntryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateToDoEntryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var createdToDoEntry = this.service.CreateToDoEntry(model);

            return RedirectToAction("Details", "ToDoEntry", new { id = createdToDoEntry.Id, tdlId = model.ToDoListId });
        }

        // GET: ToDoEntryController/Edit/5
        public IActionResult Edit(Guid id, Guid tdlId)
        {
            ViewBag.ToDoEntryId = id;
            ViewBag.ToDoListId = tdlId;
            return View();
        }



        // POST: ToDoEntryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditToDoEntryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ToDoEntryDetailsDto editedToDoEntry = this.service.EditToDoEntry(model);

            return RedirectToAction("Details", "ToDoEntry", new { id = editedToDoEntry.Id, tdlId=model.ToDoListId });
        }

        // GET: ToDoEntryController/Delete/5
        public IActionResult Delete(Guid id)
        {
            return View();
        }

        // POST: ToDoEntryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(DeleteToDoEntryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.service.DeleteToDoEntryAsync(model);


            return RedirectToAction("Index", "ToDoEntry", new { id = model.ToDoListId });
        }

        public IActionResult ChangeProgressStatus(Guid toDoEntryId, Guid toDoListId)
        {
            ViewBag.ToDoEntryId = toDoEntryId;
            ViewBag.ToDoListId = toDoListId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeProgressStatus(ChangeProgressStatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.service.ChangeProgressValue(model);

            return RedirectToAction("Index", "ToDoEntry", new { id=model.ToDoListId });
        }

        [Authorize]
        public IActionResult ReadItemsToday(int listPage = 1)
        {
            var currentUserID = this.userManager.GetUserId(User);
            var itemsDueToday = this.service.ReadTodaysItemsByUserId(currentUserID, listPage, pageSize);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView(itemsDueToday);
            }

            return View(itemsDueToday);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Complete(Guid id)
        {
            this.service.CompleteToDoEntry(id);


            return RedirectToAction("ReadItemsToday", "ToDoEntry");
        }

        [Authorize]
        [HttpPost]
        public JsonResult Reminder()
        {
            var currentUserID = this.userManager.GetUserId(User);

            ToDoEntryReminderDto theClosestToDoEntry = this.service.GetToDoEntryForReminder(currentUserID);

            return new JsonResult(Ok(theClosestToDoEntry));
        }
    }
}
