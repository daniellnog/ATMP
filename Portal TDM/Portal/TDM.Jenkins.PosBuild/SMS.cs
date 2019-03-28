using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
//using Telegram.Bot.Types.Enums;
//using Telegram.Bot;

namespace TDM.Jenkins.PosBuild
{
    public class SMS
    {
        /// <summary>
        /// Realizar envio de mensagem via Telegram.
        /// </summary>
        /// <param name="msg">Corpo da mensagem</param>
        /// <param name="canal">Id do Canal</param>
        /// <param name="TokenBotTelegram">Token do Bot</param>
        public async static void Enviar(string msg, string canal, string TokenBotTelegram)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                var bot = new TelegramBotClient(TokenBotTelegram);

                var mensagem =  bot.SendTextMessageAsync(canal, msg, true, true, 0, null, ParseMode.Html);

                mensagem.Wait();
            }
        }
    }
}