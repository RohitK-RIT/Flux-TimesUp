namespace _Project.Scripts.Core.Backend.Currency
{
    /// <summary>
    /// Class to store the player's wallet and handle adding and removing coins.
    /// </summary>
    internal class Wallet
    {
        /// <summary>
        /// The amount of coins in the wallet.
        /// </summary>
        internal int Coins { get; private set; }

        /// <summary>
        /// Constructor for the wallet.
        /// </summary>
        /// <param name="coins">starting amount in the wallet</param>
        internal Wallet(int coins)
        {
            Coins = coins;
        }

        /// <summary>
        /// Adds coins to the wallet.
        /// </summary>
        /// <param name="amount">amount to add</param>
        internal void AddCoins(int amount)
        {
            Coins += amount;
        }

        /// <summary>
        /// Removes coins from the wallet.
        /// </summary>
        /// <param name="amount">amount to remove</param>
        /// <returns>was successfully removed</returns>
        internal bool RemoveCoins(int amount)
        {
            // Check if there are enough coins in the wallet
            if (Coins < amount) return false;

            // Remove the coins
            Coins -= amount;
            return true;
        }
    }
}