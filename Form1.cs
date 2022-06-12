using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace Notepad
{
    public partial class Form1 :Form
    {
        private string filePath = "Untitled";
        private bool isChanged;
        public string fPath
        {
            set
            {
                filePath = value;
                updateInfo();
            }
            get
            {
                return filePath;
            }
        }
        public bool IsChanged
        {
            set
            {
                isChanged = value;
                updateInfo();
            }
            get
            {
                return isChanged;
            }
        }
        public Form1()
        {
            InitializeComponent();
            newToolStripMenuItem_Click(null, null);
        }
        private void updateInfo()
        {
            if (fPath == "")
            {
                this.Text = "Untitled - Notepad ";

            }
            else
            {
                this.Text = Path.GetFileName(fPath);
            }
            if (IsChanged)
            {
                this.Text ="*"+ this.Text;
               
            }               
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsChanged)
            {
                SaveWithoutSave();
            }
            fPath = "";
            richTextBox1.Clear();
            IsChanged = false;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsChanged)
            {
                SaveWithoutSave();
            }
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "TXT|*.txt|BAT|*.bat|All|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fPath = ofd.FileName;
                richTextBox1.Text = File.ReadAllText(fPath);
                IsChanged = false;
            }
        }
        private void SaveWithoutSave()
        {
            if (MessageBox.Show("Do you want to save changes to" + fPath+"?",
                                               "Notepad.",
                                               MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                saveToolStripMenuItem_Click(null, null);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fPath == "")
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                File.WriteAllText(fPath, richTextBox1.Text);
                isChanged = false;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "TXT|*.txt|BAT|*.bat|All|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                fPath = sfd.FileName;
                File.WriteAllText(fPath, richTextBox1.Text);
                isChanged = false;
            }
        }



        private void FormClosing_Ask(object sender, FormClosingEventArgs e)
        {
            if (isChanged)
            {
                DialogResult dr = MessageBox.Show("Do you want to save changes" + fPath + "?",
                                                "Notepad",
                                                MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)

                    saveToolStripMenuItem_Click(null, null);
                else if (dr == DialogResult.No)
                    Application.Exit();
                else if (dr == DialogResult.Cancel)
                    e.Cancel = true;
                return;
            }
            else
                Application.Exit();

        }
  

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormClosing_Ask(null, null);
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog ft = new FontDialog();
            if (ft.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Font =ft.Font;
            }
        }

        private void textColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.ForeColor = cd.Color;
            }
        }

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.BackColor = cd.Color;
            }
        }
     
        private void copy(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void cut(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void paste(object sender, EventArgs e)
        {
            richTextBox1.Paste();

        }

        private void TChanged(object sender, EventArgs e)
        {
            
                isChanged = true;
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = richTextBox1.SelectedText;
            richTextBox1.Find(str);
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.ZoomFactor < 64)
                richTextBox1.ZoomFactor += 2.0f; 
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(richTextBox1.ZoomFactor>1)
            richTextBox1.ZoomFactor -= 2.0f;
        }

        private void timeDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += DateTime.Now.ToString();

        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();
            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = "fileName";
            printDlg.Document = printDoc;
            printDlg.AllowSelection = true;
            printDlg.AllowSomePages = true;
            //Call ShowDialog
            if (printDlg.ShowDialog() == DialogResult.OK)
            {
                printDoc.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                printDoc.Print();
            }
        }

        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form1().Show();
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            ev.Graphics.DrawString(richTextBox1.Text, richTextBox1.Font, System.Drawing.Brushes.Black, ev.MarginBounds.Left, 0, new StringFormat());
        }

        private void defaultZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.ZoomFactor = 1.0f;

        }
    }
}
