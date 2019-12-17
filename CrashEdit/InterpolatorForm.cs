using Crash;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class InterpolatorForm : Form
    {
        private const string FuncLinear = "Linear";
        private const string FuncSquare = "Square";
        private const string FuncCubic = "Cubic";
        private const string FuncSine = "Sine";
        private const string FuncSquareInverse = "Inverse Square";
        private const string FuncCubicInverse = "Inverse Cubic";
        private const string FuncTangent = "Tangent";
        private const string FuncSquareDouble = "Double Square";

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

            dpdFunc.Items.Add(FuncLinear);
            dpdFunc.Items.Add(FuncSquare);
            dpdFunc.Items.Add(FuncCubic);
            dpdFunc.Items.Add(FuncSine);
            dpdFunc.Items.Add(FuncSquareInverse);
            dpdFunc.Items.Add(FuncCubicInverse);
            dpdFunc.Items.Add(FuncTangent);
            dpdFunc.Items.Add(FuncSquareDouble);

            dpdFunc.SelectedIndex = 0;
            numAmount.Maximum = short.MaxValue - positions.Count;
            numEnd.Maximum = positions.Count;
            numStart.Maximum = numEnd.Maximum - 1;
            UpdatePosition();
            numTension.Value = 2;
        }
        
        private double Tension => (double)numTension.Value;

        public Position[] NewPositions { get; private set; }
        public int Start => (int)numStart.Value;
        public int End => (int)numEnd.Value;
        public int Amount => (int)numAmount.Value;

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
            List<Position> subpositions = positions.GetRange(Start-1, End-Start+1);
            double[] weights = new double[subpositions.Count];
            weights[0] = weights[weights.Length-1] = 1;
            for (int i = 1; i < weights.Length-1; ++i)
                weights[i] = Tension;
            Position start = positions[Start - 1];
            Position end = positions[End - 1];
            NewPositions = new Position[Amount + 2];
            NewPositions[0] = start;
            NewPositions[NewPositions.Length-1] = end;
            Position[] oldpositions = new Position[NewPositions.Length];
            oldpositions[0] = start;
            oldpositions[oldpositions.Length-1] = end;
            double[] arclen = new double[NewPositions.Length];
            arclen[0] = 0;
            double dist = 0;
            Position distpos;
            for (int i = 1, s = oldpositions.Length-1; i < s; ++i)
            {
                double fac = (double)i / s;
                oldpositions[i] = GetBezierPoint(subpositions, weights, fac);
                distpos = oldpositions[i] - oldpositions[i-1];
                dist += Math.Sqrt(distpos.X*distpos.X + distpos.Y*distpos.Y + distpos.Z*distpos.Z);
                arclen[i] = dist;
            }
            distpos = end - oldpositions[oldpositions.Length-2];
            dist += Math.Sqrt(distpos.X*distpos.X + distpos.Y*distpos.Y + distpos.Z*distpos.Z);
            arclen[arclen.Length-1] = dist;
            dist /= oldpositions.Length-1;
            lblAverage.Text = $"Average Point Distance: {(int)dist}";
            // recalculate points for quick equidistance?
            for (int i = 1, s = NewPositions.Length - 1; i < s; ++i)
            {
                double fac = (double)i / s;
                switch (dpdFunc.SelectedItem)
                {
                    //case FuncLinear: fac = MathFunctionLinear(fac); break;
                    case FuncSquare: fac = MathFunctionSquare(fac); break;
                    case FuncCubic: fac = MathFunctionCubic(fac); break;
                    case FuncSine: fac = MathFunctionSine(fac); break;
                    case FuncTangent: fac = MathFunctionTangent(fac); break;
                    case FuncSquareInverse: fac = MathFunctionSquareInverse(fac); break;
                    case FuncCubicInverse: fac = MathFunctionCubicInverse(fac); break;
                    case FuncSquareDouble: fac = MathFunctionSquareDouble(fac); break;
                }
                NewPositions[i] = FindPointByDistance(oldpositions, arclen, fac);
            }
        }

        private Position FindPointByDistance(Position[] positions, double[] arclen, double t)
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

        private static double MathFunctionLinear(double x)
        {
            return x;
        }

        private static double MathFunctionSquare(double x)
        {
            return x*x;
        }

        private static double MathFunctionCubic(double x)
        {
            return x*x*x;
        }

        private static double MathFunctionSine(double x)
        {
            return Math.Sin((x-0.5)*Math.PI) / 2 + 0.5;
        }

        private static double MathFunctionTangent(double x)
        {
            return Math.Tan((x-0.5)*Math.PI / 2) / 2 + 0.5;
        }

        private static double MathFunctionSquareDouble(double x)
        {
            return Math.Pow(Math.Min(2*x,1),2)/2 + Math.Max(0,-Math.Pow(2*Math.Min(x-1,0),2)/2+0.5);
        }

        private static double MathFunctionSquareInverse(double x)
        {
            return Math.Sqrt(x);
        }

        private static double MathFunctionCubicInverse(double x)
        {
            return Math.Pow(x,1.0/3.0);
        }

        private void numStart_ValueChanged(object sender, EventArgs e)
        {
            CalcInterp();
        }

        private void numTension_ValueChanged(object sender, EventArgs e)
        {
            CalcInterp();
        }
    }
}
