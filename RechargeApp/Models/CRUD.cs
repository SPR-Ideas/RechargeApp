using Microsoft.EntityFrameworkCore;
using RechargeApp.Data;

namespace RechargeApp.Models
{
    public class CRUD
    {
        private readonly ApplicationDbContext _context;
        
        public CRUD(ApplicationDbContext configuration) 
        {
            _context = configuration;
        }

        public async Task<User> VerifyLogin(User user) 
        {
            User _user = null; 
            if (user!=null)
            {
                _user = await _context.Users.FirstOrDefaultAsync(m => m.PhoneNumber == user.PhoneNumber);
                if (_user != null && _user.password == _user.password)
                    return _user;
            }  
            return _user;
        }

        public async Task<List<PlanTable>> getAllPlans() {

            return await _context.planTables.ToListAsync();
        }

        public async Task addPlanToUser(int userId,int planId) {
            RechargeTable rechargeTable = new RechargeTable();

            rechargeTable.planId = planId;
            rechargeTable.userId = userId;
            rechargeTable.timestamp = DateTime.UtcNow;

            _context.Add(rechargeTable);
            await _context.SaveChangesAsync();
            
        }

    }
}
