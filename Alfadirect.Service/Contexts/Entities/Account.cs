namespace Alfadirect.Service.Contexts.Entities
{
    public class Account
    {
        /// <summary>
        /// Идентификатор счёта
        /// </summary>
        public string Id { get; init; }
        
        /// <summary>
        /// Текущий баланс
        /// </summary>
        public decimal Balance { get; init; }
    }
}