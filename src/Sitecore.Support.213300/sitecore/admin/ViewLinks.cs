using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Sitecore;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.sitecore.admin;
using Sitecore.Web;

namespace Sitecore.Support.sitecore.admin
{
  public class ViewLinks : AdminPage
  {
    public class Links : AdminPage
    {
      protected PlaceHolder _referrers;

      protected PlaceHolder _references;

      protected void Page_Load(object sender, EventArgs e)
      {
        var itemID = Data.ID.Parse(Sitecore.Web.WebUtil.GetQueryString("itemID"));
        var databaseName = Sitecore.Web.WebUtil.GetQueryString("db");
        var database = Sitecore.Configuration.Factory.GetDatabase(databaseName);
        Assert.IsNotNull(database, "Database can't be null");

        this.RenderTable(this._referrers, Globals.LinkDatabase.GetItemReferrers(database.GetItem(itemID), true));
        this.RenderTable(this._references, Globals.LinkDatabase.GetItemReferences(database.GetItem(itemID), true));
      }

      protected void RenderTable(PlaceHolder placeholder, ItemLink[] links)
      {
        var htmlTable = HtmlUtil.CreateTable(0, 0);
        htmlTable.Border = 1;
        htmlTable.CellPadding = 4;
        HtmlUtil.AddRow(htmlTable, new string[]
        {
          "Name",
          "Path",
          "ID"
        });

        foreach (var link in links)
        {
          Database database = Configuration.Factory.GetDatabase(link.SourceDatabaseName, false);
          var item = database.GetItem(link.SourceItemID);
          string linkToItem =
            "<a href=\"#\" onclick='javascript:return scForm.invoke(\"item:load(id={0}, language={1}, version={2})\")'>{3}</a>";

          HtmlUtil.AddRow(htmlTable, new string[]
          {
            item.Name,
            String.Format(linkToItem, item.ID, item.Language, item.Version, item.Paths.Path),
            item.ID.ToString()
          });
        }

        placeholder.Controls.Add(htmlTable);
      }
    }
  }
}