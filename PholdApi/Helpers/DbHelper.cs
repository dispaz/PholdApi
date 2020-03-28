using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Helpers
{
    public class DbHelper
    {
        public static DataTable BuildPholdPhotoInfoType(List<Tuple<string, string>> photoInfo)
        {
            DataTable sqlQuestionIds = null;
            if (photoInfo != null)
            {
                sqlQuestionIds = new DataTable("PholdPhotoInfoType");
                sqlQuestionIds.Columns.Add("Filename", typeof(string));
                sqlQuestionIds.Columns.Add("Years", typeof(string));
                foreach (var item in photoInfo)
                {
                    sqlQuestionIds.Rows.Add(item.Item1, item.Item2);
                }
            }

            return sqlQuestionIds;
        }
    }
}
