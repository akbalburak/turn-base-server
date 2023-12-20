using System;

namespace ModuleDTOLayer
{
    public class UserDTO : IUserDTO
    {
        public event Action<CoinChangeData> OnUserCoinChanged;
        public long Coins { get; set; }

        public string UserId { get; set; }

        public UserDataDTO UserData { get; set; }
        public InventoryDTO Inventory { get; set; }
        public CampaignDTO Campaign { get; set; }

        public bool IsInBattle { get; set; }

        public void AddCoins(long addCoins)
        {
            long oldCoin = Coins;
            Coins += addCoins;
            OnUserCoinChanged?.Invoke(new CoinChangeData(oldCoin, Coins));
        }
    }

    public class UserDataDTO
    {
        public int UserLevel { get; set; }
        public int Experience { get; set; }
        public string Username { get; set; }
    }

    public interface IUserDTO
    {
        string UserId { get; }
        long Coins { get; }

        UserDataDTO UserData { get; }
        CampaignDTO Campaign { get; }
        InventoryDTO Inventory { get; }

        event Action<CoinChangeData> OnUserCoinChanged;
        void AddCoins(long addCoins);
    }

    public struct CoinChangeData
    {
        public long OldCoins { get; }
        public long NewCoins { get; }
        public CoinChangeData(long oldCoins, long newCoins)
        {
            OldCoins = oldCoins;
            NewCoins = newCoins;
        }
    }
}