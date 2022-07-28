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

        /// <summary>
        /// Index page.
        /// </summary>
        /// <param name="Id">ToDoListId.</param>
        /// <param name="listPage">Number of page.</param>
        /// <param name="hideCompleted">True, if you don't want to see hidden toDoEntries</param>
        /// <returns></returns>
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

        /// <summary>
        /// Details of toDoEntry
        /// </summary>
        /// <param name="Id">ToDoEntry Id</param>
        /// <param name="tdlId">ToDoList Id</param>
        /// <returns></returns>
        public IActionResult Details(Guid Id, Guid tdlId)
        {
            var toDoEntryDetails = this.service.ReadToDoEntry(Id);
            ViewBag.ToDoListId = tdlId;

            return View(toDoEntryDetails);
        }


        public IActionResult Create(Guid toDoListId)
        {
            ViewBag.ToDoListId = toDoListId;
            return View();
        }


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

        /// <summary>
        /// Edit ToDoEntry
        /// </summary>
        /// <param name="id">ToDoEntry Id</param>
        /// <param name="tdlId">ToDoList Id</param>
        /// <returns></returns>
        public IActionResult Edit(Guid id, Guid tdlId)
        {
            ViewBag.ToDoEntryId = id;
            ViewBag.ToDoListId = tdlId;
            return View();
        }


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

        /// <summary>
        /// Delete ToDoEntry
        /// </summary>
        /// <param name="id">ToDoEntry Id</param>
        /// <returns></returns>
        public IActionResult Delete(Guid id)
        {
            return View();
        }


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


        /// <summary>
        /// Set Progres of todoentry to completed.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IActionResult Complete(Guid id)
        {
            this.service.CompleteToDoEntry(id);


            return RedirectToAction("ReadItemsToday", "ToDoEntry");
        }

        /// <summary>
        /// Reminder Action.
        /// </summary>
        /// <returns>Title and DueDate of ToDoEntry as a ToDoEntryReminderDto.</returns>
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
