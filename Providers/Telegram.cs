using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TelegrafistVK.Configuration;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace TelegrafistVK.Providers
{
    public class Telegram
    {
        private TelegramBotClient _telegramBotClient;
        private static Dictionary<Chat, long> _chats = new Dictionary<Chat, long>();
        private static Dictionary<Chat, string> _verificationCodes = new Dictionary<Chat, string>();

        public Telegram()
        {
            _telegramBotClient = new TelegramBotClient(ConfigManager.Config.Telegram.Token);
            _telegramBotClient.OnMessage += OnTelegramMessage;
            _telegramBotClient.StartReceiving();
        }

        private async void OnTelegramMessage(object sender, MessageEventArgs e)
        {
            Chat chat = e.Message.Chat;
            string text = e.Message.Text;

            //User telegramUser = _users.TryGetValue(e.Message.From, out ulong peerId) ? e.Message.From : null;

            if (text.Equals("/setup"))
            {
                await _telegramBotClient.SendTextMessageAsync(chat,
                    "Hello, all what you need is send into chat verification code: " + CreateVerificationCode(chat));
            }
        }

        private string CreateVerificationCode(Chat chat)
        {
            _verificationCodes.Add(chat, new Random((int)chat.Id).Next(100000, 1000000).ToString());
            return _verificationCodes[chat];
        }

        public async void OnMessageRecieved(string message, long? peerId)
        {
            Console.WriteLine($"Message: {message}");

            Regex regex = new Regex(@"\d{6}");
            if (regex.IsMatch(message))
            {
                foreach (var code in _verificationCodes)
                {
                    if (code.Value == message)
                    {
                        Chat chat = code.Key;
                        _verificationCodes.Remove(chat);
                        if (peerId!=null)
                            _chats.Add(chat, (long)peerId);
                    }
                }
            }

            foreach (var pair in _chats.Where(pair => pair.Value == (peerId != null ? (long)peerId : 0)))
            {
                await _telegramBotClient.SendTextMessageAsync(pair.Key, message);
            }
        }
    }
}
