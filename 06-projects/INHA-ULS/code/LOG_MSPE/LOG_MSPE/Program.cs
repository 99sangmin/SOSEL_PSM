using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace MSPEC
{
    class Program
    {
        static void Main(string[] args1)
        {
            string[] args = new string[3];
            args[0] = "D:\\001Works\\001Research\\002Project\\【SOSEL】\\20251115-yaoqiu\\LTs.job";
            args[1] = "D:\\001Works\\001Research\\002Project\\【SOSEL】\\20251115-yaoqiu\\RAO3";
            args[2] = "D:\\001Works\\001Research\\002Project\\【SOSEL】\\20251115-yaoqiu\\MOM2";
            
            string path_main = "";
            string path_inf = "";
            string dir = "";
            int heading_n = 0;
            string ws = "";
            string wsd_p = "";
            int wsd_n = 0;
            int ws_ud_n = 0;
            double sprea_factor = 0;
            string s_p = "";

            path_main = args[0];
            path_inf = args[1];
            dir = args[2];

            s_p = "T";

            DirectoryInfo path_o = new DirectoryInfo(dir);
            if (path_o.Exists == false)
            {
                path_o.Create();
            }

            FileStream es = new FileStream(path_main, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader er = new StreamReader(es);

            string Line_h;

            while (er.Peek() != -1)
            {
                Line_h = er.ReadLine();
                if (Line_h.Trim().StartsWith("*DIR"))
                {
                    Line_h = er.ReadLine();
                    if (Line_h.Trim().StartsWith("*"))
                    {
                        Line_h = er.ReadLine();
                    }
                    while (Line_h.Trim().StartsWith("*") == false)
                    {
                        Line_h = er.ReadLine();
                        heading_n = heading_n + 1;
                    }
                }
                if (Line_h.Trim().StartsWith("*WS,"))
                {
                    string[] w_s = Line_h.Split('=');
                    ws = w_s[1];
                    if (string.Compare("UD", ws) == 0)
                    {
                        Line_h = er.ReadLine();
                        while (Line_h.Trim().StartsWith("*") == true)
                        {
                            Line_h = er.ReadLine();
                        }
                        while (Line_h.Trim().StartsWith("*") == false)
                        {
                            Line_h = er.ReadLine();
                            ws_ud_n = ws_ud_n + 1;
                        }
                    }
                }
                if (Line_h.Trim().StartsWith("*WSD,"))
                {
                    string[] wad_s = Line_h.Split('=');
                    wsd_p = wad_s[1];
                    Line_h = er.ReadLine();
                    while (Line_h.Trim().StartsWith("*") == true)
                    {
                        Line_h = er.ReadLine();
                    }
                    while (Line_h.Trim().StartsWith("*") == false)
                    {
                        Line_h = er.ReadLine();
                        wsd_n = wsd_n + 1;
                    }
                }
                if (Line_h.Trim().StartsWith("*SCE"))
                {
                    Line_h = er.ReadLine();
                    while (Line_h.Trim().StartsWith("*") == true)
                    {
                        Line_h = er.ReadLine();
                    }
                    sprea_factor = Convert.ToDouble(Line_h);
                }
            }
            er.Close();
            es.Close();

            string[,] DIRE = new string[heading_n, 3];
            double[,] UD = new double[ws_ud_n, 2];
            double[,] WSD = new double[wsd_n, 4];

            FileStream LS_arr = new FileStream(path_main, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader LR_arr = new StreamReader(LS_arr);

            string Line_arr;
            int s = 0;
            int s_n = 0;
            double wsd_prop=0;

            while (LR_arr.Peek() != -1)
            {
                int d = 0;
                Line_arr = LR_arr.ReadLine();
                if (Line_arr.Trim().StartsWith("*DIR"))
                {
                    Line_arr = LR_arr.ReadLine();
                    while (Line_arr.Trim().StartsWith("*") == true)
                    {
                        Line_arr = LR_arr.ReadLine();
                    }
                    while (Line_arr.Trim().StartsWith("*") == false)
                    {
                        string[] r_d = Line_arr.Split(',');
                        DIRE[d, 0] = Convert.ToString(d + 1);
                        DIRE[d, 1] = r_d[0];
                        DIRE[d, 2] = r_d[1];
                        d = d + 1;
                        Line_arr = LR_arr.ReadLine();
                    }
                }
                if (Line_arr.Trim().StartsWith("*WSD,"))
                {
                    Line_arr = LR_arr.ReadLine();
                    while (Line_arr.Trim().StartsWith("*") == true)
                    {
                        Line_arr = LR_arr.ReadLine();
                    }
                    while (Line_arr.Trim().StartsWith("*") == false)
                    {
                        string[] ss_r = Line_arr.Split(',');
                        if (string.Compare("JS", ws) == 0 && string.Compare("Tz", wsd_p) == 0)
                        {
                            WSD[s, 0] = Convert.ToDouble(ss_r[0]);
                            WSD[s, 1] = Convert.ToDouble(ss_r[1]);
                            WSD[s, 2] = Convert.ToDouble(ss_r[3]);
                            WSD[s, 3] = Convert.ToDouble(ss_r[2]);
                        }
                        else if (string.Compare("PM", ws) == 0 && string.Compare("Tz", wsd_p) == 0)
                        {
                            WSD[s, 0] = Convert.ToDouble(ss_r[0]);
                            WSD[s, 1] = Convert.ToDouble(ss_r[1]);
                            WSD[s, 2] = Convert.ToDouble(ss_r[3]);
                        }
                        else if (string.Compare("JS", ws) == 0 && string.Compare("Tp", wsd_p) == 0)
                        {
                            WSD[s, 0] = Convert.ToDouble(ss_r[0]);
                            WSD[s, 1] = (Convert.ToDouble(ss_r[1]) / (Math.Pow((5 + Convert.ToDouble(ss_r[2])) / (11 + Convert.ToDouble(ss_r[2])), 0.5)));
                            WSD[s, 2] = Convert.ToDouble(ss_r[3]);
                            WSD[s, 3] = Convert.ToDouble(ss_r[2]);

                        }
                        else if (string.Compare("PM", ws) == 0 && string.Compare("Tp", wsd_p) == 0)
                        {
                            WSD[s, 0] = Convert.ToDouble(ss_r[0]);
                            WSD[s, 1] = (Convert.ToDouble(ss_r[1]) / (Math.Pow((5 + Convert.ToDouble(ss_r[2])) / (11 + Convert.ToDouble(ss_r[2])), 0.5)));
                            WSD[s, 2] = Convert.ToDouble(ss_r[3]);
                        }
                        wsd_prop = wsd_prop + WSD[s, 2];
                        s = s + 1;
                        Line_arr = LR_arr.ReadLine();
                    }
                    if (Math.Round(wsd_prop,5) - 1.0 != 0)
                    {
                        Console.WriteLine("   Error!!!: The sum of the probabilities of seastates in JOB file is not equal to 100%");
                        Environment.Exit(0);
                    }
                }
                if (Line_arr.Trim().StartsWith("*WS,"))
                {
                    string[] w_s1 = Line_arr.Split('=');
                    ws = w_s1[1];
                    if (string.Compare("UD", ws) == 0)
                    {
                        Line_arr = LR_arr.ReadLine();
                        while (Line_arr.Trim().StartsWith("*") == true)
                        {
                            Line_arr = LR_arr.ReadLine();
                        }
                        while (Line_arr.Trim().StartsWith("*") == false)
                        {
                            string[] ss_r1 = Line_arr.Split(',');
                            UD[s_n, 0] = Convert.ToDouble(ss_r1[0]);
                            UD[s_n, 1] = Convert.ToDouble(ss_r1[1]);
                            Line_arr = LR_arr.ReadLine();
                            s_n = s_n + 1;
                        }
                    }
                }
            }
            LS_arr.Close();
            LR_arr.Close();

            string file_df = Path.Combine(path_inf + "\\D001" + ".rao");
            int loc_n = 0;
            int f_n = 0;
            double w_int = 0;
            string type = "";
            int molen = 0;

            FileStream ps0 = new FileStream(file_df, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader pr0 = new StreamReader(ps0);

            string Line_1;

            while (pr0.Peek() != -1)
            {
                Line_1 = pr0.ReadLine();
                if (Line_1.Trim().StartsWith("**"))
                {
                    string[] type_f = Line_1.Split(',');
                    molen = type_f.Length;
                    type = type_f[0];
                    Line_1 = pr0.ReadLine();
                    while (Line_1.Trim().StartsWith("*END") == false)
                    {
                        string[] l_f = new string[3];
                        double l_1 = 0;
                        f_n = 0;
                        string st = "";
                        while (l_f[1] != st)
                        {
                            if (f_n == 0)
                            {
                                l_f = Line_1.Split(',');
                                st = l_f[1];
                                l_1 = Convert.ToDouble(st);
                            }
                            else if (f_n == 1)
                            {
                                l_f = Line_1.Split(',');
                                w_int = Convert.ToDouble(l_f[1]) - l_1;
                                loc_n = loc_n + 1;
                            }
                            f_n = f_n + 1;
                            Line_1 = pr0.ReadLine();
                            l_f = Line_1.Split(',');
                            if (l_f[0] == "*END")
                            {
                                break;
                            }
                        }
                    }
                }
            }
            pr0.Close();
            ps0.Close();

            double[,] sea_a = new double[heading_n, 5];
            int jk = 0;

            for (int wsd_r = 0; wsd_r < wsd_n; wsd_r++)
            {
                double[,] Spe_m = new double[heading_n, 6];

                string ha = "";

                if (Convert.ToString(wsd_r + 1).Length == 1)
                {
                    ha = ("SS00" + Convert.ToString(wsd_r + 1));
                }
                else if (Convert.ToString(wsd_r + 1).Length == 2)
                {
                    ha = ("SS0" + Convert.ToString(wsd_r + 1));
                }
                else if (Convert.ToString(wsd_r + 1).Length == 3)
                {
                    ha = ("SS" + Convert.ToString(wsd_r + 1));
                }

                string path_write = Path.Combine(path_o + "\\" + ha + ".sm");

                FileStream ss = new FileStream(path_write, FileMode.Create, FileAccess.Write, FileShare.Write);
                StreamWriter sr = new StreamWriter(ss);

                sr.WriteLine("*WSD\n**       Hs(m),       Tz(sec),         Prob.\n{0,14:0.00},{1,14:0.00},{2,14:0.0000e0}\n", WSD[wsd_r, 0], WSD[wsd_r, 1], WSD[wsd_r, 2]);
                if (molen == 7)
                {
                    sr.WriteLine("*Spectral Moment\n{0},  Heading(deg),  Acceleration,        Motion,     F-K force,    DIFF force, FK+DIFF force", type);
                }
                else if (molen == 4)
                {
                    sr.WriteLine("*Spectral Moment\n{0},  Heading(deg),   Shear force,        Moment", type);
                }
                else
                {
                    sr.WriteLine("*Spectral Moment\n{0},  Heading(deg),        Moment", type);
                }

                for (int loc = 0; loc < loc_n; loc++)
                {
                    string[] L = new string[3];

                    if (molen == 7 || molen == 4)
                    {
                        jk = 7;
                    }
                    else
                    {
                        jk = 3;
                    }

                    for (int vi = 2; vi < jk; vi++)
                    {
                        if (molen == 4 && vi == 4)
                        {
                            break;
                        }

                        for (int dire = 1; dire < heading_n + 1; dire++)
                        {
                            double[] WSD_a = new double[wsd_n];
                            string path_open = "";
                            string na = "";

                            if (Convert.ToString(dire).Length == 1)
                            {
                                na = ("D00" + Convert.ToString(dire));
                            }
                            else if (Convert.ToString(dire).Length == 2)
                            {
                                na = ("D0" + Convert.ToString(dire));
                            }
                            else if (Convert.ToString(dire).Length == 3)
                            {
                                na = ("D" + Convert.ToString(dire));
                            }
                            path_open = Path.Combine(path_inf + "\\" + na + ".rao");

                            FileStream ps = new FileStream(path_open, FileMode.Open, FileAccess.Read, FileShare.Read);
                            StreamReader pr = new StreamReader(ps);

                            string Line_o;
                            int m_r = 0;
                            int l_r = 0;
                            while (pr.Peek() != -1)
                            {
                                Line_o = pr.ReadLine();
                                if (Line_o.Trim().StartsWith("**"))
                                {
                                    Line_o = pr.ReadLine();
                                    while (Line_o.Trim().StartsWith("*END") == false)
                                    {
                                        L = Line_o.Split(',');
                                        if (l_r == loc * f_n)
                                        {
                                            for (int co = 0; co < f_n; co++)
                                            {
                                                string[] Moment = Line_o.Split(',');
                                                if (Moment.Length == 7 || Moment.Length == 4)
                                                {
                                                    if (string.Compare("PM", ws) == 0)
                                                    {
                                                        WSD_a[wsd_r] = WSD_a[wsd_r] + (Math.Pow(Convert.ToDouble(Moment[vi]), 2) * (((4 * (Math.Pow(WSD[wsd_r, 0], 2)) * (Math.Pow(Math.PI, 3))) / ((Math.Pow(WSD[wsd_r, 1], 4)) * (Math.Pow(Convert.ToDouble(Moment[1]), 5)))) * Math.Exp((-16 * (Math.Pow(Math.PI, 3))) / ((Math.Pow(WSD[wsd_r, 1], 4)) * (Math.Pow(Convert.ToDouble(Moment[1]), 4)))))) * w_int;
                                                    }
                                                    else if (string.Compare("JS", ws) == 0)
                                                    {
                                                        double para = 0.0;
                                                        double paf = 0;
                                                        paf = (2 * Math.PI) / WSD[wsd_r, 1];

                                                        if (Convert.ToDouble(Moment[1]) <= paf)
                                                        {
                                                            para = 0.07;
                                                        }
                                                        else if (Convert.ToDouble(Moment[1]) > paf)
                                                        {
                                                            para = 0.09;
                                                        }

                                                        WSD_a[wsd_r] = WSD_a[wsd_r] + (Math.Pow(Convert.ToDouble(Moment[vi]), 2) * ((5 * Math.Pow(WSD[wsd_r, 0], 2) * Math.Pow(paf, 4) * (1 - 0.287 * Math.Log(WSD[wsd_r, 3]))) / 16) * Math.Pow(Convert.ToDouble(Moment[1]), -5) * Math.Exp((-5 / 4) * Math.Pow(Convert.ToDouble(Moment[1]) / paf, -4)) * Math.Pow(WSD[wsd_r, 3], Math.Exp(-0.5 * Math.Pow(((Convert.ToDouble(Moment[1]) - paf) / (para * paf)), 2)))) * w_int;
                                                    }
                                                    else if (string.Compare("UD", ws) == 0)
                                                    {

                                                    }
                                                }
                                                else
                                                {
                                                    if (s_p == "T")
                                                    {
                                                        if (string.Compare("PM", ws) == 0)
                                                        {
                                                            WSD_a[wsd_r] = WSD_a[wsd_r] + (Math.Pow(Convert.ToDouble(Moment[14]), 2) * (((4 * (Math.Pow(WSD[wsd_r, 0], 2)) * (Math.Pow(Math.PI, 3))) / ((Math.Pow(WSD[wsd_r, 1], 4)) * (Math.Pow(Convert.ToDouble(Moment[1]), 5)))) * Math.Exp((-16 * (Math.Pow(Math.PI, 3))) / ((Math.Pow(WSD[wsd_r, 1], 4)) * (Math.Pow(Convert.ToDouble(Moment[1]), 4)))))) * w_int;
                                                        }
                                                        else if (string.Compare("JS", ws) == 0)
                                                        {
                                                            double para = 0.0;
                                                            double paf = 0;
                                                            paf = (2 * Math.PI) / WSD[wsd_r, 1];

                                                            if (Convert.ToDouble(Moment[1]) <= paf)
                                                            {
                                                                para = 0.07;
                                                            }
                                                            else if (Convert.ToDouble(Moment[1]) > paf)
                                                            {
                                                                para = 0.09;
                                                            }

                                                            WSD_a[wsd_r] = WSD_a[wsd_r] + (Math.Pow(Convert.ToDouble(Moment[14]), 2) * ((5 * Math.Pow(WSD[wsd_r, 0], 2) * Math.Pow(paf, 4) * (1 - 0.287 * Math.Log(WSD[wsd_r, 3]))) / 16) * Math.Pow(Convert.ToDouble(Moment[1]), -5) * Math.Exp((-5 / 4) * Math.Pow(Convert.ToDouble(Moment[1]) / paf, -4)) * Math.Pow(WSD[wsd_r, 3], Math.Exp(-0.5 * Math.Pow(((Convert.ToDouble(Moment[1]) - paf) / (para * paf)), 2)))) * w_int;
                                                        }
                                                        else if (string.Compare("UD", ws) == 0)
                                                        {

                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (string.Compare("PM", ws) == 0)
                                                        {
                                                            WSD_a[wsd_r] = WSD_a[wsd_r] + (Math.Pow(Convert.ToDouble(Moment[15]), 2) * (((4 * (Math.Pow(WSD[wsd_r, 0], 2)) * (Math.Pow(Math.PI, 3))) / ((Math.Pow(WSD[wsd_r, 1], 4)) * (Math.Pow(Convert.ToDouble(Moment[1]), 5)))) * Math.Exp((-16 * (Math.Pow(Math.PI, 3))) / ((Math.Pow(WSD[wsd_r, 1], 4)) * (Math.Pow(Convert.ToDouble(Moment[1]), 4)))))) * w_int;
                                                        }
                                                        else if (string.Compare("JS", ws) == 0)
                                                        {
                                                            double para = 0.0;
                                                            double paf = 0;
                                                            paf = (2 * Math.PI) / WSD[wsd_r, 1];

                                                            if (Convert.ToDouble(Moment[1]) <= paf)
                                                            {
                                                                para = 0.07;
                                                            }
                                                            else if (Convert.ToDouble(Moment[1]) > paf)
                                                            {
                                                                para = 0.09;
                                                            }

                                                            WSD_a[wsd_r] = WSD_a[wsd_r] + (Math.Pow(Convert.ToDouble(Moment[15]), 2) * ((5 * Math.Pow(WSD[wsd_r, 0], 2) * Math.Pow(paf, 4) * (1 - 0.287 * Math.Log(WSD[wsd_r, 3]))) / 16) * Math.Pow(Convert.ToDouble(Moment[1]), -5) * Math.Exp((-5 / 4) * Math.Pow(Convert.ToDouble(Moment[1]) / paf, -4)) * Math.Pow(WSD[wsd_r, 3], Math.Exp(-0.5 * Math.Pow(((Convert.ToDouble(Moment[1]) - paf) / (para * paf)), 2)))) * w_int;
                                                        }
                                                        else if (string.Compare("UD", ws) == 0)
                                                        {

                                                        }
                                                    }
                                                }
                                                m_r = m_r + 1;
                                                if (m_r == f_n)
                                                {
                                                    Spe_m[dire - 1, 0] = Convert.ToDouble(DIRE[dire - 1, 1]);
                                                    Spe_m[dire - 1, vi - 1] = WSD_a[wsd_r];
                                                }
                                                Line_o = pr.ReadLine();
                                            }
                                            break;
                                        }
                                        l_r = l_r + 1;
                                        Line_o = pr.ReadLine();
                                    }
                                }
                            }
                            ps.Close();
                            pr.Close();
                        }

                        if (sprea_factor == 0)
                        {
                            for (int n_0 = 0; n_0 < heading_n; n_0++)
                            {
                                sea_a[n_0, 0] = Spe_m[n_0, vi - 1];
                            }
                        }
                        else
                        {
                            for (int n = 0; n < heading_n; n++)
                            {
                                double c_sum = 0;
                                for (int n_1 = 0; n_1 < heading_n; n_1++)
                                {
                                    double angle = (Spe_m[n, 0] + 180) - (Spe_m[n_1, 0] + 180);
                                    if (Math.Abs(angle) <= 90 || Math.Abs(angle) >= 270)
                                    {
                                        double rad = angle * Math.PI / 180;
                                        c_sum = c_sum + Math.Pow(Math.Cos(rad), sprea_factor);
                                    }
                                }

                                double m_sum_0 = 0;
                                for (int n_2 = 0; n_2 < heading_n; n_2++)
                                {
                                    double angle_1 = (Spe_m[n, 0] + 180) - (Spe_m[n_2, 0] + 180);
                                    if (Math.Abs(angle_1) <= 90 || Math.Abs(angle_1) >= 270)
                                    {
                                        double rad_1 = angle_1 * Math.PI / 180;
                                        m_sum_0 = m_sum_0 + (1 / c_sum) * Math.Pow(Math.Cos(rad_1), sprea_factor) * Spe_m[n_2, vi - 1];
                                    }
                                }
                                sea_a[n, vi-2] = m_sum_0;
                            }
                        }
                    }
                        for (int head = 0; head < heading_n; head++)
                        {
                            if (molen == 7)
                            {
                                sr.WriteLine("{0,14},{1,14:0.00},{2,14:0.000000e0},{3,14:0.000000e0},{4,14:0.000000e0},{5,14:0.000000e0},{6,14:0.000000e0}", L[0], Spe_m[head, 0], sea_a[head, 0], sea_a[head, 1], sea_a[head, 2], sea_a[head, 3], sea_a[head, 4]);
                            }
                            else if (molen == 4)
                            {
                                sr.WriteLine("{0,14},{1,14:0.00},{2,14:0.000000e0},{3,14:0.000000e0}", L[0], Spe_m[head, 0], sea_a[head, 0], sea_a[head, 1]);
                            }
                            else
                            {
                                sr.WriteLine("{0,14},{1,14:0.00},{2,14:0.000000e0}", L[0], Spe_m[head, 0], sea_a[head, 0]);
                            }
                        }
                }
                sr.WriteLine("*END");
                sr.Close();
                ss.Close();
            }
        }
    }
}
