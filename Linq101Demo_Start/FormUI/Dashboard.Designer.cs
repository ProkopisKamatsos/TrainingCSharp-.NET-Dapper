
namespace FormUI
{
  partial class Dashboaard
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
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.yearsExperiencePicker = new System.Windows.Forms.NumericUpDown();
      this.updatePersonButton = new System.Windows.Forms.Button();
      this.allPeopleDropDown = new System.Windows.Forms.ComboBox();
      this.filteredPeopleList = new System.Windows.Forms.ListBox();
      ((System.ComponentModel.ISupportInitialize)(this.yearsExperiencePicker)).BeginInit();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(18, 22);
      this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(188, 29);
      this.label1.TabIndex = 0;
      this.label1.Text = "Linq 101 Demo";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(23, 71);
      this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(96, 24);
      this.label2.TabIndex = 1;
      this.label2.Text = "All People";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(300, 71);
      this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(105, 24);
      this.label3.TabIndex = 2;
      this.label3.Text = "Filtered List";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(23, 168);
      this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(161, 24);
      this.label4.TabIndex = 3;
      this.label4.Text = "Years Experience";
      // 
      // yearsExperiencePicker
      // 
      this.yearsExperiencePicker.Location = new System.Drawing.Point(27, 205);
      this.yearsExperiencePicker.Name = "yearsExperiencePicker";
      this.yearsExperiencePicker.Size = new System.Drawing.Size(74, 29);
      this.yearsExperiencePicker.TabIndex = 4;
      // 
      // updatePersonButton
      // 
      this.updatePersonButton.Location = new System.Drawing.Point(64, 270);
      this.updatePersonButton.Name = "updatePersonButton";
      this.updatePersonButton.Size = new System.Drawing.Size(129, 48);
      this.updatePersonButton.TabIndex = 5;
      this.updatePersonButton.Text = "Update";
      this.updatePersonButton.UseVisualStyleBackColor = true;
      this.updatePersonButton.Click += new System.EventHandler(this.updatePersonButton_Click);
      // 
      // allPeopleDropDown
      // 
      this.allPeopleDropDown.FormattingEnabled = true;
      this.allPeopleDropDown.Location = new System.Drawing.Point(27, 110);
      this.allPeopleDropDown.Name = "allPeopleDropDown";
      this.allPeopleDropDown.Size = new System.Drawing.Size(203, 32);
      this.allPeopleDropDown.TabIndex = 6;
      this.allPeopleDropDown.SelectedIndexChanged += new System.EventHandler(this.allPeopleDropDown_SelectedIndexChanged);
      // 
      // filteredPeopleList
      // 
      this.filteredPeopleList.FormattingEnabled = true;
      this.filteredPeopleList.ItemHeight = 24;
      this.filteredPeopleList.Location = new System.Drawing.Point(304, 110);
      this.filteredPeopleList.Name = "filteredPeopleList";
      this.filteredPeopleList.Size = new System.Drawing.Size(180, 244);
      this.filteredPeopleList.TabIndex = 7;
      // 
      // Dashboaard
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(522, 406);
      this.Controls.Add(this.filteredPeopleList);
      this.Controls.Add(this.allPeopleDropDown);
      this.Controls.Add(this.updatePersonButton);
      this.Controls.Add(this.yearsExperiencePicker);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
      this.Name = "Dashboaard";
      this.Text = "Linq 101 Demo";
      ((System.ComponentModel.ISupportInitialize)(this.yearsExperiencePicker)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.NumericUpDown yearsExperiencePicker;
    private System.Windows.Forms.Button updatePersonButton;
    private System.Windows.Forms.ComboBox allPeopleDropDown;
    private System.Windows.Forms.ListBox filteredPeopleList;
  }
}