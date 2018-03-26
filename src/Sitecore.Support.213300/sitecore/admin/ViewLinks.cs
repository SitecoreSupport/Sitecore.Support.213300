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

      protected Label ItemIdLabel { get; private set; }
      
      protected RadioButtonList _radioButtons;

      protected void Page_Load(object sender, EventArgs e)
      {
        this._radioButtons.SelectedIndex = -1;

        var itemID = Sitecore.Web.WebUtil.GetQueryString("itemid");
        var databaseName = Sitecore.Web.WebUtil.GetQueryString("db");
        
        if (!String.IsNullOrEmpty(itemID) && !String.IsNullOrEmpty(databaseName))
        {
          this.ItemIdLabel.Text = itemID;
          this._radioButtons.Items.FindByText(databaseName).Selected = true;
        }
        else
        {
          itemID = this.ItemIdLabel.Text;
          databaseName = this._radioButtons.SelectedItem.Text;
        }

        var database = Configuration.Factory.GetDatabase(databaseName);
        var item = database.GetItem(Data.ID.Parse(itemID));
        this.RenderTable(this._referrers, Globals.LinkDatabase.GetItemReferrers(item, true));
        this.RenderTable(this._references, Globals.LinkDatabase.GetItemReferences(item, true));
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