using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Assembly assenbly = null;
        public Form1()
        {
            InitializeComponent();
            FillListBox();
        }

        void FillListBox()
        {
            Array memberInfo = Enum.GetValues(typeof(MemberTypes));

            for (int i = 0; i < memberInfo.Length; i++)
            {
                int index = checkedListBox1.Items.Add(memberInfo.GetValue(i));
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;

                try
                {
                    assenbly = Assembly.LoadFile(path);
                    textBox1.Text = "Сборка " + path + " успішно загружена" + Environment.NewLine + Environment.NewLine;
                }
                catch (FileNotFoundException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(assenbly == null)
            {
                textBox1.Text = "Ви не вибрали файл";
                return;
            }
            textBox1.Text = "";

            textBox1.Text += "Список всіх типів збірки " + assenbly.FullName + Environment.NewLine + Environment.NewLine;

            Type[] types = assenbly.GetTypes();
            foreach (var type in types)
            {
                textBox1.Text += "Типи: " + type + Environment.NewLine;
                object[] typesAttributes = type.GetCustomAttributes(false);

                if(typesAttributes.Length > 0 && checkBox1.Checked)
                {
                    textBox1.Text += "Атрибути типу: ";
                    foreach (var atribute in typesAttributes)
                    {
                        textBox1.Text = atribute + Environment.NewLine;
                    }
                }

                var members = type.GetMembers();
                if (members != null)
                {
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        if (checkedListBox1.GetItemChecked(i))
                        {
                            object element = Enum.Parse(typeof(MemberTypes), checkedListBox1.Items[i].ToString());
                            MemberTypes memberType = (MemberTypes)element;

                            foreach (var member in members)
                            {
                                if(member.MemberType == memberType)
                                {
                                    string methStr = member.MemberType.ToString().ToUpper() + " " + member.Name + "\n";

                                    textBox1.Text = methStr + Environment.NewLine;

                                    object[] memberAttributes = member.GetCustomAttributes(false);
                                    if(memberAttributes.Length > 0 && checkBox2.Checked)
                                    {
                                        textBox1.Text += "Атрибути членів: ";
                                        foreach (var attributr in memberAttributes)
                                        {
                                            textBox1.Text += attributr + Environment.NewLine;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                textBox1.Text += Environment.NewLine;
            }
        }
    }
}
