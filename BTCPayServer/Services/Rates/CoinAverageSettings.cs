﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BTCPayServer.Services.Rates
{
    public class CoinAverageSettingsAuthenticator : ICoinAverageAuthenticator
    {
        CoinAverageSettings _Settings;
        public CoinAverageSettingsAuthenticator(CoinAverageSettings settings)
        {
            _Settings = settings;
        }
        public Task AddHeader(HttpRequestMessage message)
        {
            return _Settings.AddHeader(message);
        }
    }

    public class CoinAverageExchange
    {
        public CoinAverageExchange(string name, string display)
        {
            Name = name;
            Display = display;
        }
        public string Name { get; set; }
        public string Display { get; set; }
        public string Url
        {
            get
            {
                return Name == CoinAverageRateProvider.CoinAverageName ? $"https://apiv2.bitcoinaverage.com/indices/global/ticker/short"
                                     : $"https://apiv2.bitcoinaverage.com/exchanges/{Name}";
            }
        }
    }
    public class CoinAverageExchanges : Dictionary<string, CoinAverageExchange>
    {
        public CoinAverageExchanges()
        {
        }

        public void Add(CoinAverageExchange exchange)
        {
            TryAdd(exchange.Name, exchange);
        }
    }
    public class CoinAverageSettings : ICoinAverageAuthenticator
    {
        private static readonly DateTime _epochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public (String PublicKey, String PrivateKey)? KeyPair { get; set; }
        public CoinAverageExchanges AvailableExchanges { get; set; } = new CoinAverageExchanges();

        public CoinAverageSettings()
        {
            //GENERATED BY:
            //StringBuilder b = new StringBuilder();
            //b.AppendLine("_coinAverageSettings.AvailableExchanges = new[] {");
            //foreach (var availableExchange in _coinAverageSettings.AvailableExchanges)
            //{
            //    b.AppendLine($"(DisplayName: \"{availableExchange.DisplayName}\", Name: \"{availableExchange.Name}\"),");
            //}
            //b.AppendLine("}.ToArray()");
            AvailableExchanges = new CoinAverageExchanges();
            foreach(var item in
             new[] {
                (DisplayName: "BitBargain", Name: "bitbargain"),
                (DisplayName: "Tidex", Name: "tidex"),
                (DisplayName: "LocalBitcoins", Name: "localbitcoins"),
                (DisplayName: "EtherDelta", Name: "etherdelta"),
                (DisplayName: "Kraken", Name: "kraken"),
                (DisplayName: "BitBay", Name: "bitbay"),
                (DisplayName: "Independent Reserve", Name: "independentreserve"),
                (DisplayName: "Exmoney", Name: "exmoney"),
                (DisplayName: "Bitcoin.co.id", Name: "bitcoin_co_id"),
                (DisplayName: "Huobi", Name: "huobi"),
                (DisplayName: "GDAX", Name: "gdax"),
                (DisplayName: "Coincheck", Name: "coincheck"),
                (DisplayName: "Bittylicious", Name: "bittylicious"),
                (DisplayName: "Gemini", Name: "gemini"),
                (DisplayName: "Bit2C", Name: "bit2c"),
                (DisplayName: "Luno", Name: "luno"),
                (DisplayName: "Negocie Coins", Name: "negociecoins"),
                (DisplayName: "FYB-SE", Name: "fybse"),
                (DisplayName: "Hitbtc", Name: "hitbtc"),
                (DisplayName: "Bitex.la", Name: "bitex"),
                (DisplayName: "Korbit", Name: "korbit"),
                (DisplayName: "itBit", Name: "itbit"),
                (DisplayName: "Okex", Name: "okex"),
                (DisplayName: "Bitsquare", Name: "bitsquare"),
                (DisplayName: "Bitfinex", Name: "bitfinex"),
                (DisplayName: "CoinMate", Name: "coinmate"),
                (DisplayName: "Bitstamp", Name: "bitstamp"),
                (DisplayName: "Cryptonit", Name: "cryptonit"),
                (DisplayName: "Foxbit", Name: "foxbit"),
                (DisplayName: "QuickBitcoin", Name: "quickbitcoin"),
                (DisplayName: "Poloniex", Name: "poloniex"),
                (DisplayName: "Bit-Z", Name: "bitz"),
                (DisplayName: "Liqui", Name: "liqui"),
                (DisplayName: "BitKonan", Name: "bitkonan"),
                (DisplayName: "Kucoin", Name: "kucoin"),
                (DisplayName: "Binance", Name: "binance"),
                (DisplayName: "Rock Trading", Name: "rocktrading"),
                (DisplayName: "Mercado Bitcoin", Name: "mercado"),
                (DisplayName: "Coinsecure", Name: "coinsecure"),
                (DisplayName: "Coinfloor", Name: "coinfloor"),
                (DisplayName: "bitFlyer", Name: "bitflyer"),
                (DisplayName: "BTCTurk", Name: "btcturk"),
                (DisplayName: "Bittrex", Name: "bittrex"),
                (DisplayName: "CampBX", Name: "campbx"),
                (DisplayName: "Zaif", Name: "zaif"),
                (DisplayName: "FYB-SG", Name: "fybsg"),
                (DisplayName: "Quoine", Name: "quoine"),
                (DisplayName: "BTC Markets", Name: "btcmarkets"),
                (DisplayName: "Bitso", Name: "bitso"),
                })
            {
                AvailableExchanges.TryAdd(item.Name, new CoinAverageExchange(item.Name, item.DisplayName));
            }
        }

        public Task AddHeader(HttpRequestMessage message)
        {
            var signature = GetCoinAverageSignature();
            if (signature != null)
            {
                message.Headers.Add("X-signature", signature);
            }
            return Task.CompletedTask;
        }

        public string GetCoinAverageSignature()
        {
            var keyPair = KeyPair;
            if (!keyPair.HasValue)
                return null;
            if (string.IsNullOrEmpty(keyPair.Value.PublicKey) || string.IsNullOrEmpty(keyPair.Value.PrivateKey))
                return null;
            var timestamp = (int)((DateTime.UtcNow - _epochUtc).TotalSeconds);
            var payload = timestamp + "." + keyPair.Value.PublicKey;
            var digestValueBytes = new HMACSHA256(Encoding.ASCII.GetBytes(keyPair.Value.PrivateKey)).ComputeHash(Encoding.ASCII.GetBytes(payload));
            var digestValueHex = NBitcoin.DataEncoders.Encoders.Hex.EncodeData(digestValueBytes);
            return payload + "." + digestValueHex;
        }
    }
}
