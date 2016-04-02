using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Newtonsoft.Json;

namespace myBotty
{
   
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                return await Conversation.SendAsync(message, () => new EchoDialog());
                // calculate something for us to return
                int length = (message.Text ?? string.Empty).Length;

                // return our reply to the user
                return message.CreateReplyMessage($"You sent {length} characters");
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == "BotAddedToConversation")
            {
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
            }
            else if (message.Type == "UserAddedToConversation")
            {
            }
            else if (message.Type == "UserRemovedFromConversation")
            {
            }
            else if (message.Type == "EndOfConversation")
            {
            }

            return null;
        }

        [Serializable]
        public class EchoDialog : IDialog
        {
            private int count = 1;

            public async Task StartAsync(IDialogContext context)
            {
                context.Wait(MessageReceivedAsync);
            }

            public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<Message> argument)
            {
                var message = await argument;
                if (message.Text == "reset")
                {
                    PromptDialog.Confirm(
                        context,
                        AfterResetAsync,
                        "Are you sure you want to reset that count monsieur?",
                        "Didn't get that YA FOOK");
                }
                else
                {
                    await context.PostAsync(string.Format($"You said {this.count++}, {message.Text}"));
                    context.Wait(MessageReceivedAsync);
                }
            }

            public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
                {
                    var confirm = await argument;
                    if (confirm)
                    {
                        this.count = 1;
                        await context.PostAsync("Reset Count");
                    }
                    else
                    {
                        await context.PostAsync("Did not reset count.");
                    }
                        context.Wait(MessageReceivedAsync);
                }
            }
        }
    }
    
