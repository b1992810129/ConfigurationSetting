using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Tamir.SharpSsh;
using System.Diagnostics;
using Tamir.SharpSsh.jsch;
using System.Xml;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace ConfigurationSetting
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private StreamWriter logWriter=null;

        public delegate void NextStepDelegate();
        private NextStepDelegate nextStepEvent = null;
        public event NextStepDelegate NextStepEvent
        {
            add
            {
                if (value != null)
                {
                    nextStepEvent = value;
                }
            }
            remove
            {
                if (value == nextStepEvent)
                {
                    nextStepEvent = null;
                }
            }
        }

        private void buttonNextStep_Click(object sender, EventArgs e)
        {
            try
            {
                nextStepEvent();
            }
            catch (Exception ex)
            {
                msgBox.Text += ex.Message+Environment.NewLine;
                logWriter.WriteLine(ex.Message);
                StackTrace st=new StackTrace(ex,true);
                StackFrame[] frames = st.GetFrames();
                for (int i = 0; i < frames.Length; i++)
                {
                    for (int spaceIndex = 0; spaceIndex < i; spaceIndex++)
                    {
                        msgBox.Text += " ";
                        logWriter.Write(" ");
                    }
                    msgBox.Text += "function name:"+ frames[i].GetMethod().Name+ " line:" + frames[i].GetFileLineNumber() + " column:" +
                                   frames[i].GetFileColumnNumber()+" file:"+frames[i].GetFileName() + Environment.NewLine;
                    logWriter.WriteLine("function name:" + frames[i].GetMethod().Name + " line:" + frames[i].GetFileLineNumber() + " column:" +
                                        frames[i].GetFileColumnNumber() + " file:" + frames[i].GetFileName());
                }
            }
        }
        private void buttonSetting_Click(object sender, EventArgs e)
        {
            Visitor visitor=new Visitor();
            Machine machine=null;
            
            if (wholeFuncTab.SelectedTab == verSettingTabPage)
            {
                if (versionSettingTab.SelectedTab == ASE_D2DB_eP5)
                {
                    machine = new EP5_ASE_D2DB();
                }
                
            }

            try
            {
                machine.accept(this, visitor);
            }
            catch (Exception ex)
            {
                msgBox.Text += ex.Message + Environment.NewLine;
                logWriter.WriteLine(ex.Message);
                StackTrace st = new StackTrace(ex, true);
                StackFrame[] frames = st.GetFrames();
                for (int i = 0; i < frames.Length; i++)
                {
                    for (int spaceIndex = 0; spaceIndex < i; spaceIndex++)
                    {
                        msgBox.Text += " ";
                        logWriter.Write(" ");
                    }
                    msgBox.Text += "function name:" + frames[i].GetMethod().Name + " line:" +
                                   frames[i].GetFileLineNumber() + " column:" + frames[i].GetFileColumnNumber()
                                   + " file:" + frames[i].GetFileName() + Environment.NewLine;
                    logWriter.WriteLine("function name:" + frames[i].GetMethod().Name + " line:" +
                                        frames[i].GetFileLineNumber() + " column:" + frames[i].GetFileColumnNumber()
                                        + " file:" + frames[i].GetFileName());
                }
            }
        }

        private void msgBox_TextChanged(object sender, EventArgs e)
        {
            msgBox.SelectionStart = msgBox.Text.Length;
            msgBox.ScrollToCaret();
        }

        public void setMsgBox(object obj)
        {
            string response = obj as string;
            Invoke(new Action(() => { msgBox.Text += response + Environment.NewLine; }));
        }

        public void enableNextStepButton(object obj)
        {
            Invoke(new Action(() => { buttonNextStep.Enabled = (bool)obj; }));
        }

        public void enableSettingButton(object obj)
        {
            Invoke(new Action(() => { buttonSetting.Enabled = (bool)obj; }));
        }

        public void clearSubCmdTextBox(object obj)
        {
            Invoke(new Action(() => { subCmdBox.Clear(); }));
        }

        public string getSubCmdTextBox()
        {
            string cmd="y";
            Invoke(new Action(() => { cmd = subCmdBox.Text; }));
            return cmd;
        }

        public void openLog(string path)
        {
            logWriter=new StreamWriter(path);
        }

        public void writeLog(string msg)
        {
            logWriter.WriteLine(msg);
        }

        public void closeLog()
        {
            logWriter.Close();
        }

        public void setStepBox(object obj)
        {
            int step = (int) obj;
            Invoke(new Action(() =>
            {
                whichStepBox.Text = step.ToString();
            }));
        }

        public void showMsgBox(string msg)
        {
            Invoke(new Action(() =>
            {
                MessageBox.Show(msg);
            }));
        }
    }


    interface IMachine
    {
        void accept(Form ui, Visitor visitor);
        void setting();
    }

    abstract class Machine : IMachine
    {
        public void accept(Form ui, Visitor visitor)
        {
            visitor.visit(ui as Form1, this);
        }

        public abstract void setting();

        private AutoResetEvent autoResetEvent = new AutoResetEvent(false);

        public void setAutoResetEvent()
        {
            autoResetEvent.Set();
        }
        protected void pause()
        {
            if (!callEnableNextButtonEvent(true))
            {
                throw new Exception("you don't give \"enableButtonEvent\" a function pointer");
            }
            autoResetEvent.WaitOne();
            if (!callEnableNextButtonEvent(false))
            {
                throw new Exception("you don't give \"enableButtonEvent\" a function pointer");
            }
        }

        protected void commandLinux(string ip, string user, string password, string cmd, int sleepTime_s)
        {
            using (SshStream ssh = new SshStream(ip, user, password))
            {
                ssh.Prompt = @"\?|#|\$";
                ssh.RemoveTerminalEmulationCharacters = true;
                string response = ssh.ReadResponse();
                
                if (!callSetTextBoxEvent(response))
                {
                    throw new Exception("you don't give \"setTextBoxEvent\" a function pointer");
                }
                if (!callWriteLogEvent(response))
                {
                    throw new Exception("you don't give \"WriteLogEvent\" a function pointer");
                }


                ssh.Write(cmd);
                response = ssh.ReadResponse().Trim();
                if (!callSetTextBoxEvent(response))
                {
                    throw new Exception("you don't give \"setTextBoxEvent\" a function pointer");
                }
                if (!callWriteLogEvent(response))
                {
                    throw new Exception("you don't give \"WriteLogEvent\" a function pointer");
                }

                while (response.Length != 0 && response.EndsWith("?"))
                {
                    
                    if (!callShowMsgBoxEvent("Please enter response in subCommand textbox, and then press next step"))
                    {
                        throw new Exception("you don't give \"showMsgBoxEvent\" a function pointer");
                    }
                    pause();
                    string subCmd = "y";
                    subCmd = callGetTextBoxEvent();
                    if (!callClearTextBoxEvent(null))
                    {
                        throw new Exception("clearTextBoxEvent is null");
                    }
                    ssh.Write(subCmd);
                    response = ssh.ReadResponse();
                    
                    if (!callSetTextBoxEvent(response))
                    {
                        throw new Exception("you don't give \"setTextBoxEvent\" a function pointer");
                    }
                    if (!callWriteLogEvent(response))
                    {
                        throw new Exception("you don't give \"WriteLogEvent\" a function pointer");
                    }
                }
            }
            Thread.Sleep(sleepTime_s * 1000);
        }

        protected void executeDosCmd(string cmd)
        {
            using (Process process = new Process())
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = "cmd.exe";
                info.UseShellExecute = false;
                info.RedirectStandardInput = true;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.CreateNoWindow = true;
                info.Arguments = "/C " + cmd;
                process.StartInfo = info;
                process.Start();
                bool cmdExecutionComplete = process.WaitForExit(1000);
                while (!cmdExecutionComplete)
                {
                    if (!callShowMsgBoxEvent("Please enter response in subCommand textbox, and then press next step"))
                    {
                        throw new Exception("ShowMsgBoxEvent is null");
                    }
                    pause();
                    string subCmd = "y";
                    subCmd = callGetTextBoxEvent();
                    if (!callClearTextBoxEvent(null))
                    {
                        throw new Exception("clearTextBoxEvent is null");
                    }
                    process.StandardInput.WriteLine(subCmd);
                    process.StandardInput.Flush();
                    cmdExecutionComplete = process.WaitForExit(1000);
                }
                string response = process.StandardOutput.ReadToEnd();
                
                if (!callSetTextBoxEvent(response)) { throw new Exception("setTextBoxEvent is null"); }
                if (!callWriteLogEvent(response)) { throw new Exception("writeLogEvent is null"); }

                string error_string = process.StandardError.ReadToEnd();
                
                if (!callSetTextBoxEvent(error_string)) { throw new Exception("setTextBoxEvent is null"); }
                if (!callWriteLogEvent(error_string)) { throw new Exception("writeLogEvent is null"); }
            }

        }

        public delegate void UpdateUIDelegate(object obj);

        private UpdateUIDelegate setTextBoxEvent = null;
        public event UpdateUIDelegate SetTextBoxEvent
        {
            add
            {
                if (value != null)
                {
                    setTextBoxEvent = value;
                }
            }
            remove
            {
                if (value == setTextBoxEvent)
                {
                    setTextBoxEvent = null;
                }
            }
        }
        protected bool callSetTextBoxEvent(object obj)
        {
            if (setTextBoxEvent != null)
            {
                setTextBoxEvent(obj);
                return true;
            }
            return false;
        }

        private UpdateUIDelegate enableNextButtonEvent = null;
        public event UpdateUIDelegate EnableNextButtonEvent
        {
            add
            {
                if (value != null)
                {
                    enableNextButtonEvent = value;
                }
            }
            remove
            {
                if (value == enableNextButtonEvent)
                {
                    enableNextButtonEvent = null;
                }
            }
        }
        protected bool callEnableNextButtonEvent(object obj)
        {
            if (enableNextButtonEvent != null)
            {
                enableNextButtonEvent(obj);
                return true;
            }
            return false;
        }

        private UpdateUIDelegate enableSettingButtonEvent = null;
        public event UpdateUIDelegate EnableSettingButtonEvent
        {
            add
            {
                if (value != null)
                {
                    enableSettingButtonEvent = value;
                }
            }
            remove
            {
                if (value == enableSettingButtonEvent)
                {
                    enableSettingButtonEvent = null;
                }
            }
        }
        protected bool callEnableSettingButtonEvent(object obj)
        {
            if (enableSettingButtonEvent != null)
            {
                enableSettingButtonEvent(obj);
                return true;
            }
            return false;
        }

        private UpdateUIDelegate clearTextBoxEvent = null;
        public event UpdateUIDelegate ClearTextBoxEvent
        {
            add
            {
                if (value != null)
                {
                    clearTextBoxEvent = value;
                }
            }
            remove
            {
                if (value == clearTextBoxEvent)
                {
                    clearTextBoxEvent = null;
                }
            }
        }
        protected bool callClearTextBoxEvent(object obj)
        {
            if (clearTextBoxEvent != null)
            {
                clearTextBoxEvent(obj);
                return true;
            }
            return false;
        }

        private UpdateUIDelegate setStepBoxEvent = null;
        public event UpdateUIDelegate SetStepBoxEvent
        {
            add
            {
                if (value != null)
                {
                    setStepBoxEvent = value;
                }
            }
            remove
            {
                if (value == setStepBoxEvent)
                {
                    setStepBoxEvent = null;
                }
            }
        }
        protected bool callSetStepBoxEvent(object obj)
        {
            if (setStepBoxEvent != null)
            {
                setStepBoxEvent(obj);
                return true;
            }
            return false;
        }

        public delegate string GetUIValue();

        private GetUIValue getTextBoxEvent = null;
        public event GetUIValue GetTextBoxEvent
        {
            add
            {
                if (value != null)
                {
                    getTextBoxEvent = value;
                }
            }
            remove
            {
                if (value == getTextBoxEvent)
                {
                    getTextBoxEvent = null;
                }
            }
        }
        protected string callGetTextBoxEvent()
        {
            if (getTextBoxEvent != null)
            {
                return getTextBoxEvent();
            }
            return null;
        }

        public delegate void OpenAndWriteLogDelegate(string msg);

        private OpenAndWriteLogDelegate openLogEvent = null;
        public event OpenAndWriteLogDelegate OpenLogEvent
        {
            add
            {
                if (value != null)
                {
                    openLogEvent = value;
                }
            }
            remove
            {
                if (value == openLogEvent)
                {
                    openLogEvent = null;
                }
            }
        }
        protected bool callOpenLogEvent(string path)
        {
            if (openLogEvent != null)
            {
                openLogEvent(path);
                return true;
            }
            return false;
        }

        private OpenAndWriteLogDelegate writeLogEvent = null;
        public event OpenAndWriteLogDelegate WriteLogEvent
        {
            add
            {
                if (value != null)
                {
                    writeLogEvent = value;
                }
            }
            remove
            {
                if (value == writeLogEvent)
                {
                    writeLogEvent = null;
                }
            }
        }
        protected bool callWriteLogEvent(string msg)
        {
            if (writeLogEvent != null)
            {
                writeLogEvent(msg);
                return true;
            }
            return false;
        }

        public delegate void CloseLogDelegate();

        private CloseLogDelegate closeLogEvent = null;
        public event CloseLogDelegate CloseLogEvent
        {
            add
            {
                if (value != null)
                {
                    closeLogEvent = value;
                }
            }
            remove
            {
                if (value == closeLogEvent)
                {
                    closeLogEvent = null;
                }
            }
        }
        protected bool callCloseLogEvent()
        {
            if (closeLogEvent != null)
            {
                closeLogEvent();
                return true;
            }
            return false;
        }

        public delegate void ShowMsgBoxDelegate(string msg);

        private ShowMsgBoxDelegate showMsgBoxEvent = null;
        public event ShowMsgBoxDelegate ShowMsgBoxEvent
        {
            add
            {
                if (value != null)
                {
                    showMsgBoxEvent = value;
                }
            }
            remove
            {
                if (value == showMsgBoxEvent)
                {
                    showMsgBoxEvent = null;
                }
            }
        }
        protected bool callShowMsgBoxEvent(string msg)
        {
            if (showMsgBoxEvent != null)
            {
                showMsgBoxEvent(msg);
                return true;
            }
            return false;
        }
    }

    class EP5_ASE_D2DB : Machine
    {

        public override void setting()
        {
            Thread t = new Thread(() =>
            {

                try
                {
                    if (!callOpenLogEvent(this.GetType().ToString() + ".log"))
                    {
                        throw new Exception("OpenLogEvent is null");
                    }
                    if (!callSetTextBoxEvent(
                        "eP5-ASE_D2DB Version setting upgrade start ,please mention about all upgrade infomrmation show at plateform !")
                    )
                    {
                        throw new Exception("you don't give \"printMessageInTextBoxEvent\" a function pointer");
                    }
                    pause();
                    if (!callSetTextBoxEvent(
                        "This Active character only avaliable at eP5-FW PC , before do it please confirm currenct location  ! ! ! ")
                    )
                    {
                        throw new Exception("you don't give \"printMessageInTextBoxEvent\" a function pointer");
                    }
                    pause();
                    if (!callSetTextBoxEvent(@"Start Auto umount /mnt/clinet for All image computer !"))
                    {
                        throw new Exception("you don't give \"printMessageInTextBoxEvent\" a function pointer");
                    }


                    XmlDocument configuration_xml = new XmlDocument();
                    configuration_xml.Load("configuration.xml");

                    int step = 0;
                    try
                    {
                        step = int.Parse(callGetTextBoxEvent());
                    }
                    catch (Exception ex)
                    {
                        if ((ex is ArgumentNullException) || (ex is FormatException) || (ex is OverflowException))
                        {
                            step = 0;
                        }
                        else
                        {
                            MessageBox.Show("there is a weird exception in parse function" + " " + ex.Message);
                            throw ex;
                        }
                    }

                    //01
                    if (step <= 1)
                    {
                        if (!callSetStepBoxEvent(1))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNodeList oneNodes = configuration_xml.SelectNodes(
                            @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p01/ip");
                        for (int i = 0; i < oneNodes.Count; i++)
                        {
                            commandLinux(oneNodes.Item(i).InnerText, "root", "hermes",
                                @"umount /mnt/client/", 2);

                        }

                        for (int i = 0; i < oneNodes.Count; i++)
                        {
                            commandLinux(oneNodes.Item(i).InnerText, "root", "hermes",
                                @"umount /result/", 2);

                        }

                    }
                    //02
                    if (step <= 2)
                    {
                        if (!callSetStepBoxEvent(2))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNodeList twoNodes = configuration_xml.SelectNodes(
                            @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p02/ip");
                        for (int i = 0; i < twoNodes.Count; i++)
                        {
                            commandLinux(twoNodes.Item(i).InnerText, "root", "hermes",
                                @"umount /mnt/client/",
                                2);

                        }

                        for (int i = 0; i < twoNodes.Count; i++)
                        {
                            commandLinux(twoNodes.Item(i).InnerText, "root", "hermes",
                                @"umount /result/", 2);

                        }

                    }
                    //03
                    if (step <= 3)
                    {
                        if (!callSetStepBoxEvent(3))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNodeList threeNodes = configuration_xml.SelectNodes(
                            @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p03/ip");
                        for (int i = 0; i < threeNodes.Count; i++)
                        {
                            commandLinux(threeNodes.Item(i).InnerText, "root", "hermes",
                                @"umount /mnt/client/", 2);

                        }

                    }
                    //04
                    if (step <= 4)
                    {
                        if (!callSetStepBoxEvent(4))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNodeList fourNodes = configuration_xml.SelectNodes(
                            @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p04/ip");
                        for (int i = 0; i < fourNodes.Count; i++)
                        {
                            commandLinux(fourNodes.Item(i).InnerText, "root", "hermes",
                                @"umount /result/", 2);

                        }

                    }
                    //05
                    if (step <= 5)
                    {
                        if (!callSetStepBoxEvent(5))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNodeList fiveNodes =
                            configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p05/ip");
                        for (int i = 0; i < fiveNodes.Count; i++)
                        {
                            commandLinux(fiveNodes.Item(i).InnerText, "root", "hermes",
                                @"umount /mnt/client/", 2);

                        }

                        pause();
                    }
                    //06
                    if (step <= 6)
                    {
                        if (!callSetStepBoxEvent(6))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNodeList sixNodes = configuration_xml.SelectNodes(
                            @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p06/ip");
                        for (int i = 0; i < sixNodes.Count; i++)
                        {
                            commandLinux(sixNodes.Item(i).InnerText, "root", "hermes",
                                @"umount /mnt/client/", 2);

                        }

                        pause();


                        if (!callSetTextBoxEvent("umount /mnt/clinet & /result/  Finish !\n" +
                                                 "Start to Disconnect all Winsows Mapping Disk"
                        ))
                        {
                            throw new Exception("setTextBoxEvent is null");
                        }
                        pause();
                        executeDosCmd(@"rmdir D:\hmi\gds");

                        executeDosCmd(@"net use * /del /y");


                        if (!callSetTextBoxEvent("All Windows mapping disk locate at this PC were Disconnect !"))
                        {
                            throw new Exception("setTextBoxEvent is null");
                        }
                        pause();

                        if (!callSetTextBoxEvent("Start to Create folder please close  All MobaXterm programs !"))
                        {
                            throw new Exception("setTextBoxEvent is null");
                        }
                    }
                    //07
                    if (step <= 7)
                    {
                        if (!callSetStepBoxEvent(7))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        string cmdADC_subfolder_create =
                            @"cd /result/hmi2005 && mkdir Auto_Environment && chmod 777 Auto_Environment/";
                        XmlNodeList sevenNodes =
                            configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p07/ip");
                        for (int i = 0; i < sevenNodes.Count; i++)
                        {
                            commandLinux(sevenNodes.Item(i).InnerText, "root", "hermes",
                                cmdADC_subfolder_create, 0);

                        }
                    }

                    //08
                    if (step <= 8)
                    {
                        if (!callSetStepBoxEvent(8))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNodeList eightNodes =
                            configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p08/ip");
                        for (int i = 0; i < eightNodes.Count; i++)
                        {
                            commandLinux(eightNodes.Item(i).InnerText, "root", "hermes",
                                @"cd /usr/src/", 0);

                        }

                    }
                    string cmd_folder_create_sh = @"cd /result &&
                        mkdir mnt &&
                        chmod 777 mnt &&
                        cd /result/mnt &&
                        mkdir client &&
                        chmod 777 client &&
                        cd /result/mnt/client &&
                        mkdir hmi2005 &&
                        chmod 777 hmi2005 &&
                            mkdir recorder &&
                            chmod 777 recorder &&
                            mkdir data &&
                            chmod 777 data &&
                            mkdir PFMIN &&
                            chmod 777 PFMIN &&
                        cd /result/mnt/client/hmi2005 &&
                        mkdir hmiglm &&
                        chmod 777 hmiglm &&
                        cd /result/mnt/client/data &&
                        mkdir gds &&
                        chmod 777 gds &&
                            mkdir rawtobmp &&
                            chmod 777 rawtobmp";

                    //09
                    if (step <= 9)
                    {
                        if (!callSetStepBoxEvent(9))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNodeList nineNodes =
                            configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p09/ip");
                        for (int i = 0; i < nineNodes.Count; i++)
                        {
                            commandLinux(nineNodes.Item(i).InnerText, "root", "hermes",
                                cmd_folder_create_sh, 0);

                        }

                        pause();

                        if (!callSetTextBoxEvent(@" Start to Auto create /PFMIN folder at FW-Ic0 IP:10.8.6.204"))
                        {
                            throw new Exception("setTextBoxEvent is null");
                        }
                    }
                    //10
                    if (step <= 10)
                    {
                        if (!callSetStepBoxEvent(10))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNodeList tenNodes =
                            configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p10/ip");
                        foreach (XmlNode node in tenNodes)
                        {
                            commandLinux(node.InnerText, "root", "hermes",
                                "cd .. && mkdir PFMIN && chmod 777 PFMIN",
                                0);

                        }

                        if (!callSetTextBoxEvent(
                            "eP5 Storage computer & ADC computer & FW-Ic0 /PFMIN folder Auto create successful !"))
                        {
                            throw new Exception("setTextBoxEvent is null");
                        }
                        pause();

                        if (!callSetTextBoxEvent(
                            " Start to Auto create /PFMIN folder at ep5-Ic0 IP:10.8.6.254"))
                        {
                            throw new Exception("setTextBoxEvent is null");
                        }
                    }
                    //11
                    if (step <= 11)
                    {
                        if (!callSetStepBoxEvent(11))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNodeList elevenNodes = configuration_xml.SelectNodes(
                            @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p11/ip");
                        foreach (XmlNode node in elevenNodes)
                        {
                            commandLinux(node.InnerText, "root", "hermes",
                                "cd .. && mkdir PFMIN && chmod 777 PFMIN",
                                0);
                        }

                        if (!callSetTextBoxEvent(
                            @"eP5 Storage computer & ADC computer & ep5-IC0/PFMIN/ folder, Auto create successful !")
                        )
                        {
                            throw new Exception("setTextBoxEvent is null");
                        }
                        pause();
                        if (!callSetTextBoxEvent("start Storage PC known issue Auto debug!"))
                        {
                            throw new Exception("setTextBoxEvent is null");
                        }


                    }
                    //12
                    if (step <= 12)
                    {
                        if (!callSetStepBoxEvent(12))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        string cmd_ifcfg_debug =
                            @"cd /etc/sysconfig/network-scripts/ && rm -rf ifcfg-eth2 && rm -rf ifcfg-eth3 && service network restart";
                        XmlNodeList twelveNodes =
                            configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p12/ip");
                        foreach (XmlNode node in twelveNodes)
                        {
                            commandLinux(node.InnerText, "root", "hermes", cmd_ifcfg_debug, 0);
                        }

                        if (!callSetTextBoxEvent(
                            "eP5 Storage computer ifcfg-eth2 & ifcfg - eth3 Auto debug finish !"))
                        {
                            throw new Exception("setTextBoxEvent is null");
                        }
                        pause();
                    }
                    //13
                    SFTP sftp = null;
                    bool isConnectSuccess = false;
                    string path_fstab = "";
                    if (step <= 13)
                    {
                        if (!callSetStepBoxEvent(13))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNode thirteenNode = configuration_xml.SelectSingleNode(
                            @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p13/ip");
                        sftp = new SFTP(thirteenNode.InnerText, "root", "hermes");


                        isConnectSuccess = sftp.Connect();
                        if (!isConnectSuccess)
                        {
                            Exception ex = new Exception("step 13 connection fail");
                            throw ex;
                        }
                        path_fstab = @"C:\HMI\sn_online\Version_setting_upgrade\fstab\fstab";
                        if (!sftp.Put(path_fstab, @"/etc"))
                        {
                            Exception ex = new Exception("in step 13,sftp.Put fail ");
                            throw ex;
                        }
                        sftp.Disconnect();
                        pause();
                    }
                    //14
                    if (step <= 14)
                    {
                        if (!callSetStepBoxEvent(14))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNode fourteenNode = configuration_xml.SelectSingleNode(
                            @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p14/ip");
                        sftp = new SFTP(fourteenNode.InnerText, "root", "hermes");

                        isConnectSuccess = sftp.Connect();
                        if (!isConnectSuccess)
                        {
                            Exception ex = new Exception("step 14 connection fail");
                            throw ex;
                        }
                        if (!sftp.Put(path_fstab, @"/etc"))
                        {
                            Exception ex = new Exception("in step 14,sftp.Put fail ");
                            throw ex;
                        }
                        pause();

                        if (!callSetTextBoxEvent(
                            @"eP5 Ic0 & Storage PC fstab update finish please check it with cat /etc/fstab command !")
                        )
                        {
                            throw new Exception("setTextBoxEvent is null");
                        }
                        string samba_conf_path =
                            @"C:\HMI\sn_online\Version_setting_upgrade\samba_setting\storage\smb.conf";
                        if (!sftp.Put(samba_conf_path, "/etc/samba"))
                        {
                            Exception ex = new Exception("in step 14,sftp.Put fail ");
                            throw ex;
                        }
                        sftp.Disconnect();
                    }
                    //15
                    if (step <= 15)
                    {
                        if (!callSetStepBoxEvent(15))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNode fifthteenNode = configuration_xml.SelectSingleNode(
                            @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p15/ip");
                        sftp = new SFTP(fifthteenNode.InnerText, "root", "hermes");
                        //sftp = new SFTP(@"10.8.6.199", "root", "hermes");
                        isConnectSuccess = sftp.Connect();
                        if (!isConnectSuccess)
                        {
                            Exception ex = new Exception("step 15 connection fail");
                            throw ex;
                        }
                        if (!sftp.Put(@"C:\HMI\sn_online\Version_setting_upgrade\samba_setting\adc\smb.conf",
                            "/etc/samba"))
                        {
                            Exception ex = new Exception("in step 15,sftp.Put fail ");
                            throw ex;
                        }
                        sftp.Disconnect();
                    }
                    //16
                    if (step <= 16)
                    {
                        if (!callSetStepBoxEvent(16))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNode sixteenNode = configuration_xml.SelectSingleNode(
                            @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p16/ip");
                        sftp = new SFTP(sixteenNode.InnerText, "root", "hermes");
                        //sftp = new SFTP(@"10.8.6.254", "root", "hermes");
                        isConnectSuccess = sftp.Connect();
                        if (!isConnectSuccess)
                        {
                            Exception ex = new Exception("step 16 connection fail");
                            throw ex;
                        }
                        if (!sftp.Put(@"C:\HMI\sn_online\Version_setting_upgrade\samba_setting\ic0\smb.conf",
                            "/etc/samba"))
                        {
                            Exception ex = new Exception("in step 16,sftp.Put fail ");
                            throw ex;
                        }
                        sftp.Disconnect();
                    }
                    //17
                    if (step <= 17)
                    {
                        if (!callSetStepBoxEvent(17))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNodeList seventeenNodes = configuration_xml.SelectNodes(
                            @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p17/ip");
                        foreach (XmlNode node in seventeenNodes)
                        {
                            sftp = new SFTP(node.InnerText, "root", "hermes");
                            isConnectSuccess = sftp.Connect();
                            if (!isConnectSuccess)
                            {
                                Exception ex = new Exception("step 17 connection fail");
                                throw ex;
                            }
                            if (!sftp.Put(@"C:\HMI\sn_online\Version_setting_upgrade\samba_setting\hpc\smb.conf",
                                "/etc/samba"))
                            {
                                Exception ex = new Exception("in step 17,sftp.Put fail ");
                                throw ex;
                            }
                            Thread.Sleep(1000);
                            sftp.Disconnect();
                        }

                        pause();
                        if (!callSetTextBoxEvent("sama service restart for eP5!"))
                        {
                            throw new Exception("setTextBoxEvent ,is null");
                        }

                    }
                    //18
                    if (step <= 18)
                    {
                        if (!callSetStepBoxEvent(18))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNodeList eighteenNodes = configuration_xml.SelectNodes(
                            @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p18/ip");
                        foreach (XmlNode node in eighteenNodes)
                        {
                            commandLinux(node.InnerText, "root", "hermes",
                                @"/etc/init.d/smb stop && /etc/init.d/smb start",
                                1);

                        }

                        if (!callSetTextBoxEvent(
                            "eP5 HPC1~6 & Storage & Ic0 & ADC computer  samb.config update Finish !"))
                        {
                            throw new Exception("setTextBoxEvent is null");
                        }
                        pause();


                        if (!callSetTextBoxEvent(
                            "Stat to upgrade eP5 all computer rc.local setting ! ! !"))
                        {
                            throw new Exception("setTextBoxEvent is null");
                        }
                        pause();
                    }
                    //19
                    if (step <= 19)
                    {
                        if (!callSetStepBoxEvent(19))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNodeList nineteenNodes = configuration_xml.SelectNodes(
                            @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p19/ip");
                        foreach (XmlNode node in nineteenNodes)
                        {
                            sftp = new SFTP(node.InnerText, "root", "hermes");
                            isConnectSuccess = sftp.Connect();
                            if (!isConnectSuccess)
                            {
                                Exception ex = new Exception("step 19 connection fail");
                                throw ex;
                            }
                            if (!sftp.Put(
                                @"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\hpc\rc.local",
                                "/etc"))
                            {
                                Exception ex = new Exception("in step 19,sftp.Put fail ");
                                throw ex;
                            }
                            Thread.Sleep(1000);
                            sftp.Disconnect();
                        }

                    }
                    //20
                    if (step <= 20)
                    {
                        if (!callSetStepBoxEvent(20))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNodeList twentyNodes =
                            configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p20/ip");
                        foreach (XmlNode node in twentyNodes)
                        {
                            sftp = new SFTP(node.InnerText, "root", "hermes");
                            isConnectSuccess = sftp.Connect();
                            if (!isConnectSuccess)
                            {
                                Exception ex = new Exception("step 20 connection fail");
                                throw ex;
                            }
                            if (!sftp.Put(
                                @"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\client\rc.local",
                                "/etc"))
                            {
                                Exception ex = new Exception("in step 20,sftp.Put fail ");
                                throw ex;
                            }
                            Thread.Sleep(1000);
                            sftp.Disconnect();
                        }

                    }
                    //21
                    if (step <= 21)
                    {
                        if (!callSetStepBoxEvent(21))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNodeList twentyoneNodes = configuration_xml.SelectNodes(
                            @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p21/ip");
                        foreach (XmlNode node in twentyoneNodes)
                        {
                            sftp = new SFTP(node.InnerText, "root", "hermes");
                            isConnectSuccess = sftp.Connect();
                            if (!isConnectSuccess)
                            {
                                Exception ex = new Exception("step 21 connection fail");
                                throw ex;
                            }
                            if (!sftp.Put(
                                @"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\client\rc.local",
                                "/etc"))
                            {
                                Exception ex = new Exception("in step 21,sftp.Put fail ");
                                throw ex;
                            }
                            Thread.Sleep(1000);
                            sftp.Disconnect();
                        }

                        pause();
                    }
                    //22
                    if (step <= 22)
                    {
                        if (!callSetStepBoxEvent(22))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNode twentytwoNode = configuration_xml.SelectSingleNode(
                            @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p22/ip");
                        sftp = new SFTP(twentytwoNode.InnerText, "root", "hermes");
                        isConnectSuccess = sftp.Connect();
                        if (!isConnectSuccess)
                        {
                            Exception ex = new Exception("step 22 connection fail");
                            throw ex;
                        }
                        if (!sftp.Put(
                            @"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\glm\rc.local",
                            "/etc"))
                        {
                            Exception ex = new Exception("in step 22,sftp.Put fail ");
                            throw ex;
                        }
                        sftp.Disconnect();
                    }
                    //23
                    if (step <= 23)
                    {
                        if (!callSetStepBoxEvent(23))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNode twentythreeNode = configuration_xml.SelectSingleNode(
                            @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p23/ip");
                        sftp = new SFTP( /*"10.8.6.200"*/twentythreeNode.InnerText, "root", "hermes");
                        isConnectSuccess = sftp.Connect();
                        if (!isConnectSuccess)
                        {
                            Exception ex = new Exception("step 23 connection fail");
                            throw ex;
                        }
                        if (!sftp.Put(
                            @"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\storage\rc.local",
                            "/etc"))
                        {
                            Exception ex = new Exception("in step 23,sftp.Put fail ");
                            throw ex;
                        }
                        sftp.Disconnect();
                    }
                    //24
                    if (step <= 24)
                    {
                        if (!callSetStepBoxEvent(24))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNode twentyfourNode = configuration_xml.SelectSingleNode(
                            @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p24/ip");
                        sftp = new SFTP(twentyfourNode.InnerText /*"10.8.6.199"*/, "root", "hermes");
                        isConnectSuccess = sftp.Connect();
                        if (!isConnectSuccess)
                        {
                            Exception ex = new Exception("step 24 connection fail");
                            throw ex;
                        }
                        if (!sftp.Put(
                            @"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\adc\rc.local",
                            "/etc"))
                        {
                            Exception ex = new Exception("in step 24,sftp.Put fail ");
                            throw ex;
                        }
                        sftp.Disconnect();
                    }
                    //25
                    if (step <= 25)
                    {
                        if (!callSetStepBoxEvent(25))
                        {
                            throw new Exception("SetStepBoxEvent is null");
                        }
                        XmlNode twentyfiveNode = configuration_xml.SelectSingleNode(
                            @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p25/ip");
                        sftp = new SFTP( /*"10.8.6.254"*/twentyfiveNode.InnerText, "root", "hermes");
                        isConnectSuccess = sftp.Connect();
                        if (!isConnectSuccess)
                        {
                            Exception ex = new Exception("step 25 connection fail");
                            throw ex;
                        }
                        if (!sftp.Put(
                            @"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\ico\rc.local",
                            "/etc"))
                        {
                            Exception ex = new Exception("in step 25,sftp.Put fail ");
                            throw ex;
                        }
                        sftp.Disconnect();
                        Thread.Sleep(1000);

                        if (!callSetTextBoxEvent(
                            "eP5 All computer rc.local setting update finish, please reboot PC as sequence , FW reboot >> Storage reboot >> IC0 reboot >> ADC reboot >> HPC reboot >> Client reboot !")
                        )
                        {
                            throw new Exception("setTextBoxEvent is null");
                        }
                        pause();
                    }
                }
                catch (XmlException xmlexception)
                {
                    StackTrace st = new StackTrace(xmlexception, true);
                    StackFrame[] frames = st.GetFrames();
                    for (int i = 0; i < frames.Length; i++)
                    {
                        string space = "";
                        for (int spaceIndex = 0; spaceIndex < i; spaceIndex++)
                        {
                            space += " ";
                        }
                        if (!callSetTextBoxEvent(space + "function name:" + frames[i].GetMethod().Name + " line:" +
                                                 frames[i].GetFileLineNumber() + " column:" +
                                                 frames[i].GetFileColumnNumber()
                                                 + " file:" + frames[i].GetFileName()))
                        {
                            StreamWriter sw = new StreamWriter(
                                "your setTextBoxEvent is null,happen in" +
                                new StackTrace(true).GetFrame(0).GetFileLineNumber().ToString() + ".txt", true);
                            sw.WriteLine(space + "function name:" + frames[i].GetMethod().Name + " line:" +
                                         frames[i].GetFileLineNumber() + " column:" + frames[i].GetFileColumnNumber()
                                         + " file:" + frames[i].GetFileName());
                            sw.Close();
                        }

                        if (!callWriteLogEvent(space + "function name:" + frames[i].GetMethod().Name + " line:" +
                                               frames[i].GetFileLineNumber() + " column:" +
                                               frames[i].GetFileColumnNumber()
                                               + " file:" + frames[i].GetFileName()))
                        {
                            StreamWriter sw = new StreamWriter(
                                "your WriteLogEvent is null,happen in" +
                                new StackTrace(true).GetFrame(0).GetFileLineNumber().ToString() + ".txt", true);
                            sw.WriteLine(space + "function name:" + frames[i].GetMethod().Name + " line:" +
                                         frames[i].GetFileLineNumber() + " column:" + frames[i].GetFileColumnNumber()
                                         + " file:" + frames[i].GetFileName());
                            sw.Close();
                        }

                    }
                    if (!callSetTextBoxEvent(xmlexception.Message + " LineNumber=" + xmlexception.LineNumber +
                                             " LinePosition=" +
                                             xmlexception.LinePosition))
                    {
                        StreamWriter sw =
                            new StreamWriter(
                                "your setTextBoxEvent is null,happen in" +
                                new StackTrace(true).GetFrame(0).GetFileLineNumber().ToString() + ".txt", true);
                        sw.WriteLine(xmlexception.Message + " LineNumber=" + xmlexception.LineNumber +
                                     " LinePosition=" +
                                     xmlexception.LinePosition);
                        sw.Close();
                    }
                    if (!callWriteLogEvent(xmlexception.Message + " LineNumber=" + xmlexception.LineNumber +
                                           " LinePosition=" +
                                           xmlexception.LinePosition))
                    {
                        StreamWriter sw =
                            new StreamWriter(
                                "your WriteLogEvent is null,happen in" +
                                new StackTrace(true).GetFrame(0).GetFileLineNumber().ToString() + ".txt", true);
                        sw.WriteLine(xmlexception.Message + " LineNumber=" + xmlexception.LineNumber +
                                     " LinePosition=" +
                                     xmlexception.LinePosition);
                        sw.Close();
                    }
                }
                catch (System.Xml.XPath.XPathException exception)
                {
                    if (!callSetTextBoxEvent(exception.Message))
                    {
                        StreamWriter sw =
                            new StreamWriter(
                                "your SetTextBoxEvent is null,happen in" +
                                new StackTrace(true).GetFrame(0).GetFileLineNumber().ToString() + ".txt", true);
                        sw.WriteLine(exception.Message);
                        sw.Close();
                    }
                    if (!callWriteLogEvent(exception.Message))
                    {
                        StreamWriter sw =
                            new StreamWriter(
                                "your WriteLogEvent is null,happen in" +
                                new StackTrace(true).GetFrame(0).GetFileLineNumber().ToString() + ".txt", true);
                        sw.WriteLine(exception.Message);
                        sw.Close();
                    }
                    StackTrace st = new StackTrace(exception, true);
                    StackFrame[] frames = st.GetFrames();
                    for (int i = 0; i < frames.Length; i++)
                    {
                        string space = "";
                        for (int spaceIndex = 0; spaceIndex < i; spaceIndex++)
                        {
                            space += " ";
                        }

                        if (!callSetTextBoxEvent(space + "function name:" + frames[i].GetMethod().Name + " line:" +
                                                 frames[i].GetFileLineNumber() + " column:" +
                                                 frames[i].GetFileColumnNumber()
                                                 + " file:" + frames[i].GetFileName()))
                        {
                            StreamWriter sw = new StreamWriter(
                                "your SetTextBoxEvent is null,happen in" +
                                new StackTrace(true).GetFrame(0).GetFileLineNumber().ToString() + ".txt", true);
                            sw.WriteLine(space + "function name:" + frames[i].GetMethod().Name + " line:" +
                                         frames[i].GetFileLineNumber() + " column:" + frames[i].GetFileColumnNumber()
                                         + " file:" + frames[i].GetFileName());
                            sw.Close();
                        }

                        if (!callWriteLogEvent(space + "function name:" + frames[i].GetMethod().Name + " line:" +
                                               frames[i].GetFileLineNumber() + " column:" +
                                               frames[i].GetFileColumnNumber()
                                               + " file:" + frames[i].GetFileName()))
                        {
                            StreamWriter sw = new StreamWriter(
                                "your WriteLogEvent is null,happen in" +
                                new StackTrace(true).GetFrame(0).GetFileLineNumber().ToString() + ".txt", true);
                            sw.WriteLine(space + "function name:" + frames[i].GetMethod().Name + " line:" +
                                         frames[i].GetFileLineNumber() + " column:" + frames[i].GetFileColumnNumber()
                                         + " file:" + frames[i].GetFileName());
                            sw.Close();
                        }
                    }

                }
                catch (Exception exception)
                {
                    if (!callSetTextBoxEvent(exception.Message))
                    {
                        StreamWriter sw =
                            new StreamWriter(
                                "your SetTextBoxEvent is null,happen in" +
                                new StackTrace(true).GetFrame(0).GetFileLineNumber().ToString() + ".txt", true);
                        sw.WriteLine(exception.Message);
                        sw.Close();
                    }
                    if (!callWriteLogEvent(exception.Message))
                    {
                        StreamWriter sw =
                            new StreamWriter(
                                "your WriteLogEvent is null,happen in" +
                                new StackTrace(true).GetFrame(0).GetFileLineNumber().ToString() + ".txt", true);
                        sw.WriteLine(exception.Message);
                        sw.Close();
                    }
                    StackTrace st = new StackTrace(exception, true);
                    StackFrame[] frames = st.GetFrames();
                    for (int i = 0; i < frames.Length; i++)
                    {
                        string space = "";
                        for (int spaceIndex = 0; spaceIndex < i; spaceIndex++)
                        {
                            space += " ";
                        }

                        if (!callSetTextBoxEvent(space + "function name:" + frames[i].GetMethod().Name + " line:" +
                                                 frames[i].GetFileLineNumber() + " column:" +
                                                 frames[i].GetFileColumnNumber()
                                                 + " file:" + frames[i].GetFileName()))
                        {
                            StreamWriter sw = new StreamWriter(
                                "your SetTextBoxEvent is null,happen in" +
                                new StackTrace(true).GetFrame(0).GetFileLineNumber().ToString() + ".txt", true);
                            sw.WriteLine(space + "function name:" + frames[i].GetMethod().Name + " line:" +
                                         frames[i].GetFileLineNumber() + " column:" + frames[i].GetFileColumnNumber()
                                         + " file:" + frames[i].GetFileName());
                            sw.Close();
                        }

                        if (!callWriteLogEvent(space + "function name:" + frames[i].GetMethod().Name + " line:" +
                                               frames[i].GetFileLineNumber() + " column:" +
                                               frames[i].GetFileColumnNumber()
                                               + " file:" + frames[i].GetFileName()))
                        {
                            StreamWriter sw = new StreamWriter(
                                "your WriteLogEvent is null,happen in" +
                                new StackTrace(true).GetFrame(0).GetFileLineNumber().ToString() + ".txt", true);
                            sw.WriteLine(space + "function name:" + frames[i].GetMethod().Name + " line:" +
                                         frames[i].GetFileLineNumber() + " column:" + frames[i].GetFileColumnNumber()
                                         + " file:" + frames[i].GetFileName());
                            sw.Close();
                        }

                    }

                }
                finally
                {
                    callCloseLogEvent();

                    if (!callEnableSettingButtonEvent(true))
                    {
                        StreamWriter sw = new StreamWriter("your EnableSettingButtonEvent is null,happen in" + new StackTrace(true).GetFrame(0).GetFileLineNumber().ToString() + ".txt", true);
                        sw.Close();
                    }
                }
            });

            
            if (!callEnableSettingButtonEvent(false))
            {
                StreamWriter sw = new StreamWriter("your EnableSettingButtonEvent is null,happen in" + new StackTrace(true).GetFrame(0).GetFileLineNumber().ToString() + ".txt", true);
                sw.Close();
            }
            t.Start();
        }
    }

    class Visitor
    {

        public void visit(Form1 ui, Machine machine)
        {
            machine.SetTextBoxEvent += ui.setMsgBox;
            machine.EnableNextButtonEvent += ui.enableNextStepButton;
            machine.EnableSettingButtonEvent += ui.enableSettingButton;
            machine.ClearTextBoxEvent += ui.clearSubCmdTextBox;
            machine.GetTextBoxEvent += ui.getSubCmdTextBox;
            machine.OpenLogEvent += ui.openLog;
            machine.WriteLogEvent += ui.writeLog;
            machine.CloseLogEvent += ui.closeLog;
            machine.ShowMsgBoxEvent += ui.showMsgBox;
            machine.SetStepBoxEvent += ui.setStepBox;
            ui.NextStepEvent += machine.setAutoResetEvent;
            
            machine.setting();
        }
    }

    public class SFTP
    {
        private Session m_session;
        private Channel m_channel;
        private ChannelSftp m_sftp;

        // constructor(SFTP address ///username ///password )
        public SFTP(string host, string user, string pwd)
        {
            string[] arr = host.Split(':');
            string ip = arr[0];
            int port = 22;
            if (arr.Length > 1) port = Int32.Parse(arr[1]);
            JSch jsch = new JSch();
            m_session = jsch.getSession(user, ip, port);
            MyUserInfo ui = new MyUserInfo();
            ui.setPassword(pwd);
            m_session.setUserInfo(ui);
        }

        // get SFTP connection status
        public bool Connected
        {
            get
            {
                return m_session.isConnected();
            }
        }

        // connect to SFTP
        public bool Connect()
        {
            try
            {
                if (!Connected)
                {
                    m_session.connect();
                    m_channel = m_session.openChannel("sftp");
                    m_channel.connect();
                    m_sftp = (ChannelSftp)m_channel;
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        // disconnect the connection to SFTP
        public void Disconnect()
        {
            if (Connected)
            {
                m_channel.disconnect();
                m_session.disconnect();
            }
        }

        // put local file to SFTP
        //local path ///remote path
        public bool Put(string localPath, string remotePath)
        {
            try
            {
                Tamir.SharpSsh.java.String src = new Tamir.SharpSsh.java.String(localPath);
                Tamir.SharpSsh.java.String dst = new Tamir.SharpSsh.java.String(remotePath);
                m_sftp.put(src, dst);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // get remote file from SFTP
        //remote path ///local path ///
        public bool Get(string remotePath, string localPath)
        {
            try
            {
                Tamir.SharpSsh.java.String src = new Tamir.SharpSsh.java.String(remotePath);
                Tamir.SharpSsh.java.String dst = new Tamir.SharpSsh.java.String(localPath);
                m_sftp.get(src, dst);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // delete file on sftp
        //remote path ///
        public bool Delete(string remoteFile)
        {
            try
            {
                m_sftp.rm(remoteFile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 移動SFTP文件
        //sftp遠程文件地址
        public bool Move(string currentFilename, string newDirectory)
        {
            try
            {
                if (this.Connected)
                {
                    Tamir.SharpSsh.java.String src = new Tamir.SharpSsh.java.String(currentFilename);
                    Tamir.SharpSsh.java.String dst = new Tamir.SharpSsh.java.String(newDirectory);
                    m_sftp.rename(src, dst);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        // get file list from SFTP
        //remote path ///file type ///
        public ArrayList GetFileList(string remotePath, string fileType)
        {
            try
            {
                Tamir.SharpSsh.java.util.Vector vvv = m_sftp.ls(remotePath);
                ArrayList objList = new ArrayList();
                foreach (Tamir.SharpSsh.jsch.ChannelSftp.LsEntry qqq in vvv)
                {
                    string sss = qqq.getFilename();
                    if (sss.Length > (fileType.Length + 1) && fileType == sss.Substring(sss.Length - fileType.Length))
                    {
                        objList.Add(sss);
                    }
                    else
                    {
                        continue;
                    }
                }

                return objList;
            }
            catch
            {
                return null;
            }
        }

        // SFTP login user info
        public class MyUserInfo : UserInfo
        {
            String passwd;
            public String getPassword() { return passwd; }
            public void setPassword(String passwd) { this.passwd = passwd; }

            public String getPassphrase() { return null; }
            public bool promptPassphrase(String message) { return true; }

            public bool promptPassword(String message) { return true; }
            public bool promptYesNo(String message) { return true; }
            public void showMessage(String message) { }
        }
    }

}
