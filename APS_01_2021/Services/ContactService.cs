using APS_01_2021.Data;
using APS_01_2021.Models;
using APS_01_2021.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using App.Services.Exceptions;

namespace APS_01_2021.Services
{
    public class ContactService
    {
        private readonly MyDbContext _context;
        private readonly UserService _userServices;

        public ContactService(MyDbContext context, UserService userServices)
        {
            _context = context;
            _userServices = userServices;
        }

        /*create*/
        public async Task<string> InsertAsync(string user, string contact)
        {
            try
            {
                var userid = await _userServices.FindIdByNickName(user);
                var contactid = await _userServices.FindIdByNickName(contact);
                var contactobj = await _context.Contact
                    .Where(x => x.UserOneId == userid || x.UserOneId == contactid)
                    .Where(x => x.UserTwoId == userid || x.UserTwoId == contactid)
                    .FirstOrDefaultAsync();

                if(contactobj == null)
                {
                    ContactModel contactMod = new () 
                    { 
                        UserOneId = userid,
                        UserTwoId = contactid,
                    };

                    _context.Add(contactMod);
                    await _context.SaveChangesAsync();
                    return "OK";
                }
                else
                {
                    throw new IntegrityException("Contact Existis");
                }
            }
            catch (NullReferenceException ex)
            {
                throw new IntegrityException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }

        public async Task<String> FindStatusConncectionAsync(string user)
        {
            try
            {
                return await _userServices.FindStatusConnectionByNickNameAsync(user);
            }
            catch (Exception ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }
        

        public async Task<ContactModel> FindByNickNamesAsync(string user, string contact)
        {
            try
            {
                var userid = await _userServices.FindIdByNickName(user);
                var contactid = await _userServices.FindIdByNickName(contact);
                var contactobj = await _context.Contact
                    .Where(x => x.UserOneId == userid || x.UserOneId == contactid)
                    .Where(x => x.UserTwoId == userid || x.UserTwoId == contactid)
                    .FirstOrDefaultAsync();
                return contactobj;
            }
            catch (NullReferenceException ex)
            {
                throw new IntegrityException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }

        public async Task<List<ContactModel>> FindAllByNickNameAsync(string nickName)
        {
            try
            {
                var userid = await _userServices.FindIdByNickName(nickName);
                var list = await _context.Contact
                    .Where(x => x.UserOneId == userid || x.UserTwoId == userid)
                    .ToListAsync();

                foreach (var item in list)
                {
                    if (item.UserOneId == userid)
                    {
                        var user = await _userServices.FindNickNameAndStatusConnectionById(item.UserTwoId);
                        item.StatusConnection = user.StatusConnection;
                        item.ContactNickName = user.NickName;
                    }
                    else
                    {
                        var user = await _userServices.FindNickNameAndStatusConnectionById(item.UserOneId);
                        item.StatusConnection = user.StatusConnection;
                        item.ContactNickName = user.NickName;
                    }
                }
                return list;
            }
            catch (NullReferenceException ex)
            {
                throw new IntegrityException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }

        public async Task DeleteAsync(string user,string contact)
        {
            try
            {
                var contactobj = await FindByNickNamesAsync(user,contact);
                _context.Contact.Remove(contactobj);
                await _context.SaveChangesAsync();   
            }
            catch(DbUpdateException ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }
    }
}
