using Nito.AsyncEx;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurosApp
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            try
            {
                AsyncContext.Run(() => MainAsync(args));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }
        
        static async void MainAsync(string[] args)
        {
            bool Success = false;
            List<int> arr = new List<int> {1,1,1,1,1,1,1,1,1};
            List<int> inputs = new List<int> {1,2};
            while (!Success)
            {
                arr = MagicNumberGenerator.Cartesian(arr, inputs, null);
                if (arr.Distinct().Count() == 1 && arr[0] == inputs[inputs.Count -1])
                {
                    Success = true; //Stop processing
                }
                Console.WriteLine("Attempting with with " + string.Join(",", arr));
                var res = await MagicNumberGenerator.GetMagicNums(arr);
                if (res.Success)
                {
                    Success = true;
                    Console.WriteLine("Success with " +  string.Join(",", res.MagicSquareNumbers));
                    logger.Log(new LogEventInfo(LogLevel.Info, "Success", string.Join(",", res.MagicSquareNumbers)));
                }
            }
        }
    }
}


public class MagicResult
{
    public bool Success { get; set; }
    public List<int> MagicSquareNumbers { get; set; }
}

public static class MagicNumberGenerator{

    public static async Task<MagicResult> GetMagicNums(List<int> arr)
    {
        return await Task.Run(() => CalcMagicSquare(arr));
    }

    public static List<int> Cartesian(List<int> startingArray, List<int> inputs, int? currentColumn)
    {
        if (currentColumn == null)
        {
            currentColumn = startingArray.Count - 1;
        }

        var currentItemsInputIndex = inputs.IndexOf(startingArray[currentColumn.Value]);

        if (currentItemsInputIndex == inputs.Count - 1)
        {
            startingArray[currentColumn.Value] = inputs[0];
            startingArray = Cartesian(startingArray, inputs, currentColumn - 1);
        }
        else
        {
            startingArray[currentColumn.Value] = inputs[currentItemsInputIndex + 1];
        }

        return startingArray;
    }

    private static MagicResult CalcMagicSquare(List<int> arr)
    {
        var a = arr[0];
        var b = arr[1];
        var c = arr[2];
        var d = arr[3];
        var e = arr[4];
        var f = arr[5];
        var g = arr[6];
        var h = arr[7];
        var i = arr[8];

        var rowA = a + b + c;
        var rowB = d + e + f;
        var rowC = g + h + i;
        var colA = a + d + g;
        var colB = b + e + h;
        var colC = c + f + i;
        var diagA = a + e + i;
        var diagB = g + e + c;

        var list = new List<int> { rowA, rowB, rowC, colA, colB, colC, diagA, diagB };

        if (rowA == rowB && rowB == rowC && rowC == colA && colA == colB && colB == colC && colC == diagA && diagA == diagB 
            && list.Distinct().Count() >= 7)
        {
            Console.WriteLine("Success result" + a + b + c + d + e + f + g + h + i);
            return new MagicResult
            {
                Success = true,
                MagicSquareNumbers = new List<int> { a,b,c,d,e,f,g,h,i }
            };

        }
        else
        {
            return new MagicResult
            {
                Success = false,
                MagicSquareNumbers = new List<int> { a, b, c, d, e, f, g, h, i }
            };
        }
    }
}