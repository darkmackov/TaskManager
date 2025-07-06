using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TaskManager.Entities.Enums;

namespace TaskManager.Controllers
{
    public abstract class ExtendedController : Controller
    {
        /// <summary>
        /// Helper method to add validation results to the ModelState.
        /// </summary>
        protected void AddValidationResults(IEnumerable<ValidationResult> results)
        {
            foreach (var result in results)
            {
                foreach (var member in result.MemberNames)
                {
                    ModelState.AddModelError(member, result.ErrorMessage!);
                }
            }
        }

        /// <summary>
        /// Sets a temporary message and its type to be displayed to the user, 
        /// </summary>
        protected void SetMessage(string message, MessageType type = MessageType.Success)
        {
            TempData["Message"] = message;
            TempData["MessageType"] = type.ToString().ToLower();
        }
    }
}
