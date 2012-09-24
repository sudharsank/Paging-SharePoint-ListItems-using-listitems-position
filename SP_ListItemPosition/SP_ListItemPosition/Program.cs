using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using SP = Microsoft.SharePoint.Client;
using System.Net;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace SP_ListItemPosition
{
    public class Program
    {
        public static string strSiteURL = "<SITE URL>";
        public static string strList = "<LIST NAME>";
        public static string itemsPerPage = "2";
        static void Main(string[] args)
        {
            #region Using Client Object Model 
            ClientContext clientContext = new ClientContext(strSiteURL);
            List lst = clientContext.Web.Lists.GetByTitle(strList);
            ListItemCollectionPosition itemPosition = null;
            Console.WriteLine("Displaying items in a batch manner...");
            Console.WriteLine("-------------------------------------");
            while (true)
            {
                CamlQuery camlQuery = new CamlQuery();
                camlQuery.ListItemCollectionPosition = itemPosition;
                camlQuery.ViewXml = "<View><ViewFields><FieldRef Name='Title'/>" +
                                    "</ViewFields><RowLimit>" + itemsPerPage + "</RowLimit></View>";
                ListItemCollection collListItem = lst.GetItems(camlQuery);
                clientContext.Load(collListItem);
                clientContext.ExecuteQuery();
                itemPosition = collListItem.ListItemCollectionPosition;
                foreach (ListItem oListItem in collListItem)
                {
                    Console.WriteLine("Title: {0}", oListItem["Title"]);
                }
                if (itemPosition == null)
                {
                    break;
                }
                Console.WriteLine("\n" + itemPosition.PagingInfo + "\n");
                Console.WriteLine("Next page...");
                Console.ReadLine();
            }
            Console.WriteLine("");
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
            #endregion

            #region Using Server Object Model 
            using (SPSite SiteCollection = new SPSite(strSiteURL))
            {
                using (SPWeb Site = SiteCollection.OpenWeb())
                {
                    SPList TargetList = Site.Lists.TryGetList(strList);
                    SPListItemCollectionPosition splistitemPosition = null;
                    Console.WriteLine("Displaying items in a batch manner...");
                    Console.WriteLine("-------------------------------------");
                    while (true)
                    {
                        SPQuery query = new SPQuery();
                        query.ListItemCollectionPosition = splistitemPosition;
                        query.ViewXml = "<View><ViewFields><FieldRef Name='Title'/>" +
                                            "</ViewFields><RowLimit>" + itemsPerPage + "</RowLimit></View>";
                        SPListItemCollection apcollListItem = TargetList.GetItems(query);
                        splistitemPosition = apcollListItem.ListItemCollectionPosition;
                        foreach (SPListItem oListItem in apcollListItem)
                        {
                            Console.WriteLine("Title: {0}", oListItem["Title"]);
                        }
                        if (splistitemPosition == null)
                        {
                            break;
                        }
                        Console.WriteLine("\n" + splistitemPosition.PagingInfo + "\n");
                        Console.WriteLine("Next page...");
                        Console.ReadLine();
                    }                    
                }
            }
            Console.WriteLine("");
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
            #endregion
        }
    }
}
