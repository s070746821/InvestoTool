using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CurrencyPriceRetriever;
using Microsoft.Win32;
using System.IO;


namespace InvestoTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();

            ImageBrush brush = new ImageBrush();
            BitmapImage bitmapImage = new BitmapImage(new Uri("background2.jpg", UriKind.Relative));
            brush.ImageSource = bitmapImage;
           

            Tabc.Background = brush;
        }

        private void FindProfitablePath(object sender, RoutedEventArgs e)
        {

        }

        private void SaveCurrencies(object sender, RoutedEventArgs e)
        {    
            CheckBox[] checkBoxArray= {AUD, CAD, CHF, CNY, EUR, GBP, JPY, MXN, NZD, RUB, SEK, USD};
            LinkedList<string> selectedCurrencies = new LinkedList<string>{};
            string currency;
            Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();

            Thread.Sleep(1000);
            for (int i=0; i<checkBoxArray.Length;i++)
            {
                if(checkBoxArray[i].IsChecked == true)
                {
                    currency = checkBoxArray[i].Content.ToString();
                    selectedCurrencies.AddLast(currency);
                }
                
            }
            CurrencyRates currentRates = new CurrencyRates();
            float[,] FW_Array = new float[selectedCurrencies.Count(), selectedCurrencies.Count()];
            FW_Array = currentRates.GetCurrencyRates(selectedCurrencies); // This array Contains the magic
      
            string test1 ="";  
            for (int i = 0; i < selectedCurrencies.Count; i++)
            {
                for(int j = 0; j< selectedCurrencies.Count; j++)
                {
                    test1 += "\t"+ FW_Array[i, j].ToString() +"\t";
                }
                test1 += "\n"; 
            }
           
            TestBox.Text = test1.ToString();
            Loading.Visibility = Visibility.Visible;
            newWindowThread.Abort();


            double[,] Algoarray = new double[selectedCurrencies.Count(), selectedCurrencies.Count()];

            for (int i = 0; i < selectedCurrencies.Count; i++)
            {
                for (int j = 0; j < selectedCurrencies.Count; j++)
                {
                    Algoarray[i,j]= FW_Array[i, j];
                }
                
            }


            String Result = "";

            FW algo = new FW(Algoarray, selectedCurrencies.Count);

            Double maxp = 0.01;
            int maxt = selectedCurrencies.Count;

            if (optionsCheck.IsChecked.HasValue && optionsCheck.IsChecked.Value)
            {
                Double.TryParse(profitTextbox.Text, out maxp);
                int.TryParse(TransTextbox.Text, out maxt);
            }



            Result += (maxp + "    " + maxt + "\n");

            //This part is what appends the returned result to the textbox. You can just store Returnproft to a string and pass that string to a new window
            String[] checkarray = { "AUD", "CAD", "CHF", "CNY", "EUR", "GBP", "JPY", "MXN", "NZD", "RUB", "SEK", "USD" };
            Result += algo.ReturnProft(maxp, maxt, checkarray);
            MessageBox.Show(Result);        //display current resulting path on screen


        }

        

        private void selectAll_Click(object sender, RoutedEventArgs e)
        {
            CheckBox[] checkBoxArray = {AUD, CAD, CHF, CNY, EUR, GBP, JPY, MXN, NZD, RUB, SEK, USD, };
            if (selectAll.IsChecked == true)
            {
                for (int i = 0; i < checkBoxArray.Count(); i++)
                {
                    checkBoxArray.ElementAt(i).IsChecked = true;
                }
            }
            else
            {
                for (int i = 0; i < checkBoxArray.Count(); i++)
                {
                    checkBoxArray.ElementAt(i).IsChecked = false;
                }
            }
        }
        public void ThreadStartingPoint()
        {
            Window1 w1 = new Window1();
            w1.ShowDialog();
        }


        public bool ErrCheck(String Fname)
        {
            StreamReader Reader = new StreamReader(@Fname);

            int dimensions;
            bool check= int.TryParse(Reader.ReadLine(), out dimensions);

            if (!check)
            {

                Console.WriteLine("Error 1111111111111");
                return false;
            }
            String[] currencyarray = Reader.ReadLine().Split(' ');

            if (currencyarray.Length != dimensions) {

                Console.WriteLine("Error 2222222222222");
                return false;
            } 





            Double[,] Inputarray = new Double[dimensions, dimensions];

            for (int i = 0; i < dimensions; i++)
            {
                if (Reader.Peek() < 0 && i < dimensions - 1)
                {

                    Console.WriteLine("Error 3333333333333333");
                    return false;
                }



                String phrase = Reader.ReadLine();
                String[] line = phrase.Split(' ');

                if (line.Length != dimensions) {

                    Console.WriteLine("Error 44444444444444444444"+ "   "+i+"  "+line.Length+"  "+dimensions);

                    for(int j=0; j < line.Length; j++)
                    {
                        Console.WriteLine(j+"   " + line[j]);
                    }

                    return false;
                } 




                for (int j = 0; j < dimensions; j++)
                {



                    Double item;



                    bool mybool=Double.TryParse(line[j], out item);

                    if (!mybool) {


                        Console.WriteLine("Error 555555555555555555");

                        return false;

                    } 

                    Inputarray[i, j] = item;

                }

                



            }

            for (int i = 0; i < dimensions; i++)
            {
                if (Inputarray[i, i] != 1) {

                    Console.WriteLine("Error 6666666666666666");
                    return false;

                }
            }

            if (Reader.Peek() >= 0)
            {
                Console.WriteLine("Error 7777777777777");
                return false;
            }

            return true;


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            String Filename = "no file chosen";

            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() != true)
                return;
            else
            {
                Filename = openFileDialog.FileName;
            }

            if (!System.IO.Path.GetExtension(@Filename).Equals(".csv") && !System.IO.Path.GetExtension(@Filename).Equals(".txt"))
            {
                MessageBox.Show("Incompatible Filen Type");
                return;
            }


            using (StreamReader sr = new StreamReader(@Filename))
            {

                if (!ErrCheck(Filename))
                {
                    MessageBox.Show("Errors in file detected");
                    return;
                }



                while (sr.Peek() >= 0)
                {

                    String Result = "";
                    

                    //getting input
                    int dimensions;
                    int.TryParse(sr.ReadLine(), out dimensions);
                    Result+=(dimensions + "\n");

                    //getting currencyarray

                    String[] currencyarray = sr.ReadLine().Split(' ');

                    


                    


                        //Parsing input into double array


                        Double[,] Inputarray = new Double[dimensions, dimensions];

                    for (int i = 0; i < dimensions; i++)
                    {

                        
                        String phrase = sr.ReadLine();
                        String[] line = phrase.Split(' ');


                       



                        for (int j = 0; j < dimensions; j++)
                        {



                            Double item;


                            
                            Double.TryParse(line[j], out item);

                            

                            Inputarray[i, j] = item;

                        }



                    }

                   


                    

                    //algo is the algorithm object of class FW the method in class FW is RetrunProft
                    FW algo = new FW(Inputarray, dimensions);

                    Double maxp = 0.01;
                    int maxt = dimensions;

                    if (optionsCheck.IsChecked.HasValue && optionsCheck.IsChecked.Value)
                    {
                        bool optionsflag = true;

                        optionsflag=Double.TryParse(profitTextbox.Text, out maxp);

                        if (!optionsflag) {
                            MessageBox.Show("Incompatible Parameters. Profit must be greater than 0 and less than 1, and transactions must be greater than 0 and less than 100");
                            return;
                        }

                        optionsflag=int.TryParse(TransTextbox.Text, out maxt);

                        if (!optionsflag)
                        {
                            MessageBox.Show("Incompatible Parameters. Profit must be greater than 0 and less than 1, and transactions must be greater than 0 and less than 100");
                            return;
                        }

                        if(maxp<=0 || maxp >= 1)
                        {
                            MessageBox.Show("Incompatible Parameters. Profit must be greater than 0 and less than 1, and transactions must be greater than 0 and less than 100");
                            return;
                        }


                        if (maxt <= 0 || maxt >= 100)
                        {
                            MessageBox.Show("Incompatible Parameters. Profit must be greater than 0 and less than 1, and transactions must be greater than 0 and less than 100");
                            return;
                        }
                    }

                    

                    Result+=(maxp + "    " + maxt + "\n");

                    //This part is what appends the returned result to the textbox. You can just store Returnproft to a string and pass that string to a new window
                    Result+= algo.ReturnProft(maxp, maxt, currencyarray);



                    /*
                    IAccessDB my_db = new AccessDB();       //open database connections
                    my_db.initialize_db();

                    my_db.write_db(maxp, maxt, dimensions, Result);

                    int num_history = 5;                    //num_history is number of rows from DB to display
                    object[,] history = my_db.read_db(num_history);
                    Result += ("\n\nPast Transactions\n date and time\t\tprofit\tthreshold\tdimension\ttransactional pathway\n");

                    for (int i = 0; i < history.GetLength(0); i++)//write history to screen

                    {

                        for (int j = 0; j < history.GetLength(1)-1; j++)

                        {

                            Result += history[i, j].ToString() + '\t';

                        }

                        Result += '\n';

                    }
                    */
                    
                    MessageBox.Show(Result);        //display current resulting path on screen





                    







                }



            }

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

}
