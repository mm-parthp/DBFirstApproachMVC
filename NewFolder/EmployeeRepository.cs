using DBFirstApproachLatest.Data;
using DBFirstApproachLatest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBFirstApproachLatest.NewFolder
{
    public class EmployeeRepository : GenericRepository<Employees>, IEmployeeRepository
    {
        public EmployeeRepository(DBContext context, ILogger logger) : base(context, logger)
        {
        }
        public override async Task<IEnumerable<Employees>> All()
        {
            try
            {
                return await dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(EmployeeRepository));
                return new List<Employees>();
            }
        }
        public override async Task<bool> Upsert(Employees entity)
        {
            try
            {
                var existingUser = await dbSet.Where(x => x.EmployeeId == entity.EmployeeId)
                                                    .FirstOrDefaultAsync();

                if (existingUser == null)
                    return await Add(entity);

                //existingUser.FirstName = entity.FirstName;
                //existingUser.LastName = entity.LastName;
                //existingUser.Email = entity.Email;
                //existingUser.PhoneNumber = entity.PhoneNumber;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert function error", typeof(EmployeeRepository));
                return false;
            }
        }

        public override async Task<bool> Delete(int id)
        {
            try
            {
                var exist = await dbSet.Where(x => x.EmployeeId == id)
                                        .FirstOrDefaultAsync();

                if (exist == null) return false;

                dbSet.Remove(exist);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete function error", typeof(EmployeeRepository));
                return false;
            }
        }

    }
}
