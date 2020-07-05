/* Jaringan Saraf Tiruan C#
 * Back Propagation
 * ibmgrx.github.io
 * =========*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jaringanSarafTiruan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // fungsi sigmoid
        class sigmoid
        {
            public static double keluaran(double x)
            {
                return 1.0 / (1.0 + Math.Exp(-x));
            }

            public static double turunan(double x)
            {
                return x * (1 - x);
            }
        }

        class Saraf
        {
            public double[] masukan = new double[2];
            public double[] bobot = new double[2];
            public double galat;

            private double bobotBias;

            private Random r = new Random();

            public double keluaran
            {
                get { return sigmoid.keluaran(bobot[0] * masukan[0] + bobot[1] * masukan[1] + bobotBias); }
            }

            public void bobotAcak()
            {
                bobot[0] = r.NextDouble();
                bobot[1] = r.NextDouble();
                bobotBias = r.NextDouble();
            }

            public void sesuaikanBobot()
            {
                bobot[0] += galat * masukan[0];
                bobot[1] += galat * masukan[1];
                bobotBias += galat;
            }
        }


        private void latih()
        {
            int vx1, vx2, vx3, vx4,
                vy1, vy2, vy3, vy4,
                vz1, vz2, vz3, vz4, vL;
            bool x1ok = int.TryParse(x1.Text, out vx1), x2ok = int.TryParse(x2.Text, out vx2), x3ok = int.TryParse(x3.Text, out vx3), x4ok = int.TryParse(x4.Text, out vx4),
                y1ok = int.TryParse(y1.Text, out vy1), y2ok = int.TryParse(y2.Text, out vy2), y3ok = int.TryParse(y3.Text, out vy3), y4ok = int.TryParse(y4.Text, out vy4),
                z1ok = int.TryParse(z1.Text, out vz1), z2ok = int.TryParse(z2.Text, out vz2), z3ok = int.TryParse(z3.Text, out vz3), z4ok = int.TryParse(z4.Text, out vz4),
                setOk = int.TryParse(setLatih.Text, out vL);
                
            if (!x1ok || !x2ok || !x3ok || !x4ok &&
                !y1ok || !y2ok || !y3ok || !y4ok &&
                !z1ok || !z2ok || !z3ok || !z4ok &&
                !setOk)
            {
                MessageBox.Show("Masukan Nilai Terlebih Dahulu");
            }
            else
            {
                // beri nilai masukan
                double[,] masukan = 
                {
                    { vx1, vy1},
                    { vx2, vy2},
                    { vx3, vy3},
                    { vx4, vy4}
                };

                // target keluaran
                double[] target = { vz1, vz2, vz3, vz4 };

                // buat jaringan saraf
                Saraf sarafTersembunyi1 = new Saraf();
                Saraf sarafTersembunyi2 = new Saraf();
                Saraf sarafKeluaran = new Saraf();

                // bobot acak
                sarafTersembunyi1.bobotAcak();
                sarafTersembunyi2.bobotAcak();
                sarafKeluaran.bobotAcak();
            
                int dataLatih = 0;

                Ulangi:
                    dataLatih++;
                    for (int i = 0; i < 4; i++)  // latih setiap persamaan
                    {
                        // 1. Propagasi maju (hitung keluaran)
                        sarafTersembunyi1.masukan = new double[] { masukan[i, 0], masukan[i, 1] };
                        sarafTersembunyi2.masukan = new double[] { masukan[i, 0], masukan[i, 1] };

                        sarafKeluaran.masukan = new double[] { sarafTersembunyi1.keluaran, sarafTersembunyi2.keluaran };

                        // 2. Propagasi balik (sesuaikan bobot)

                        // sesuaikan bobot keluaran berdasarkan nilai kesalahan
                        sarafKeluaran.galat = sigmoid.turunan(sarafKeluaran.keluaran) * (target[i] - sarafKeluaran.keluaran);
                        sarafKeluaran.sesuaikanBobot();

                        // sesuaikan bobot saraf berdasarkan nilai kesalahan
                        sarafTersembunyi1.galat = sigmoid.turunan(sarafTersembunyi1.keluaran) * sarafKeluaran.galat * sarafKeluaran.bobot[0];
                        sarafTersembunyi2.galat = sigmoid.turunan(sarafTersembunyi2.keluaran) * sarafKeluaran.galat * sarafKeluaran.bobot[1];

                        sarafTersembunyi1.sesuaikanBobot();
                        sarafTersembunyi2.sesuaikanBobot();

                        // tampilkan hasil
                        tampilHasil.AppendText("x: " + masukan[i, 0] + "   y: " + masukan[i, 1] + "   hasil: " + sarafKeluaran.keluaran + "\n");
                    }
                    progressBar1.Value = dataLatih * progressBar1.Maximum / vL;
                    if (dataLatih < vL)
                    {
                        goto Ulangi;
                    }
            }
        }

        private void tombolLatih_Click(object sender, EventArgs e)
        {
            latih();
        }

        private void tombolHapus_Click(object sender, EventArgs e)
        {
            tampilHasil.Text = String.Empty;
        }
    }
}
