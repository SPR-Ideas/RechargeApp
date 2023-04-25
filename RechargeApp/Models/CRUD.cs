using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RechargeApp.Data;
using System.Linq.Expressions;

namespace RechargeApp.Models
{
    public class CRUD
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public CRUD(ApplicationDbContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

        public List<RechargeHistory> getRechargeHistory(int userid)
        {
            List<RechargeHistory> recharges = new List<RechargeHistory>();
            try 
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("localDb")))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(
                        "SELECT * FROM  rechargeTables " +
                        "JOIN planTables  ON planTables.id = rechargeTables.planId " +
                        $"where rechargeTables.userId = {userid}",
                        connection
                       );
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) 
                    {
                        RechargeHistory recharge = new RechargeHistory();
                        recharge.Id = reader.GetInt32(0);
                        recharge.description = (string) reader["description"];
                        recharge.name = (string)reader["Name"];
                        recharge.price = (double)reader["amount"];
                        recharge.validity = (int)reader["validity"];
                        recharge.timestamp = (DateTime)reader["timestamp"];
                        recharges.Add(recharge);

                    }

                }
                
            }
            catch(SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            return recharges;
        }

    }
}
