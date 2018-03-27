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
    protected PlaceHolder _referrers;

    protected PlaceHolder _references;

    protected TextBox _itemIdTextBox;

    protected Label _errorMessage;

    protected RadioButtonList _radioButtons;

    protected override void OnInit(EventArgs e)
    {
      CheckSecurity(true);
      base.OnInit(e);

      var itemID = WebUtil.GetQueryString("itemid");
      var databaseName = WebUtil.GetQueryString("db");
      if (!String.IsNullOrEmpty(itemID) && !String.IsNullOrEmpty(databaseName))
      {
        this._itemIdTextBox.Text = itemID;
        var radioButton = this._radioButtons.Items.FindByText(databaseName);
        if (radioButton == null)
        {
          this._errorMessage.Text = $"Database '{databaseName}' doesn't exist";
          return;
        }

        radioButton.Selected = true;
      }

      this.DisplayLinks(itemID, databaseName);
    }

    protected void DisplayLinks(string itemID, string databaseName)
    {
      this._referrers.Controls.Clear();
      this._references.Controls.Clear();
      this._errorMessage.Text = String.Empty;
      this._errorMessage.Attributes["style"] = "color:red; font-weight:bold;";
      
      if (String.IsNullOrEmpty(itemID))
      {
        this._errorMessage.Text = $"Item ID was not specified.";
        return;
      }

      if (String.IsNullOrEmpty(databaseName))
      {
        this._errorMessage.Text = $"Database was not specified.";
        return;
      }

      var database = Configuration.Factory.GetDatabase(databaseName);
      if (database == null)
      {
        this._errorMessage.Text = $"Database '{databaseName}' doesn't exist";
        return;
      }

      ID id;
      if (!Data.ID.TryParse(itemID, out id))
      {
        this._errorMessage.Text = $"Specified id {itemID} is not a valid GUID";
        return;
      }
      

      var item = database.GetItem(id);
      if (item == null)
      {
        this._errorMessage.Text = $"Item with ID '{itemID}' doesn't exist in database '{databaseName}'";
        return;
      }
      
      this.RenderLinks(this._referrers, Globals.LinkDatabase.GetItemReferrers(item, true), (link) => link.SourceItemID, (link) => link.SourceDatabaseName);
      this.RenderLinks(this._references, item.Links.GetAllLinks(true, true), (link) => link.TargetItemID, (link) => link.TargetDatabaseName);
    }
    
    protected void RenderLinks(PlaceHolder placeholder, ItemLink[] links, Func<ItemLink, ID> getID,
      Func<ItemLink, string> getDatabase)
    {
      var htmlTable = HtmlUtil.CreateTable(0, 0);
      htmlTable.Border = 1;
      htmlTable.CellPadding = 4;
      HtmlUtil.AddRow(htmlTable, new string[]
      {
        "ID",
        "Name",
        "Path",
        "Language",
        "Version"
      });

      foreach (var link in links)
      {
        Database database = Configuration.Factory.GetDatabase(getDatabase(link), false);
        var item = database.GetItem(getID(link));
        if (item != null)
        {
          HtmlUtil.AddRow(htmlTable, new string[]
          {
            item.ID.ToString(),
            item.Name,
            item.Paths.Path,
            link.SourceItemLanguage.Name,
            link.SourceItemVersion.Number.ToString()
            //String.Format(linkToItem, item.ID, item.Language, item.Version, item.Paths.Path),
          });
        }
      }

      placeholder.Controls.Add(htmlTable);
    }

    protected void GetLinksButton_Click(object sender, EventArgs e)
    {
      if (Request.QueryString.Count != 0)
      {
        Response.Redirect(WebUtil.RemoveQueryString(Request.Url.LocalPath));
      }

      var itemID = this._itemIdTextBox.Text;
      var databaseName = this._radioButtons.SelectedItem?.Text;
      this.DisplayLinks(itemID, databaseName);
    }
  }
}