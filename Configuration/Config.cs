using System;
using System.Collections.Generic;
using System.Text;

namespace TelegrafistVK.Configuration
{
    public class Config
    {
        public Vk VK { get; }
        public Telegram Telegram { get; }

        public Config()
        {
            VK = new Vk { Token = "ChangeIt", GroupId = 0 };
            Telegram = new Telegram {Token = "ChangeIt"};
        }
    }

    public class Vk
    {
        public string Token { get; set; }
        public ulong GroupId { get; set; }
    }
    public class Telegram
    {
        public string Token { get; set; }
    }
}
