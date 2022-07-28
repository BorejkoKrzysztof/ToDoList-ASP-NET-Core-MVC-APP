using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoListInfrastructure.Models.Services;
using ToDoListInfrastructure.Models.ViewModels.ToDoList;

namespace ToDoList.Controllers
{
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
        
        /// <summary>
        /// Index Page with table of todolists.
        /// </summary>
        /// <param name="listPage">Number of page.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Index Page with table of todolists with hidden items.
        /// </summary>
        /// <param name="listPage">Number of page.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Edit ToDoList.
        /// </summary>
        /// <param name="id">ToDoList Id</param>
        /// <returns></returns>
        public IActionResult Edit(Guid id)
        {
            ViewBag.SelectedIdToEdit = id;
            return View();
        }

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

        /// <summary>
        /// Delete ToDoList.
        /// </summary>
        /// <param name="id">ToDoList id</param>
        /// <returns></returns>
        public IActionResult Delete(Guid id)
        {
            ViewBag.SelectedIdToDelete = id;
            return View();
        }


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

        /// <summary>
        /// Copy ToDoList Action.
        /// </summary>
        /// <param name="model">Model with ToDoList Id.</param>
        /// <returns></returns>
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
