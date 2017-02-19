﻿using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioAutomation.ShapeSheet
{
    public struct ShapeSheetSurface
    {
        public readonly SurfaceTarget Target;

        public ShapeSheetSurface(SurfaceTarget target)
        {
            this.Target = target;
        }

        public ShapeSheetSurface(IVisio.Page page)
        {
            this.Target = new SurfaceTarget(page);
        }

        public ShapeSheetSurface(IVisio.Master master)
        {
            this.Target = new SurfaceTarget(master);
        }

        public ShapeSheetSurface(IVisio.Shape shape)
        {
            this.Target = new SurfaceTarget(shape);
        }

        public int SetFormulas(short[] stream, object[] formulas, short flags)
        {
            if (this.Target.Shape != null)
            {
                return this.Target.Shape.SetFormulas(stream, formulas, flags);
            }
            else if (this.Target.Master != null)
            {
                return this.Target.Master.SetFormulas(stream, formulas, flags);
            }
            else if (this.Target.Page != null)
            {
                return this.Target.Page.SetFormulas(stream, formulas, flags);
            }

            throw new System.ArgumentException("Unhandled Target");
        }

        public int SetResults(short[] stream, object[] unitcodes, object[] results, short flags)
        {

            if (this.Target.Shape != null)
            {
                return this.Target.Shape.SetResults(stream, unitcodes, results, flags);
            }
            else if (this.Target.Master != null)
            {
                return this.Target.Master.SetResults(stream, unitcodes, results, flags);
            }
            else if (this.Target.Page != null)
            {
                return this.Target.Page.SetResults(stream, unitcodes, results, flags);
            }

            throw new System.ArgumentException("Unhandled Target");
        }

        public TResult[] GetResults<TResult>(short[] stream, object[] unitcodes)
        {
            if (stream.Length == 0)
            {
                return new TResult[0];
            }

            EnforceValidResultType(typeof(TResult));

            var flags = TypeToVisGetSetArgs(typeof(TResult));

            System.Array results_sa = null;

            if (this.Target.Master != null)
            {
                this.Target.Master.GetResults(stream, (short)flags, unitcodes, out results_sa);
            }
            else if (this.Target.Page != null)
            {
                this.Target.Page.GetResults(stream, (short)flags, unitcodes, out results_sa);
            }
            else if (this.Target.Shape != null)
            {
                this.Target.Shape.GetResults(stream, (short)flags, unitcodes, out results_sa);
            }
            else
            {
                throw new System.ArgumentException("Unhandled Target");
            }

            var results = new TResult[results_sa.Length];
            results_sa.CopyTo(results, 0);
            return results;
        }

        public string[] GetFormulasU(short[] stream)
        {
            if (stream.Length==0)
            {
                return new string[0];
            }

            System.Array formulas_sa = null;

            if (this.Target.Master != null)
            {
                this.Target.Master.GetFormulasU(stream, out formulas_sa);
            }
            else if (this.Target.Page != null)
            {
                this.Target.Page.GetFormulasU(stream, out formulas_sa);
            }
            else if (this.Target.Shape != null)
            {
                this.Target.Shape.GetFormulasU(stream, out formulas_sa);
            }
            else
            {
                throw new System.ArgumentException("Unhandled Drawing Surface");
            }

            object[] formulas_obj_array = (object[])formulas_sa;

            string[] formulas = new string[formulas_obj_array.Length];
            formulas_obj_array.CopyTo(formulas, 0);
            return formulas;
        }

        private static void EnforceValidResultType(System.Type result_type)
        {
            if (!IsValidResultType(result_type))
            {
                string msg = string.Format("Unsupported Result Type: {0}", result_type.Name);
                throw new VisioAutomation.Exceptions.InternalAssertionException(msg);
            }
        }

        private static bool IsValidResultType(System.Type result_type)
        {
            return (result_type == typeof(int)
                    || result_type == typeof(double)
                    || result_type == typeof(string));
        }

        private static IVisio.VisGetSetArgs TypeToVisGetSetArgs(System.Type type)
        {
            IVisio.VisGetSetArgs flags;
            if (type == typeof(int))
            {
                flags = IVisio.VisGetSetArgs.visGetTruncatedInts;
            }
            else if (type == typeof(double))
            {
                flags = IVisio.VisGetSetArgs.visGetFloats;
            }
            else if (type == typeof(string))
            {
                flags = IVisio.VisGetSetArgs.visGetStrings;
            }
            else
            {
                string msg = string.Format("Unsupported Result Type: {0}", type.Name);
                throw new VisioAutomation.Exceptions.InternalAssertionException(msg);
            }
            return flags;
        }
    }
}