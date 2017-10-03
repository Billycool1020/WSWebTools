using FollowUpTestClient.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowUpTestClient
{
    class AllThreads
    {
       public static List<FollowUpThread> GetList()
        {
            List<FollowUpThread> list = new List<FollowUpThread>();


            try
            {
                list = GetDBThreads.GetCosmosDBList();
            }
            catch
            {
                FollowContext db = new FollowContext();
              //  var Products = db.Products.Where(x => x.URL != null).ToList();
                //foreach (var p in Products)
                //{
                //    list.AddRange(GetWEBThreads.GetMSDNThreads(p.URL, p.ProductName));
                //}
            }




            return list;
        }
    }
}
