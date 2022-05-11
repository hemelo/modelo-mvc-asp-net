using ECommerce.Models;

namespace ECommerce.Repositories
{
    public class ProdutoRepository : BaseRepository<Produto>
    {
        public ProdutoRepository(ApplicationContext context) : base(context)
        {
        }

        public Produto? Find(string codigo)
        {
            return set.Where(entity => entity.Codigo == codigo).SingleOrDefault();
        }

        public void Add(List<Produto> data)
        {
            set.AddRange(data);
            context.SaveChanges();
        }

        public async Task Add(List<Livro>? livros)
        {
            if (livros is null)
            {
                return;
            }

            foreach (var livro in livros)
            {
                if ( ! set.Where(p => p.Codigo == livro.Codigo).Any() )
                {
                    await set.AddAsync(new Produto(livro.Codigo, livro.Nome, livro.Preco));
                }
            }
            await context.SaveChangesAsync();
        }

        public void Add(Produto data)
        {
            set.Add(data);
            context.SaveChanges();
        }

        public IList<Produto> Get()
        {
            return set.ToList();
        }
    }
}
