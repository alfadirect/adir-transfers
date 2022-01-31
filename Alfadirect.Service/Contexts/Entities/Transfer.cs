namespace Alfadirect.Service.Contexts.Entities
{
    public class Transfer
    {
        /// <summary>
        /// Счет С которого был выполнен перевод
        /// </summary>
        public string SourceAccountId { get; init; }
        
        /// <summary>
        /// Счет НА который был выполнен перевод
        /// </summary>
        public string DestinationAccountId { get; init; }
        
        /// <summary>
        /// Объем средств перевода
        /// </summary>
        public decimal Amount { get; init; }
    }
}