using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;

namespace BayesClassifier
{
    public partial class MainForm : Form
    {
        private Classifier classifier;
        private SynchronizationContext uiContext;

        public MainForm()
        {
            InitializeComponent();
            uiContext = SynchronizationContext.Current;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            txtLearnDirectory.Text = Path.GetFullPath(@"Sample");
        }

        private void loadTexts(string fullDirectoryName)
        {
            classifier = new Classifier();
            log("Previous learned data cleared");
            string[] subDerictories = Directory.GetDirectories(fullDirectoryName, "*", SearchOption.AllDirectories);
            log("Found: " + subDerictories.Count() + " subderictories");
            foreach (string subdirectory in subDerictories)
            {
                DirectoryInfo d = new DirectoryInfo(subdirectory);
                log("Load " + d.Name + "...");
                foreach (var file in d.GetFiles("*.*"))
                {
                    log("Read " + file.Name + "...");
                    string text = System.IO.File.ReadAllText(file.FullName);
                    classifier.learnText(d.Name, text);
                }
            }
            log("Learning finished");
        }

        private void btnLearn_Click(object sender, EventArgs e)
        {
            loadTexts(txtLearnDirectory.Text);
        }

        private void btnClassify_Click(object sender, EventArgs e)
        {
            log("Run classifier...");
            string cluster = classifier.classifyMessage(txtTextToClassify.Text);
            log("Classificationt finished, result: " + cluster);
            txtClassifyResult.Text = cluster;
        }

        private void log(String message)
        {
            uiContext.Send(d => displayLogMessage(message), null);
        }

        private void displayLogMessage(String s)
        {
            txtLog.Text += "[" + DateTime.Now + "] " + s + "\r\n";
        }
    }
}
