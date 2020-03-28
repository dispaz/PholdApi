using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Services.Sql
{
    public static class SqlSp
    {
        public static string CreateOrUpdatePhold => "dbo.spCreateOrUpdatePholdObjects";
    }
}
