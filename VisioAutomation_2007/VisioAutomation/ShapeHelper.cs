using System.Collections.Generic;
using VisioAutomation.Extensions;
using IVisio = Microsoft.Office.Interop.Visio;
using VA=VisioAutomation;

namespace VisioAutomation
{
    public enum GroupSelectMode
    {
        GroupFirst = IVisio.VisCellVals.visGrpSelModeGroup1st,
        GroupOnly = IVisio.VisCellVals.visGrpSelModeGroupOnly,
        MembersFirst = IVisio.VisCellVals.visGrpSelModeMembers1st
    }

    public static class ShapeHelper
    {
        /// <summary>
        /// Enumerates all shapes contained by a set of shapes recursively
        /// </summary>
        /// <param name="shapes">the set of shapes to start the enumeration</param>
        /// <returns>The enumeration</returns>
        public static IList<IVisio.Shape> GetNestedShapes(IEnumerable<IVisio.Shape> shapes)
        {
            if (shapes == null)
            {
                throw new System.ArgumentNullException("shapes");
            }

            var result = new List<IVisio.Shape>();
            var stack = new Stack<IVisio.Shape>(shapes);

            while (stack.Count > 0)
            {
                var s = stack.Pop();
                var subshapes = s.Shapes;
                if (subshapes.Count > 0)
                {
                    foreach (var child in subshapes.AsEnumerable())
                    {
                        stack.Push(child);
                    }
                }

                result.Add(s);
            }

            return result;
        }

        public static IList<IVisio.Shape> GetNestedShapes(IVisio.Shape shape)
        {
            if (shape== null)
            {
                throw new System.ArgumentNullException("shape");
            }

            var shapes = new[] {shape};

            return GetNestedShapes(shapes);
        }

        public static IList<IVisio.Shape> GetShapesFromIDs(IVisio.Shapes shapes, IList<short> shapeids)
        {
            var shape_objs = new List<IVisio.Shape>(shapeids.Count);
            foreach (short shapeid in shapeids)
            {
                var shape = shapes.ItemFromID16[shapeid];
                shape_objs.Add(shape);
            }
            return shape_objs;
        }

        public static void SetGroupSelectMode(IVisio.Shape shape, IVisio.VisCellVals mode)
        {
            var src_selectmode = VA.ShapeSheet.SRCConstants.SelectMode;
            var select_mode_cell = shape.CellsSRC[src_selectmode.Section, src_selectmode.Row, src_selectmode.Cell];
            select_mode_cell.FormulaU = ((int)mode).ToString();
        }

        public static void SetGroupSelectMode(IVisio.Shape shape, GroupSelectMode mode)
        {
            var src_selectmode = VA.ShapeSheet.SRCConstants.SelectMode;
            var select_mode_cell = shape.CellsSRC[src_selectmode.Section, src_selectmode.Row, src_selectmode.Cell];
            select_mode_cell.FormulaU = ((int)mode).ToString();
        }
    }
}