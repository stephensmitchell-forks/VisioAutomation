﻿using System.Collections.Generic;
using System.Data;
using VisioAutomation.ShapeSheet.Query;

namespace VisioPowerShell.Models
{
    static class DataTableHelpers
    {
        private static DataTable querytable_to_datatable<T>(CellQuery cell_query, CellQueryOutputList<T> query_output)
        {
            // First Construct a Datatable with a compatible schema
            var dt = new DataTable();
            dt.Columns.Add("ShapeID", typeof(int));
            foreach (var col in cell_query.Columns)
            {
                dt.Columns.Add(col.Name, typeof(T));
            }

            // Then populate the rows of the datatable
            dt.BeginLoadData();
            int colcount = cell_query.Columns.Count;
            var rowbuf = new object[colcount+1];
            for (int r = 0; r < query_output.Count; r++)
            {
                // populate the row buffer

                rowbuf[0] = query_output[r].ShapeID;

                for (int i = 0; i < colcount; i++)
                {
                    rowbuf[i+1] = query_output[r].Cells[i];
                }

                // load it into the table
                dt.Rows.Add(rowbuf);
            }
            dt.EndLoadData();
            return dt;
        }

        public static DataTable QueryToDataTable(CellQuery cell_query, bool getresults, ResultType result_type, IList<int> shapeids, VisioAutomation.SurfaceTarget surface)
        {
            if (!getresults)
            {
                var output = cell_query.GetFormulas(surface, shapeids);
                return DataTableHelpers.querytable_to_datatable(cell_query, output);
            }

            switch (result_type)
            {
                case ResultType.String:
                {
                    var output = cell_query.GetResults<string>(surface, shapeids);
                    return DataTableHelpers.querytable_to_datatable(cell_query, output);
                }
                case ResultType.Boolean:
                {
                    var output = cell_query.GetResults<bool>(surface, shapeids);
                    return DataTableHelpers.querytable_to_datatable(cell_query, output);
                }
                case ResultType.Double:
                {
                    var output = cell_query.GetResults<double>(surface, shapeids);
                    return DataTableHelpers.querytable_to_datatable(cell_query, output);
                }
                case ResultType.Integer:
                {
                    var output = cell_query.GetResults<int>(surface, shapeids);
                    return DataTableHelpers.querytable_to_datatable(cell_query, output);
                }
            }

            throw new System.ArgumentOutOfRangeException("Unsupported Result type");
        }
    }
}
