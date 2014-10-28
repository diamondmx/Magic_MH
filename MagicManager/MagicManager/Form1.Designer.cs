namespace MagicManager
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
			this.components = new System.ComponentModel.Container();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.playerTab = new System.Windows.Forms.TabPage();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.matchTab = new System.Windows.Forms.TabPage();
			this.dataGridView2 = new System.Windows.Forms.DataGridView();
			this.player1DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.player2DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.roundDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.eventDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.player1WinsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.player2WinsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.drawsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.inProgressDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.matchesBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.magicDataSet1 = new MagicManager.MagicDataSet1();
			this.scoresTab = new System.Windows.Forms.TabPage();
			this.PlayerScores = new System.Windows.Forms.DataGridView();
			this.playersWinsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.magicDataSet_Matches_Players = new MagicManager.MagicDataSet_Matches_Players();
			this.magicDataSetMatchesPlayersBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.matchesTableAdapter = new MagicManager.MagicDataSet1TableAdapters.MatchesTableAdapter();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.playersBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.magicDataSet = new MagicManager.MagicDataSet();
			this.magicDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.playersTableAdapter = new MagicManager.MagicDataSetTableAdapters.PlayersTableAdapter();
			this.nameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.scoreDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.gameWinsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabControl.SuspendLayout();
			this.playerTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.matchTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.matchesBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.magicDataSet1)).BeginInit();
			this.scoresTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PlayerScores)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.playersWinsBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.magicDataSet_Matches_Players)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.magicDataSetMatchesPlayersBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.playersBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.magicDataSet)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.magicDataSetBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.playerTab);
			this.tabControl.Controls.Add(this.matchTab);
			this.tabControl.Controls.Add(this.scoresTab);
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(763, 341);
			this.tabControl.TabIndex = 0;
			// 
			// playerTab
			// 
			this.playerTab.AutoScroll = true;
			this.playerTab.Controls.Add(this.dataGridView1);
			this.playerTab.Location = new System.Drawing.Point(4, 22);
			this.playerTab.Name = "playerTab";
			this.playerTab.Padding = new System.Windows.Forms.Padding(3);
			this.playerTab.Size = new System.Drawing.Size(755, 315);
			this.playerTab.TabIndex = 0;
			this.playerTab.Text = "Players";
			this.playerTab.UseVisualStyleBackColor = true;
			// 
			// dataGridView1
			// 
			this.dataGridView1.AutoGenerateColumns = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn});
			this.dataGridView1.DataSource = this.playersBindingSource;
			this.dataGridView1.Location = new System.Drawing.Point(0, 0);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(755, 265);
			this.dataGridView1.TabIndex = 0;
			// 
			// matchTab
			// 
			this.matchTab.Controls.Add(this.dataGridView2);
			this.matchTab.Location = new System.Drawing.Point(4, 22);
			this.matchTab.Name = "matchTab";
			this.matchTab.Padding = new System.Windows.Forms.Padding(3);
			this.matchTab.Size = new System.Drawing.Size(755, 315);
			this.matchTab.TabIndex = 1;
			this.matchTab.Text = "Matches";
			this.matchTab.UseVisualStyleBackColor = true;
			// 
			// dataGridView2
			// 
			this.dataGridView2.AutoGenerateColumns = false;
			this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.player1DataGridViewTextBoxColumn,
            this.player2DataGridViewTextBoxColumn,
            this.roundDataGridViewTextBoxColumn,
            this.eventDataGridViewTextBoxColumn,
            this.player1WinsDataGridViewTextBoxColumn,
            this.player2WinsDataGridViewTextBoxColumn,
            this.drawsDataGridViewTextBoxColumn,
            this.inProgressDataGridViewCheckBoxColumn});
			this.dataGridView2.DataSource = this.matchesBindingSource;
			this.dataGridView2.Location = new System.Drawing.Point(0, 0);
			this.dataGridView2.Name = "dataGridView2";
			this.dataGridView2.Size = new System.Drawing.Size(759, 312);
			this.dataGridView2.TabIndex = 0;
			// 
			// player1DataGridViewTextBoxColumn
			// 
			this.player1DataGridViewTextBoxColumn.DataPropertyName = "Player1";
			this.player1DataGridViewTextBoxColumn.HeaderText = "Player1";
			this.player1DataGridViewTextBoxColumn.Name = "player1DataGridViewTextBoxColumn";
			// 
			// player2DataGridViewTextBoxColumn
			// 
			this.player2DataGridViewTextBoxColumn.DataPropertyName = "Player2";
			this.player2DataGridViewTextBoxColumn.HeaderText = "Player2";
			this.player2DataGridViewTextBoxColumn.Name = "player2DataGridViewTextBoxColumn";
			// 
			// roundDataGridViewTextBoxColumn
			// 
			this.roundDataGridViewTextBoxColumn.DataPropertyName = "Round";
			this.roundDataGridViewTextBoxColumn.HeaderText = "Round";
			this.roundDataGridViewTextBoxColumn.Name = "roundDataGridViewTextBoxColumn";
			// 
			// eventDataGridViewTextBoxColumn
			// 
			this.eventDataGridViewTextBoxColumn.DataPropertyName = "Event";
			this.eventDataGridViewTextBoxColumn.HeaderText = "Event";
			this.eventDataGridViewTextBoxColumn.Name = "eventDataGridViewTextBoxColumn";
			// 
			// player1WinsDataGridViewTextBoxColumn
			// 
			this.player1WinsDataGridViewTextBoxColumn.DataPropertyName = "Player1Wins";
			this.player1WinsDataGridViewTextBoxColumn.HeaderText = "Player1Wins";
			this.player1WinsDataGridViewTextBoxColumn.Name = "player1WinsDataGridViewTextBoxColumn";
			// 
			// player2WinsDataGridViewTextBoxColumn
			// 
			this.player2WinsDataGridViewTextBoxColumn.DataPropertyName = "Player2Wins";
			this.player2WinsDataGridViewTextBoxColumn.HeaderText = "Player2Wins";
			this.player2WinsDataGridViewTextBoxColumn.Name = "player2WinsDataGridViewTextBoxColumn";
			// 
			// drawsDataGridViewTextBoxColumn
			// 
			this.drawsDataGridViewTextBoxColumn.DataPropertyName = "Draws";
			this.drawsDataGridViewTextBoxColumn.HeaderText = "Draws";
			this.drawsDataGridViewTextBoxColumn.Name = "drawsDataGridViewTextBoxColumn";
			// 
			// inProgressDataGridViewCheckBoxColumn
			// 
			this.inProgressDataGridViewCheckBoxColumn.DataPropertyName = "InProgress";
			this.inProgressDataGridViewCheckBoxColumn.HeaderText = "InProgress";
			this.inProgressDataGridViewCheckBoxColumn.Name = "inProgressDataGridViewCheckBoxColumn";
			// 
			// matchesBindingSource
			// 
			this.matchesBindingSource.DataMember = "Matches";
			this.matchesBindingSource.DataSource = this.magicDataSet1;
			// 
			// magicDataSet1
			// 
			this.magicDataSet1.DataSetName = "MagicDataSet1";
			this.magicDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// scoresTab
			// 
			this.scoresTab.Controls.Add(this.PlayerScores);
			this.scoresTab.Location = new System.Drawing.Point(4, 22);
			this.scoresTab.Name = "scoresTab";
			this.scoresTab.Padding = new System.Windows.Forms.Padding(3);
			this.scoresTab.Size = new System.Drawing.Size(755, 315);
			this.scoresTab.TabIndex = 2;
			this.scoresTab.Text = "Scores";
			this.scoresTab.UseVisualStyleBackColor = true;
			// 
			// PlayerScores
			// 
			this.PlayerScores.AutoGenerateColumns = false;
			this.PlayerScores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.PlayerScores.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn1,
            this.scoreDataGridViewTextBoxColumn,
            this.gameWinsDataGridViewTextBoxColumn});
			this.PlayerScores.DataMember = "PlayerScores";
			this.PlayerScores.DataSource = this.magicDataSetBindingSource;
			this.PlayerScores.Location = new System.Drawing.Point(0, 0);
			this.PlayerScores.Name = "PlayerScores";
			this.PlayerScores.ShowRowErrors = false;
			this.PlayerScores.Size = new System.Drawing.Size(664, 236);
			this.PlayerScores.TabIndex = 0;
			// 
			// magicDataSet_Matches_Players
			// 
			this.magicDataSet_Matches_Players.DataSetName = "MagicDataSet_Matches_Players";
			this.magicDataSet_Matches_Players.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// magicDataSetMatchesPlayersBindingSource
			// 
			this.magicDataSetMatchesPlayersBindingSource.DataSource = this.magicDataSet_Matches_Players;
			this.magicDataSetMatchesPlayersBindingSource.Position = 0;
			// 
			// matchesTableAdapter
			// 
			this.matchesTableAdapter.ClearBeforeFill = true;
			// 
			// nameDataGridViewTextBoxColumn
			// 
			this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			// 
			// playersBindingSource
			// 
			this.playersBindingSource.DataMember = "Players";
			this.playersBindingSource.DataSource = this.magicDataSet;
			// 
			// magicDataSet
			// 
			this.magicDataSet.DataSetName = "MagicDataSet";
			this.magicDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// magicDataSetBindingSource
			// 
			this.magicDataSetBindingSource.DataSource = this.magicDataSet;
			this.magicDataSetBindingSource.Position = 0;
			// 
			// playersTableAdapter
			// 
			this.playersTableAdapter.ClearBeforeFill = true;
			// 
			// nameDataGridViewTextBoxColumn1
			// 
			this.nameDataGridViewTextBoxColumn1.DataPropertyName = "Name";
			this.nameDataGridViewTextBoxColumn1.HeaderText = "Name";
			this.nameDataGridViewTextBoxColumn1.Name = "nameDataGridViewTextBoxColumn1";
			// 
			// scoreDataGridViewTextBoxColumn
			// 
			this.scoreDataGridViewTextBoxColumn.DataPropertyName = "Score";
			this.scoreDataGridViewTextBoxColumn.HeaderText = "Score";
			this.scoreDataGridViewTextBoxColumn.Name = "scoreDataGridViewTextBoxColumn";
			this.scoreDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// gameWinsDataGridViewTextBoxColumn
			// 
			this.gameWinsDataGridViewTextBoxColumn.DataPropertyName = "GameWins";
			this.gameWinsDataGridViewTextBoxColumn.HeaderText = "GameWins";
			this.gameWinsDataGridViewTextBoxColumn.Name = "gameWinsDataGridViewTextBoxColumn";
			this.gameWinsDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(775, 341);
			this.Controls.Add(this.tabControl);
			this.Name = "Form1";
			this.Text = "MagicManager";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.tabControl.ResumeLayout(false);
			this.playerTab.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.matchTab.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.matchesBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.magicDataSet1)).EndInit();
			this.scoresTab.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PlayerScores)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.playersWinsBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.magicDataSet_Matches_Players)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.magicDataSetMatchesPlayersBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.playersBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.magicDataSet)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.magicDataSetBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage playerTab;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.TabPage matchTab;
		private MagicDataSet magicDataSet;
		private System.Windows.Forms.BindingSource playersBindingSource;
		private MagicDataSetTableAdapters.PlayersTableAdapter playersTableAdapter;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridView dataGridView2;
		private System.Windows.Forms.BindingSource magicDataSetMatchesPlayersBindingSource;
		private MagicDataSet_Matches_Players magicDataSet_Matches_Players;
		private MagicDataSet1 magicDataSet1;
		private System.Windows.Forms.BindingSource matchesBindingSource;
		private MagicDataSet1TableAdapters.MatchesTableAdapter matchesTableAdapter;
		private System.Windows.Forms.DataGridViewTextBoxColumn player1DataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn player2DataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn roundDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn eventDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn player1WinsDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn player2WinsDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn drawsDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn inProgressDataGridViewCheckBoxColumn;
		private System.Windows.Forms.TabPage scoresTab;
		private System.Windows.Forms.BindingSource playersWinsBindingSource;
		private System.Windows.Forms.DataGridView PlayerScores;
		private System.Windows.Forms.BindingSource magicDataSetBindingSource;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn scoreDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn gameWinsDataGridViewTextBoxColumn;
	}
}

