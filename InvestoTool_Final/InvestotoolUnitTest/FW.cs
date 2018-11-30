using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestoToolUnitTest
{
    class FW
    {

        int dimensions = 0;
        Double[,,] maxProfit = new Double[100, 100, 100];
        int[,,] Transactions = new int[100, 100, 100];




        public FW(Double[,] Inputarray, int dimensions)
        {
            this.dimensions = dimensions;

            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < dimensions; j++)
                {
                    maxProfit[0, i, j] = Inputarray[i, j];
                    Transactions[0, i, j] = i;
                }


            }


        }



        public String ReturnProft(Double marg, int addTrans, String[] currencycode)
        {

            //modified Floyd-Warshall

            for (int trans = 1; trans < addTrans; trans++) //max transaction number (currently 20)
            {
                for (int k = 0; k < dimensions; k++) //intermediate node k
                {
                    for (int i = 0; i < dimensions; i++) //path from i to j
                    {
                        for (int j = 0; j < dimensions; j++)
                        {
                            Double temp = maxProfit[trans - 1, i, k] * maxProfit[0, k, j];
                            if (temp > maxProfit[trans, i, j])
                            {
                                maxProfit[trans, i, j] = temp;
                                Transactions[trans, i, j] = k;
                            }
                        }
                    }
                }
            }


            Double finalprofit = 0;




            //Determines if arbitrage exists

            int steps, Arbitrage = -1;
            for (steps = 1; steps < dimensions; steps++)
            {
                for (int i = 0; i < dimensions; i++)
                {
                    if (maxProfit[steps, i, i] > 1 + marg)
                    {

                        finalprofit = maxProfit[steps, i, i];
                        Arbitrage = i;
                        break;
                    }
                }
                if (Arbitrage != -1)
                    break;
            }

            String result = "";

            //Output final data
            if (Arbitrage == -1)
            {
                result = "no arbitrage found";
            }
            else
            {
                int[] outputSeq = new int[20];
                int index = 0;

                int currentPo = Arbitrage;
                outputSeq[index++] = Arbitrage;
                for (int s = steps; s >= 0; --s)
                {
                    currentPo = Transactions[s, Arbitrage, currentPo];
                    outputSeq[index++] = currentPo;
                }



                for (int i = index - 1; i > 0; --i)
                    result += outputSeq[i] + 1 +" "+currencycode[outputSeq[i]] + " --> ";

                result += outputSeq[0] + 1+" " + currencycode[outputSeq[0]] +"\n";
                result += "Final Profit is " + finalprofit;
            }





            return result;
        }
    }
}
