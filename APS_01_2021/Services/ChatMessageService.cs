using App.Services.Exceptions;
using APS_01_2021.Data;
using APS_01_2021.Models;
using APS_01_2021.Models.ViewModel;
using APS_01_2021.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APS_01_2021.Services
{
    public class ChatMessageService
    {
        private readonly MyDbContext _context;
        private readonly UserService _userServices;
        private readonly ContactService _contactService;

        public ChatMessageService(MyDbContext context, UserService userServices,ContactService contactService)
        {
            _context = context;
            _userServices = userServices;
            _contactService = contactService;
        }

        public async Task<string> InsertAsync(ChatMessageModel chatMessage)
        {
            if(chatMessage == null || chatMessage.userId == 0 || chatMessage.receiverId == 0)
            {
                return "Error";
            }
            else
            {
                try
                {
                    _context.ChatMessage.Add(chatMessage);
                    await _context.SaveChangesAsync();

                    if(chatMessage.type == "contact")
                    {
                        await _contactService.ContatcNotReadMessageAsync(chatMessage.userId, chatMessage.receiverId);
                    }
                } catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }
            return "OK";
        }

        public async Task UserReadTheMessageAsync(string type, int userId, string receiver)
        {
            if (type == "contact")
            {
                var receiverid = await _userServices.FindIdByNickName(receiver);
                await _contactService.UserReadMessageAsync(userId, receiverid);
            }
            else if (type == "meet")
            {
                //receiverid = await _meetService.FindIdByKeyName(receiver);
            }
        }

        public async Task<List<MessageForChatViewModel>> GetAllMessageContact(int userId, int receiverId,
            string userNickName, string receiveNickName)
        {
            if(userId == 0 || receiverId == 0)
            {
                throw new IntegrityException("Some of the values is null or 0");
            }
            else
            {
                var list =  await _context.ChatMessage
                    .Where(x => x.userId == userId && x.receiverId == receiverId
                        || x.userId == receiverId && x.receiverId == userId)

                    .ToListAsync();

                list.ForEach(x => {
                    if (x.userId == userId)
                    {
                        x.WhoSendMessage = userNickName;
                    }
                    else
                    {
                        x.WhoSendMessage = receiveNickName;
                    }
                });

                await _contactService.UserReadMessageAsync(userId, receiverId);

                return ConvertChatMessageForMessageViewModel(list);
            }
            throw new IntegrityException("DbError");
        }


        //------------------------------------------------------------------------------------------------
        public MessageForChatViewModel ConvertChatMessageForMessageViewModel(ChatMessageModel chatMessage)
        {
            return new MessageForChatViewModel
            {
                Message = chatMessage.message,
                WhoSendMessage = chatMessage.WhoSendMessage,
                Time = chatMessage.time.ToString()
            };
        }

        private List<MessageForChatViewModel> ConvertChatMessageForMessageViewModel(List<ChatMessageModel> chatMessage)
        {
            var finalList = new List<MessageForChatViewModel>();

            foreach (var elementChat in chatMessage)
            {
                finalList.Add(ConvertChatMessageForMessageViewModel(elementChat));
            }
            return finalList;
        }
    }
}
