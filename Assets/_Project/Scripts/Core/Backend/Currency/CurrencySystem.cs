using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Core.Backend.Currency
{
    /// <summary>
    /// System to handle currency.
    /// </summary>
    public class CurrencySystem : BaseSystem<CurrencySystem>
    {
        protected override bool IsPersistent => true;

        /// <summary>
        /// The starting amount of coins for a new wallet.
        /// </summary>
        private const int StartingCoins = 2000;

        /// <summary>
        /// Dictionary of wallets.
        /// </summary>
        private readonly Dictionary<string, Wallet> _wallets = new();

        /// <summary>
        /// Creates a new wallet and returns the wallet ID.
        /// </summary>
        /// <returns></returns>
        public string CreateWallet()
        {
            // Create a new wallet and add it to the dictionary
            var wallet = new Wallet(StartingCoins);
            var walletID = wallet.GetHashCode().ToString();
            _wallets.Add(walletID, wallet);

            // Return the wallet ID
            return walletID;
        }

        /// <summary>
        /// Removes a wallet from the system.
        /// </summary>
        /// <param name="walletID">wallet ID to remove</param>
        public void RemoveWallet(string walletID)
        {
            // Remove and destroy the wallet
            _wallets[walletID] = null;
            _wallets.Remove(walletID);
        }

        /// <summary>
        /// Adds coins to a wallet.
        /// </summary>
        /// <param name="walletID">wallet ID to add coins</param>
        /// <param name="amount">amount of coins to add</param>
        public void AddCoins(string walletID, int amount)
        {
            // Get the wallet and add coins
            var wallet = GetWallet(walletID);
            wallet?.AddCoins(amount);
        }

        /// <summary>
        /// Removes coins from a wallet.
        /// </summary>
        /// <param name="walletID">wallet ID to remove coins</param>
        /// <param name="amount">amount of coins to remove</param>
        /// <returns></returns>
        public bool RemoveCoins(string walletID, int amount)
        {
            // Get the wallet and remove coins
            var wallet = GetWallet(walletID);
            return wallet != null && wallet.RemoveCoins(amount);
        }

        /// <summary>
        /// Get the amount of coins in a wallet.
        /// </summary>
        /// <param name="walletID">wallet ID to return the coins in it</param>
        /// <returns>amount of coins in a wallet of specified ID</returns>
        public int GetCoins(string walletID)
        {
            // Get the wallet and return the coins value
            var wallet = GetWallet(walletID);
            return wallet?.Coins ?? 0;
        }

        /// <summary>
        /// Get a wallet by its ID.
        /// </summary>
        /// <param name="walletID">wallet ID to return the wallet</param>
        /// <returns>wallet of the specified ID</returns>
        private Wallet GetWallet(string walletID)
        {
            var wallet = _wallets.GetValueOrDefault(walletID);
            if (wallet != null)
                return wallet;

            Debug.LogError("Wallet not found!");
            return null;
        }
    }
}