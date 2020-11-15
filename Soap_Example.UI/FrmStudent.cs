using MetroFramework;
using MetroFramework.Forms;
using Soap_Example.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Soap_Example.UI
{
    public partial class FrmStudent : MetroForm
    {
        private EntityState objState = EntityState.Unchanged;

        public FrmStudent()
        {
            InitializeComponent();
        }

        private void FrmStudent_Load(object sender, EventArgs e)
        {
            chkGender_CheckedChanged(null, null);

            try
            {
                StudentService client = new StudentService();
                studentBindingSource.DataSource = client.GetAll();
                pContainer.Enabled = false;
                LoadStudent();
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message, "Messege", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStudent()
        {
            Student obj = studentBindingSource.Current as Student;
            if (obj != null)
            {
                if (!string.IsNullOrEmpty(obj.ImageUrl))
                {
                    picStudent.Image = Image.FromFile(obj.ImageUrl);
                }
                else
                {
                    picStudent.Image = null;
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "JPEG|*.jpg|PNG|*.png", ValidateNames = true })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    picStudent.Image = Image.FromFile(openFileDialog.FileName);
                    Student obj = studentBindingSource.Current as Student;
                    if (obj != null)
                        obj.ImageUrl = openFileDialog.FileName;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            objState = EntityState.Added;
            picStudent.Image = null;
            pContainer.Enabled = true;
            List<Student> students = ((IEnumerable<Student>)studentBindingSource.DataSource).ToList();
            students.Add(new Student());
            studentBindingSource.DataSource = students.AsEnumerable();
            studentBindingSource.MoveLast();
            txtFullName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            objState = EntityState.Changed;
            pContainer.Enabled = true;
            txtFullName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            objState = EntityState.Deleted;
            if (MetroMessageBox.Show(this, "Are you sure want to delete this record?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                try
                {
                    Student obj = studentBindingSource.Current as Student;
                    if (obj != null)
                    {
                        StudentService client = new StudentService();

                        bool result = client.Delete(obj.StudentID);
                        if (result)
                        {
                            studentBindingSource.RemoveCurrent();
                            pContainer.Enabled = false;
                            picStudent.Image = null;
                            objState = EntityState.Unchanged;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MetroMessageBox.Show(this, ex.Message, "Messege", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            pContainer.Enabled = false;
            studentBindingSource.ResetBindings(false);
            FrmStudent_Load(sender, e);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                studentBindingSource.EndEdit();
                Student obj = studentBindingSource.Current as Student;
                if (obj != null)
                {
                    StudentService client = new StudentService();

                    //new student record
                    if (objState == EntityState.Added)
                    {
                        obj.StudentID = client.Insert(obj);
                    }

                    //update student record
                    else if (objState == EntityState.Changed)
                    {
                        client.Update(obj);
                    }

                    grdStudent.Refresh();
                    pContainer.Enabled = false;
                    objState = EntityState.Unchanged;
                }
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message, "Messege", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkGender_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGender.CheckState == CheckState.Checked)
            {
                chkGender.Text = "Female";
            }
            else
                chkGender.Text = "Male";
        }

        private void grdStudent_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                LoadStudent();
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, ex.Message, "Messege", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}