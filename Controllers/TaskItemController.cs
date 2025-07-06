using Microsoft.AspNetCore.Mvc;
using TaskManager.Database;

namespace TaskManager.Controllers
{
    public class TaskItemController : Controller
    {
        private readonly DatabaseContext _context;

        public TaskItemController(DatabaseContext context) 
        {
            _context = context;
        }
    }
}
