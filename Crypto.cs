using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
namespace Crypto_HelperV4
{
    class Crypto
    {
        public const string StartPageLink = @"https://www.worldcoinindex.com/";//ссылка на биржу откуда берутся актуальные данные
        public string name { get; set; }// общее название крипты
        public string ticket { get; set; }//сокращенное название
        public double allTokens { get; set; }//количество купленных пользователем монет
        public decimal usdForTokenBought { get; set; }//стоимость покупки за одну монету
        public decimal usdForAllToken { get; set; }// общая стоимость покупки
        public decimal usdForTokenNow { get; set; }// цена монеты сейчас
        public decimal usdForAllTokensNow { get; set; }// общая цена всех монет сейчас
        public decimal usdIncome { get; set; }// прибыль в долларах
        public float percentOfIncome { get; set; }//прибыль в процентах
        public void setName(string nameX)
        {
            this.name = nameX;
        }
        public void setTicket() // считывание сокращенного названия
        {
            var htmlDoc = new HtmlWeb().Load(StartPageLink);
            var rows = htmlDoc.DocumentNode.SelectNodes($"//tbody//tr[@class='{this.name + " coinzoeken "}']//td[@class='ticker']");
            {
                foreach (var cell in rows)
                {
                    this.ticket = cell.InnerText.ToLower();
                    break;
                }
            }
        }
        public void setUsdFAT()// задать общую стоимость
        {
            this.usdForAllToken = (decimal)((float)this.usdForTokenBought * this.allTokens);
        }
        public void setUsdFTN()// задать текущую стоимость
        {
            decimal currency = 0;
            var htmlDoc = new HtmlWeb().Load(StartPageLink);
            var rows = htmlDoc.DocumentNode.SelectNodes($"//tbody//tr[@class='{this.name + " coinzoeken "}']//td[@class='number pricekoers lastprice']");
            foreach (var cell in rows)
            {
                string target = cell.InnerText;
                string result = "";
                foreach (var q in target)
                {
                    if (q != ',' && q != ' ' && q != '$')
                    {

                        if (q == '.')
                        {
                            result += ",";
                        }
                        else
                        {
                            result += q;
                        }
                    }
                }
                currency = Convert.ToDecimal(result);
            }
            this.usdForTokenNow = currency;
        }
        public void setUsdFATN()// задать текущую общую стоимость
        {
            this.usdForAllTokensNow = (decimal)this.allTokens * this.usdForTokenNow;
        }
        public void setUsdIncome()// задать прибыль в долларах
        {
            this.usdIncome = this.usdForAllTokensNow - usdForAllToken;
        }
        public void setPercentOI()// задать прибыль в процентах
        {
            this.percentOfIncome = (float)this.usdIncome / (float)this.usdForAllToken * 100;
        }
    }
}
