using ECommerce.Models;

namespace ECommerce.Repositories
{
    public class CadastroRepository : BaseRepository<Cadastro>
    {
        public CadastroRepository(ApplicationContext context) : base(context)
        {
        }

        public Cadastro? Find(int cadastroId)
        {
            return set.Where(entity => entity.Id == cadastroId).SingleOrDefault();
        }

        public void Update(int cadastroId, Cadastro cadastroAtualizado)
        {
            Cadastro? cadastro = Find(cadastroId);

            if(cadastro is not null)
            {
                cadastro.Update(cadastroAtualizado);
            }

            context.SaveChanges();
        }
    }
}
