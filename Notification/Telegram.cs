
using CMS.Management.Structure.Model;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace CMS.Notification
{
    public partial class ITelegram
    {
        private static ITelegramBotClient botClient;

        public ITelegram()
        {
            botClient = new TelegramBotClient(M_Config.GetTelegramAPI());
        }

        public static async Task Send(int Telegram_ID, string Message)
        {
            botClient = new TelegramBotClient(M_Config.GetTelegramAPI());

            Message message = await botClient.SendTextMessageAsync(
              chatId: Telegram_ID,
              text: Message
            );
        }

        public static async Task Send(int Telegram_ID, string Text_Message, string TelegramAPI)
        {
            botClient = new TelegramBotClient(TelegramAPI);

            string replacement = "<br/>";
            Text_Message = Text_Message.Replace("<br />", replacement).Replace("<br>", replacement).Replace("\n", replacement);
            var raw_message = Text_Message.Split(new string[] { "<br/>" }, StringSplitOptions.None);
            string formatted_message = "";
            for (int x = 0; x < raw_message.Length; x++)
            {
                formatted_message += raw_message[x] + "\n";
            }

            Message message = await botClient.SendTextMessageAsync(
              chatId: Telegram_ID,
              text: formatted_message,
              parseMode: Telegram.Bot.Types.Enums.ParseMode.Html
            );
        }
    }
}