using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityS4
{
    public class EFContext : DbContext
    {
        public EFContext(DbContextOptions<EFContext> options) : base(options)
        { }

        //实体集
        public DbSet<Admin> Admin { get; set; }
    }

    public class Admin
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Remark { get; set; }
    }


    public interface IAdminService
    {
        Task<Admin> GetByStr(string username, string pwd);//根据用户名和密码查找用户
    }

    public class AdminService : IAdminService
    {
        public EFContext db;

        public AdminService(EFContext _efContext)
        {
            db = _efContext;
        }

        /// <summary>
        /// 验证用户，成功则返回用户信息，否则返回null
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public async Task<Admin> GetByStr(string username, string pwd)
        {
            Admin m = await db.Admin.Where(a => a.UserName == username && a.Password == pwd).SingleOrDefaultAsync();
            if (m != null)
            {
                return m;
            }
            else
            {
                return null;
            }
        }
    }
}
