using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Microsoft.ProjectOxford.Face;
using Newtonsoft.Json;

namespace Bot_Application1.Dialogs
{
    [LuisModel("751c1cf1-0bdc-440d-873b-aeb461e73924", "2a3c7a37e1d6468c99ea170ae4931913")]
    [Serializable]
    public class RootLuisDialog : LuisDialog<object>
    {
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string msg = string.Empty;
            Random random = new Random(DateTime.Now.Millisecond);
            var tmp = random.Next() % 4;
            switch (tmp)
            {
                case 0:
                    msg = "抱歉，我沒聽懂你說的話";
                    break;
                case 1:
                    msg = "實在聽不懂你在說甚麼阿...";
                    break;
                case 2:
                    msg = "抱歉，我不知道你想要告訴我什麼";
                    break;
                case 3:
                    msg = "我不太懂你想要表達的是什麼";
                    break;
            }

            await context.PostAsync(msg);
            context.Wait(MessageReceived);
        }

        [LuisIntent("問候")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            string msg = string.Empty;
            Random random = new Random(DateTime.Now.Millisecond);
            var tmp = random.Next() % 3;
            switch (tmp)
            {
                case 0:
                    msg = "Whats' up";
                    break;
                case 1:
                    msg = "很高興見到你";
                    break;
                case 2:
                    msg = "您好";
                    break;
            }

            await context.PostAsync(msg);
            context.Wait(MessageReceived);
        }

        [LuisIntent("前往")]
        public async Task Goto(IDialogContext context, LuisResult result)
        {
            var tmp = result.Entities.FirstOrDefault()?.Entity;
            await context.PostAsync("你想去：" + tmp);
            context.Wait(MessageReceived);
        }
    }
}