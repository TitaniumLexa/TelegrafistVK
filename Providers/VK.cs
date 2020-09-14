using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TelegrafistVK.Configuration;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace TelegrafistVK.Providers
{
    public class VK
    {
        private readonly VkApi _vkApi;

        public delegate void MessageHandler(string message, long? peerId);

        public event MessageHandler MessageRecieved;


        public VK()
        {
            _vkApi = new VkApi();
            _vkApi.Authorize(
                new ApiAuthParams()
                {
                    AccessToken = ConfigManager.Config.VK.Token
                }
            );
        }

        public async Task AsyncStart()
        {
            await Task.Run(() => Start());
        }

        private void Start()
        {
            while (true)
            {
                var longPollServerResponse = _vkApi.Groups.GetLongPollServer(ConfigManager.Config.VK.GroupId);
                var poll = _vkApi.Groups.GetBotsLongPollHistory(
                    new BotsLongPollHistoryParams()
                    {
                        Server = longPollServerResponse.Server, Ts = longPollServerResponse.Ts,
                        Key = longPollServerResponse.Key, Wait = 25
                    });

                if (poll?.Updates == null)
                    continue;

                foreach (var update in poll.Updates)
                {
                    if (update.Type == GroupUpdateType.MessageNew)
                    {
                        Console.WriteLine(update.MessageNew.Message.Text);
                        MessageRecieved?.Invoke(update.MessageNew.Message.Text, update.MessageNew.Message.PeerId);
                    }
                }
            }
        }
    }
}