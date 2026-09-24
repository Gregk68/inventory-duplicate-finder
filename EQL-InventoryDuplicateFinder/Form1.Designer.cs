namespace InventoryDuplicateFinder;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        lblFile = new Label();
        txtFilePath = new TextBox();
        btnBrowse = new Button();
        btnAnalyze = new Button();
        btnAbout = new Button();
        btnToggleExaltation = new Button();
        btnClear = new Button();
        lblSummary = new Label();
        lblIgnoredItems = new Label();
        lblWarnings = new Label();
        dgvDuplicates = new DataGridView();
        dgvIgnored = new DataGridView();
        dgvWarnings = new DataGridView();
        ((System.ComponentModel.ISupportInitialize)dgvDuplicates).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvIgnored).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvWarnings).BeginInit();
        SuspendLayout();
        // 
        // lblFile
        // 
        lblFile.AutoSize = true;
        lblFile.Location = new Point(12, 15);
        lblFile.Name = "lblFile";
        lblFile.Size = new Size(69, 15);
        lblFile.TabIndex = 0;
        lblFile.Text = "Inventory file";
        // 
        // txtFilePath
        // 
        txtFilePath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtFilePath.Location = new Point(101, 12);
        txtFilePath.Name = "txtFilePath";
        txtFilePath.Size = new Size(569, 23);
        txtFilePath.TabIndex = 1;
        // 
        // btnBrowse
        // 
        btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBrowse.Location = new Point(676, 11);
        btnBrowse.Name = "btnBrowse";
        btnBrowse.Size = new Size(96, 25);
        btnBrowse.TabIndex = 2;
        btnBrowse.Text = "Browse...";
        btnBrowse.UseVisualStyleBackColor = true;
        btnBrowse.Click += btnBrowse_Click;
        // 
        // btnAnalyze
        // 
        btnAnalyze.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAnalyze.Location = new Point(676, 43);
        btnAnalyze.Name = "btnAnalyze";
        btnAnalyze.Size = new Size(96, 27);
        btnAnalyze.TabIndex = 3;
        btnAnalyze.Text = "Analyze";
        btnAnalyze.UseVisualStyleBackColor = true;
        btnAnalyze.Click += btnAnalyze_Click;
        // 
        // btnAbout
        // 
        btnAbout.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        btnAbout.Location = new Point(12, 43);
        btnAbout.Name = "btnAbout";
        btnAbout.Size = new Size(80, 27);
        btnAbout.TabIndex = 4;
        btnAbout.Text = "About";
        btnAbout.UseVisualStyleBackColor = true;
        btnAbout.Click += btnAbout_Click;
        // 
        // btnToggleExaltation
        // 
        btnToggleExaltation.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnToggleExaltation.Location = new Point(544, 43);
        btnToggleExaltation.Name = "btnToggleExaltation";
        btnToggleExaltation.Size = new Size(126, 27);
        btnToggleExaltation.TabIndex = 5;
        btnToggleExaltation.Text = "Toggle Exaltation";
        btnToggleExaltation.UseVisualStyleBackColor = true;
        btnToggleExaltation.Click += btnToggleExaltation_Click;
        // 
        // btnClear
        // 
        btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnClear.Location = new Point(676, 77);
        btnClear.Name = "btnClear";
        btnClear.Size = new Size(96, 27);
        btnClear.TabIndex = 6;
        btnClear.Text = "Clear";
        btnClear.UseVisualStyleBackColor = true;
        btnClear.Click += btnClear_Click;
        // 
        // lblSummary
        // 
        lblSummary.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblSummary.AutoSize = true;
        lblSummary.Location = new Point(12, 84);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(38, 15);
        lblSummary.TabIndex = 5;
        lblSummary.Text = "Ready";
        // 
        // lblIgnoredItems
        // 
        lblIgnoredItems.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lblIgnoredItems.AutoSize = true;
        lblIgnoredItems.Location = new Point(12, 268);
        lblIgnoredItems.Name = "lblIgnoredItems";
        lblIgnoredItems.Size = new Size(76, 15);
        lblIgnoredItems.TabIndex = 6;
        lblIgnoredItems.Text = "Ignored Items";
        // 
        // lblWarnings
        // 
        lblWarnings.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblWarnings.AutoSize = true;
        lblWarnings.Location = new Point(12, 358);
        lblWarnings.Name = "lblWarnings";
        lblWarnings.Size = new Size(85, 15);
        lblWarnings.TabIndex = 9;
        lblWarnings.Text = "Warning Lines";
        // 
        // dgvDuplicates
        // 
        dgvDuplicates.AllowUserToAddRows = false;
        dgvDuplicates.AllowUserToDeleteRows = false;
        dgvDuplicates.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        dgvDuplicates.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvDuplicates.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvDuplicates.Location = new Point(12, 115);
        dgvDuplicates.Name = "dgvDuplicates";
        dgvDuplicates.ReadOnly = true;
        dgvDuplicates.Size = new Size(760, 145);
        dgvDuplicates.TabIndex = 7;
        dgvDuplicates.CellClick += dgvDuplicates_CellClick;
        // 
        // dgvIgnored
        // 
        dgvIgnored.AllowUserToAddRows = false;
        dgvIgnored.AllowUserToDeleteRows = false;
        dgvIgnored.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        dgvIgnored.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvIgnored.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvIgnored.Location = new Point(12, 286);
        dgvIgnored.Name = "dgvIgnored";
        dgvIgnored.ReadOnly = true;
        dgvIgnored.Size = new Size(760, 65);
        dgvIgnored.TabIndex = 8;
        dgvIgnored.CellClick += dgvIgnored_CellClick;
        // 
        // dgvWarnings
        // 
        dgvWarnings.AllowUserToAddRows = false;
        dgvWarnings.AllowUserToDeleteRows = false;
        dgvWarnings.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvWarnings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvWarnings.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvWarnings.Location = new Point(12, 376);
        dgvWarnings.Name = "dgvWarnings";
        dgvWarnings.ReadOnly = true;
        dgvWarnings.Size = new Size(760, 93);
        dgvWarnings.TabIndex = 10;
        // 
        // Form1
        // 
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 481);
        Controls.Add(dgvWarnings);
        Controls.Add(dgvIgnored);
        Controls.Add(lblIgnoredItems);
        Controls.Add(lblWarnings);
        Controls.Add(dgvDuplicates);
        Controls.Add(lblSummary);
        Controls.Add(btnClear);
        Controls.Add(btnToggleExaltation);
        Controls.Add(btnAbout);
        Controls.Add(btnAnalyze);
        Controls.Add(btnBrowse);
        Controls.Add(txtFilePath);
        Controls.Add(lblFile);
        MinimumSize = new Size(800, 520);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "EQL Inventory Duplicate Finder";
        ((System.ComponentModel.ISupportInitialize)dgvDuplicates).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvIgnored).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvWarnings).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblFile;
    private TextBox txtFilePath;
    private Button btnBrowse;
    private Button btnAnalyze;
    private Button btnAbout;
    private Button btnToggleExaltation;
    private Button btnClear;
    private Label lblSummary;
    private Label lblIgnoredItems;
    private Label lblWarnings;
    private DataGridView dgvDuplicates;
    private DataGridView dgvIgnored;
    private DataGridView dgvWarnings;
}
