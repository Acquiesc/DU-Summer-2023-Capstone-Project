using DU_Summer_2023_Capstone.Data.Models;

namespace DU_Summer_2023_Capstone.Data.Interfaces
{
    public interface IPizzaRepository
    {
        IEnumerable<Pizza> Pizzas
        {
            get;
        }

        Pizza GetPizzaById(int drinkId);

        public void DisablePizza(int pizzaId);

        public void EnablePizza(int pizzaId);
    }
}
