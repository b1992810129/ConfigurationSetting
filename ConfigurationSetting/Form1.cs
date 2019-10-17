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

namespace ConfigurationSetting
{
    public partial class Form1 : Form
    {
        private AutoResetEvent autoResetEvent=new AutoResetEvent(false);
        

        public Form1()
        {
            InitializeComponent();
        }


        private List<string> commandLinux(string ip, string user, string password, string cmd, int sleepTime_s)
        {
            List<string> list_response=new List<string>();
            using (SshStream ssh = new SshStream(ip, user, password))
            {
                ssh.Prompt = @"\?|#|\$";
                ssh.RemoveTerminalEmulationCharacters = true;
                string response = ssh.ReadResponse();
                Invoke( new Action(() => { msgBox.Text += response + Environment.NewLine; }) );
                list_response.Add(response);

                ssh.Write(cmd);
                response = ssh.ReadResponse().Trim();
                Invoke(new Action(() => { msgBox.Text += response + Environment.NewLine; }));
                list_response.Add(response);

                while (response.Length != 0 && response.EndsWith("?"))
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show("Please enter response in subCommand textbox, and then press next step");
                    }));
                    pause();
                    string subCmd="y";
                    Invoke(new Action(() =>
                    {
                        subCmd = subCmdBox.Text;
                        subCmdBox.Clear();
                    }));
                    ssh.Write(subCmd);
                    response = ssh.ReadResponse();
                    Invoke(new Action(() => { msgBox.Text += response + Environment.NewLine; }));
                    list_response.Add(response);
                }
            }
            Thread.Sleep(sleepTime_s*1000);
            return list_response;
        }

        private List<string> commandLinux420(string ip, string user, string password, string cmd, int sleepTime_s)
        {
            List<string> list_response = new List<string>();
            using (SshStream ssh = new SshStream(ip, user, password))
            {
                ssh.Prompt = @"\?|#|\$";
                ssh.RemoveTerminalEmulationCharacters = true;
                string response = ssh.ReadResponse();
                Invoke(new Action(() => { msgBox420.Text += response + Environment.NewLine; }));
                list_response.Add(response);

                ssh.Write(cmd);
                response = ssh.ReadResponse().Trim();
                Invoke(new Action(() => { msgBox420.Text += response + Environment.NewLine; }));
                list_response.Add(response);

                while (response.Length != 0 && response.EndsWith("?"))
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show("Please enter response in subCommand textbox, and then press next step");
                    }));
                    pause();
                    string subCmd = "y";
                    Invoke(new Action(() =>
                    {
                        subCmd = subCmdBox420.Text;
                        subCmdBox420.Clear();
                    }));
                    ssh.Write(subCmd);
                    response = ssh.ReadResponse();
                    Invoke(new Action(() => { msgBox420.Text += response + Environment.NewLine; }));
                    list_response.Add(response);
                }
            }
            Thread.Sleep(sleepTime_s * 1000);
            return list_response;
        }

        private List<string> executeDosCmd(string cmd)
        {
            List<string> list_response = new List<string>();
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
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show("Please enter response in subCommand textbox, and then press next step");
                    }));
                    pause();
                    string subCmd = "y";
                    Invoke(new Action(() =>
                    {
                        subCmd = subCmdBox.Text;
                        subCmdBox.Clear();
                    }));
                    process.StandardInput.WriteLine(subCmd);
                    process.StandardInput.Flush();
                    cmdExecutionComplete = process.WaitForExit(1000);
                }
                string response = process.StandardOutput.ReadToEnd();
                Invoke(new Action(() => { msgBox.Text += response + Environment.NewLine; }));
                list_response.Add(response);
                string error_string = process.StandardError.ReadToEnd();
                Invoke(new Action(() => { msgBox.Text += error_string + Environment.NewLine; }));
                list_response.Add(error_string);
            }
            return list_response;
        }

        private List<string> executeDosCmd420(string cmd)
        {
            List<string> list_response = new List<string>();
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
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show("Please enter response in subCommand textbox, and then press next step");
                    }));
                    pause();
                    string subCmd = "y";
                    Invoke(new Action(() =>
                    {
                        subCmd = subCmdBox420.Text;
                        subCmdBox420.Clear();
                    }));
                    process.StandardInput.WriteLine(subCmd);
                    process.StandardInput.Flush();
                    cmdExecutionComplete = process.WaitForExit(1000);
                }
                string response = process.StandardOutput.ReadToEnd();
                Invoke(new Action(() => { msgBox420.Text += response + Environment.NewLine; }));
                list_response.Add(response);
                string error_string = process.StandardError.ReadToEnd();
                Invoke(new Action(() => { msgBox420.Text += error_string + Environment.NewLine; }));
                list_response.Add(error_string);
            }
            return list_response;
        }

        private void pause()
        {
            Invoke(new Action(() =>
            {
                buttonNextStep.Enabled = true;
            }));
            autoResetEvent.WaitOne();
            Invoke(new Action(() =>
            {
                buttonNextStep.Enabled = false;
            }));
        }

        private void pause420()
        {
            Invoke(new Action(() =>
            {
                buttonNextStep420.Enabled = true;
            }));
            autoResetEvent.WaitOne();
            Invoke(new Action(() =>
            {
                buttonNextStep420.Enabled = false;
            }));
        }

        private void buttonNextStep_Click(object sender, EventArgs e)
        {
            autoResetEvent.Set();
        }
        private void buttonSetting_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
                using (StreamWriter logWriter = new StreamWriter("eP5-ASE_D2DB.log"))
                {
                    try
                    {
                        Invoke(new Action(() =>
                        {
                            msgBox.Text +=
                                "eP5-ASE_D2DB Version setting upgrade start ,please mention about all upgrade infomrmation show at plateform !" +
                                Environment.NewLine;
                        }));
                        pause();
                        Invoke(new Action(() =>
                        {
                            msgBox.Text +=
                                "This Active character only avaliable at eP5-FW PC , before do it please confirm currenct location  ! ! ! " +
                                Environment.NewLine;
                        }));
                        pause();
                        Invoke(new Action(() =>
                        {
                            msgBox.Text +=
                                @"Start Auto umount /mnt/clinet for All image computer !" + Environment.NewLine;
                        }));
                        pause();

                        XmlDocument configuration_xml = new XmlDocument();
                        configuration_xml.Load("configuration.xml");

                        int step = 0;
                        try
                        {
                            step = int.Parse(whichStepBox.Text);
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

                        List<string> list_response;

                        //01
                        if (step <= 1)
                        {
                            XmlNodeList oneNodes = configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p01/ip");
                            for (int i = 0; i < oneNodes.Count; i++)
                            {
                                list_response = commandLinux(oneNodes.Item(i).InnerText, "root", "hermes",
                                    @"umount /mnt/client/", 2);
                                foreach (string response in list_response)
                                {
                                    logWriter.WriteLine(response);
                                }
                            }
                            //for (int i = 101; i <= 124; i++)
                            //{
                            //    commandLinux("10.8.6." + i.ToString() , "root", "hermes", @"umount /mnt/client/",
                            //        2);
                            //}
                            for (int i = 0; i < oneNodes.Count; i++)
                            {
                                list_response = commandLinux(oneNodes.Item(i).InnerText, "root", "hermes",
                                    @"umount /result/", 2);
                                foreach (string response in list_response)
                                {
                                    logWriter.WriteLine(response);
                                }
                            }
                            //for (int i = 101; i <= 124; i++)
                            //{
                            //    commandLinux("10.8.6." + i.ToString() , "root", "hermes", @"umount /result/", 2);
                            //}
                        }
                        //02
                        if (step <= 2)
                        {
                            XmlNodeList twoNodes = configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p02/ip");
                            for (int i = 0; i < twoNodes.Count; i++)
                            {
                                list_response = commandLinux(twoNodes.Item(i).InnerText, "root", "hermes",
                                    @"umount /mnt/client/",
                                    2);
                                foreach (string response in list_response)
                                {
                                    logWriter.WriteLine(response);
                                }
                            }
                            //for (int i = 201; i <= 206; i++)
                            //{
                            //    commandLinux("10.8.6." + i.ToString() , "root", "hermes", @"umount /mnt/client/",
                            //        2);
                            //}
                            for (int i = 0; i < twoNodes.Count; i++)
                            {
                                list_response = commandLinux(twoNodes.Item(i).InnerText, "root", "hermes",
                                    @"umount /result/", 2);
                                foreach (string response in list_response)
                                {
                                    logWriter.WriteLine(response);
                                }
                            }
                            //for (int i = 201; i <= 206; i++)
                            //{
                            //    commandLinux("10.8.6." + i.ToString() , "root", "hermes", @"umount /result/", 2);
                            //}
                        }
                        //03
                        if (step <= 3)
                        {
                            XmlNodeList threeNodes = configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p03/ip");
                            for (int i = 0; i < threeNodes.Count; i++)
                            {
                                list_response = commandLinux(threeNodes.Item(i).InnerText, "root", "hermes",
                                    @"umount /mnt/client/", 2);
                                foreach (string response in list_response)
                                {
                                    logWriter.WriteLine(response);
                                }
                            }
                            //for (int i = 2; i <= 5; i++)
                            //{
                            //    commandLinux("10.8.6." + i.ToString() , "root", "hermes", @"umount /mnt/client/",
                            //        2);
                            //}
                        }
                        //04
                        if (step <= 4)
                        {
                            XmlNodeList fourNodes = configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p04/ip");
                            for (int i = 0; i < fourNodes.Count; i++)
                            {
                                list_response = commandLinux(fourNodes.Item(i).InnerText, "root", "hermes",
                                    @"umount /result/", 2);
                                foreach (string response in list_response)
                                {
                                    logWriter.WriteLine(response);
                                }
                            }
                            //for (int i = 1; i <= 5; i++)
                            //{
                            //    commandLinux("10.8.6." + i.ToString() , "root", "hermes", @"umount /result/", 2);
                            //}
                        }
                        //05
                        if (step <= 5)
                        {
                            XmlNodeList fiveNodes =
                                configuration_xml.SelectNodes(
                                    @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p05/ip");
                            for (int i = 0; i < fiveNodes.Count; i++)
                            {
                                list_response = commandLinux(fiveNodes.Item(i).InnerText, "root", "hermes",
                                    @"umount /mnt/client/", 2);
                                foreach (string response in list_response)
                                {
                                    logWriter.WriteLine(response);
                                }
                            }
                            //commandLinux(@"10.8.6.200", "root", "hermes", @"umount /mnt/client/", 2);
                            pause();
                        }
                        //06
                        if (step <= 6)
                        {
                            XmlNodeList sixNodes = configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p06/ip");
                            for (int i = 0; i < sixNodes.Count; i++)
                            {
                                list_response = commandLinux(sixNodes.Item(i).InnerText, "root", "hermes",
                                    @"umount /mnt/client/", 2);
                                foreach (string response in list_response)
                                {
                                    logWriter.WriteLine(response);
                                }
                            }
                            //commandLinux(@"10.8.6.254", "root", "hermes", @"umount /mnt/client/", 2);
                            pause();

                            Invoke(new Action(() =>
                            {
                                msgBox.Text += "umount /mnt/clinet & /result/  Finish !" + Environment.NewLine;
                                msgBox.Text += "Start to Disconnect all Winsows Mapping Disk" + Environment.NewLine;
                            }));
                            pause();
                            list_response = executeDosCmd(@"rmdir D:\hmi\gds");
                            foreach (string response in list_response)
                            {
                                Invoke(new Action(() => { msgBox.Text += response; }));
                                logWriter.WriteLine(response);
                            }
                            list_response = executeDosCmd(@"net use * /del /y");
                            foreach (string response in list_response)
                            {
                                Invoke(new Action(() => { msgBox.Text += response; }));
                                logWriter.WriteLine(response);
                            }
                            Invoke(new Action(() =>
                            {
                                msgBox.Text += "All Windows mapping disk locate at this PC were Disconnect !" +
                                               Environment.NewLine;
                            }));
                            pause();
                            Invoke(new Action(() =>
                            {
                                msgBox.Text += "Start to Create folder please close  All MobaXterm programs !" +
                                               Environment.NewLine;
                            }));

                        }
                        //07
                        if (step <= 7)
                        {
                            string cmdADC_subfolder_create =
                                @"cd /result/hmi2005 && mkdir Auto_Environment && chmod 777 Auto_Environment/";
                            XmlNodeList sevenNodes =
                                configuration_xml.SelectNodes(
                                    @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p07/ip");
                            for (int i = 0; i < sevenNodes.Count; i++)
                            {
                                list_response = commandLinux(sevenNodes.Item(i).InnerText, "root", "hermes",
                                    cmdADC_subfolder_create, 0);
                                foreach (string response in list_response)
                                {
                                    logWriter.WriteLine(response);
                                }
                            }
                        }
                        //commandLinux(@"10.8.6.199", "root", "hermes", cmdADC_subfolder_create, 0);
                        //08
                        if (step <= 8)
                        {
                            XmlNodeList eightNodes =
                                configuration_xml.SelectNodes(
                                    @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p08/ip");
                            for (int i = 0; i < eightNodes.Count; i++)
                            {
                                list_response = commandLinux(eightNodes.Item(i).InnerText, "root", "hermes",
                                    @"cd /usr/src/", 0);
                                foreach (string response in list_response)
                                {
                                    logWriter.WriteLine(response);
                                }
                            }
                            //commandLinux(@"10.8.6.200", "root", "hermes", @"cd /usr/src/", 0);
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
                            XmlNodeList nineNodes =
                                configuration_xml.SelectNodes(
                                    @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p09/ip");
                            for (int i = 0; i < nineNodes.Count; i++)
                            {
                                list_response = commandLinux(nineNodes.Item(i).InnerText, "root", "hermes",
                                    cmd_folder_create_sh, 0);
                                foreach (string response in list_response)
                                {
                                    logWriter.WriteLine(response);
                                }
                            }
                            //commandLinux(@"10.8.6.200", "root", "hermes", cmd_folder_create_sh, 0);
                            pause();
                            Invoke(new Action(() =>
                            {
                                msgBox.Text += @" Start to Auto create /PFMIN folder at FW-Ic0 IP:10.8.6.204" +
                                               Environment.NewLine;
                            }));
                        }
                        //10
                        if (step <= 10)
                        {
                            XmlNodeList tenNodes =
                                configuration_xml.SelectNodes(
                                    @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p10/ip");
                            foreach (XmlNode node in tenNodes)
                            {
                                list_response = commandLinux(node.InnerText, "root", "hermes",
                                    "cd .. && mkdir PFMIN && chmod 777 PFMIN",
                                    0);
                                foreach (string response in list_response)
                                {
                                    logWriter.WriteLine(response);
                                }
                            }
                            //commandLinux(@"10.8.6.204", "root", "hermes", "cd .. && mkdir PFMIN && chmod 777 PFMIN", 0);
                            Invoke(new Action(() =>
                            {
                                msgBox.Text +=
                                    "eP5 Storage computer & ADC computer & FW-Ic0 /PFMIN folder Auto create successful !" +
                                    Environment.NewLine;
                            }));
                            pause();
                            Invoke(new Action(() =>
                            {
                                msgBox.Text += " Start to Auto create /PFMIN folder at ep5-Ic0 IP:10.8.6.254" +
                                               Environment.NewLine;
                            }));
                        }
                        //11
                        if (step <= 11)
                        {
                            XmlNodeList elevenNodes = configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p11/ip");
                            foreach (XmlNode node in elevenNodes)
                            {
                                list_response = commandLinux(node.InnerText, "root", "hermes",
                                    "cd .. && mkdir PFMIN && chmod 777 PFMIN",
                                    0);
                                foreach (string response in list_response)
                                {
                                    logWriter.WriteLine(response);
                                }
                            }
                            //commandLinux(@"10.8.6.254", "root", "hermes", "cd .. && mkdir PFMIN && chmod 777 PFMIN", 0);
                            Invoke(new Action(() =>
                            {
                                msgBox.Text +=
                                    "eP5 Storage computer & ADC computer & ep5-IC0/PFMIN/ folder, Auto create successful !" +
                                    Environment.NewLine;
                            }));
                            pause();
                            msgBox.Text += "start Storage PC known issue Auto debug!" + Environment.NewLine;

                        }
                        //12
                        if (step <= 12)
                        {
                            string cmd_ifcfg_debug =
                                @"cd /etc/sysconfig/network-scripts/ && rm -rf ifcfg-eth2 && rm -rf ifcfg-eth3 && service network restart";
                            XmlNodeList twelveNodes =
                                configuration_xml.SelectNodes(
                                    @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p12/ip");
                            foreach (XmlNode node in twelveNodes)
                            {
                                list_response = commandLinux(node.InnerText, "root", "hermes", cmd_ifcfg_debug, 0);
                                foreach (string response in list_response)
                                {
                                    logWriter.WriteLine(response);
                                }
                            }
                            //commandLinux(@"10.8.6.200", "root", "hermes", cmd_ifcfg_debug, 0);
                            Invoke(new Action(() =>
                            {
                                msgBox.Text += "eP5 Storage computer ifcfg-eth2 & ifcfg - eth3 Auto debug finish !" +
                                               Environment.NewLine;
                            }));
                            pause();
                        }
                        //13
                        SFTP sftp = null;
                        bool isConnectSuccess = false;
                        string path_fstab = "";
                        if (step <= 13)
                        {
                            XmlNode thirteenNode = configuration_xml.SelectSingleNode(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p13/ip");
                            sftp = new SFTP(thirteenNode.InnerText, "root", "hermes");

                            //SFTP sftp = new SFTP(@"10.8.6.254", "root", "hermes");
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
                            XmlNode fourteenNode = configuration_xml.SelectSingleNode(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p14/ip");
                            sftp = new SFTP(fourteenNode.InnerText, "root", "hermes");
                            //sftp = new SFTP(@"10.8.6.200", "root", "hermes");
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
                            Invoke(new Action(() =>
                            {
                                msgBox.Text +=
                                    @"eP5 Ic0 & Storage PC fstab update finish please check it with cat /etc/fstab command !" +
                                    Environment.NewLine;
                            }));
                            string samba_conf_path =
                                @"C:\HMI\sn_online\Version_setting_upgrade\samba_setting\storage\smb.conf";
                            sftp.Put(samba_conf_path, "/etc/samba");
                            sftp.Disconnect();
                        }
                        //15
                        if (step <= 15)
                        {
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
                            //for (int i = 201; i <= 206; i++)
                            //{
                            //    sftp = new SFTP(@"10.8.6." + i.ToString(), "root", "hermes");
                            //    isConnectSuccess = sftp.Connect();
                            //    sftp.Put(@"C:\HMI\sn_online\Version_setting_upgrade\samba_setting\hpc\smb.conf", "/etc/samba");
                            //    Thread.Sleep(1000);
                            //    sftp.Disconnect();
                            //}
                            pause();
                            Invoke(new Action(() =>
                            {
                                msgBox.Text += "sama service restart for eP5!" + Environment.NewLine;
                            }));
                        }
                        //18
                        if (step <= 18)
                        {
                            XmlNodeList eighteenNodes = configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eP5VersionSettingUpgrade/p18/ip");
                            foreach (XmlNode node in eighteenNodes)
                            {
                                list_response = commandLinux(node.InnerText, "root", "hermes",
                                    @"/etc/init.d/smb stop && /etc/init.d/smb start",
                                    1);
                                foreach (string response in list_response)
                                {
                                    logWriter.WriteLine(response);
                                }
                            }
                            //commandLinux(@"10.8.6.200", "root", "hermes",
                            //    @"/etc/init.d/smb stop && /etc/init.d/smb start",
                            //    1);
                            //commandLinux(@"10.8.6.201", "root", "hermes",
                            //    @"/etc/init.d/smb stop && /etc/init.d/smb start",
                            //    1);
                            //commandLinux(@"10.8.6.199", "root", "hermes",
                            //    @"/etc/init.d/smb stop && /etc/init.d/smb start", 1);
                            //commandLinux(@"10.8.6.254", "root", "hermes",
                            //    @"/etc/init.d/smb stop && /etc/init.d/smb start", 1);
                            Invoke(new Action(() =>
                            {
                                msgBox.Text +=
                                    "eP5 HPC1~6 & Storage & Ic0 & ADC computer  samb.config update Finish !" +
                                    Environment.NewLine;
                            }));
                            pause();
                            Invoke(new Action(() =>
                            {
                                msgBox.Text += "Stat to upgrade eP5 all computer rc.local setting ! ! !" +
                                               Environment.NewLine;
                            }));
                            pause();
                        }
                        //19
                        if (step <= 19)
                        {
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
                            //for (int i = 201; i <= 206; i++)
                            //{
                            //    sftp = new SFTP(@"10.8.6." + i.ToString(), "root", "hermes");
                            //    isConnectSuccess = sftp.Connect();
                            //    sftp.Put(@"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\hpc\rc.local", "/etc");
                            //    Thread.Sleep(1000);
                            //    sftp.Disconnect();
                            //}
                        }
                        //20
                        if (step <= 20)
                        {
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
                            //for (int i = 101; i <= 124; i++)
                            //{
                            //    sftp = new SFTP(@"10.8.6." + i.ToString(), "root", "hermes");
                            //    isConnectSuccess = sftp.Connect();
                            //    sftp.Put(@"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\client\rc.local",
                            //        "/etc");
                            //    Thread.Sleep(1000);
                            //    sftp.Disconnect();
                            //}
                        }
                        //21
                        if (step <= 21)
                        {
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
                            //for (int i = 2; i <= 5; i++)
                            //{
                            //    sftp = new SFTP(@"10.8.6." + i.ToString(), "root", "hermes");
                            //    isConnectSuccess = sftp.Connect();
                            //    sftp.Put(@"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\client\rc.local",
                            //        "/etc");
                            //    Thread.Sleep(1000);
                            //    sftp.Disconnect();
                            //}
                            pause();
                        }
                        //22
                        if (step <= 22)
                        {
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
                            Invoke(new Action(() =>
                            {
                                msgBox.Text +=
                                    "eP5 All computer rc.local setting update finish, please reboot PC as sequence , FW reboot >> Storage reboot >> IC0 reboot >> ADC reboot >> HPC reboot >> Client reboot !" +
                                    Environment.NewLine;
                            }));
                            pause();
                        }
                    }
                    catch (XmlException xmlexception)
                    {
                        Invoke(new Action(() =>
                        {
                            msgBox.Text += "xml parse error" + Environment.NewLine;
                            logWriter.WriteLine("xml parse error");
                            StackTrace st = new StackTrace(xmlexception, true);
                            StackFrame[] frames = st.GetFrames();
                            foreach (StackFrame frame in frames)
                            {
                                msgBox.Text += "function name:" + frame.GetMethod().Name + " line:" +
                                               frame.GetFileLineNumber()
                                               + " file:" + frame.GetFileName() + Environment.NewLine;
                                logWriter.WriteLine("function name:" + frame.GetMethod().Name + " line:" +
                                                    frame.GetFileLineNumber()
                                                    + " file:" + frame.GetFileName());
                            }
                            msgBox.Text += xmlexception.Message + " LineNumber=" + xmlexception.LineNumber +
                                           " LinePosition=" +
                                           xmlexception.LinePosition +
                                           Environment.NewLine;
                            logWriter.WriteLine(xmlexception.Message + " LineNumber=" + xmlexception.LineNumber +
                                                " LinePosition=" +
                                                xmlexception.LinePosition);
                        }));
                    }
                    catch (System.Xml.XPath.XPathException exception)
                    {
                        Invoke(new Action(() =>
                        {
                            msgBox.Text += exception.Message + Environment.NewLine;
                            logWriter.WriteLine(exception.Message);

                            StackTrace st = new StackTrace(exception, true);
                            StackFrame[] frames = st.GetFrames();
                            foreach (StackFrame frame in frames)
                            {
                                msgBox.Text += "function name:" + frame.GetMethod().Name + " line:" +
                                               frame.GetFileLineNumber()
                                               + " file:" + frame.GetFileName() + Environment.NewLine;
                                logWriter.WriteLine("function name:" + frame.GetMethod().Name + " line:" +
                                                    frame.GetFileLineNumber()
                                                    + " file:" + frame.GetFileName());
                            }
                        }));
                    }
                    catch (Exception exception)
                    {
                        Invoke(new Action(() =>
                        {
                            msgBox.Text += exception.Message + Environment.NewLine;
                            logWriter.WriteLine(exception.Message);

                            StackTrace st = new StackTrace(exception, true);
                            StackFrame[] frames = st.GetFrames();
                            foreach (StackFrame frame in frames)
                            {
                                msgBox.Text += "function name:" + frame.GetMethod().Name + " line:" +
                                               frame.GetFileLineNumber()
                                               + " file:" + frame.GetFileName() + Environment.NewLine;
                                logWriter.WriteLine("function name:" + frame.GetMethod().Name + " line:" +
                                                    frame.GetFileLineNumber()
                                                    + " file:" + frame.GetFileName());
                            }
                        }));
                    }

                }

                Invoke(new Action(() =>
                {
                    buttonSetting.Enabled = true;
                }));
            });

            buttonSetting.Enabled = false;
            t.Start();
        }

        private void msgBox_TextChanged(object sender, EventArgs e)
        {
            
            msgBox.SelectionStart = msgBox.Text.Length;
            msgBox.ScrollToCaret();
            
        }

        private void msgBox420_TextChanged(object sender, EventArgs e)
        {

            msgBox420.SelectionStart = msgBox420.Text.Length;
            msgBox420.ScrollToCaret();

        }

        private void buttonSetting420_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
                
                Invoke(new Action(() =>
                {
                    msgBox420.Text +=
                        @"eScan420-ASE_D2DB Version setting upgrade start ,please mention about all upgrade infomrmation show at plateform !" +
                        Environment.NewLine;
                }));
                pause420();
                Invoke(new Action(() =>
                {
                    msgBox420.Text +=
                        @"This Active character only avaliable at eScan420-FW PC , before do it please confirm currenct location  ! ! ! " +
                        Environment.NewLine;
                }));
                pause420();
                Invoke(new Action(() =>
                {
                    msgBox420.Text += @"Start Auto umount /mnt/clinet for All image computer !" +
                                        Environment.NewLine;
                }));
                pause420();

                int step = 0;
                try
                {
                    step = int.Parse(whichStepBox420.Text);
                }
                catch (Exception ex)
                {
                    step = 0;
                    if (ex is ArgumentNullException || ex is FormatException || ex is OverflowException)
                    {
                        MessageBox.Show("step is still 0 even though " + ex.GetType().ToString() + " happen");
                    }
                    else
                    {
                        MessageBox.Show("step is still 0 even though there is an unknown exception in parse function:" + " " + ex.Message+"\n"+"type: "+ex.GetType().ToString());
                    }
                }


                using (StreamWriter sw = new StreamWriter("eScan420-ASE_D2DB.log"))
                {
                    try
                    {
                        XmlDocument configuration_xml = new XmlDocument();
                        configuration_xml.Load("configuration.xml");

                        List<string> list_response;
                        //01
                        if (step <= 1)
                        {
                            XmlNodeList oneNodeList = configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eScan420VersionSettingUpgrade/p01/ip");
                            foreach (XmlNode node in oneNodeList)
                            {
                                list_response = commandLinux420(node.InnerText, "root", "hermes",
                                    @"umount /mnt/client/",
                                    2);
                                foreach (string response in list_response)
                                {
                                    sw.WriteLine(response);
                                }
                            }
                            foreach (XmlNode node in oneNodeList)
                            {
                                list_response = commandLinux420(node.InnerText, "root", "hermes",
                                    @"umount /result/",
                                    2);
                                foreach (string response in list_response)
                                {
                                    sw.WriteLine(response);
                                }
                            }
                        }
                        //02
                        if (step <= 2)
                        {
                            XmlNodeList twoNodeList = configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eScan420VersionSettingUpgrade/p02/ip");
                            foreach (XmlNode node in twoNodeList)
                            {
                                list_response = commandLinux420(node.InnerText, "root", "hermes",
                                    @"umount /mnt/client/",
                                    2);
                                foreach (string response in list_response)
                                {
                                    sw.WriteLine(response);
                                }
                            }
                            foreach (XmlNode node in twoNodeList)
                            {
                                list_response = commandLinux420(node.InnerText, "root", "hermes",
                                    @"umount /result/",
                                    2);
                                foreach (string response in list_response)
                                {
                                    sw.WriteLine(response);
                                }
                            }
                        }
                        //03
                        if (step <= 3)
                        {
                            XmlNodeList threeNodeList = configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eScan420VersionSettingUpgrade/p03/ip");
                            foreach (XmlNode node in threeNodeList)
                            {
                                list_response = commandLinux420(node.InnerText, "root", "hermes",
                                    @"umount /mnt/client/",
                                    2);
                                foreach (string response in list_response)
                                {
                                    sw.WriteLine(response);
                                }
                            }
                            pause420();
                            Invoke(new Action(() =>
                            {
                                msgBox420.Text += @"umount /mnt/clinet & /result/  Finish !" + Environment.NewLine;
                                msgBox420.Text +=
                                    @"Start to Disconnect all Winsows Mapping Disk, please type in Y  !" +
                                    Environment.NewLine;
                            }));
                            pause420();
                            list_response = executeDosCmd420(@"rmdir D:\hmi\gds");
                            foreach (string response in list_response)
                            {
                                sw.WriteLine(response);
                            }
                            list_response = executeDosCmd420(@"net use * /del /y");
                            foreach (string response in list_response)
                            {
                                sw.WriteLine(response);
                            }
                            Invoke(new Action(() =>
                            {
                                msgBox420.Text += "All Windows mapping disk locate at this PC were Disconnect !" +
                                                    Environment.NewLine;
                            }));
                            pause420();
                            Invoke(new Action(() =>
                            {
                                msgBox420.Text += "Sart to Create folder please close  All MobaXterm programs !" +
                                                    Environment.NewLine;
                            }));
                        }
                        //04
                        if (step <= 4)
                        {
                            XmlNode fourNode = configuration_xml.SelectSingleNode(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eScan420VersionSettingUpgrade/p04/ip");
                            list_response = commandLinux420(fourNode.InnerText, "root", "hermes",
                                @"cd /result/hmi2005 && mkdir Auto_Environment && chmod 777 Auto_Environment/", 2);
                            foreach (string response in list_response)
                            {
                                sw.WriteLine(response);
                            }
                        }
                        //05
                        if (step <= 5)
                        {
                            string folder_create_content = @"cd /result &&
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
                            XmlNode fiveNode = configuration_xml.SelectSingleNode(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eScan420VersionSettingUpgrade/p05/ip");
                            list_response = commandLinux420(fiveNode.InnerText, "root", "hermes",
                                folder_create_content,
                                0);
                            foreach (string response in list_response)
                            {
                                sw.WriteLine(response);
                            }
                            pause420();
                        }
                        //06
                        if (step <= 6)
                        {
                            Invoke(new Action(() =>
                            {
                                msgBox420.Text += @" Start to Auto create /PFMIN folder at FW-Ic0 IP:10.8.6.204" +
                                                    Environment.NewLine;
                            }));
                            string pfmin_create_content = @"cd .. && mkdir PFMIN &&
                            chmod 777 PFMIN";
                            XmlNode sixNode = configuration_xml.SelectSingleNode(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eScan420VersionSettingUpgrade/p06/ip");
                            list_response = commandLinux420(sixNode.InnerText, "root", "hermes",
                                pfmin_create_content,
                                0);
                            foreach (string response in list_response)
                            {
                                sw.WriteLine(response);
                            }
                            Invoke(new Action(() =>
                            {
                                msgBox420.Text +=
                                    @"eP5 Storage computer & ADC computer & FW-Ic0 /PFMIN folder Auto create successful !" +
                                    Environment.NewLine;
                            }));
                            pause420();
                        }
                        //07
                        if (step <= 7)
                        {
                            Invoke(new Action(() =>
                            {
                                msgBox420.Text += @" Start to Auto create /PFMIN folder at FW-Ic0 IP:10.8.6.254" +
                                                    Environment.NewLine;
                            }));
                            string pfmin_create_content = @"cd .. && mkdir PFMIN &&
                            chmod 777 PFMIN";
                            XmlNode sevenNode = configuration_xml.SelectSingleNode(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eScan420VersionSettingUpgrade/p07/ip");
                            list_response = commandLinux420(sevenNode.InnerText, "root", "hermes",
                                pfmin_create_content,
                                0);
                            foreach (string response in list_response)
                            {
                                sw.WriteLine(response);
                            }
                            Invoke(new Action(() =>
                            {
                                msgBox420.Text +=
                                    @"eP5 Storage computer & ADC computer & ep5-IC0/PFMIN/ folder, Auto create successful !" +
                                    Environment.NewLine;
                            }));
                            pause420();
                        }
                        //08
                        if (step <= 8)
                        {
                            Invoke(new Action(() =>
                            {
                                msgBox420.Text += @"start Storage PC known issue Auto debug!" + Environment.NewLine;
                            }));
                            XmlNode eightNode = configuration_xml.SelectSingleNode(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eScan420VersionSettingUpgrade/p08/ip");
                            list_response = commandLinux420(eightNode.InnerText, "root", "hermes",
                                @"cd /etc/sysconfig/network-scripts/ && rm -rf ifcfg-eth2 && rm -rf ifcfg-eth3 && service network restart",
                                0);
                            foreach (string response in list_response)
                            {
                                sw.WriteLine(response);
                            }
                            Invoke(new Action(() =>
                            {
                                msgBox420.Text +=
                                    @"eP5 Storage computer ifcfg-eth2 & ifcfg-eth3 Auto debug finish !" +
                                    Environment.NewLine;
                            }));
                            pause420();
                        }
                        SFTP sftp;
                        //09
                        if (step <= 9)
                        {
                            XmlNodeList nineNodeList = configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eScan420VersionSettingUpgrade/p09/ip");
                            foreach (XmlNode node in nineNodeList)
                            {
                                sftp = new SFTP(node.InnerText, "root", "hermes");
                                if (!sftp.Connect())
                                {
                                    throw new Exception("sftp connect fail in line " +
                                                        new StackTrace(true).GetFrame(0).GetFileLineNumber());
                                }
                                if (!sftp.Put(@"C:\HMI\sn_online\Version_setting_upgrade\fstab\fstab", @"/etc"))
                                {
                                    throw new Exception("sftp put fail in line " +
                                                        new StackTrace(true).GetFrame(0).GetFileLineNumber());
                                }
                                sftp.Disconnect();
                            }
                        }
                        //10
                        if (step <= 10)
                        {
                            Invoke(new Action(() =>
                            {
                                msgBox420.Text +=
                                    "eP5 Ic0 & Storage PC fstab update finish please check it with cat /etc/fstab command !" +
                                    Environment.NewLine;
                            }));
                            XmlNode tenNode = configuration_xml.SelectSingleNode(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eScan420VersionSettingUpgrade/p10/ip");
                            sftp = new SFTP(tenNode.InnerText, "root", "hermes");
                            if (!sftp.Connect())
                            {
                                throw new Exception("sftp connect fail in line " +
                                                    new StackTrace(true).GetFrame(0).GetFileLineNumber());
                            }
                            if (!sftp.Put(
                                @"C:\HMI\sn_online\Version_setting_upgrade\samba_setting\storage\smb.conf",
                                @"/etc/samba"))
                            {
                                throw new Exception("sftp put fail in line " +
                                                    new StackTrace(true).GetFrame(0).GetFileLineNumber());
                            }
                            sftp.Disconnect();
                        }
                        //11
                        if (step <= 11)
                        {
                            XmlNode elevenNode = configuration_xml.SelectSingleNode(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eScan420VersionSettingUpgrade/p11/ip");
                            sftp = new SFTP(elevenNode.InnerText, "root", "hermes");
                            if (!sftp.Connect())
                            {
                                throw new Exception("sftp connect fail in line " +
                                                    new StackTrace(true).GetFrame(0).GetFileLineNumber());
                            }
                            if (!sftp.Put(@"C:\HMI\sn_online\Version_setting_upgrade\samba_setting\adc\smb.conf",
                                @"/etc/samba"))
                            {
                                throw new Exception("sftp put fail in line " +
                                                    new StackTrace(true).GetFrame(0).GetFileLineNumber());
                            }
                            sftp.Disconnect();
                        }
                        //12
                        if (step <= 12)
                        {
                            XmlNode twelveNode = configuration_xml.SelectSingleNode(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eScan420VersionSettingUpgrade/p12/ip");
                            sftp = new SFTP(twelveNode.InnerText, "root", "hermes");
                            if (!sftp.Connect())
                            {
                                throw new Exception("sftp connect fail in line " +
                                                    new StackTrace(true).GetFrame(0).GetFileLineNumber());
                            }
                            if (!sftp.Put(@"C:\HMI\sn_online\Version_setting_upgrade\samba_setting\ic0\smb.conf",
                                @"/etc/samba"))
                            {
                                throw new Exception("sftp put fail in line " +
                                                    new StackTrace(true).GetFrame(0).GetFileLineNumber());
                            }
                            sftp.Disconnect();
                        }
                        //13
                        if (step <= 13)
                        {
                            XmlNodeList thirteenNodeList = configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eScan420VersionSettingUpgrade/p13/ip");
                            foreach (XmlNode node in thirteenNodeList)
                            {
                                sftp = new SFTP(node.InnerText, "root", "hermes");
                                if (!sftp.Connect())
                                {
                                    throw new Exception("sftp connect fail in line " +
                                                        new StackTrace(true).GetFrame(0).GetFileLineNumber());
                                }
                                if (!sftp.Put(
                                    @"C:\HMI\sn_online\Version_setting_upgrade\samba_setting\hpc\smb.conf",
                                    @"/etc/samba"))
                                {
                                    throw new Exception("sftp put fail in line " +
                                                        new StackTrace(true).GetFrame(0).GetFileLineNumber());
                                }
                                sftp.Disconnect();
                                Thread.Sleep(1000);
                            }
                            pause420();
                        }
                        //14
                        if (step <= 14)
                        {
                            Invoke(new Action(() =>
                            {
                                msgBox420.Text += "sama service restart for eScan420!" + Environment.NewLine;
                            }));
                            XmlNodeList fourteenNodeList = configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eScan420VersionSettingUpgrade/p14/ip");
                            foreach (XmlNode node in fourteenNodeList)
                            {
                                list_response = commandLinux420(node.InnerText, "root", "hermes",
                                    @"/etc/init.d/smb stop && /etc/init.d/smb start", 1);
                                foreach (string response in list_response)
                                {
                                    sw.WriteLine(response);
                                }
                            }
                            Invoke(new Action(() =>
                            {
                                msgBox420.Text +=
                                    @"eP5 HPC1~6 & Storage & Ic0 & ADC computer  samb.config update Finish !" +
                                    Environment.NewLine;
                            }));
                            pause420();
                            Invoke(new Action(() =>
                            {
                                msgBox420.Text += @"Stat to upgrade eScan420 all computer rc.local setting ! ! !" +
                                                    Environment.NewLine;
                            }));
                            pause420();
                        }
                        //15
                        if (step <= 15)
                        {
                            XmlNodeList fifteenNodeList = configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eScan420VersionSettingUpgrade/p15/ip");
                            foreach (XmlNode node in fifteenNodeList)
                            {
                                sftp = new SFTP(node.InnerText, "root", "hermes");
                                if (!sftp.Connect())
                                {
                                    throw new Exception("sftp connect fail in line " +
                                                        new StackTrace(true).GetFrame(0).GetFileLineNumber());
                                }
                                if (!sftp.Put(
                                    @"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\hpc\rc.local",
                                    @"/etc"))
                                {
                                    throw new Exception("sftp put fail in line " +
                                                        new StackTrace(true).GetFrame(0).GetFileLineNumber());
                                }
                                sftp.Disconnect();
                                Thread.Sleep(1000);
                            }
                        }
                        //16
                        if (step <= 16)
                        {
                            XmlNodeList sixteenNodeList = configuration_xml.SelectNodes(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eScan420VersionSettingUpgrade/p16/ip");
                            foreach (XmlNode node in sixteenNodeList)
                            {
                                sftp = new SFTP(node.InnerText, "root", "hermes");
                                if (!sftp.Connect())
                                {
                                    throw new Exception("sftp connect fail in line " +
                                                        new StackTrace(true).GetFrame(0).GetFileLineNumber());
                                }
                                if (!sftp.Put(
                                    @"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\client\rc.local",
                                    @"/etc"))
                                {
                                    throw new Exception("sftp put fail in line " +
                                                        new StackTrace(true).GetFrame(0).GetFileLineNumber());
                                }
                                sftp.Disconnect();
                                Thread.Sleep(1000);
                            }
                        }
                        //17
                        if (step <= 17)
                        {
                            XmlNode seventeenNode = configuration_xml.SelectSingleNode(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eScan420VersionSettingUpgrade/p17/ip");
                            sftp = new SFTP(seventeenNode.InnerText, "root", "hermes");
                            if (!sftp.Connect())
                            {
                                throw new Exception("sftp connect fail in line " +
                                                    new StackTrace(true).GetFrame(0).GetFileLineNumber());
                            }
                            if (!sftp.Put(
                                @"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\storage\rc.local",
                                @"/etc"))
                            {
                                throw new Exception("sftp put fail in line " +
                                                    new StackTrace(true).GetFrame(0).GetFileLineNumber());
                            }
                            sftp.Disconnect();
                        }
                        //18
                        if (step <= 18)
                        {
                            XmlNode eighteenNode = configuration_xml.SelectSingleNode(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eScan420VersionSettingUpgrade/p18/ip");
                            sftp = new SFTP(eighteenNode.InnerText, "root", "hermes");
                            if (!sftp.Connect())
                            {
                                throw new Exception("sftp connect fail in line " +
                                                    new StackTrace(true).GetFrame(0).GetFileLineNumber());
                            }
                            if (!sftp.Put(
                                @"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\adc\rc.local",
                                @"/etc"))
                            {
                                throw new Exception("sftp put fail in line " +
                                                    new StackTrace(true).GetFrame(0).GetFileLineNumber());
                            }
                            sftp.Disconnect();
                        }
                        //19
                        if (step <= 19)
                        {
                            XmlNode nineteenNode = configuration_xml.SelectSingleNode(
                                @"/ConfigurationSetting/VersionSettingUpgrade/ASE_D2DB-eScan420VersionSettingUpgrade/p19/ip");
                            sftp = new SFTP(nineteenNode.InnerText, "root", "hermes");
                            if (!sftp.Connect())
                            {
                                throw new Exception("sftp connect fail in line " +
                                                    new StackTrace(true).GetFrame(0).GetFileLineNumber());
                            }
                            if (!sftp.Put(
                                @"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\ico\rc.local",
                                @"/etc"))
                            {
                                throw new Exception("sftp put fail in line " +
                                                    new StackTrace(true).GetFrame(0).GetFileLineNumber());
                            }
                            sftp.Disconnect();
                            Thread.Sleep(1000);
                            Invoke(new Action(() =>
                            {
                                msgBox420.Text +=
                                    "eScan420 All computer rc.local setting update finish ,please reboot PC as sequence , FW reboot >> Storage reboot >> IC0 reboot >> ADC reboot >> HPC reboot >> Client reboot !" +
                                    Environment.NewLine;
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        StackTrace st = new StackTrace(ex, true);
                        StackFrame[] frames = st.GetFrames();
                        foreach (StackFrame frame in frames)
                        {
                            Invoke(new Action(() =>
                            {
                                msgBox420.Text += frame.GetMethod().Name + " launch " + ex.GetType().ToString() +
                                                    "in " + frame.GetFileLineNumber() + " line and " +
                                                    frame.GetFileColumnNumber() + " column of" + frame.GetFileName() +
                                                    Environment.NewLine;
                            }));
                            sw.WriteLine(frame.GetMethod().Name + " launch " + ex.GetType().ToString() +
                                            "in " + frame.GetFileLineNumber() + " line and " +
                                            frame.GetFileColumnNumber() + " column of" + frame.GetFileName());
                        }
                    }
                }
                
                Invoke(new Action(() =>
                {
                    buttonSetting420.Enabled = true;
                }));
            });
            t.IsBackground = true;
            buttonSetting420.Enabled = false;
            t.Start();
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
