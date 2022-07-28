using Microsoft.AspNetCore.Mvc;
using ToDoListInfrastructure.Models.ViewModels.ToDoEntry;

namespace ToDoList.ViewComponents
{
    // ViewComponent with cards of toDoEntries for ToDoEntries/Index page.
    public class ToDoEntryCardsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ToDoEntryCollectionViewModel model, string actionName)
        {
            ViewBag.ActionName = actionName;
            return View();
        }
    }
}
