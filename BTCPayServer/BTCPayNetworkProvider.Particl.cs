using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTCPayServer.Services.Rates;
using NBitcoin;
using NBXplorer;

namespace BTCPayServer
{
    public partial class BTCPayNetworkProvider
    {
        public void InitParticl()
        {
            var nbxplorerNetwork = NBXplorerNetworkProvider.GetFromCryptoCode("PART");
            Add(new BTCPayNetwork()
            {
                CryptoCode = nbxplorerNetwork.CryptoCode,
                DisplayName = "Particl",
                BlockExplorerLink = NetworkType == NetworkType.Mainnet ? "https://explorer.particl.io/tx/{0}" : "https://explorer-testnet.particl.io/tx/{0}",
                NBitcoinNetwork = nbxplorerNetwork.NBitcoinNetwork,
                NBXplorerNetwork = nbxplorerNetwork,
                UriScheme = "particl",
                DefaultRateRules = new[] 
                {
                                "PART_X = PART_BTC * BTC_X",
                                "PART_BTC = bittrex(PART_BTC)"
                },
                CryptoImagePath = "imlegacy/particl.svg",
                DefaultSettings = BTCPayDefaultSettings.GetDefaultSettings(NetworkType),
                CoinType = NetworkType == NetworkType.Mainnet ? new KeyPath("44'") : new KeyPath("1'")
            });
        }
    }
}
