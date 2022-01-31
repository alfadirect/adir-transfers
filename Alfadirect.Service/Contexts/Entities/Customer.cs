using System.Collections.Generic;

namespace Alfadirect.Service.Contexts.Entities
{
    public class Customer
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int CustomerId { get; init; }
        
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; init; }
        
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; init; }
        
        /// <summary>
        /// Список счетов
        /// </summary>
        public IEnumerable<string> Accounts { get; init; }
    }
}