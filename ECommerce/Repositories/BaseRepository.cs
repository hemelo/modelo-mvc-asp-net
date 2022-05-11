using ECommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repositories
{
    public abstract class BaseRepository<T> where T : BaseModel
    {
        protected readonly ApplicationContext context;
        protected readonly DbSet<T> set; 

        public BaseRepository(ApplicationContext context)
        {
            this.context = context;
            set = this.context.Set<T>();
        }
    }
}
