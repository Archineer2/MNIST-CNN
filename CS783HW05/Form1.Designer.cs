namespace CS783HW05
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.StartButton01 = new System.Windows.Forms.Button();
            this.taskProgressBar01 = new System.Windows.Forms.ProgressBar();
            this.ThresholdCheckBox01 = new System.Windows.Forms.CheckBox();
            this.StopButton02 = new System.Windows.Forms.Button();
            this.Label07 = new System.Windows.Forms.Label();
            this.label07_TASK = new System.Windows.Forms.Label();
            this.Label01 = new System.Windows.Forms.Label();
            this.Label02 = new System.Windows.Forms.Label();
            this.Label03 = new System.Windows.Forms.Label();
            this.Label04 = new System.Windows.Forms.Label();
            this.Label05 = new System.Windows.Forms.Label();
            this.Label06 = new System.Windows.Forms.Label();
            this.label06_TEST = new System.Windows.Forms.Label();
            this.label05_TRAIN = new System.Windows.Forms.Label();
            this.label04_EPOCH = new System.Windows.Forms.Label();
            this.label03_THR = new System.Windows.Forms.Label();
            this.label02_ETA = new System.Windows.Forms.Label();
            this.label01_HL = new System.Windows.Forms.Label();
            this.OutputCheckBox01 = new System.Windows.Forms.CheckBox();
            this.UpdateButton03 = new System.Windows.Forms.Button();
            this.textBox01_HL = new System.Windows.Forms.TextBox();
            this.textBox02_ETA = new System.Windows.Forms.TextBox();
            this.textBox03_THR = new System.Windows.Forms.TextBox();
            this.Label08 = new System.Windows.Forms.Label();
            this.label08_CURR = new System.Windows.Forms.Label();
            this.OutputCheckBox02 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // StartButton01
            // 
            this.StartButton01.Location = new System.Drawing.Point(12, 12);
            this.StartButton01.Name = "StartButton01";
            this.StartButton01.Size = new System.Drawing.Size(104, 54);
            this.StartButton01.TabIndex = 0;
            this.StartButton01.Text = "START";
            this.StartButton01.UseVisualStyleBackColor = true;
            this.StartButton01.Click += new System.EventHandler(this.StartButton01_Click);
            // 
            // taskProgressBar01
            // 
            this.taskProgressBar01.Location = new System.Drawing.Point(12, 415);
            this.taskProgressBar01.Maximum = 60000;
            this.taskProgressBar01.Name = "taskProgressBar01";
            this.taskProgressBar01.Size = new System.Drawing.Size(337, 23);
            this.taskProgressBar01.TabIndex = 1;
            // 
            // ThresholdCheckBox01
            // 
            this.ThresholdCheckBox01.AutoSize = true;
            this.ThresholdCheckBox01.Checked = true;
            this.ThresholdCheckBox01.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ThresholdCheckBox01.Location = new System.Drawing.Point(134, 19);
            this.ThresholdCheckBox01.Name = "ThresholdCheckBox01";
            this.ThresholdCheckBox01.Size = new System.Drawing.Size(175, 21);
            this.ThresholdCheckBox01.TabIndex = 2;
            this.ThresholdCheckBox01.Text = "Train Until Max Epochs";
            this.ThresholdCheckBox01.UseVisualStyleBackColor = true;
            // 
            // StopButton02
            // 
            this.StopButton02.Location = new System.Drawing.Point(12, 72);
            this.StopButton02.Name = "StopButton02";
            this.StopButton02.Size = new System.Drawing.Size(104, 24);
            this.StopButton02.TabIndex = 3;
            this.StopButton02.Text = "STOP";
            this.StopButton02.UseVisualStyleBackColor = true;
            this.StopButton02.Click += new System.EventHandler(this.StopButton02_Click);
            // 
            // Label07
            // 
            this.Label07.AutoSize = true;
            this.Label07.Location = new System.Drawing.Point(12, 395);
            this.Label07.Name = "Label07";
            this.Label07.Size = new System.Drawing.Size(94, 17);
            this.Label07.TabIndex = 4;
            this.Label07.Text = "Current Task:";
            // 
            // label07_TASK
            // 
            this.label07_TASK.AutoSize = true;
            this.label07_TASK.Location = new System.Drawing.Point(131, 395);
            this.label07_TASK.Name = "label07_TASK";
            this.label07_TASK.Size = new System.Drawing.Size(96, 17);
            this.label07_TASK.TabIndex = 5;
            this.label07_TASK.Text = "Initial Training";
            // 
            // Label01
            // 
            this.Label01.AutoSize = true;
            this.Label01.Location = new System.Drawing.Point(12, 119);
            this.Label01.Name = "Label01";
            this.Label01.Size = new System.Drawing.Size(157, 17);
            this.Label01.TabIndex = 6;
            this.Label01.Text = "Nodes in Hidden Layer:";
            // 
            // Label02
            // 
            this.Label02.AutoSize = true;
            this.Label02.Location = new System.Drawing.Point(12, 147);
            this.Label02.Name = "Label02";
            this.Label02.Size = new System.Drawing.Size(102, 17);
            this.Label02.TabIndex = 7;
            this.Label02.Text = "Learning Rate:";
            // 
            // Label03
            // 
            this.Label03.AutoSize = true;
            this.Label03.Location = new System.Drawing.Point(12, 175);
            this.Label03.Name = "Label03";
            this.Label03.Size = new System.Drawing.Size(121, 17);
            this.Label03.TabIndex = 8;
            this.Label03.Text = "Maximum Epochs:";
            // 
            // Label04
            // 
            this.Label04.AutoSize = true;
            this.Label04.Location = new System.Drawing.Point(12, 271);
            this.Label04.Name = "Label04";
            this.Label04.Size = new System.Drawing.Size(106, 17);
            this.Label04.TabIndex = 9;
            this.Label04.Text = "Epoch Number:";
            // 
            // Label05
            // 
            this.Label05.AutoSize = true;
            this.Label05.Location = new System.Drawing.Point(12, 288);
            this.Label05.Name = "Label05";
            this.Label05.Size = new System.Drawing.Size(126, 17);
            this.Label05.TabIndex = 10;
            this.Label05.Text = "Training Accuracy:";
            // 
            // Label06
            // 
            this.Label06.AutoSize = true;
            this.Label06.Location = new System.Drawing.Point(12, 305);
            this.Label06.Name = "Label06";
            this.Label06.Size = new System.Drawing.Size(121, 17);
            this.Label06.TabIndex = 11;
            this.Label06.Text = "Testing Accuracy:";
            // 
            // label06_TEST
            // 
            this.label06_TEST.AutoSize = true;
            this.label06_TEST.Location = new System.Drawing.Point(189, 305);
            this.label06_TEST.Name = "label06_TEST";
            this.label06_TEST.Size = new System.Drawing.Size(32, 17);
            this.label06_TEST.TabIndex = 17;
            this.label06_TEST.Text = "000";
            // 
            // label05_TRAIN
            // 
            this.label05_TRAIN.AutoSize = true;
            this.label05_TRAIN.Location = new System.Drawing.Point(189, 288);
            this.label05_TRAIN.Name = "label05_TRAIN";
            this.label05_TRAIN.Size = new System.Drawing.Size(32, 17);
            this.label05_TRAIN.TabIndex = 16;
            this.label05_TRAIN.Text = "000";
            // 
            // label04_EPOCH
            // 
            this.label04_EPOCH.AutoSize = true;
            this.label04_EPOCH.Location = new System.Drawing.Point(189, 271);
            this.label04_EPOCH.Name = "label04_EPOCH";
            this.label04_EPOCH.Size = new System.Drawing.Size(32, 17);
            this.label04_EPOCH.TabIndex = 15;
            this.label04_EPOCH.Text = "000";
            // 
            // label03_THR
            // 
            this.label03_THR.AutoSize = true;
            this.label03_THR.Location = new System.Drawing.Point(189, 175);
            this.label03_THR.Name = "label03_THR";
            this.label03_THR.Size = new System.Drawing.Size(32, 17);
            this.label03_THR.TabIndex = 14;
            this.label03_THR.Text = "000";
            // 
            // label02_ETA
            // 
            this.label02_ETA.AutoSize = true;
            this.label02_ETA.Location = new System.Drawing.Point(189, 147);
            this.label02_ETA.Name = "label02_ETA";
            this.label02_ETA.Size = new System.Drawing.Size(32, 17);
            this.label02_ETA.TabIndex = 13;
            this.label02_ETA.Text = "000";
            // 
            // label01_HL
            // 
            this.label01_HL.AutoSize = true;
            this.label01_HL.Location = new System.Drawing.Point(189, 119);
            this.label01_HL.Name = "label01_HL";
            this.label01_HL.Size = new System.Drawing.Size(32, 17);
            this.label01_HL.TabIndex = 12;
            this.label01_HL.Text = "000";
            // 
            // OutputCheckBox01
            // 
            this.OutputCheckBox01.AutoSize = true;
            this.OutputCheckBox01.Location = new System.Drawing.Point(134, 46);
            this.OutputCheckBox01.Name = "OutputCheckBox01";
            this.OutputCheckBox01.Size = new System.Drawing.Size(196, 21);
            this.OutputCheckBox01.TabIndex = 18;
            this.OutputCheckBox01.Text = "Display Result Every 1000";
            this.OutputCheckBox01.UseVisualStyleBackColor = true;
            // 
            // UpdateButton03
            // 
            this.UpdateButton03.Location = new System.Drawing.Point(12, 206);
            this.UpdateButton03.Name = "UpdateButton03";
            this.UpdateButton03.Size = new System.Drawing.Size(337, 28);
            this.UpdateButton03.TabIndex = 19;
            this.UpdateButton03.Text = "Update Variables with User Input";
            this.UpdateButton03.UseVisualStyleBackColor = true;
            this.UpdateButton03.Click += new System.EventHandler(this.UpdateButton03_Click);
            // 
            // textBox01_HL
            // 
            this.textBox01_HL.Location = new System.Drawing.Point(249, 116);
            this.textBox01_HL.Name = "textBox01_HL";
            this.textBox01_HL.Size = new System.Drawing.Size(100, 22);
            this.textBox01_HL.TabIndex = 20;
            this.textBox01_HL.Text = "60";
            this.textBox01_HL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox02_ETA
            // 
            this.textBox02_ETA.Location = new System.Drawing.Point(249, 144);
            this.textBox02_ETA.Name = "textBox02_ETA";
            this.textBox02_ETA.Size = new System.Drawing.Size(100, 22);
            this.textBox02_ETA.TabIndex = 21;
            this.textBox02_ETA.Text = "0.1";
            this.textBox02_ETA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox03_THR
            // 
            this.textBox03_THR.Location = new System.Drawing.Point(249, 172);
            this.textBox03_THR.Name = "textBox03_THR";
            this.textBox03_THR.Size = new System.Drawing.Size(100, 22);
            this.textBox03_THR.TabIndex = 22;
            this.textBox03_THR.Text = "20";
            this.textBox03_THR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label08
            // 
            this.Label08.AutoSize = true;
            this.Label08.Location = new System.Drawing.Point(12, 345);
            this.Label08.Name = "Label08";
            this.Label08.Size = new System.Drawing.Size(121, 17);
            this.Label08.TabIndex = 23;
            this.Label08.Text = "Current Accuracy:";
            // 
            // label08_CURR
            // 
            this.label08_CURR.AutoSize = true;
            this.label08_CURR.Location = new System.Drawing.Point(189, 345);
            this.label08_CURR.Name = "label08_CURR";
            this.label08_CURR.Size = new System.Drawing.Size(32, 17);
            this.label08_CURR.TabIndex = 24;
            this.label08_CURR.Text = "000";
            // 
            // OutputCheckBox02
            // 
            this.OutputCheckBox02.AutoSize = true;
            this.OutputCheckBox02.Checked = true;
            this.OutputCheckBox02.CheckState = System.Windows.Forms.CheckState.Checked;
            this.OutputCheckBox02.Location = new System.Drawing.Point(134, 73);
            this.OutputCheckBox02.Name = "OutputCheckBox02";
            this.OutputCheckBox02.Size = new System.Drawing.Size(184, 21);
            this.OutputCheckBox02.TabIndex = 25;
            this.OutputCheckBox02.Text = "Display Progress Output";
            this.OutputCheckBox02.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 450);
            this.Controls.Add(this.OutputCheckBox02);
            this.Controls.Add(this.label08_CURR);
            this.Controls.Add(this.Label08);
            this.Controls.Add(this.textBox03_THR);
            this.Controls.Add(this.textBox02_ETA);
            this.Controls.Add(this.textBox01_HL);
            this.Controls.Add(this.UpdateButton03);
            this.Controls.Add(this.OutputCheckBox01);
            this.Controls.Add(this.label06_TEST);
            this.Controls.Add(this.label05_TRAIN);
            this.Controls.Add(this.label04_EPOCH);
            this.Controls.Add(this.label03_THR);
            this.Controls.Add(this.label02_ETA);
            this.Controls.Add(this.label01_HL);
            this.Controls.Add(this.Label06);
            this.Controls.Add(this.Label05);
            this.Controls.Add(this.Label04);
            this.Controls.Add(this.Label03);
            this.Controls.Add(this.Label02);
            this.Controls.Add(this.Label01);
            this.Controls.Add(this.label07_TASK);
            this.Controls.Add(this.Label07);
            this.Controls.Add(this.StopButton02);
            this.Controls.Add(this.ThresholdCheckBox01);
            this.Controls.Add(this.taskProgressBar01);
            this.Controls.Add(this.StartButton01);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Load_Form1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton01;
        private System.Windows.Forms.ProgressBar taskProgressBar01;
        private System.Windows.Forms.CheckBox ThresholdCheckBox01;
        private System.Windows.Forms.Button StopButton02;
        private System.Windows.Forms.Label Label07;
        private System.Windows.Forms.Label label07_TASK;
        private System.Windows.Forms.Label Label01;
        private System.Windows.Forms.Label Label02;
        private System.Windows.Forms.Label Label03;
        private System.Windows.Forms.Label Label04;
        private System.Windows.Forms.Label Label05;
        private System.Windows.Forms.Label Label06;
        private System.Windows.Forms.Label label06_TEST;
        private System.Windows.Forms.Label label05_TRAIN;
        private System.Windows.Forms.Label label04_EPOCH;
        private System.Windows.Forms.Label label03_THR;
        private System.Windows.Forms.Label label02_ETA;
        private System.Windows.Forms.Label label01_HL;
        private System.Windows.Forms.CheckBox OutputCheckBox01;
        private System.Windows.Forms.Button UpdateButton03;
        private System.Windows.Forms.TextBox textBox01_HL;
        private System.Windows.Forms.TextBox textBox02_ETA;
        private System.Windows.Forms.TextBox textBox03_THR;
        private System.Windows.Forms.Label Label08;
        private System.Windows.Forms.Label label08_CURR;
        private System.Windows.Forms.CheckBox OutputCheckBox02;
    }
}

