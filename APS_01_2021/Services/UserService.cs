using APS_01_2021.Data;
using APS_01_2021.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using App.Services.Exceptions;
using System;
using System.Linq;



namespace APS_01_2021.Services
{
    public class UserService
    {
        private MyDbContext _context;

        public UserService(MyDbContext context)
        {
            _context = context;
        }

        //CREATE
        public async Task InsertAsync(UserModel user)
        {
            if (user.Name == null || user.Email == null)
            {
                throw new IntegrityException("Nome e Email não podem estar Vazios");
            }
            else if (user.Name.Length < 4 || user.Email.Length < 4)
            {
                throw new IntegrityException("Nome e Email não podem ter menos que 4 caracteres");
            }
            else if(user.IsPasswordStrong(user.Password) != "Success")
            {
                throw new IntegrityException(user.IsPasswordStrong(user.Password));
            }
            else if (!user.IsConfirmPasswordConfirmed(user.Password, user.ConfirmPassword))
            {
                throw new IntegrityException("Passwords não estão iguais");
            }
            try
            {
                var email = await FindByEmailAsync(user.Email);
                var nickname = await FindByNickName(user.NickName);

                if(email != null)
                {
                    throw new IntegrityException("Email Já Utilizado");
                }
                else if (nickname != null)
                {
                    throw new IntegrityException("Apelido Já Utilizado");
                }
                _context.User.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }
        //------------------------------------------------------------

        //FIND
        public async Task<String> FindNickNameById(int id)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Id == id);
            return user.NickName;
        }
        public async Task<UserModel> FindByEmailAsync(string email)
        {
            return await _context.User.FirstOrDefaultAsync(x => x.Email == email);
        }
        public async Task<UserModel> FindByNickName(string nickname)
        {
            return await _context.User.FirstOrDefaultAsync(x => x.NickName == nickname);
        }
        public async Task<int> FindIdByNickName(string nickname)
        {
            var user = await FindByNickName(nickname);
            if(user == null)
            {
                return 0;
            }
            return user.Id;
        }
    }
}
