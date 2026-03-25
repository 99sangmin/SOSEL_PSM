using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Net.NetworkInformation;

namespace LONGTERM_GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            GB_LTA.Visible = false;

            //MAC Address 주소 입력
//            string strMacAddress = "00FF98E87939";
//            string MacAddress = NetworkInterface.GetAllNetworkInterfaces()[0].GetPhysicalAddress().ToString();

//            if (strMacAddress != MacAddress)
//            {
//                MessageBox.Show("Invalid physical address!!", "WARMING!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                System.Environment.Exit(0);
//            }

            DateTime now = DateTime.Now;
            //Deadline 입력
            DateTime end = DateTime.Parse("2030-11-25 20:03:00");
            TimeSpan ts = end.Subtract(now);

            if (ts.Days < 0)
            {
                //Console.WriteLine("Now the Software were Forbidden Due to Expiration Date");
                MessageBox.Show("License expired", "WARMING!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                System.Environment.Exit(0);
            }
            //else if (DateTime.Compare(end, now) < 30)
            else if (ts.Days < 30 && ts.Days > 0)
            {
                //Console.WriteLine(string.Format("The Software Will be Forbidden after %s"), ts);
                MessageBox.Show(("Will be expired in " + ts.Days + " days"), "WARMING!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        string work_path = "";
        Regex regEnglish = new Regex("^[a-zA-Z]");
        

        private void TOAQWA_CheckedChanged(object sender, EventArgs e)
        {

            if (RB_TOPANEL.Checked == true)
            {
                GB_TOPANEL.Visible = true;
                GB_LTA.Visible = false;
                GB_LTV.Visible = false;
                GB_RAO_EXTR.Visible = false;
                GB_SM.Visible = false;
            }
            else
            {
                GB_TOPANEL.Visible = false;
                GB_LTA.Visible = true;
            }
        }

        #region ABAQUS-HYDRO

        //private void TOAQWA_CheckedChanged(object sender, EventArgs e)
        //{

        //    if (RB_TOPANEL.Checked == true)
        //    {
        //        GB_TOPANEL.Visible = true;
        //    }
        //    else
        //    {
        //        GB_TOPANEL.Visible = false;
        //    }
        //}

        private void B_A2H_ABA_Click(object sender, EventArgs e)
        {
            OFD.Filter = "Input file(*.inp)|*.inp";
            OFD.FileName = "";
            DialogResult Result_A2H_ABA;
            Result_A2H_ABA = OFD.ShowDialog();
            if (DialogResult.OK == Result_A2H_ABA)
            {
                TB_A2H_ABA.Text = OFD.FileName;
                work_path = Path.GetDirectoryName(OFD.FileName);
            }
        }

        private void B_A2H_OP_Click(object sender, EventArgs e)
        {
            DialogResult Result_A2H_HYO;
            FBD.SelectedPath = work_path;
            Result_A2H_HYO = FBD.ShowDialog();
            TB_A2H_OP.Text = FBD.SelectedPath;
        }

        string A2H_unit = "";
        string A2H_ABA = "";
        string A2H_EXE = Path.Combine(Directory.GetCurrentDirectory(), "ABAQUS2AQWA.exe");
        string A2H_HYRO = "";
        string A2H_diff = "";
        string A2H_nondiff = "";
        string A2H_check = "";

        private void B_A2H_RUN_Click(object sender, EventArgs e)
        {
            A2H_ABA = TB_A2H_ABA.Text;
            A2H_HYRO = TB_A2H_OP.Text;
            A2H_diff = TB_A2H_DIFF.Text;
            A2H_nondiff = TB_A2H_NONDIFF.Text;

            if (UNIT_1.Checked == true)
            {
                A2H_unit = "1";
            }
            else if (UNIT_2.Checked == true)
            {
                A2H_unit = "2";
            }
            else if (UNIT_3.Checked == true)
            {
                A2H_unit = "3";
            }
            else if (UNIT_4.Checked == true)
            {
                A2H_unit = "4";
            }

            if (A2H_ABA == "" || A2H_HYRO == "")
            {
                MessageBox.Show("Check file or file path", "WARMING!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (A2H_diff == "")
            {
                MessageBox.Show("Input ELSET name for diffraction elements", "WARMING!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (A2H_unit == "")
            {
                MessageBox.Show("Select a unit conversion", "WARMING!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (A2H_nondiff != "")
            {
                A2H_check = "yes";
            }

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = A2H_EXE;
            if (A2H_check == "yes")
            {
                p.StartInfo.Arguments = Path.Combine(A2H_ABA + " " + A2H_HYRO + " " + A2H_unit + " " + A2H_diff + " " + A2H_nondiff);
            }
            else
            {
                p.StartInfo.Arguments = Path.Combine(A2H_ABA + " " + A2H_HYRO + " " + A2H_unit + " " + A2H_diff);
            }
            p.Start();
            p.WaitForExit();

            A2H_ABA = "";
            A2H_HYRO = "";
            A2H_unit = "";
            A2H_diff = "";
            A2H_nondiff = "";
            A2H_check = "";
        }

        private void B_A2H_CL_Click(object sender, EventArgs e)
        {
            TB_A2H_ABA.Text = "";
            TB_A2H_OP.Text = "";
            TB_A2H_DIFF.Text = "";
            TB_A2H_NONDIFF.Text = "";
            UNIT_1.Checked = false;
            UNIT_2.Checked = false;
            UNIT_3.Checked = false;
            UNIT_4.Checked = false;
            RB_TOPANEL.Checked = false;
            GB_TOPANEL.Visible = false;
        }
        #endregion

        private void LTV_Changed(object sender, EventArgs e)
        {

            if (RB_LTA.Checked == true)
            {
                GB_TOPANEL.Visible = false;
                GB_LTA.Visible = true;
            }
            else
            {
                GB_TOPANEL.Visible = true;
                GB_LTA.Visible = false;
            }
        }

        string JOB = "";

        private void B_JOB_Click(object sender, EventArgs e)
        {
            OFD.Filter = "Input file(*.job)|*.job";
            OFD.FileName = "";
            DialogResult Result_job;
            Result_job = OFD.ShowDialog();
            if (DialogResult.OK == Result_job)
            {
                JOB = OFD.FileName;
                TB_JOB.Text = JOB;
                work_path = Path.GetDirectoryName(OFD.FileName);
            }
        }

        #region RAO EXTRACTION

        //private void B_RAO_EXE_Click(object sender, EventArgs e)
        //{
        //    OFD.Filter = "Executive file(*.exe)|*.exe";
        //    OFD.FileName = "";
        //    DialogResult Result_RAO_exe;
        //    Result_RAO_exe = OFD.ShowDialog();
        //    TB_RAO_EXE.Text = OFD.FileName;
        //}

        //private void B_RAO_JOB_Click(object sender, EventArgs e)
        //{
        //    OFD.Filter = "Input file(*.job)|*.job";
        //    OFD.FileName = "";
        //    DialogResult Result_MT_job;
        //    Result_MT_job = OFD.ShowDialog();
        //    TB_RAO_JOB.Text = OFD.FileName;
        //}

        private void B_RAO_OP_Click(object sender, EventArgs e)
        {
            DialogResult Result_LE_lrao;
            FBD.SelectedPath = work_path;
            Result_LE_lrao = FBD.ShowDialog();
            TB_RAO_PATH.Text = FBD.SelectedPath;
        }

        private void RB_RAO_EXTR_Checked(object sender, EventArgs e)
        {
            if (TB_JOB.Text == "")
            {
                MessageBox.Show("Select job file first", "WARMING!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                RB_RAO_EXTR.Checked = false;
                return;
            }

            if (RB_RAO_EXTR.Checked == true)
            {
                GB_RAO_EXTR.Visible = true;
            }
            else
            {
                GB_RAO_EXTR.Visible = false;
            }

            string key = "";
            int loc_n = 0;
            int h_n = 0;

            FileStream es = new FileStream(JOB, FileMode.Open, FileAccess.Read, FileShare.Read);
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
                        h_n = h_n + 1;
                        Line_h = er.ReadLine();
                    }
                }
                if (Line_h.Trim().StartsWith("*LT"))
                {
                    Line_h = er.ReadLine();
                    while (Line_h.Trim().StartsWith("*") == true)
                    {
                        Line_h = er.ReadLine();
                    }
                    
                    string[] loc_s = Line_h.Split(',');

                    if (loc_s.Length != 0 && regEnglish.IsMatch(loc_s[0])==false)
                    {
                        Line_h = er.ReadLine();
                        while (Line_h.Trim().StartsWith("*") == true && er.Peek() != -1)
                        {
                            Line_h = er.ReadLine();
                        }
                    }

                    if (Line_h == null)
                    {
                        break;
                    }

                    string[] loc = Line_h.Split(',');
                    //if (loc.Length != 0 && regEnglish.IsMatch(loc[0]))
                    if (regEnglish.IsMatch(loc[0]))
                    {
                        key = loc[0];
                        //loc_n = loc.Length - 1;
                        if (loc.Length == 2)
                        {
                            if (loc[1] == "")
                            {
                                loc_n = 0;
                            }
                            else
                            {
                                loc_n = loc.Length - 1;
                            }
                        }
                        else
                        {
                            loc_n = loc.Length - 1;
                        }
                        TB_RAO_EXTR.Text = key;
                    }
                }
            }
            er.Close();
            es.Close();

            if ((key != "mom" && key != "MOM"))
            {
                TB_RAO_WFM.Text = "N/A";
                TB_RAO_LCN.Text = "N/A";

                if (key != "acc" && key != "ACC" && key != "mot" && key != "MOT" && key != "fk" && key != "FK" && key != "diff" && key != "DIFF" && key != "fk+diff" && key != "FK+DIFF")
                {
                    TB_RAO_EXTR.Text = "Error!!!";
                    TB_RAO_WFM.Text = "Not loaded! Check file path!";
                    TB_RAO_LCN.Text = "Not loaded! No station numbers!";
                }
                TB_RAO_AQWA.Text = "Not loaded! Check file!";
            }
            else
            {
                TB_RAO_AQWA.Text = "N/A";
                TB_RAO_WFM.Text = "Not loaded! Check file path!";
                TB_RAO_LCN.Text = "Not loaded! No station numbers!";
            }

            if (h_n != 0)
            {
                TB_RAO_DIR.Text = "Loaded Successfully";
            }
            else
            {
                TB_RAO_DIR.Text = "Not loaded! Check wave headings!";
            }

            FileStream LS_arr = new FileStream(JOB, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader LR_arr = new StreamReader(LS_arr);

            string Line_arr;
            string hydro = "";

            while (LR_arr.Peek() != -1)
            {
                Line_arr = LR_arr.ReadLine();
                if ((key != "mom" && key != "MOM") && Line_arr.Trim().StartsWith("*AQWA"))
                {
                    Line_arr = LR_arr.ReadLine();
                    if (Line_arr.Trim().StartsWith("*"))
                    {
                        Line_arr = LR_arr.ReadLine();
                    }
                    hydro = Line_arr;

                    if (File.Exists(hydro))
                    {
                        TB_RAO_AQWA.Text = "Loaded Successfully";
                    }
                }
                if ((key == "mom" || key == "MOM") && Line_arr.Trim().StartsWith("*WFM"))
                {
                    int d0 = 0;
                    Line_arr = LR_arr.ReadLine();
                    while (Line_arr.Trim().StartsWith("*") == true)
                    {
                        Line_arr = LR_arr.ReadLine();
                    }
                    while (Line_arr.Trim().StartsWith("*") == false)
                    {
                        string[] r_d = Line_arr.Split(',');

                        if (File.Exists(r_d[0]) == false)
                        {
                            TB_RAO_WFM.Text = string.Format("Not loaded! No files for Dir #{0,1}", d0 + 1);
                            break;
                        }
                        d0 = d0 + 1;
                        Line_arr = LR_arr.ReadLine();
                    }
                    if (h_n == d0)
                    {
                        TB_RAO_WFM.Text = "Loaded Successfully";
                    }
                }
                if (Line_arr.Trim().StartsWith("*LT") && (key == "mom" || key == "MOM"))
                {
                    if (loc_n != 0)
                    {
                        TB_RAO_LCN.Text = "Loaded Successfully";
                    }
                }
            }
            LS_arr.Close();
            LR_arr.Close();

        }

//        string RAO_EXE = Path.Combine(Directory.GetCurrentDirectory(), "LOG_MRAO.exe");
        string RAO_JOB = "";
        string RAO = "";

        private void B_RAO_RUN_Click(object sender, EventArgs e)
        {
            //RAO_EXE = TB_RAO_EXE.Text;
            string RAO_EXE = Path.Combine(Directory.GetCurrentDirectory(), "LOG_MRAO.exe");
            RAO_JOB = TB_JOB.Text;
            RAO = TB_RAO_PATH.Text;

            if (RAO_JOB == "" || RAO == "" || RAO_EXE == "")
            {
                MessageBox.Show("Check file or file path", "WARMING!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            System.Diagnostics.Process P_RAO = new System.Diagnostics.Process();
            P_RAO.StartInfo.FileName = RAO_EXE;

            P_RAO.StartInfo.Arguments = Path.Combine(RAO_JOB + " " + RAO);

            P_RAO.Start();
            P_RAO.WaitForExit();

            RAO_JOB = "";
            RAO_EXE = "";
            RAO = "";
        }

        private void B_RAO_CLE_Click(object sender, EventArgs e)
        {
            TB_RAO_PATH.Text = "";
            string RAO_EXE = Path.Combine(Directory.GetCurrentDirectory(), "LOG_MRAO.exe");
            //TB_RAO_JOB.Text = "";
            RB_RAO_EXTR.Checked = false;
            GB_RAO_EXTR.Visible = false;
            TB_RAO_EXTR.Text = "";
            TB_RAO_AQWA.Text = "";
            TB_RAO_DIR.Text = "";
            TB_RAO_WFM.Text = "";
            TB_RAO_LCN.Text = "";
        }

        #endregion

        #region Spectrum moment

        //private void B_SM_EXE_Click(object sender, EventArgs e)
        //{
        //    OFD.Filter = "Executive file(*.exe)|*.exe";
        //    OFD.FileName = "";
        //    DialogResult Result_sm_exe;
        //    Result_sm_exe = OFD.ShowDialog();
        //    TB_SM_EXE.Text = OFD.FileName;
        //}

        //private void B_SM_JOB_Click(object sender, EventArgs e)
        //{
        //    OFD.Filter = "Input file(*.job)|*.job";
        //    OFD.FileName = "";
        //    DialogResult Result_SM_job;
        //    Result_SM_job = OFD.ShowDialog();
        //    TB_SM_JOB.Text = OFD.FileName;
        //}

        private void B_SM_PATH_RAO_Click(object sender, EventArgs e)
        {
            DialogResult Result_SM_RAO;
            FBD.SelectedPath = work_path;
            Result_SM_RAO = FBD.ShowDialog();
            TB_SM_PATH_RAO.Text = FBD.SelectedPath;
        }

        private void B_SM_OP_Click(object sender, EventArgs e)
        {
            DialogResult Result_SM_OP;
            FBD.SelectedPath = work_path;
            Result_SM_OP = FBD.ShowDialog();
            TB_SM_PATH.Text = FBD.SelectedPath;
        }

        private void RB_SM_Checked(object sender, EventArgs e)
        {
            if (TB_JOB.Text == "")
            {
                MessageBox.Show("Select job file first", "WARMING!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                RB_SM.Checked = false;
                return;
            }

            if (RB_SM.Checked == true)
            {
                GB_SM.Visible = true;
            }
            else
            {
                GB_SM.Visible = false;
            }

            int heading_n = 0;
            string ws = "";
            int wsd_n = 0;
            double sprea_factor = 0;

            TB_SM_WS.Text = "Not Loaded! Check Wave Spectrum Type!)";
            TB_SM_SCE.Text = "Not Loaded! Check Spreading Cosine Exponent!)";
            TB_SM_DIR.Text = "Not Loaded! Check Wave Heading!)";
            TB_SM_WSD.Text = "Not Loaded! Check Sea state!)";

            FileStream es = new FileStream(JOB, FileMode.Open, FileAccess.Read, FileShare.Read);
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

                    if (heading_n != 0)
                    {
                        TB_SM_DIR.Text = "Loaded Successfully";
                    }
                }
                if (Line_h.Trim().StartsWith("*WS,"))
                {
                    string[] w_s = Line_h.Split('=');
                    ws = w_s[1];
                    if (string.Compare("PM", ws) == 0)
                    {
                        TB_SM_WS.Text = string.Format("Loaded Successfully ({0,1} spectrum)", "Pierson-Moskowitz");
                    }
                    else if (string.Compare("JS", ws) == 0)
                    {
                        TB_SM_WS.Text = string.Format("Loaded Successfully ({0,1} spectrum)", "JONSWAP");
                    }
                    else if (string.Compare("UD", ws) == 0)
                    {
                        TB_SM_WS.Text = string.Format("Loaded Successfully ({0,1} spectrum)", "User-Defined");
                    }
                }
                if (Line_h.Trim().StartsWith("*WSD,"))
                {
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

                    if (wsd_n != 0)
                    {
                        TB_SM_WSD.Text = "Loaded Successfully";
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

                    TB_SM_SCE.Text = "Loaded Successfully";
                }
            }
            er.Close();
            es.Close();
            
        }

        string SM_EXE = Path.Combine(Directory.GetCurrentDirectory(), "LOG_MSPE.exe");
        string SM_JOB = "";
        string SM_RAO = "";
        string SM_PATH = "";

        private void B_SM_RUN_Click(object sender, EventArgs e)
        {

            //SM_EXE = TB_SM_EXE.Text;
            SM_JOB = TB_JOB.Text;
            SM_RAO = TB_SM_PATH_RAO.Text;
            SM_PATH = TB_SM_PATH.Text;


            if (SM_JOB == "" || SM_RAO == "" || SM_EXE == "" || SM_PATH == "")
            {
                MessageBox.Show("Check file or file path", "WARMING!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            System.Diagnostics.Process P_SM = new System.Diagnostics.Process();
            P_SM.StartInfo.FileName = SM_EXE;

            P_SM.StartInfo.Arguments = Path.Combine(SM_JOB + " " + SM_RAO + " " + SM_PATH);

            P_SM.Start();
            P_SM.WaitForExit();

            SM_JOB = "";
            SM_RAO = "";
            SM_PATH = "";
        }

        private void B_SM_CL_Click(object sender, EventArgs e)
        {
            TB_SM_PATH.Text = "";
            SM_EXE = Path.Combine(Directory.GetCurrentDirectory(), "LOG_MSPE.exe");
            TB_SM_PATH_RAO.Text = "";
            //TB_SM_JOB.Text = "";
            RB_SM.Checked = false;
            GB_SM.Visible = false;
            TB_SM_DIR.Text = "";
            TB_SM_WS.Text = "";
            TB_SM_WSD.Text = "";
            TB_SM_SCE.Text = "";
            //SM_EXE = "";
        }

        #endregion

        #region Longter prediction

        //private void B_LTV_EXE_Click(object sender, EventArgs e)
        //{
        //    OFD.Filter = "Executive file(*.exe)|*.exe";
        //    OFD.FileName = "";
        //    DialogResult Result_ltv_exe;
        //    Result_ltv_exe = OFD.ShowDialog();
        //    TB_LTV_EXE.Text = OFD.FileName;
        //}

        //private void B_LTV_JOB_Click(object sender, EventArgs e)
        //{
        //    OFD.Filter = "Input file(*.job)|*.job";
        //    OFD.FileName = "";
        //    DialogResult Result_LTV_job;
        //    Result_LTV_job = OFD.ShowDialog();
        //    TB_LTV_JOB.Text = OFD.FileName;
        //}

        private void B_LTV_SM_Click(object sender, EventArgs e)
        {
            DialogResult Result_LTV_SM;
            FBD.SelectedPath = work_path;
            Result_LTV_SM = FBD.ShowDialog();
            TB_LTV_PATH_SM.Text = FBD.SelectedPath;
        }

        private void B_LTV_OP_Click(object sender, EventArgs e)
        {
            DialogResult Result_LTV_OP;
            FBD.SelectedPath = work_path;
            Result_LTV_OP = FBD.ShowDialog();
            TB_LTV_PATH.Text = FBD.SelectedPath;
        }

        private void RB_LTV_Checked(object sender, EventArgs e)
        {

            if (TB_JOB.Text == "")
            {
                MessageBox.Show("Select job file first", "WARMING!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                RB_LTV.Checked = false;
                return;
            }

            if (RB_LTV.Checked == true)
            {
                GB_LTV.Visible = true;
            }
            else
            {
                GB_LTV.Visible = false;
            }

            int heading_n = 0;
            int wsd_n = 0;
            int pl_n = 0;
            double tol_n = 0;

            TB_LT_DIR.Text = "Not Loaded! Check Wave Heading!!)";
            TB_LT_WSD.Text = "Not Loaded! Check Sea State!!)";
            TB_LT_PL.Text = "Not Loaded! Check Probability Levels)";
            TB_LT_TOL.Text = "Not Loaded! Check Tolerance)";

            FileStream es = new FileStream(JOB, FileMode.Open, FileAccess.Read, FileShare.Read);
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

                    if (heading_n != 0)
                    {
                        TB_LT_DIR.Text = "Loaded Successfully";
                    }
                }
                if (Line_h.Trim().StartsWith("*WSD,"))
                {
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

                    if (wsd_n != 0)
                    {
                        TB_LT_WSD.Text = "Loaded Successfully";
                    }
                }
                if (Line_h.Trim().StartsWith("*LT"))
                {
                    Line_h = er.ReadLine();
                    while (Line_h.Trim().StartsWith("*") == true)
                    {
                        Line_h = er.ReadLine();
                    }
                    string[] p_n = Line_h.Split(',');

                    pl_n = p_n.Length;

                    if (pl_n != 0 && regEnglish.IsMatch(p_n[0])==false)
                    {
                        TB_LT_PL.Text = "Loaded Successfully";
                    }
                }
                if (Line_h.Trim().StartsWith("*TOL"))
                {
                    Line_h = er.ReadLine();
                    while (Line_h.Trim().StartsWith("*") == true)
                    {
                        Line_h = er.ReadLine();
                    }
                    tol_n = Convert.ToDouble(Line_h);

                    if (tol_n != 0)
                    {
                        TB_LT_TOL.Text = "Loaded Successfully";
                    }
                }
            }
            er.Close();
            es.Close();

        }

        string LTV_EXE = Path.Combine(Directory.GetCurrentDirectory(), "LOG_DCAL.exe");
        string LTV_JOB = "";
        string LTV_SM = "";
        string LTV_PATH = "";

        private void B_LTV_RUN_Click(object sender, EventArgs e)
        {

            //LTV_EXE = TB_LTV_EXE.Text;
            LTV_JOB = TB_JOB.Text;
            LTV_SM = TB_LTV_PATH_SM.Text;
            LTV_PATH = TB_LTV_PATH.Text;


            if (LTV_JOB == "" || LTV_SM == "" || LTV_EXE == "" || LTV_PATH == "")
            {
                MessageBox.Show("Check file or file path", "WARMING!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            System.Diagnostics.Process P_LTV = new System.Diagnostics.Process();
            P_LTV.StartInfo.FileName = LTV_EXE;

            P_LTV.StartInfo.Arguments = Path.Combine(LTV_JOB + " " + LTV_SM + " " + LTV_PATH);

            P_LTV.Start();
            P_LTV.WaitForExit();

            LTV_JOB = "";
            LTV_SM = "";
            LTV_PATH = "";
        }

        private void B_LTV_CL_Click(object sender, EventArgs e)
        {
            TB_LTV_PATH.Text = "";
            LTV_EXE = Path.Combine(Directory.GetCurrentDirectory(), "LOG_DCAL.exe");
            TB_LTV_PATH_SM.Text = "";
            //TB_LTV_JOB.Text = "";
            RB_LTV.Checked = false;
            GB_LTV.Visible = false;
        }

        #endregion
    }
}
