using System;
using System.Web;
using System.Web.UI;

namespace GolfClubMLD.Models.Classes
{
    public class ProductBrowser : Page
    {
        protected override void OnInit(EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.MinValue);

            base.OnInit(e);
        }
    }
}