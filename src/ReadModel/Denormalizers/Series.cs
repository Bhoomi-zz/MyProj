using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadModel.Denormalizers
{
    internal static class Series
    {
        public static int AutoGenerateNumber(MyNotesReadModelEntities context, string tableName, string columnName)
        {
            var series = context.SeriesTransactions.SingleOrDefault(x => x.Series.tableName == tableName && x.Series.columnName == columnName);
            bool Isnew = false;
            if (series == null)
            {
                series = new SeriesTransaction();
                var seriesMaster = context.Series.SingleOrDefault(x => x.tableName == tableName && x.columnName == columnName);
                series.seriesId = seriesMaster.seriesId;
                series.seriesTransactionId = Guid.NewGuid();
                series.maxNo = seriesMaster.seed ?? 1;
                Isnew = true;
            }
            int maxNo = 0;
            if (series.seriesId != null)
            {
                series.maxNo += 1;
                maxNo = series.maxNo ??1;                
            }
            else
            {
                maxNo = 1;
            }            
            if (Isnew)
            {
                context.SeriesTransactions.AddObject(series);
            }
            return maxNo;
        }
    }
}
