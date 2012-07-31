namespace HTA.Adventures.RegionTestGUI
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
            this.btnAddLocation = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtLon = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLat = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLatSearch = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtLonSearch = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtMilesSearch = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnAddLocation
            // 
            this.btnAddLocation.Location = new System.Drawing.Point(12, 105);
            this.btnAddLocation.Name = "btnAddLocation";
            this.btnAddLocation.Size = new System.Drawing.Size(100, 23);
            this.btnAddLocation.TabIndex = 0;
            this.btnAddLocation.Text = "Add Location";
            this.btnAddLocation.UseVisualStyleBackColor = true;
            this.btnAddLocation.Click += new System.EventHandler(this.btnAddLocation_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(67, 6);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 20);
            this.txtName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Address";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(67, 27);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(100, 20);
            this.txtAddress.TabIndex = 4;
            // 
            // txtLon
            // 
            this.txtLon.Location = new System.Drawing.Point(67, 53);
            this.txtLon.Name = "txtLon";
            this.txtLon.Size = new System.Drawing.Size(45, 20);
            this.txtLon.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Lon";
            // 
            // txtLat
            // 
            this.txtLat.Location = new System.Drawing.Point(67, 79);
            this.txtLat.Name = "txtLat";
            this.txtLat.Size = new System.Drawing.Size(45, 20);
            this.txtLat.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Lat";
            // 
            // txtLatSearch
            // 
            this.txtLatSearch.Location = new System.Drawing.Point(67, 169);
            this.txtLatSearch.Name = "txtLatSearch";
            this.txtLatSearch.Size = new System.Drawing.Size(45, 20);
            this.txtLatSearch.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 172);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Lat";
            // 
            // txtLonSearch
            // 
            this.txtLonSearch.Location = new System.Drawing.Point(67, 143);
            this.txtLonSearch.Name = "txtLonSearch";
            this.txtLonSearch.Size = new System.Drawing.Size(45, 20);
            this.txtLonSearch.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 146);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Lon";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(12, 227);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 23);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "Search Locations";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtMilesSearch
            // 
            this.txtMilesSearch.Location = new System.Drawing.Point(67, 195);
            this.txtMilesSearch.Name = "txtMilesSearch";
            this.txtMilesSearch.Size = new System.Drawing.Size(45, 20);
            this.txtMilesSearch.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 198);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Miles";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(157, 79);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 16;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(269, 281);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.txtMilesSearch);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtLatSearch);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtLonSearch);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtLat);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtLon);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnAddLocation);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddLocation;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtLon;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLatSearch;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtLonSearch;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtMilesSearch;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox1;
    }
}

