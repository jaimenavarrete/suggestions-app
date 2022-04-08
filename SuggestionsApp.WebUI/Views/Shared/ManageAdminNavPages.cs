using Microsoft.AspNetCore.Mvc.Rendering;

namespace SuggestionsApp.WebUI.Views.Shared
{
    public static class ManageAdminNavPages
    {
        public static string SuggestionsApproval => "SuggestionsApproval";

        public static string Index => "Index";

        public static string CategoriesList => "CategoriesList";

        public static string StatesList => "StatesList";

        public static string UsersList => "UsersList";


        public static string SuggestionsApprovalNavClass(ViewContext viewContext) => PageNavClass(viewContext, SuggestionsApproval);

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string CategoriesListNavClass(ViewContext viewContext) => PageNavClass(viewContext, CategoriesList);

        public static string StatesListNavClass(ViewContext viewContext) => PageNavClass(viewContext, StatesList);

        public static string UsersListNavClass(ViewContext viewContext) => PageNavClass(viewContext, UsersList);


        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : "";
        }
    }
}
