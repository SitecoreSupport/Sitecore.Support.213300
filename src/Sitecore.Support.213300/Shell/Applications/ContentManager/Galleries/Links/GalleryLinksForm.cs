using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using Sitecore.Collections;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Shell;
using System.Web.UI;
using Sitecore.Diagnostics;

namespace Sitecore.Support.Shell.Applications.ContentManager.Galleries.Links
{
  public class GalleryLinksForm : Sitecore.Shell.Applications.ContentManager.Galleries.Links.GalleryLinksForm
  {

    private bool _showLink = false;

    /// <summary>
    /// Maximum number of referrers or references that can be displayed in Content Editor Navigation->Links menu.
    /// The value is defined in the MaxNavigationLinks setting.
    /// </summary>
    private static int MaxNavigationLinks
    {
      get
      {
        int maxNavigationLinks;
        Int32.TryParse(Settings.GetSetting("MaxNavigationLinks", "0"), out maxNavigationLinks);

        return maxNavigationLinks;
      }
    }

    protected override void OnLoad(EventArgs e)
    {
      this._showLink = false;

      Assert.ArgumentNotNull(e, "e");
      base.OnLoad(e);
      var item = UIUtil.GetItemFromQueryString(Context.ContentDatabase);
      if (this._showLink)
      {
        this.Links.Controls.Add(
          new LiteralControl(
            $"<a href=\"sitecore\\admin\\ViewLinks.aspx?itemid={item.ID}&db={item.Database.Name}\"></a>"));
      }
    }

    protected override void ProcessReferrers(Item item, StringBuilder result)
    {
      ItemLink[] referrers = this.GetRefererers(item);
      List<Pair<Item, ItemLink>> list = new List<Pair<Item, ItemLink>>();
      for (int i = 0; i < referrers.Length; i++)
      {
        ItemLink itemLink = referrers[i];
        Database database = Factory.GetDatabase(itemLink.SourceDatabaseName, false);
        if (database != null)
        {
          Item item2 = database.GetItem(itemLink.SourceItemID);
          if (item2 == null || !this.IsHiddenItem(item2) || UserOptions.View.ShowHiddenItems)
          {
            list.Add(new Pair<Item, ItemLink>(item2, itemLink));
            if (MaxNavigationLinks != 0 && list.Count >= MaxNavigationLinks)
            {
              this._showLink = true;
              break;
            }
          }
        }
      }

      typeof(Sitecore.Shell.Applications.ContentManager.Galleries.Links.GalleryLinksForm)
        .GetMethod("RenderReferrers", BindingFlags.NonPublic | BindingFlags.Instance)
        .Invoke(this, new object[] { result, list });
    }

    protected override void ProcessReferences(Item item, StringBuilder result)
    {
      ItemLink[] references = this.GetReferences(item);
      List<Pair<Item, ItemLink>> list = new List<Pair<Item, ItemLink>>();
      for (int i = 0; i < references.Length; i++)
      {
        ItemLink itemLink = references[i];
        Database database = Factory.GetDatabase(itemLink.TargetDatabaseName, false);
        if (database != null)
        {
          Item item2 = database.GetItem(itemLink.TargetItemID);
          if (item2 == null || !this.IsHiddenItem(item2) || UserOptions.View.ShowHiddenItems)
          {
            list.Add(new Pair<Item, ItemLink>(item2, itemLink));
            if (MaxNavigationLinks != 0 && list.Count >= MaxNavigationLinks)
            {
              this._showLink = true;
              break;
            }
          }
        }
      }

      typeof(Sitecore.Shell.Applications.ContentManager.Galleries.Links.GalleryLinksForm)
        .GetMethod("RenderReferences", BindingFlags.NonPublic | BindingFlags.Instance)
        .Invoke(this, new object[] { result, list });
    }

    private bool IsHiddenItem(Item item)
    {
      return (bool)typeof(Sitecore.Shell.Applications.ContentManager.Galleries.Links.GalleryLinksForm)
        .GetMethod("IsHidden", BindingFlags.NonPublic | BindingFlags.Instance)
        .Invoke(this, new object[] { item });
    }
  }
}