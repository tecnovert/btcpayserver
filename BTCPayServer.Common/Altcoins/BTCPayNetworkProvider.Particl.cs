using NBitcoin;

namespace BTCPayServer
{
    public partial class BTCPayNetworkProvider
    {
        public void InitParticl()
        {
            //not needed: NBitcoin.Altcoins.Dash.Instance.EnsureRegistered();
            var nbxplorerNetwork = NBXplorerNetworkProvider.GetFromCryptoCode("PART");
            Add(new BTCPayNetwork()
            {
                CryptoCode = nbxplorerNetwork.CryptoCode,
                DisplayName = "Particl",
                BlockExplorerLink = NetworkType == ChainName.Mainnet
                    ? "https://explorer.particl.io/tx/{0}"
                    : "https://explorer-testnet.particl.io/tx/{0}",
                NBXplorerNetwork = nbxplorerNetwork,
                DefaultRateRules = new[]
                    {
                        "PART_X = PART_BTC * BTC_X",
                        "PART_BTC = bittrex(PART_BTC)"
                    },
                CryptoImagePath = "imlegacy/particl.svg",
                DefaultSettings = BTCPayDefaultSettings.GetDefaultSettings(NetworkType),
                //https://github.com/satoshilabs/slips/blob/master/slip-0044.md
                CoinType = NetworkType == ChainName.Mainnet ? new KeyPath("44'")
                    : new KeyPath("1'")
            });
        }
    }
}
