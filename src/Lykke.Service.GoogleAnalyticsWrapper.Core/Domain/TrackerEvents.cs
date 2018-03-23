namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain
{
    /// <summary>
    /// GA event names
    /// </summary>
    public static class TrackerEvents
    {
        /// <summary>
        /// User registered
        /// </summary>
        public const string UserRegistered = "UserRegistered";
        /// <summary>
        /// Kyc completed
        /// </summary>
        public const string KycCompleted = "KycCompleted";
        /// <summary>
        /// Order created
        /// </summary>
        public const string OrderCreated = "OrderCreated";
        /// <summary>
        /// Withdraw 0 - 100
        /// </summary>
        public const string WslotA = "WslotA";
        /// <summary>
        /// Withdraw > 100
        /// </summary>
        public const string WslotB = "WslotB";
        /// <summary>
        /// Withdraw > 1000
        /// </summary>
        public const string WslotC = "WslotC";
        /// <summary>
        /// Deposit 0 - 100
        /// </summary>
        public const string DslotA = "DslotA";
        /// <summary>
        /// Deposit > 100
        /// </summary>
        public const string DslotB = "DslotB";
        /// <summary>
        /// Deposit > 1000
        /// </summary>
        public const string DslotC = "DslotC";
    }
}
