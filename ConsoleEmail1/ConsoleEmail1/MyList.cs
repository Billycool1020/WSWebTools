using ConsoleEmail1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEmail1
{
    class MyList
    {

        public static List<MyThread> thread1List = new List<MyThread>()
            {
            new MyThread{ThreadId="aa1",ThreadName=".NET MVC",cat_URL="https:\\www.baidu1.com",cat_msalias="v-lezha1",LastOP=DateTime.Now},
            new MyThread{ThreadId="aa2",ThreadName="Web API",cat_URL="https:\\www.baidu2.com",cat_msalias="v-lezha1",LastOP=DateTime.Now},
            new MyThread{ThreadId="bb1",ThreadName=".NET MVC",cat_URL="https:\\www.baidu3.com",cat_msalias="v-yayu1",LastOP=DateTime.Now}
            };
        public static List<MyThread> thread2List = new List<MyThread>()
            {
                new MyThread{ThreadId="aa2",ThreadName="Web API",cat_URL="https:\\www.baidu2.com",cat_msalias="v-lezha1",LastOP=DateTime.Now},
                new MyThread{ThreadId="bb2",ThreadName=".NET MVC",cat_URL="https:\\www.baidu4.com",cat_msalias="v-yayu1",LastOP=DateTime.Now}
            };
        public  List<MyThread> GetThreadList()
        {
            foreach (var thread2 in thread2List)
            {
                if (thread1List.Contains(thread2))
                {
                    thread1List.Remove(thread2); //remove the same thread
                }
            }
            return thread1List; 
        }
        public void ClearDatabase()
        {
            connStr db = new connStr();
            List<MyThread> dataList = db.MyThreads.ToList(); //get model from database
            foreach(var data in dataList)
            {
                dataList.Remove(data);
            }
            db.SaveChanges();
        }

     }
}
