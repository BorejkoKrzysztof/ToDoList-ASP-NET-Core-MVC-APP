using Microsoft.AspNetCore.Mvc;
using ToDoListInfrastructure.Models.ViewModels.ToDoEntry;

namespace ToDoList.ViewComponents
{
    public class ToDoEntryCardsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ToDoEntryCollectionViewModel model, string actionName)
        {
            ViewBag.ActionName = actionName;
            return View();
        }
    }
}
