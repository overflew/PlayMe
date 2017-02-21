using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Common.Util
{
    public static class WeightingUtil
    {
        private static IEnumerable<IWeighted> FindValidItems(this IEnumerable<IWeighted> items)
        {
            return items.Where(i => i.Weight > 0);
        }

        public static int TotalWeight(this IEnumerable<IWeighted> items)
        {
            return items.FindValidItems().Sum(i => i.Weight);
        }

        // Taken from http://stackoverflow.com/questions/56692/random-weighted-choice
        public static T Choose<T>(IList<T> list) where T : IWeighted
        {
            if (list.Count == 0)
            {
                return default(T);
            }

            int totalweight = list.Sum(c => c.Weight);
            Random rand = new Random();
            int choice = rand.Next(totalweight);
            int sum = 0;

            foreach (var obj in list)
            {
                for (int i = sum; i < obj.Weight + sum; i++)
                {
                    if (i >= choice)
                    {
                        return obj;
                    }
                }
                sum += obj.Weight;
            }

            return list.First();
        }
    }
}
