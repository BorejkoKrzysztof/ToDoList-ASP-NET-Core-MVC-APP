using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoListInfrastructure.Models.Services;
using ToDoListInfrastructure.Models.ViewModels.ToDoList;

namespace ToDoList.Controllers
{
    //[Route("todolist")]
    public class ToDoListController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IToDoListService service;

        private readonly int pageSize = 5;

        public ToDoListController(UserManager<IdentityUser> userManager, IToDoListService service)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager), "Given userManager is null.");
            this.service = service ?? throw new ArgumentNullException(nameof(service), "Given service is null.");
        }
        // GET: ToDoListController

        [Authorize]
        public IActionResult Index(int listPage = 1)
        {
            var currentUserID = this.userManager.GetUserId(User);
            var toDoListsCollection = this.service.ReadAllLists(currentUserID, listPage, pageSize);
            ViewBag.ActionNameTagLink = "Index";

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView(toDoListsCollection);
            }

            return View(toDoListsCollection);
        }

        [Authorize]
        public IActionResult HiddenList(int listPage = 1)
        {
            var currentUserID = this.userManager.GetUserId(User);
            var toDoListsHiddenModel = this.service.ReadAllHiddenLists(currentUserID, listPage, pageSize);
            ViewBag.ActionNameTagLink = "HiddenList";

            return View("Index", toDoListsHiddenModel);
        }


        // GET: ToDoListController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToDoListController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateToDoListViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUserID = this.userManager.GetUserId(User);
            this.service.CreateToDoList(model, currentUserID);

            return RedirectToAction("Index", "ToDoList");
        }

        // GET: ToDoListController/Edit/5
        public IActionResult Edit(Guid id)
        {
            ViewBag.SelectedIdToEdit = id;
            return View();
        }

        // POST: ToDoListController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UpdateToDoListViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.service.UpdateToDoList(model);

            return RedirectToAction("Index", "ToDoList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("SetHide")]
        public IActionResult SwitchHide(HideToDoListViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "ToDoList");
            }
            this.service.SwitchHideToDoList(model.Id);

            return RedirectToAction("Index", "ToDoList");
        }

        // GET: ToDoListController/Delete/5
        public IActionResult Delete(Guid id)
        {
            ViewBag.SelectedIdToDelete = id;
            return View();
        }

        // POST: ToDoListController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(DeleteToDoListViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.service.DeleteToDoList(model);

            return RedirectToAction("Index", "ToDoList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Copy(CopyToDoListViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.service.CopyToDoList(model.Id);

            return RedirectToAction("Index", "ToDoList");
        }
    }
}
