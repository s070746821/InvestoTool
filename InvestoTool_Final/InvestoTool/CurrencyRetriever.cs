using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml.Linq;


namespace CurrencyPriceRetriever
{
    class CurrencyRates
    {
        private static string CreateUrl(LinkedList<string> currencyType, int i)
        {
            string url = "http://www.floatrates.com/daily/" + currencyType.ElementAt(i) + ".xml"; //creates floatrates url to selected currency
            return url;
        }
        public float[,] GetCurrencyRates(LinkedList<string> CurrencyType)
        {
            float[,] FW_Array = new float [CurrencyType.Count(),CurrencyType.Count()]; //floyd Warshall Array

            for (int i = 0; i < CurrencyType.Count(); i++)
            {
                var url = CreateUrl(CurrencyType,i);
                XDocument xml = XDocument.Load(url); //load currency Info file 

                //parses xml page and extracts given elements, then creates an object in which data will be stored
                var currencyType = xml.Descendants("item").Select(cur =>
                    new
                    {
                        Currency = (string)cur.Element("targetCurrency"),
                        Rate = (string)cur.Element("exchangeRate")
                    });
                foreach (var result in currencyType) // loops through results found on xml page 
                {

                    for (int j = 0; j < CurrencyType.Count(); j++)
                    {
                        if (i == j)
                        {
                            FW_Array[i, j] = 1;
                        }
                        else if (result.Currency == CurrencyType.ElementAt(j))  //checks to see if Currency from xml page matches currency we want
                        {
                            FW_Array[i, j] = float.Parse(result.Rate);
                        }

                    }
                }
            }
            return FW_Array;
        } 
    }
}