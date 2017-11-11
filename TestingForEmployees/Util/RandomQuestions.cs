using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingForEmployees.Util
{
    public static class RandomQuestions
    {
        public static List<int> ReturnRandomUnique(int CountOut, List<int> data)
        {
            if (data != null && CountOut > 0)
            {
                List<int> returnList = new List<int>();


                while (returnList.Count() != CountOut)
                {
                    Random random = new Random();
                    int randomIndex = random.Next(0, data.Count());
                    bool flag = false;
                    foreach (var item in returnList)
                    {
                        if (item == data[randomIndex])
                            flag = true;
                    }
                    if (!flag)
                    {
                        returnList.Add(data[randomIndex]);
                    }
                }
                return returnList;
            }
            return new List<int>();
        }
    }
}
