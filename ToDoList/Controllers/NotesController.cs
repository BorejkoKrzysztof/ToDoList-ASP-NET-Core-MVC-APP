using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoListInfrastructure.Models.Services;
using ToDoListInfrastructure.Models.ViewModels.Notes;

namespace ToDoList.Controllers
{
    public class NotesController : Controller
    {
        private readonly INotesService service;
        private readonly int pageSize = 3;

        public NotesController(INotesService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service), "Given service is null.");
        }

        
        public ActionResult Index(Guid toDoEntryId, Guid toDoListId, int listPage = 1)
        {
            ViewBag.ToDoListId = toDoListId;
            var model = this.service.GetNotesByToDoEntryId(toDoEntryId, listPage, pageSize);

            return View(model);
        }

        
        public IActionResult Create(Guid tdeId, Guid tdlId)
        {
            ViewBag.ToDoEntryId = tdeId;
            ViewBag.ToDoListId = tdlId;

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateNotesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.service.CreateNote(model);

            return RedirectToAction("Index", "Notes", new { toDoEntryId = model.ToDoEntryId, toDoListId = model.ToDoListId });
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(DeleteNotesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.service.DeleteNote(model);

            return RedirectToAction("Index", "Notes", new { toDoEntryId = model.ToDoEntryId, toDoListId = model.ToDoListId });
        }
    }
}
