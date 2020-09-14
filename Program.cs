using System;
using System.Threading.Tasks;
using VkNet;
using Telegram.Bot;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using TelegrafistVK.Configuration;
using TelegrafistVK.Providers;


namespace TelegrafistVK
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ConfigManager.Initialize();
            
            var vk = new Providers.VK();
            var telegram = new Providers.Telegram();

            vk.MessageRecieved += telegram.OnMessageRecieved;

            await vk.AsyncStart();

            await Task.Delay(-1);
        }
    }
}
