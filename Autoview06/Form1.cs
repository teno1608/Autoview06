using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Windows.Automation;

namespace Autoview06
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static string getOperaUrlFromProcess(Process proc)
        {
            // find the automation element
            AutomationElement elm = AutomationElement.FromHandle(proc.MainWindowHandle);

            // manually walk through the tree, searching using TreeScope.Descendants is too slow (even if it's more reliable)
            AutomationElement elmUrlBar = null;
            try
            {
                var elm1 = elm.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Browser container"));
                if (elm1 == null) { return null; }
                elmUrlBar = elm.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Address field"));
            }
            catch
            {
                // Chrome has probably changed something, and above walking needs to be modified. :(
                // put an assertion here or something to make sure you don't miss it
                return null;
            }

            // make sure it's valid
            if (elmUrlBar == null)
            {
                // it's not..
                return null;
            }

            // there might not be a valid pattern to use, so we have to make sure we have one
            AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
            if (patterns.Length == 1)
            {
                string ret = "";
                try
                {
                    ret = ((ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0])).Current.Value;
                }
                catch { }
                if (ret != "")
                {
                    return ret;
                }
                return null;
            }
            return null;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Thread.Sleep(150000);
            try
            {
                foreach (Process proc in Process.GetProcessesByName("opera"))
                {
                    proc.Kill();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Thread.Sleep(5000);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Process proc in Process.GetProcessesByName("opera"))
                {
                    proc.Kill();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void Button4_Click(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Interval = (Convert.ToInt16(textBox5.Text)) * 60000;
            textBox5.Clear();
            textBox5.AppendText(timer2.Interval.ToString());
            timer2.Start();
        }


        string link_new = "false", link_old = "false";

        bool link_moi = true;
        public bool sosanh(string s1, string s2)
        {
            return String.Compare(s1, s2) == 0;
        }


        private int sl_loi = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            if (link_moi == true)
            {
                link_moi = false;
                int Num_rd;
                Random rd = new Random();
                Num_rd = rd.Next(0, textBox1.Lines.Length);
                link_old = link_new = textBox1.Lines[Num_rd];
                textBox2.Clear();
                textBox2.AppendText(link_new);
                Process.Start("opera", textBox1.Lines[Num_rd]);
                timer3.Stop();
                timer3.Interval = 210000;
                timer3.Start();
            }


            if (sosanh(link_old, link_new) == true)
            {
                Process[] OProcesses = Process.GetProcessesByName("opera");

                foreach (Process proc in OProcesses)
                {
                    if (proc.MainWindowHandle != IntPtr.Zero)
                    {
                        link_new = getOperaUrlFromProcess(proc);
                        break;
                    }
                }

            }
            else
            {
                sl_loi++;

                if (sl_loi <= 1)
                {
                    link_old = link_new;
                    textBox3.Clear();
                    textBox3.AppendText(link_new);
                    
                }
                else
                {
                    sl_loi = 0;
                    try
                    {
                        foreach (Process proc in Process.GetProcessesByName("opera"))
                        {
                            proc.Kill();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    link_moi = true;

                }
            }

            timer1.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                foreach (Process proc in Process.GetProcessesByName("opera"))
                {
                    proc.Kill();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            sl_loi = 2;
            link_old = "false";
        }

    }
}
