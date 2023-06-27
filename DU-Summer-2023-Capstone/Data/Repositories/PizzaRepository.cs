using DU_Summer_2023_Capstone.Data.Interfaces;
using DU_Summer_2023_Capstone.Data.Models;

namespace DU_Summer_2023_Capstone.Data.Repositories
{
    public class PizzaRepository : IPizzaRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        public PizzaRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Pizza> Pizzas => _appDbContext.Pizzas;


        public Pizza GetPizzaById(int drinkId) => _appDbContext.Pizzas.FirstOrDefault(p => p.PizzaId == drinkId);

        public void DisablePizza(int pizzaId)
        {
            var newPizza = _appDbContext.Pizzas.Find(pizzaId);
            newPizza.InStock = false;
            _appDbContext.SaveChanges();
        }

        public void EnablePizza(int pizzaId)
        {
            var newPizza = _appDbContext.Pizzas.Find(pizzaId);
            newPizza.InStock = true;
            _appDbContext.SaveChanges();
        }
    }
}
