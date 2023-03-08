﻿using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text;
using WomanSite.Database;
using WomanSite.Models;
namespace WomanSite.Controllers
{
    public class DialogueController
    {
        public string GetMessage(int id, string womanName)
        {
            string s = "";
            string currName = "";
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var item in db.Questions)
                {
                    if (item.id==id)
                    {
                        s= item.text + item.guess;
                        currName = item.personName;
                    }
                }
            }
            if(womanName==currName)
            {
                return s;
            }
            else
            {
                return "end";
            }
        }
        public bool addAnswer(Answer ans)
        {
            using (ApplicationContext db = new ApplicationContext())
            { 
                db.Answers.Add(ans);
                return true;
            }
        }
    }
}
