﻿using System;
using System.Collections.Generic;
using System.Linq;
using SMA = System.Management.Automation;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioPowerShell.Commands
{
    [SMA.Cmdlet(SMA.VerbsCommon.Get, VisioPowerShell.Commands.Nouns.VisioPageCells)]
    public class GetVisioPageCells: VisioCmdlet
    {
        [SMA.Parameter(Mandatory = false)]
        public IVisio.Page[] Pages { get; set; }

        [SMA.Parameter(Mandatory = false)]
        public VisioPowerShell.Models.CellOutputType OutputType = VisioPowerShell.Models.CellOutputType.Formula;

        protected override void ProcessRecord()
        {
            var target_pages = this.Pages ?? new[] { this.Client.Page.Get() };

            if (target_pages.Length < 1)
            {
                return;
            }

            var template = new VisioPowerShell.Models.PageCells();
            var celldic = VisioPowerShell.Models.NamedCellDictionary.FromCells(template);
            var cellnames = celldic.Keys.ToArray();
            var query = _CreateQuery(celldic, cellnames);
            var surface = this.Client.ShapeSheet.GetShapeSheetSurface();

            var result_dt = new System.Data.DataTable();

            foreach (var target_page in target_pages)
            {
                var target_pagesheet = target_page.PageSheet;
                var target_shapeids = new List<int> { target_pagesheet.ID };
                var dt = VisioPowerShell.Models.DataTableHelpers.QueryToDataTable(query, this.OutputType, target_shapeids, surface);
                result_dt.Merge(dt);
            }

            // Annotate the returned datatable to disambiguate rows
            var pageindex_col = result_dt.Columns.Add("PageIndex", typeof(System.Int32));
            pageindex_col.SetOrdinal(0);
            for (int row_index = 0; row_index < target_pages.Length; row_index++)
            {
                result_dt.Rows[row_index][pageindex_col.ColumnName] = target_pages[row_index].Index;
            }

            this.WriteObject(result_dt);
        }

        private VisioAutomation.ShapeSheet.Query.CellQuery _CreateQuery(
            VisioPowerShell.Models.NamedCellDictionary celldic,
            IList<string> cellnames)
        {
            var invalid_names = cellnames.Where(cellname => !celldic.ContainsKey(cellname)).ToList();

            if (invalid_names.Count > 0)
            {
                string msg = "Invalid cell names: " + string.Join(",", invalid_names);
                throw new ArgumentException(msg);
            }

            var query = new VisioAutomation.ShapeSheet.Query.CellQuery();

            foreach (string cell in cellnames)
            {
                foreach (var resolved_cellname in celldic.ExpandKeyWildcard(cell))
                {
                    if (!query.Columns.Contains(resolved_cellname))
                    {
                        var resolved_src = celldic[resolved_cellname];
                        query.Columns.Add(resolved_src, resolved_cellname);
                    }
                }
            }

            return query;
        }
    }

}