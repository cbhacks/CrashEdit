using Crash;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class InterpolatorForm : Form
    {
        public static Dictionary<string,MathCalc> MathFuncs = new Dictionary<string, MathCalc>()
        {
            { "Linear", MathFunctionLinear },
            { "Inverse Linear", MathFunctionInverseLinear },
            { "Quadratic", MathFunctionDouble },
            { "Inverse Quadratic", MathFunctionInverseDouble }
        };

        private List<Position> positions;
        private int positionindex;

        public InterpolatorForm(ICollection<Position> positions)
        {
            if (positions.Count < 2)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
            this.positions = new List<Position>(positions);
            NewPositions = new Position [0];

            InitializeComponent();

            foreach (string name in MathFuncs.Keys)
                dpdFunc.Items.Add(name);

            dpdFunc.SelectedIndex = 0;
            numAmount.Maximum = short.MaxValue - positions.Count;
            numEnd.Maximum = positions.Count;
            numEnd_ValueChanged(null,null);
            UpdatePosition();

            Text = Properties.Resources.InterpolatorForm;
            cmdCancel.Text = Properties.Resources.InterpolatorForm_cmdCancel;
            cmdFirst.Text = Properties.Resources.InterpolatorForm_cmdFirst;
            cmdLast.Text = Properties.Resources.InterpolatorForm_cmdLast;
            cmdNext.Text = Properties.Resources.InterpolatorForm_cmdNext;
            cmdOK.Text = Properties.Resources.InterpolatorForm_cmdOK;
            cmdPrev.Text = Properties.Resources.InterpolatorForm_cmdPrev;
            fraAmount.Text = Properties.Resources.InterpolatorForm_fraAmount;
            fraBound.Text = Properties.Resources.InterpolatorForm_fraBound;
            fraFunction.Text = Properties.Resources.InterpolatorForm_fraFunction;
            fraOrder.Text = Properties.Resources.InterpolatorForm_fraOrder;
            fraPosition.Text = Properties.Resources.InterpolatorForm_fraPosition;
            fraTension.Text = Properties.Resources.InterpolatorForm_fraTension;
        }
        
        private double Tension => (double)numTension.Value;

        public Position[] NewPositions { get; private set; }
        public int Start => (int)numStart.Value;
        public int End => (int)numEnd.Value;
        public int Amount => (int)numAmount.Value;
        public string Func => (string)dpdFunc.SelectedItem;
        public double Order => (double)numOrder.Value;

        private void cmdOK_Click(object sender, EventArgs e)
        {
            CalcInterp();
            DialogResult = DialogResult.OK;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void UpdatePosition()
        {
            lblPosition.Text = $"{positionindex+1} / {positions.Count}";
            numX.Value = (decimal)positions[positionindex].X;
            numY.Value = (decimal)positions[positionindex].Y;
            numZ.Value = (decimal)positions[positionindex].Z;
            cmdPrev.Enabled = positionindex > 0;
            cmdNext.Enabled = positionindex < positions.Count-1;
        }

        private void cmdPrev_Click(object sender, EventArgs e)
        {
            --positionindex;
            UpdatePosition();
        }

        private void cmdNext_Click(object sender, EventArgs e)
        {
            ++positionindex;
            UpdatePosition();
        }

        private void cmdFirst_Click(object sender, EventArgs e)
        {
            positionindex = 0;
            UpdatePosition();
        }

        private void cmdLast_Click(object sender, EventArgs e)
        {
            positionindex = positions.Count - 1;
            UpdatePosition();
        }

        private void numEnd_ValueChanged(object sender, EventArgs e)
        {
            numStart.Value = Math.Min(numEnd.Value - 1, numStart.Value);
            numStart.Maximum = numEnd.Value - 1;
            CalcInterp();
        }

        private void numAmount_ValueChanged(object sender, EventArgs e)
        {
            CalcInterp();
        }

        private void CalcInterp()
        {
            if (End - Start == 1)
            {
                Position start = positions[Start - 1];
                Position end = positions[End - 1];
                Position delta = end - start;
                NewPositions = new Position[Amount + 2];
                NewPositions[0] = start;
                NewPositions[NewPositions.Length - 1] = end;
                for (int i = 1, s = NewPositions.Length-1; i < s + 1; ++i)
                {
                    NewPositions[i] = delta * MathFuncs[Func].Invoke((double)i/s,Order) + start;
                }
                delta /= Amount+1;
                lblAverage.Text = $"Average Point Distance: {(int)Math.Sqrt(delta.X*delta.X+delta.Y*delta.Y+delta.Z*delta.Z)}";
            }
            else
            {
                List<Position> subpositions = positions.GetRange(Start - 1, End - Start + 1);
                double[] weights = new double[subpositions.Count];
                weights[0] = weights[weights.Length - 1] = 1;
                for (int i = 1; i < weights.Length - 1; ++i)
                    weights[i] = Tension;
                Position start = positions[Start - 1];
                Position end = positions[End - 1];
                NewPositions = new Position[Amount + 2];
                NewPositions[0] = start;
                NewPositions[NewPositions.Length - 1] = end;
                Position[] oldpositions = new Position[NewPositions.Length * 2];
                oldpositions[0] = start;
                oldpositions[oldpositions.Length - 1] = end;
                double[] arclen = new double[oldpositions.Length];
                arclen[0] = 0;
                double dist = 0;
                Position distpos;
                for (int i = 1, s = oldpositions.Length-1; i < s + 1; ++i)
                {
                    oldpositions[i] = GetBezierPoint(subpositions, weights, MathFuncs[Func].Invoke((double)i/s,Order));
                    distpos = oldpositions[i] - oldpositions[i - 1];
                    dist += Math.Sqrt(distpos.X * distpos.X + distpos.Y * distpos.Y + distpos.Z * distpos.Z);
                    arclen[i] = dist;
                }
                dist /= NewPositions.Length - 1;
                lblAverage.Text = $"Average Point Distance: {(int)dist}";
                // recalculate points for equidistance
                for (int i = 1, s = NewPositions.Length-1; i < s; ++i)
                {
                    NewPositions[i] = FindPointByDistance(oldpositions, arclen, MathFuncs[Func].Invoke((double)i/s,Order));
                }
            }
        }

        private static Position FindPointByDistance(Position[] positions, double[] arclen, double t)
        {
            double targetlen = t * arclen[arclen.Length-1];
            for (int i = 0; i < arclen.Length; ++i)
            {
                if (targetlen == arclen[i])
                    return positions[i];
                else if (targetlen < arclen[i])
                    return positions[i-1] + (positions[i] - positions[i-1]) * ((targetlen - arclen[i-1]) / (arclen[i] - arclen[i-1]));
            }
            return positions[positions.Length - 1];
        }

        private static List<long[]> Binomials = new List<long[]>();
        private static long GetBinomial(int n, int o)
        {
            while (n >= Binomials.Count)
            {
                int m = Binomials.Count;
                long[] binomial = new long[m+1];
                binomial[0] = 1;
                for (int i = 1; i < m; ++i)
                {
                    binomial[i] = Binomials[m-1][i-1] + Binomials[m-1][i];
                }
                binomial[m] = 1;
                Binomials.Add(binomial);
            }
            return Binomials[n][o];
        }

        private static Position GetBezierBasisPoint(int controlcount, double[] weights, double t)
        {
            Position newpos = new Position(0, 0, 0);
            int n = controlcount-1;
            for (int i = 0; i < controlcount; ++i)
            {
                newpos += GetBinomial(n,i) * Math.Pow(1.0-t,n-i) * Math.Pow(t,i) * weights[i] * Position.Unit;
            }
            return newpos;
        }

        private static Position GetBezierPoint(IList<Position> control, double[] weights, double t)
        {
            Position newpos = new Position(0, 0, 0);
            int n = control.Count-1;
            for (int i = 0; i < control.Count; ++i)
            {
                newpos += GetBinomial(n,i) * Math.Pow(1.0-t,n-i) * Math.Pow(t,i) * weights[i] * control[i] / GetBezierBasisPoint(control.Count, weights, t);
            }
            return newpos;
        }

        public delegate double MathCalc(double x,double o);

        private static double MathFunctionLinear(double x,double o)
        {
            return Math.Pow(x,o);
        }

        private static double MathFunctionInverseLinear(double x,double o)
        {
            return 1 - MathFunctionLinear(-x+1,o);
        }

        internal static double MathFuncQuadrPolinomial1(double x,double o)
        {
            return Math.Min(Math.Pow(2*Math.Max(x,0),o)/2,0.5);
        }

        internal static double MathFuncQuadrPolinomial2(double x,double o)
        {
            return 0.5 - MathFuncQuadrPolinomial1(1 - x,o);
        }

        private static double MathFunctionDouble(double x,double o)
        {
            return MathFuncQuadrPolinomial1(x,o) + MathFuncQuadrPolinomial2(x,o);
        }

        private static double MathFunctionInverseDouble(double x,double o)
        {
            return MathFuncQuadrPolinomial1(x-0.5,o) + MathFuncQuadrPolinomial2(x+0.5,o);
        }

        private void numStart_ValueChanged(object sender, EventArgs e)
        {
            CalcInterp();
        }

        private void numTension_ValueChanged(object sender, EventArgs e)
        {
            CalcInterp();
        }

        private void numOrder_ValueChanged(object sender, EventArgs e)
        {
            CalcInterp();
        }
    }
}
