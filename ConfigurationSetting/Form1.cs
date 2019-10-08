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

namespace ConfigurationSetting
{
    public partial class Form1 : Form
    {
        private AutoResetEvent autoResetEvent=new AutoResetEvent(false);

        public Form1()
        {
            InitializeComponent();
        }


        private void commandLinux(string ip, string user, string password, string cmd, int sleepTime_s)
        {
            using (SshStream ssh = new SshStream(ip, user, password))
            {
                ssh.Prompt = @"\?|#|$";
                ssh.RemoveTerminalEmulationCharacters = true;
                string response = ssh.ReadResponse();
                Invoke( new Action(() => { msgBox.Text += response + Environment.NewLine; }) );
                
                ssh.Write(cmd);
                response = ssh.ReadResponse().Trim();
                Invoke(new Action(() => { msgBox.Text += response + Environment.NewLine; }));
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
                }
            }
            Thread.Sleep(sleepTime_s*1000);
        }

        private void executeDosCmd(string cmd)
        {
            using (Process process = new Process())
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = "cmd.exe";
                info.UseShellExecute = false;
                info.RedirectStandardInput = false;
                info.RedirectStandardOutput = true;
                info.CreateNoWindow = true;
                info.Arguments = "/C " + cmd;
                process.StartInfo = info;
                process.Start();
                process.WaitForExit();
                Invoke(new Action(() => { msgBox.Text += process.StandardOutput.ReadToEnd() + Environment.NewLine; }));
            }
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
        private void buttonNextStep_Click(object sender, EventArgs e)
        {
            autoResetEvent.Set();
        }
        private void buttonSetting_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
                XmlDocument configuration_xml=new XmlDocument();
                configuration_xml.Load("configuration.xml");

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
                //01
                for (int i = 101; i <= 124; i++)
                {
                    commandLinux("10.8.6." + i.ToString() , "root", "hermes", @"umount /mnt/client/",
                        2);
                }
                for (int i = 101; i <= 124; i++)
                {
                    commandLinux("10.8.6." + i.ToString() , "root", "hermes", @"umount /result/", 2);
                }
                //02
                for (int i = 201; i <= 206; i++)
                {
                    commandLinux("10.8.6." + i.ToString() , "root", "hermes", @"umount /mnt/client/",
                        2);
                }
                for (int i = 201; i <= 206; i++)
                {
                    commandLinux("10.8.6." + i.ToString() , "root", "hermes", @"umount /result/", 2);
                }
                //03
                for (int i = 2; i <= 5; i++)
                {
                    commandLinux("10.8.6." + i.ToString() , "root", "hermes", @"umount /mnt/client/",
                        2);
                }
                //04
                for (int i = 1; i <= 5; i++)
                {
                    commandLinux("10.8.6." + i.ToString() , "root", "hermes", @"umount /result/", 2);
                }
                //05
                commandLinux(@"10.8.6.200", "root", "hermes", @"umount /mnt/client/", 2);
                pause();
                //06
                commandLinux(@"10.8.6.254", "root", "hermes", @"umount /mnt/client/", 2);
                pause();

                Invoke(new Action(() =>
                {
                    msgBox.Text += "umount /mnt/clinet & /result/  Finish !" + Environment.NewLine;
                    msgBox.Text += "Start to Disconnect all Winsows Mapping Disk" + Environment.NewLine;
                }));
                pause();
                executeDosCmd(@"rmdir D:\hmi\gds");
                executeDosCmd(@"net use * /del");
                Invoke(new Action(() =>
                {
                    msgBox.Text += "All Windows mapping disk locate at this PC were Disconnect !" + Environment.NewLine;
                }));
                pause();
                Invoke(new Action(() =>
                {
                    msgBox.Text += "Start to Create folder please close  All MobaXterm programs !" +
                                   Environment.NewLine;
                }));
                string cmdADC_subfolder_create =
                    @"cd /result/hmi2005 && mkdir Auto_Environment && chmod 777 Auto_Environment/";
                //07
                commandLinux(@"10.8.6.199", "root", "hermes", cmdADC_subfolder_create, 0);
                //08
                commandLinux(@"10.8.6.200", "root", "hermes", @"cd /usr/src/", 0);
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
                commandLinux(@"10.8.6.200", "root", "hermes", cmd_folder_create_sh, 0);
                pause();
                Invoke(new Action(() =>
                {
                    msgBox.Text += @" Start to Auto create /PFMIN folder at FW-Ic0 IP:10.8.6.204" + Environment.NewLine;
                }));
                //10
                commandLinux(@"10.8.6.204", "root", "hermes", "cd .. && mkdir PFMIN && chmod 777 PFMIN", 0);
                Invoke(new Action(() =>
                {
                    msgBox.Text +=
                        "eP5 Storage computer & ADC computer & FW-Ic0 /PFMIN folder Auto create successful !" +
                        Environment.NewLine;
                }));
                pause();
                Invoke(new Action(() =>
                {
                    msgBox.Text += " Start to Auto create /PFMIN folder at ep5-Ic0 IP:10.8.6.254" + Environment.NewLine;
                }));
                //11
                commandLinux(@"10.8.6.254", "root", "hermes", "cd .. && mkdir PFMIN && chmod 777 PFMIN", 0);
                Invoke(new Action(() =>
                {
                    msgBox.Text +=
                        "eP5 Storage computer & ADC computer & ep5-IC0/PFMIN/ folder, Auto create successful !" +
                        Environment.NewLine;
                }));
                pause();
                msgBox.Text += "start Storage PC known issue Auto debug!" + Environment.NewLine;
                string cmd_ifcfg_debug =
                    @"cd /etc/sysconfig/network-scripts/ && rm -rf ifcfg-eth2 && rm -rf ifcfg-eth3 && service network restart";
                //12
                commandLinux(@"10.8.6.200", "root", "hermes", cmd_ifcfg_debug, 0);
                Invoke(new Action(() =>
                {
                    msgBox.Text += "eP5 Storage computer ifcfg-eth2 & ifcfg - eth3 Auto debug finish !" +
                                   Environment.NewLine;
                }));
                pause();
                //13
                SFTP sftp = new SFTP(@"10.8.6.254", "root", "hermes");
                bool isConnectSuccess=sftp.Connect();
                string path_fstab = @"C:\HMI\sn_online\Version_setting_upgrade\fstab\fstab";
                sftp.Put(path_fstab, @"/etc");
                sftp.Disconnect();
                pause();
                //14
                sftp = new SFTP(@"10.8.6.200", "root", "hermes");
                isConnectSuccess = sftp.Connect();
                sftp.Put(path_fstab, @"/etc");
                pause();
                Invoke(new Action(() =>
                {
                    msgBox.Text +=
                        "eP5 Ic0 & Storage PC fstab update finish please check it with cat /etc/fstab command !" +
                        Environment.NewLine;
                }));
                string samba_conf_path = @"C:\HMI\sn_online\Version_setting_upgrade\samba_setting\storage\smb.conf";
                sftp.Put(samba_conf_path, "/etc/samba");
                sftp.Disconnect();
                //15
                sftp = new SFTP(@"10.8.6.199", "root", "hermes");
                isConnectSuccess = sftp.Connect();
                sftp.Put(@"C:\HMI\sn_online\Version_setting_upgrade\samba_setting\adc\smb.conf", "/etc/samba");
                sftp.Disconnect();
                //16
                sftp = new SFTP(@"10.8.6.254", "root", "hermes");
                isConnectSuccess = sftp.Connect();
                sftp.Put(@"C:\HMI\sn_online\Version_setting_upgrade\samba_setting\ic0\smb.conf", "/etc/samba");
                sftp.Disconnect();
                //17
                for (int i = 201; i <= 206; i++)
                {
                    sftp = new SFTP(@"10.8.6." + i.ToString(), "root", "hermes");
                    isConnectSuccess = sftp.Connect();
                    sftp.Put(@"C:\HMI\sn_online\Version_setting_upgrade\samba_setting\hpc\smb.conf", "/etc/samba");
                    Thread.Sleep(1000);
                    sftp.Disconnect();
                }
                pause();
                Invoke(new Action(() =>
                {
                    msgBox.Text += "sama service restart for eP5!" + Environment.NewLine;
                }));
                //18
                commandLinux(@"10.8.6.200", "root", "hermes",
                    @"/etc/init.d/smb stop && /etc/init.d/smb start",
                    1);
                commandLinux(@"10.8.6.201", "root", "hermes",
                    @"/etc/init.d/smb stop && /etc/init.d/smb start",
                    1);
                commandLinux(@"10.8.6.199", "root", "hermes",
                    @"/etc/init.d/smb stop && /etc/init.d/smb start", 1);
                commandLinux(@"10.8.6.254", "root", "hermes",
                    @"/etc/init.d/smb stop && /etc/init.d/smb start", 1);
                Invoke(new Action(() =>
                {
                    msgBox.Text += "eP5 HPC1~6 & Storage & Ic0 & ADC computer  samb.config update Finish !" +
                                   Environment.NewLine;
                }));
                pause();
                Invoke(new Action(() =>
                {
                    msgBox.Text += "Stat to upgrade eP5 all computer rc.local setting ! ! !" + Environment.NewLine;
                }));
                pause();
                //19
                for (int i = 201; i <= 206; i++)
                {
                    sftp = new SFTP(@"10.8.6." + i.ToString(), "root", "hermes");
                    isConnectSuccess = sftp.Connect();
                    sftp.Put(@"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\hpc\rc.local", "/etc");
                    Thread.Sleep(1000);
                    sftp.Disconnect();
                }
                //20
                for (int i = 101; i <= 124; i++)
                {
                    sftp = new SFTP(@"10.8.6." + i.ToString(), "root", "hermes");
                    isConnectSuccess = sftp.Connect();
                    sftp.Put(@"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\client\rc.local",
                        "/etc");
                    Thread.Sleep(1000);
                    sftp.Disconnect();
                }
                //21
                for (int i = 2; i <= 5; i++)
                {
                    sftp = new SFTP(@"10.8.6." + i.ToString(), "root", "hermes");
                    isConnectSuccess = sftp.Connect();
                    sftp.Put(@"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\client\rc.local",
                        "/etc");
                    Thread.Sleep(1000);
                    sftp.Disconnect();
                }
                pause();

                sftp = new SFTP("10.8.6.1", "root", "hermes");
                isConnectSuccess = sftp.Connect();
                sftp.Put(@"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\glm\rc.local", "/etc");
                sftp.Disconnect();

                sftp = new SFTP("10.8.6.200", "root", "hermes");
                isConnectSuccess = sftp.Connect();
                sftp.Put(@"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\storage\rc.local", "/etc");
                sftp.Disconnect();

                sftp = new SFTP("10.8.6.199", "root", "hermes");
                isConnectSuccess = sftp.Connect();
                sftp.Put(@"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\adc\rc.local", "/etc");
                sftp.Disconnect();

                sftp = new SFTP("10.8.6.254", "root", "hermes");
                isConnectSuccess = sftp.Connect();
                sftp.Put(@"C:\HMI\sn_online\Version_setting_upgrade\rc.local\eScan\ASE_D2DB\ico\rc.local", "/etc");
                sftp.Disconnect();

                Thread.Sleep(1000);
                Invoke(new Action(() =>
                {
                    msgBox.Text +=
                        "eP5 All computer rc.local setting update finish, please reboot PC as sequence , FW reboot >> Storage reboot >> IC0 reboot >> ADC reboot >> HPC reboot >> Client reboot !" +
                        Environment.NewLine;
                }));
                pause();
            });
            t.Start();

        }

        private void msgBox_TextChanged(object sender, EventArgs e)
        {
            msgBox.SelectionStart = msgBox.Text.Length;
            msgBox.ScrollToCaret();
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
