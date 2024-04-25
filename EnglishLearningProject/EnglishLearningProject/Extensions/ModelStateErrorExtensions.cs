using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EnglishLearningProject.Extensions
{
    public static class ModelStateErrorExtensions
    {

        public static void AddModelErrorList(this ModelStateDictionary modelstate, List<string> errors)
        {
            errors.ForEach(error =>
            {
                modelstate.AddModelError(string.Empty, error);
            });
        }

        public static void AddModelErrorList(this ModelStateDictionary modelstate, IEnumerable<IdentityError> errors)
        {
            errors.ToList().ForEach(error =>
            {
                modelstate.AddModelError(string.Empty, error.Description);
            });
        }   
    }
}
