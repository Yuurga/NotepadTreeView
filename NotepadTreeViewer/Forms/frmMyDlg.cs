using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NotepadTreeViewer.Utils;
using NppPluginNET;



namespace NotepadTreeViewer
{
    public partial class frmMyDlg : Form
    {
        public frmMyDlg()
        {
            InitializeComponent();
            imageList1.Images.Add(ShellIcon.GetLargeFolderIcon());
            imageList1.Images.Add(ShellIcon.GetSmallIconFromExtension(".txt"));
            fileTree.ImageList = imageList1;
            SetTree();

        }

        private void SetTree()
        {
            fileTree.Nodes.Clear();
            foreach (var drive in System.IO.DriveInfo.GetDrives())
            {
                TreeNode driveItem = new TreeNode();
                if (drive.IsReady)
                    driveItem.Text = String.Format(@"{0}:\", drive.Name[0]);
                else
                    driveItem.Text = String.Format(@"{0}:\", drive.Name[0]);

                fileTree.Nodes.Add(driveItem);
                FillChildNodes(driveItem,2);
                
                
            }

            fileTree.Refresh();

        }


        void FillChildNodes(TreeNode node, int deep)
        {
            String exFolder ="";
            try
            {
                DirectoryInfo dirs = new DirectoryInfo(node.FullPath);
                if (dirs.GetDirectories().Length < 1 && dirs.GetFiles().Length < 1)
                    return;

                foreach (DirectoryInfo dir in dirs.GetDirectories())
                {
         
                    TreeNode newnode = new TreeNode(dir.Name);
                    newnode.ImageIndex = 0;
                    newnode.SelectedImageIndex = 0;
                    exFolder =dir.Name;
                    node.Nodes.Add(newnode);
                    if (deep > 1)
                        FillChildNodes(newnode, deep - 1);
                }
                foreach (FileInfo f in dirs.GetFiles())
                {
                    TreeNode t = new TreeNode(f.Name);
                    t.ImageIndex = 1 ;
                    t.SelectedImageIndex = 1;
                    node.Nodes.Add(t);
                }

            }
            catch (Exception ex)
            {
               
            }
        }

        private void onExpand(object sender, TreeViewCancelEventArgs e)
        {
                e.Node.Nodes.Clear();
                FillChildNodes(e.Node, 2);
                
        }

        private void frmMyDlg_Load(object sender, EventArgs e)
        {
            
        }

        private void fileTree_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void onNodeDOubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            String name = e.Node.FullPath;            
            if(File.Exists(name)) {
                Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_DOOPEN, 0, name);
            }
        }



    }
}
