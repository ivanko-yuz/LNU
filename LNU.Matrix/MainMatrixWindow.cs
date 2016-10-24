using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LNU.Matrix
{
    public partial class MainMatrixWindow : Form
    {
        List<double[,]> Ke;
        List<double[,]> Me;

        public MainMatrixWindow()
        {
            InitializeComponent();
            Ke = new List<double[,]>();
            Me = new List<double[,]>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataStorage storage = new DataStorage();
            List<double> squerArea = new List<double>();
            List<EquationStorage> equationStorage = new List<EquationStorage>();


            foreach (var item in storage.NT)
            {
                squerArea.Add(HeronArea.getArea(item[0], item[1], item[2]));
                equationStorage.Add(MatrixKeConstants.getConstants(item[0], item[1], item[2]));
            }

            for (int i = 0; i < equationStorage.Count; i++)
            {
                Ke.Add(new double[3, 3]
                {
                    {
                        (Math.Pow(equationStorage[i].X.B,2)+ Math.Pow(equationStorage[i].X.C,2)),
                        (equationStorage[i].X.B*equationStorage[i].Y.B + equationStorage[i].X.C*equationStorage[i].Y.C),
                        (equationStorage[i].X.B*equationStorage[i].Z.B + equationStorage[i].X.C*equationStorage[i].Z.C)

                    },
                       {(equationStorage[i].X.B*equationStorage[i].Y.B + equationStorage[i].X.C*equationStorage[i].Y.C),
                        Math.Pow(equationStorage[i].Y.B,2)+ Math.Pow(equationStorage[i].Y.C,2),
                        (equationStorage[i].Y.B*equationStorage[i].Z.B + equationStorage[i].Y.C*equationStorage[i].Z.C)
                    },
                    {
                        (equationStorage[i].X.B*equationStorage[i].Z.B + equationStorage[i].X.C*equationStorage[i].Z.C),
                        (equationStorage[i].Y.B*equationStorage[i].Z.B + equationStorage[i].Y.C*equationStorage[i].Z.C) ,
                        (Math.Pow(equationStorage[i].Z.B,2)+ Math.Pow(equationStorage[i].Z.C,2))
                    }
                });
            }

            double constant = 0;
            for (int i = 0; i < squerArea.Count; i++)
            {
                constant = squerArea[i] / 24;
                Me.Add(new double[3, 3] {
                    {
                        2 * constant, 1 * constant, 1 * constant
                    },
                    {
                        1 * constant, 2 * constant, 1 * constant
                    },
                    {
                        1 * constant, 1 * constant, 2 * constant
                    }
                });

            }

        }
    }
}
