using FollowUpTestClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowUpTestClient
{
    class CRUD
    {
        public static void ClearFollowData()
        {
            FollowContext db = new FollowContext();
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE [FollowUpThreads]");
        }
        public static async Task SaveFollowData(List<FollowUpThread> list)
        {            
            using (FollowContext db = new FollowContext())
            {
                db.FollowUpThreads.AddRange(list);
                int x = await (db.SaveChangesAsync());
            }
        }
    }
}
