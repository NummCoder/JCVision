using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisionUtil.NLog
{
    public partial class FormLogFileSetting : Form
    {
        public FormLogFileSetting()
        {
            InitializeComponent();
        }
        public FormLogFileSetting(TabPage page)
        {
            InitializeComponent();
            this.TopLevel = false;
            page.Controls.Add(this);
            if (page.Size.Width < 612 || page.Size.Height < 304)
            {
                this.Size = page.Size;
            }
            else
            {
                this.Size = new Size(612, 304);
            }
            this.Show();
        }
        public FormLogFileSetting(Panel panel)
        {
            InitializeComponent();
            this.TopLevel = false;
            panel.Controls.Add(this);
            if (panel.Size.Width< 612||panel.Size.Height< 304)
            {
                this.Size = panel.Size;
            }
            else
            {
                this.Size = new Size(612,304);
            }
            this.Show();
        }

        private void WriteConfig()
        {
            string strMessage = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n";
            strMessage += "<nlog xmlns=\"http://www.nlog-project.org/schemas/NLog.xsd\"\n";
            strMessage += "      xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"\n";
            strMessage += "      xsi:schemaLocation=\"http://www.nlog-project.org/schemas/NLog.xsd NLog.xsd\"\n";
            strMessage += "      autoReload=\"true\"\n";
            strMessage += "      throwExceptions=\"false\"\n";
            strMessage += "      internalLogLevel=\"Off\" internalLogFile=\"c:\\temp\\nlog -internal.log\">\n\n";
            strMessage += "    <!-- optional, add some variables\n";
            strMessage += "     https://github.com/nlog/NLog/wiki/Configuration-file#variables\n";
            strMessage += "    -->\n";
            strMessage += "    <variable name=\"myvar\" value=\"myvalue\"/>\n\n";
            strMessage += "  <targets>\n";
            strMessage += "    <!--\n";
            strMessage += "    add your targets here\n";
            strMessage += "    See https://github.com/nlog/NLog/wiki/Targets for possible targets.\n";
            strMessage += "    See https://github.com/nlog/NLog/wiki/Layout-Renderers for the possible layout renderers.\n";
            strMessage += "    -->\n\n";
            strMessage += "    <!--\n";
            strMessage += "    Write events to a file with the date in the filename.\n";
            strMessage += "    <target xsi:type=\"File\" name=\"f\" fileName=\"${basedir}/logs/${shortdate}.log\"\n";
            strMessage += "            layout=\"${longdate} ${uppercase:${level}} ${message}\" />\n";
            strMessage += "    -->\n";
            strMessage += "    <!--<target xsi:type=\"File\" name=\"f\" fileName=\"${basedir}/Logs/${shortdate}.txt\"\n";
            strMessage += "            layout=\"${longdate} ${uppercase:${level}} ${message}\" />-->\n\n";
            foreach (LogFileItem item in LogFileManager.pDoc.logFileList)
            {
                strMessage += string.Format("    <target xsi:type=\"File\" name=\"{0}\"", item.fileName + "file");
                strMessage += " fileName=\"${basedir}/MachineLogs/" + item.fileName + "/${shortdate}.txt\"\n";
                strMessage += "           layout=\"${longdate} ${uppercase:${level}} ${message}\" />\n\n";

                strMessage += "    <target xsi:type=\"RichTextBox\"\n";
                strMessage += string.Format("            name=\"{0}\"\n", "m_rtb" + item.fileName);
                strMessage += "            layout=\"${longdate} ${uppercase:${level}} ${message}\"\n";
                strMessage += "            formName =\"LogWindow\"\n";
                strMessage += string.Format("            controlName=\"{0}\"\n", "richTextBox" + item.fileName);
                strMessage += "            autoScroll=\"true\"\n";
                strMessage += "            maxLines=\"80\"\n";
                strMessage += string.Format("            useDefaultRowColoringRules=\"true\" />\n");
            }
            strMessage += "  </targets>\n\n";

            strMessage += "  <rules>\n";
            strMessage += "    <!-- add your logging rules here -->\n";
            strMessage += "   <!--\n";
            strMessage += "    Write all events with minimal level of Debug (So Debug, Info, Warn, Error and Fatal, but not Trace)  to \"f\"\n";
            strMessage += "    <logger name=\" * \" minlevel=\"Debug\" writeTo=\"f\" />\n";
            strMessage += "    -->\n\n";
            foreach (LogFileItem item in LogFileManager.pDoc.logFileList)
            {
                strMessage += string.Format("    <logger name=\"{0}\" minlevel=\"Trace\" writeTo=\"{1}\" />\n", item.fileName, item.fileName + "file");
                strMessage += string.Format("    <logger name=\"{0}\" minlevel=\"Trace\" writeTo=\"{1}\" />\n", item.fileName, "m_rtb" + item.fileName);
            }
            strMessage += "  </rules>\n";
            strMessage += "</nlog>\n";

            System.IO.File.WriteAllText(@".//Parameter/NLog.config", strMessage);
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            string name = tbName.Text;
            string path = tbPath.Text;
            dataGridView1.Rows.Add(new object[] { name, true, path });
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            LogFileManager.pDoc.logFileList.Clear();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                LogFileItem item = new LogFileItem();
                item.fileName = dataGridView1[0, i].Value.ToString();
                item.filePath = dataGridView1[2, i].Value.ToString();
                item.bUsing = Convert.ToBoolean(dataGridView1[1, i].Value);
                LogFileManager.pDoc.logFileList.Add(item);
            }
            LogFileManager.pDoc.SaveDocument();
            WriteConfig();
        }

        private void FormLogFileSetting_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            foreach (LogFileItem item in LogFileManager.pDoc.logFileList)
            {
                dataGridView1.Rows.Add(new object[] { item.fileName, item.bUsing, item.filePath });
            }
        }
    }
}
