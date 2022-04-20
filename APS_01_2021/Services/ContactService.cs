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
                    ContactModel contactMod = new()
                    {
                        UserOneId = userid,
                        UserTwoId = contactid,
                        UserOnePlaceInTheList = 1,
                        UserTwoPlaceInTheList = 1,
                        IsUserOneReadMessage = true,
                        IsUserTwoReadMessage = true
                    };

                    await ReorderListContactAsync(user);

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
                        item.OrderInUserList = item.UserOnePlaceInTheList;
                    }
                    else
                    {
                        var user = await _userServices.FindNickNameAndStatusConnectionById(item.UserOneId);
                        item.StatusConnection = user.StatusConnection;
                        item.ContactNickName = user.NickName;
                        item.OrderInUserList = item.UserTwoPlaceInTheList;
                    }
                }
                return list.OrderBy(x => x.OrderInUserList).ToList();
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
        
        public async Task UpdateAsync(ContactModel contactobj)
        {
            try
            {
                _context.Contact.Update(contactobj);
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }

        //***********************ContatcNotReadMessageAsync***********************
        public async Task UserReadMessageAsync(int user, int contact)
        {
            var usernick = await _userServices.FindNickNameById(user);
            var contactnick = await _userServices.FindNickNameById(contact);
            await ContactReadMessage(true, usernick, contactnick);
        }

        public async Task ContatcNotReadMessageAsync(int user, int contact)
        {
            var usernick = await _userServices.FindNickNameById(user);
            var contactnick = await _userServices.FindNickNameById(contact);
            await ContactReadMessage(false, usernick, contactnick);
        }

        private async Task ContactReadMessage(bool isUser, string user, string contact)
        {
            try
            {
                if (user != null || contact != null)
                {
                    var userid = await _userServices.FindIdByNickName(user);
                    var contactobj = await FindByNickNamesAsync(user, contact);

                    //user read the message
                    if(isUser == true)
                    {
                        if (contactobj.UserOneId == userid)
                        {
                            contactobj.IsUserOneReadMessage = true;
                        }
                        else
                        {
                            contactobj.IsUserTwoReadMessage = true;
                        }
                    }
                    //contact receive the message but not read yet
                    else
                    {
                        if (contactobj.UserOneId == userid)
                        {
                            contactobj.IsUserTwoReadMessage = false;
                        }
                        else
                        {
                            contactobj.IsUserOneReadMessage = false;
                        }
                    }

                    await UpdateAsync(contactobj);

                    await ReorderListContactAsync(user, contact, "first");
                }
            }
            catch (Exception ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }

        //***********************DeleteAsync***********************
        public async Task DeleteAsync(string user,string contact)
        {
            try
            {
                var contactobj = await FindByNickNamesAsync(user,contact);
                _context.Contact.Remove(contactobj);
                await _context.SaveChangesAsync();

                await ReorderListContactAsync(user, contact, "delete");
            }
            catch(DbUpdateException ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }

        //***********************ReorderListContactAsync***********************
        public async Task ReorderListContactAsync(string user)
        {
            await ReorderListContactAsync(user,null, "reorder");
        }

        public async Task ReorderListContactAsync(string user, string contact, string action)
        {
            try
            {
                var userid = await _userServices.FindIdByNickName(user);
                var contactid = await _userServices.FindIdByNickName(contact);

                var userList = await FindAllByNickNameAsync(user);
                var contactList = await FindAllByNickNameAsync(contact);

                if(action == "add" && userid != 0 && contactid != 0)
                {
                    var userlistOrganized = BasicReorderListAdd(userList,userid);
                    var contactlistOrganized = BasicReorderListAdd(contactList, contactid);

                    _context.Contact.UpdateRange(userlistOrganized);
                    _context.Contact.UpdateRange(contactlistOrganized);
                    await _context.SaveChangesAsync();
                }
            
                if(action == "delete" && userid != 0 && contactid != 0)
                {
                    var userlistOrganized = BasicReorderListDel(userList, userid);
                    var contactlistOrganized = BasicReorderListDel(contactList, contactid);

                    _context.Contact.UpdateRange(userlistOrganized);
                    _context.Contact.UpdateRange(contactlistOrganized);
                    await _context.SaveChangesAsync();
                }
            
                if(action == "reorder" && userid != 0)
                {
                    var userlistOrganized = BasicReorderList(userList, userid);
                    if(userlistOrganized != null)
                    {
                        _context.Contact.UpdateRange(userlistOrganized);
                        await _context.SaveChangesAsync();
                    }
                }

                if (action == "first" && userid != 0 && contactid != 0)
                {
                    var userlistOrganized = BasicReorderListFirst(userList, userid, contactid);
                    var contactlistOrganized = BasicReorderListFirst(contactList, contactid, userid);

                    _context.Contact.UpdateRange(userlistOrganized);
                    _context.Contact.UpdateRange(contactlistOrganized);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }

        //***********************BasicReorderListAdd***********************
        private List<ContactModel> BasicReorderListAdd(List<ContactModel> list, int userid)
        {
            var listResult = list;
            if (listResult != null)
            {
                foreach (var contactItem in listResult)
                {
                    if (contactItem.UserOneId == userid)
                    {
                        contactItem.UserOnePlaceInTheList++;
                    }
                    else
                    {
                        contactItem.UserTwoPlaceInTheList++;
                    }
                }
            }
            return listResult;
        }

        private List<ContactModel> BasicReorderListDel(List<ContactModel> list, int userid)
        {
            var listResult = list;
            if (listResult != null)
            {
                int index = 1;
                foreach (var contactItem in listResult)
                {
                    if (contactItem.UserOneId == userid)
                    {
                        contactItem.UserOnePlaceInTheList = index;
                    }
                    else
                    {
                        contactItem.UserTwoPlaceInTheList = index;
                    }
                    index++;
                }
            }
            return listResult;
        }

        private List<ContactModel> BasicReorderListFirst(List<ContactModel> list, int userid, int contactid)
        {
            var listResult = list;

            var firstItem = list
                .Where(item => item.UserOneId == userid && item.UserTwoId == contactid ||
                        item.UserTwoId == userid && item.UserOneId == contactid)
                .FirstOrDefault();

            if (firstItem != null)
            {
                listResult.Remove(firstItem);
                listResult.Insert(0, firstItem);

                var index = 1;
                foreach (var contactItem in listResult)
                {
                    if (contactItem.UserOneId == userid)
                    {
                        contactItem.UserOnePlaceInTheList = index;
                    }
                    else
                    {
                        contactItem.UserTwoPlaceInTheList = index;
                    }
                    index++;
                }
            }
            else
            {
                return null;
            }
            return listResult;
        }

        public List<ContactModel> BasicReorderList(List<ContactModel> list, int userid)
        {
            try
            {
                //verify if need correction
                bool needReorder = false;

                for(int i=0; i<list.Count; i++)
                {
                    ContactModel contactItem = list[i];

                    if (contactItem.OrderInUserList != (i + 1))
                    {
                        needReorder = true;
                        break;
                    }   
                }

                //----------------------------------

                if (needReorder)
                {
                    var listItens = list.Where(list => list.OrderInUserList != 0)
                        .OrderBy(list => list.OrderInUserList).ToList();
                    var listZeros = list.Where(list => list.OrderInUserList == 0).ToList();

                    List<ContactModel> newlist = new List<ContactModel>();
                    newlist.AddRange(listItens);
                    newlist.AddRange(listZeros);

                    for (int i = 0; i < newlist.Count; i++)
                    {
                        if (newlist[i].UserOneId == userid)
                        {
                            newlist[i].UserOnePlaceInTheList = i + 1;
                        }
                        else
                        {
                            newlist[i].UserTwoPlaceInTheList = i + 1;
                        }
                    }

                    return newlist;
                }
                return null;
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
    }
}
