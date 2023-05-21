using DBLayer;
using Evaluation_Manager.Models;
using Evaluation_Manager.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Evaluation_Manager
{
    public partial class FrmEvaluation : Form
    {
        private Student student;

        public Student SelectedStudent { get; private set; }

        public FrmEvaluation(Student selectedStudent)
        {
            InitializeComponent();
            student = selectedStudent;
        }


        private void FrmEvaluation_Load(object sender, EventArgs e)
        {
            SetFormText();
            var activities = ActivityRepository.GetActivities();
            cboActivities.DataSource = activities;

        }
        private void SetFormText()
        {
            Text = student.FirstName + " " + student.LastName;
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtMinForSignature_TextChanged(object sender, EventArgs e)
        {

        }

        private void numPoints_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cboActivities_SelectedIndexChanged(object sender, EventArgs e)
        {
            var currentActivity = cboActivities.SelectedItem as Activity;
            txtActivityDescription.Text = currentActivity.Description;
            txtMinForGrade.Text = currentActivity.MinPointsForGrade + "/" +
           currentActivity.MaxPoints;
            txtMinForSignature.Text = currentActivity.MinPointsForSignature + "/" +
           currentActivity.MaxPoints;
            numPoints.Minimum = 0;
            numPoints.Maximum = currentActivity.MaxPoints;
            var evaluation = EvaluationRepository.GetEvaluation(SelectedStudent, currentActivity);
            if (evaluation != null)
            {
                txtTeacher.Text = evaluation.Evaluator.ToString();
                txtEvaluationDate.Text = evaluation.EvaluationDate.ToString();
                numPoints.Value = evaluation.Points;
            }
            else
            {
                txtTeacher.Text = FrmLogin.LoggedTeacher.ToString();
                txtEvaluationDate.Text = "-";
                numPoints.Value = 0;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        public static void InsertEvaluation(Student student, Activity activity,Teacher teacher, int points)
        {
            string sql = $"INSERT INTO Evaluations (IdActivities, IdStudents, IdTeachers, EvaluationDate, Points) VALUES({ activity.Id}, { student.Id},{ teacher.Id}, GETDATE(), { points})";
             DB.OpenConnection();
            DB.ExecuteCommand(sql);
            DB.CloseConnection();
        }
        public static void UpdateEvaluation(Evaluation evaluation, Teacher teacher, int points)
        {
            string sql = $"UPDATE Evaluations SET IdTeachers = {teacher.Id},Points = { points}, EvaluationDate = GETDATE() WHERE IdActivities ={evaluation.Activity.Id} AND IdStudents = { evaluation.Student.Id }";
            DB.OpenConnection();
            DB.ExecuteCommand(sql);
            DB.CloseConnection();
        }

    }
}
