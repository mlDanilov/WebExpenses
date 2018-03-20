using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;


namespace WebExpenses
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles_)
        {
            bundles_.Add(new StyleBundle("~/Content/itemCard").Include(
                "~/Content/itemCard.css"));
            bundles_.Add(new StyleBundle("~/Content/groupCard").Include(
               "~/Content/groupCard.css"));
            bundles_.Add(new StyleBundle("~/Content/shopCard").Include(
              "~/Content/shopCard.css"));
            bundles_.Add(new StyleBundle("~/Content/purchaseCard").Include(
              "~/Content/purchaseCard.css"));

            bundles_.Add(new ScriptBundle("~/bundles/jQuery").Include(
                "~/Scripts/jquery-1.8.0.min.js"));

            bundles_.Add(new ScriptBundle("~/bundles/validate").Include(
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js"));

            bundles_.Add(new ScriptBundle("~/bundles/unobtrusiveAjax").Include(
            "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles_.Add(new ScriptBundle("~/bundles/purchCard").Include(
                "~/Scripts/purchaseCard.js"));



            
        }
    }
}