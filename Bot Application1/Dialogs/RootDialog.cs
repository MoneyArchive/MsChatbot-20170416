using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            var openJson = File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/YouBikeTP.json"));
            List<Models.UbikeStation> stations = JsonConvert.DeserializeObject<List<Models.UbikeStation>>(openJson);

            var datas = stations.Where(x => x.sna.Contains(activity.Text)).ToList();
            if (!datas.Any())
            {
                await context.PostAsync($"搜尋不到相關ubike站點資訊");
            }
            else
            {
                var reply = context.MakeMessage();
                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                reply.Attachments = new List<Attachment>();

                foreach (Models.UbikeStation data in datas)
                {

                    var hero = new HeroCard();
                    hero.Title = data.sna;
                    hero.Subtitle = data.ar;
                    hero.Images = new List<CardImage>();
                    hero.Images.Add(new CardImage()
                    {
                        Url =
                            $"http://maps.google.com/maps/api/staticmap?center={data.lat},{data.lng}&zoom=16&size=400x400&sensor=false&format=png32&maptype=roadmap&markers={data.lat},{data.lng}"
                    });
                    hero.Buttons = new List<CardAction>();
                    hero.Buttons.Add(new CardAction()
                    {
                        Title = "用地圖開啟",
                        Type = ActionTypes.OpenUrl,
                        Value = $"http://maps.google.com/maps?z=12&t=m&q=loc:{data.lat}+{data.lng}"
                    });

                    reply.Attachments.Add(hero.ToAttachment());
                }
                await context.PostAsync(reply);
            }

            context.Wait(MessageReceivedAsync);
        }
    }
}