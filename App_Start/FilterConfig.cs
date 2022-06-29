using System.Web;
using System.Web.Mvc;

namespace HTTP_5212_RNA_Group4_HospitalProject
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
