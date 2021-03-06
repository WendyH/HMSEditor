﻿namespace HMSEditorNS {
	partial class HMSEditor {
		/// <summary> 
		/// Требуется переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			MouseTimer.Dispose();
			if (InvisibleCharsStyle!=null) InvisibleCharsStyle.Dispose();
			if (SameWordsStyle != null) SameWordsStyle.Dispose();
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором компонентов

		/// <summary> 
		/// Обязательный метод для поддержки конструктора - не изменяйте 
		/// содержимое данного метода при помощи редактора кода.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HMSEditor));
			FastColoredTextBoxNS.ServiceColors serviceColors1 = new FastColoredTextBoxNS.ServiceColors();
			this.panel1 = new System.Windows.Forms.Panel();
			this.Editor = new FastColoredTextBoxNS.FastColoredTextBox();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.btnContextMenuBack = new System.Windows.Forms.ToolStripMenuItem();
			this.btnContextMenuForward = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItemUndo = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItemRedo = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
			this.btnContextMenuCommentBlock = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItemSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItemCut = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItemPaste = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
			this.btnGotoContextMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.btnContextMenuGotoDef = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
			this.btnContextMenuToggleBookmark = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItemBookmarkClear = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItemClearBreakpoints = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItemZoom100 = new System.Windows.Forms.ToolStripMenuItem();
			this.btnInsertTemplate = new System.Windows.Forms.ToolStripMenuItem();
			this.btnContextMenuToolBar = new System.Windows.Forms.ToolStripMenuItem();
			this.tsMain = new System.Windows.Forms.ToolStrip();
			this.btnNavigateBack = new System.Windows.Forms.ToolStripButton();
			this.btnNavigateForward = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.btnOpen = new System.Windows.Forms.ToolStripButton();
			this.btnSave = new System.Windows.Forms.ToolStripButton();
			this.btnPrint = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.btnCut = new System.Windows.Forms.ToolStripButton();
			this.btnCopy = new System.Windows.Forms.ToolStripButton();
			this.btnPaste = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
			this.btnUndo = new System.Windows.Forms.ToolStripButton();
			this.btnRedo = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabelFind = new System.Windows.Forms.ToolStripLabel();
			this.tbFind = new System.Windows.Forms.ToolStripTextBox();
			this.btnFindPrev = new System.Windows.Forms.ToolStripButton();
			this.btnFindNext = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.btnBookmarkPlus = new System.Windows.Forms.ToolStripButton();
			this.btnBookmarkMinus = new System.Windows.Forms.ToolStripButton();
			this.btnBookmarkPrevious = new System.Windows.Forms.ToolStripButton();
			this.btnBookmarkNext = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnGoTo = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripDropDownButtonSettings = new System.Windows.Forms.ToolStripDropDownButton();
			this.btnHighlightCurrentLine = new System.Windows.Forms.ToolStripMenuItem();
			this.btnShowLineNumbers = new System.Windows.Forms.ToolStripMenuItem();
			this.btnShowFoldingLines = new System.Windows.Forms.ToolStripMenuItem();
			this.btnInvisibleChars = new System.Windows.Forms.ToolStripMenuItem();
			this.btnVerticalLineText = new System.Windows.Forms.ToolStripMenuItem();
			this.btnEnableFolding = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
			this.btnHighlightSameWords = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSubMenuIntelliSense = new System.Windows.Forms.ToolStripMenuItem();
			this.btnSetIntelliSense = new System.Windows.Forms.ToolStripMenuItem();
			this.btnMouseHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.btnAutoIdent = new System.Windows.Forms.ToolStripMenuItem();
			this.btnMarkChangedLines = new System.Windows.Forms.ToolStripMenuItem();
			this.btnAutoIdentChars = new System.Windows.Forms.ToolStripMenuItem();
			this.btnIntelliSenseFunctions = new System.Windows.Forms.ToolStripMenuItem();
			this.btnAutoCompleteBrackets = new System.Windows.Forms.ToolStripMenuItem();
			this.btnEvaluateByMouse = new System.Windows.Forms.ToolStripMenuItem();
			this.btnHints4CtrlSpace = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
			this.btnUnderlinePascalKeywords = new System.Windows.Forms.ToolStripMenuItem();
			this.btnRedStringsHighlight = new System.Windows.Forms.ToolStripMenuItem();
			this.btnToolStripMenuItemFONT = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripButtonHotKeys = new System.Windows.Forms.ToolStripMenuItem();
			this.btnThemes = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.btnUnload = new System.Windows.Forms.ToolStripMenuItem();
			this.btnAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.Editor)).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			this.tsMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.Editor);
			this.panel1.Controls.Add(this.tsMain);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(948, 530);
			this.panel1.TabIndex = 0;
			// 
			// Editor
			// 
			this.Editor.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
			this.Editor.AutoCompleteBrackets = true;
			this.Editor.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
			this.Editor.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;]+);^\\s*(case|default)\\s*[^:]*(?<" +
    "range>:)\\s*(?<range>[^;]+);";
			this.Editor.AutoIndentExistingLines = false;
			this.Editor.AutoScrollMinSize = new System.Drawing.Size(25, 15);
			this.Editor.BackBrush = null;
			this.Editor.BookmarkIcon = global::HMSEditorNS.Properties.Resources.togglebookmark;
			this.Editor.BreakpointIcon = global::HMSEditorNS.Properties.Resources.breakpoint_x16;
			this.Editor.BreakpointLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			this.Editor.CausesValidation = false;
			this.Editor.ChangedLineColor = System.Drawing.Color.PaleGreen;
			this.Editor.CharHeight = 15;
			this.Editor.CharWidth = 7;
			this.Editor.ContextMenuStrip = this.contextMenuStrip1;
			this.Editor.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.Editor.DebugCurrentLineIcon = global::HMSEditorNS.Properties.Resources.arrow_run_16xMD;
			this.Editor.DelayedEventsInterval = 600;
			this.Editor.DelayedTextChangedInterval = 500;
			this.Editor.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.Editor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Editor.Font = new System.Drawing.Font("Consolas", 9.75F);
			this.Editor.HighlightingRangeType = FastColoredTextBoxNS.HighlightingRangeType.VisibleRange;
			this.Editor.Hotkeys = resources.GetString("Editor.Hotkeys");
			this.Editor.IsReplaceMode = false;
			this.Editor.Language = FastColoredTextBoxNS.Language.PascalScript;
			this.Editor.LeftBracket = '(';
			this.Editor.Location = new System.Drawing.Point(0, 25);
			this.Editor.Name = "Editor";
			this.Editor.Paddings = new System.Windows.Forms.Padding(0);
			this.Editor.RightBracket = ')';
			this.Editor.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			serviceColors1.CollapseMarkerBackColor = System.Drawing.Color.White;
			serviceColors1.CollapseMarkerBorderColor = System.Drawing.Color.Silver;
			serviceColors1.CollapseMarkerForeColor = System.Drawing.Color.Silver;
			serviceColors1.ExpandMarkerBackColor = System.Drawing.Color.White;
			serviceColors1.ExpandMarkerBorderColor = System.Drawing.Color.Silver;
			serviceColors1.ExpandMarkerForeColor = System.Drawing.Color.Red;
			this.Editor.ServiceColors = serviceColors1;
			this.Editor.Size = new System.Drawing.Size(948, 505);
			this.Editor.TabIndex = 10;
			this.Editor.TabLength = 2;
			this.Editor.Zoom = 100;
			this.Editor.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.Editor_TextChanged);
			this.Editor.SelectionChanged += new System.EventHandler(this.Editor_SelectionChanged);
			this.Editor.TextChangedDelayed += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.Editor_TextChangedDelayed);
			this.Editor.SelectionChangedDelayed += new System.EventHandler(this.Editor_SelectionChangedDelayed);
			this.Editor.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Editor_Scroll);
			this.Editor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Editor_KeyDown);
			this.Editor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Editor_MouseClick);
			this.Editor.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Editor_MouseDoubleClick);
			this.Editor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Editor_MouseMove);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnContextMenuBack,
            this.btnContextMenuForward,
            this.ToolStripMenuItemUndo,
            this.ToolStripMenuItemRedo,
            this.toolStripSeparator9,
            this.btnContextMenuCommentBlock,
            this.ToolStripMenuItemSelectAll,
            this.ToolStripMenuItemCut,
            this.ToolStripMenuItemCopy,
            this.ToolStripMenuItemPaste,
            this.ToolStripMenuItemDelete,
            this.toolStripSeparator10,
            this.btnGotoContextMenu,
            this.btnContextMenuGotoDef,
            this.toolStripSeparator13,
            this.btnContextMenuToggleBookmark,
            this.ToolStripMenuItemBookmarkClear,
            this.ToolStripMenuItemClearBreakpoints,
            this.ToolStripMenuItemZoom100,
            this.btnInsertTemplate,
            this.btnContextMenuToolBar});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(235, 418);
			this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
			// 
			// btnContextMenuBack
			// 
			this.btnContextMenuBack.Image = global::HMSEditorNS.Properties.Resources.NavigateBackwards_6270;
			this.btnContextMenuBack.Name = "btnContextMenuBack";
			this.btnContextMenuBack.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Left)));
			this.btnContextMenuBack.Size = new System.Drawing.Size(234, 22);
			this.btnContextMenuBack.Text = "Назад";
			this.btnContextMenuBack.Click += new System.EventHandler(this.btnContextMenuBack_Click);
			// 
			// btnContextMenuForward
			// 
			this.btnContextMenuForward.Image = global::HMSEditorNS.Properties.Resources.NavigateForward_6271;
			this.btnContextMenuForward.Name = "btnContextMenuForward";
			this.btnContextMenuForward.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Right)));
			this.btnContextMenuForward.Size = new System.Drawing.Size(234, 22);
			this.btnContextMenuForward.Text = "Вперёд";
			this.btnContextMenuForward.Click += new System.EventHandler(this.btnContextMenuForward_Click);
			// 
			// ToolStripMenuItemUndo
			// 
			this.ToolStripMenuItemUndo.Image = global::HMSEditorNS.Properties.Resources.Arrow_UndoRevertRestore_16xLG_color;
			this.ToolStripMenuItemUndo.Name = "ToolStripMenuItemUndo";
			this.ToolStripMenuItemUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.ToolStripMenuItemUndo.Size = new System.Drawing.Size(234, 22);
			this.ToolStripMenuItemUndo.Text = "Отменить";
			this.ToolStripMenuItemUndo.Click += new System.EventHandler(this.ToolStripMenuItemUndo_Click);
			// 
			// ToolStripMenuItemRedo
			// 
			this.ToolStripMenuItemRedo.Image = global::HMSEditorNS.Properties.Resources.Arrow_RedoRetry_16xLG_color;
			this.ToolStripMenuItemRedo.Name = "ToolStripMenuItemRedo";
			this.ToolStripMenuItemRedo.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Z)));
			this.ToolStripMenuItemRedo.Size = new System.Drawing.Size(234, 22);
			this.ToolStripMenuItemRedo.Text = "Вернуть";
			this.ToolStripMenuItemRedo.Click += new System.EventHandler(this.ToolStripMenuItemRedo_Click);
			// 
			// toolStripSeparator9
			// 
			this.toolStripSeparator9.Name = "toolStripSeparator9";
			this.toolStripSeparator9.Size = new System.Drawing.Size(231, 6);
			// 
			// btnContextMenuCommentBlock
			// 
			this.btnContextMenuCommentBlock.Image = global::HMSEditorNS.Properties.Resources.Comment_11698;
			this.btnContextMenuCommentBlock.Name = "btnContextMenuCommentBlock";
			this.btnContextMenuCommentBlock.Size = new System.Drawing.Size(234, 22);
			this.btnContextMenuCommentBlock.Text = "Закомментировать строки";
			this.btnContextMenuCommentBlock.Click += new System.EventHandler(this.btnContextMenuCommentBlock_Click);
			// 
			// ToolStripMenuItemSelectAll
			// 
			this.ToolStripMenuItemSelectAll.Name = "ToolStripMenuItemSelectAll";
			this.ToolStripMenuItemSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.ToolStripMenuItemSelectAll.Size = new System.Drawing.Size(234, 22);
			this.ToolStripMenuItemSelectAll.Text = "Выделить всё";
			this.ToolStripMenuItemSelectAll.Click += new System.EventHandler(this.ToolStripMenuItemSelectAll_Click);
			// 
			// ToolStripMenuItemCut
			// 
			this.ToolStripMenuItemCut.Image = global::HMSEditorNS.Properties.Resources.Cut_6523;
			this.ToolStripMenuItemCut.Name = "ToolStripMenuItemCut";
			this.ToolStripMenuItemCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.ToolStripMenuItemCut.Size = new System.Drawing.Size(234, 22);
			this.ToolStripMenuItemCut.Text = "Вырезать";
			this.ToolStripMenuItemCut.Click += new System.EventHandler(this.ToolStripMenuItemCut_Click);
			// 
			// ToolStripMenuItemCopy
			// 
			this.ToolStripMenuItemCopy.Image = global::HMSEditorNS.Properties.Resources.Copy_6524;
			this.ToolStripMenuItemCopy.Name = "ToolStripMenuItemCopy";
			this.ToolStripMenuItemCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.ToolStripMenuItemCopy.Size = new System.Drawing.Size(234, 22);
			this.ToolStripMenuItemCopy.Text = "Копировать";
			this.ToolStripMenuItemCopy.Click += new System.EventHandler(this.ToolStripMenuItemCopy_Click);
			// 
			// ToolStripMenuItemPaste
			// 
			this.ToolStripMenuItemPaste.Image = global::HMSEditorNS.Properties.Resources.Paste_6520;
			this.ToolStripMenuItemPaste.Name = "ToolStripMenuItemPaste";
			this.ToolStripMenuItemPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.ToolStripMenuItemPaste.Size = new System.Drawing.Size(234, 22);
			this.ToolStripMenuItemPaste.Text = "Вставить";
			this.ToolStripMenuItemPaste.Click += new System.EventHandler(this.ToolStripMenuItemPaste_Click);
			// 
			// ToolStripMenuItemDelete
			// 
			this.ToolStripMenuItemDelete.Image = global::HMSEditorNS.Properties.Resources.Clearallrequests_8816;
			this.ToolStripMenuItemDelete.Name = "ToolStripMenuItemDelete";
			this.ToolStripMenuItemDelete.Size = new System.Drawing.Size(234, 22);
			this.ToolStripMenuItemDelete.Text = "Удалить";
			this.ToolStripMenuItemDelete.Click += new System.EventHandler(this.ToolStripMenuItemDelete_Click);
			// 
			// toolStripSeparator10
			// 
			this.toolStripSeparator10.Name = "toolStripSeparator10";
			this.toolStripSeparator10.Size = new System.Drawing.Size(231, 6);
			// 
			// btnGotoContextMenu
			// 
			this.btnGotoContextMenu.Name = "btnGotoContextMenu";
			this.btnGotoContextMenu.Size = new System.Drawing.Size(234, 22);
			this.btnGotoContextMenu.Text = "Перейти";
			// 
			// btnContextMenuGotoDef
			// 
			this.btnContextMenuGotoDef.Name = "btnContextMenuGotoDef";
			this.btnContextMenuGotoDef.ShortcutKeys = System.Windows.Forms.Keys.F12;
			this.btnContextMenuGotoDef.Size = new System.Drawing.Size(234, 22);
			this.btnContextMenuGotoDef.Text = "Перейти к определению";
			this.btnContextMenuGotoDef.Click += new System.EventHandler(this.btnContextMenuGotoDef_Click);
			// 
			// toolStripSeparator13
			// 
			this.toolStripSeparator13.Name = "toolStripSeparator13";
			this.toolStripSeparator13.Size = new System.Drawing.Size(231, 6);
			// 
			// btnContextMenuToggleBookmark
			// 
			this.btnContextMenuToggleBookmark.Image = global::HMSEditorNS.Properties.Resources.Bookmark_5776;
			this.btnContextMenuToggleBookmark.Name = "btnContextMenuToggleBookmark";
			this.btnContextMenuToggleBookmark.Size = new System.Drawing.Size(234, 22);
			this.btnContextMenuToggleBookmark.Text = "Переключить закладку";
			this.btnContextMenuToggleBookmark.Click += new System.EventHandler(this.btnContextMenuToggleBookmark_Click);
			// 
			// ToolStripMenuItemBookmarkClear
			// 
			this.ToolStripMenuItemBookmarkClear.Image = global::HMSEditorNS.Properties.Resources.BookmarkClear_2381;
			this.ToolStripMenuItemBookmarkClear.Name = "ToolStripMenuItemBookmarkClear";
			this.ToolStripMenuItemBookmarkClear.Size = new System.Drawing.Size(234, 22);
			this.ToolStripMenuItemBookmarkClear.Text = "Удалить все закладки";
			this.ToolStripMenuItemBookmarkClear.Click += new System.EventHandler(this.ToolStripMenuItemBookmarkClear_Click);
			// 
			// ToolStripMenuItemClearBreakpoints
			// 
			this.ToolStripMenuItemClearBreakpoints.Image = global::HMSEditorNS.Properties.Resources.clearallbreakpoints_6551;
			this.ToolStripMenuItemClearBreakpoints.Name = "ToolStripMenuItemClearBreakpoints";
			this.ToolStripMenuItemClearBreakpoints.Size = new System.Drawing.Size(234, 22);
			this.ToolStripMenuItemClearBreakpoints.Text = "Удалить все точки останова";
			this.ToolStripMenuItemClearBreakpoints.Click += new System.EventHandler(this.ToolStripMenuItemClearBreakpoints_Click);
			// 
			// ToolStripMenuItemZoom100
			// 
			this.ToolStripMenuItemZoom100.Name = "ToolStripMenuItemZoom100";
			this.ToolStripMenuItemZoom100.Size = new System.Drawing.Size(234, 22);
			this.ToolStripMenuItemZoom100.Text = "Zoom 100%";
			this.ToolStripMenuItemZoom100.Click += new System.EventHandler(this.ToolStripMenuItemZoom100_Click);
			// 
			// btnInsertTemplate
			// 
			this.btnInsertTemplate.Image = global::HMSEditorNS.Properties.Resources.Template_Application_16xLG;
			this.btnInsertTemplate.Name = "btnInsertTemplate";
			this.btnInsertTemplate.Size = new System.Drawing.Size(234, 22);
			this.btnInsertTemplate.Text = "Вставить шаблон";
			// 
			// btnContextMenuToolBar
			// 
			this.btnContextMenuToolBar.CheckOnClick = true;
			this.btnContextMenuToolBar.Image = global::HMSEditorNS.Properties.Resources.toggle_16xLG;
			this.btnContextMenuToolBar.Name = "btnContextMenuToolBar";
			this.btnContextMenuToolBar.ShortcutKeys = System.Windows.Forms.Keys.F11;
			this.btnContextMenuToolBar.Size = new System.Drawing.Size(234, 22);
			this.btnContextMenuToolBar.Text = "Панель инструментов";
			this.btnContextMenuToolBar.Click += new System.EventHandler(this.btnContextMenuToolBar_Click);
			// 
			// tsMain
			// 
			this.tsMain.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
			this.tsMain.AllowDrop = true;
			this.tsMain.AutoSize = false;
			this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNavigateBack,
            this.btnNavigateForward,
            this.toolStripSeparator2,
            this.btnOpen,
            this.btnSave,
            this.btnPrint,
            this.toolStripSeparator3,
            this.btnCut,
            this.btnCopy,
            this.btnPaste,
            this.toolStripSeparator8,
            this.btnUndo,
            this.btnRedo,
            this.toolStripSeparator5,
            this.toolStripLabelFind,
            this.tbFind,
            this.btnFindPrev,
            this.btnFindNext,
            this.toolStripSeparator6,
            this.btnBookmarkPlus,
            this.btnBookmarkMinus,
            this.btnBookmarkPrevious,
            this.btnBookmarkNext,
            this.toolStripSeparator1,
            this.btnGoTo,
            this.toolStripSeparator7,
            this.toolStripDropDownButtonSettings});
			this.tsMain.Location = new System.Drawing.Point(0, 0);
			this.tsMain.Name = "tsMain";
			this.tsMain.Size = new System.Drawing.Size(948, 25);
			this.tsMain.TabIndex = 11;
			// 
			// btnNavigateBack
			// 
			this.btnNavigateBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnNavigateBack.Image = ((System.Drawing.Image)(resources.GetObject("btnNavigateBack.Image")));
			this.btnNavigateBack.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnNavigateBack.Name = "btnNavigateBack";
			this.btnNavigateBack.Size = new System.Drawing.Size(23, 22);
			this.btnNavigateBack.Text = "Назад (Alt + Влево)";
			this.btnNavigateBack.Click += new System.EventHandler(this.btnNavigateBack_Click);
			// 
			// btnNavigateForward
			// 
			this.btnNavigateForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnNavigateForward.Image = ((System.Drawing.Image)(resources.GetObject("btnNavigateForward.Image")));
			this.btnNavigateForward.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnNavigateForward.Name = "btnNavigateForward";
			this.btnNavigateForward.Size = new System.Drawing.Size(23, 22);
			this.btnNavigateForward.Text = "Вперёд (Alt + Вправо)";
			this.btnNavigateForward.Click += new System.EventHandler(this.btnNavigateForward_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// btnOpen
			// 
			this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
			this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(23, 22);
			this.btnOpen.Text = "&Открыть файл";
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// btnSave
			// 
			this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
			this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(23, 22);
			this.btnSave.Text = "&Сохранить в файл";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnPrint
			// 
			this.btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
			this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(23, 22);
			this.btnPrint.Text = "&Печать";
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// btnCut
			// 
			this.btnCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnCut.Image = ((System.Drawing.Image)(resources.GetObject("btnCut.Image")));
			this.btnCut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnCut.Name = "btnCut";
			this.btnCut.Size = new System.Drawing.Size(23, 22);
			this.btnCut.Text = "Вырезать";
			this.btnCut.Click += new System.EventHandler(this.btnCut_Click);
			// 
			// btnCopy
			// 
			this.btnCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.Image")));
			this.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new System.Drawing.Size(23, 22);
			this.btnCopy.Text = "Копировать";
			this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
			// 
			// btnPaste
			// 
			this.btnPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnPaste.Image = ((System.Drawing.Image)(resources.GetObject("btnPaste.Image")));
			this.btnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnPaste.Name = "btnPaste";
			this.btnPaste.Size = new System.Drawing.Size(23, 22);
			this.btnPaste.Text = "Вставить";
			this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
			// 
			// toolStripSeparator8
			// 
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
			// 
			// btnUndo
			// 
			this.btnUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnUndo.Image = ((System.Drawing.Image)(resources.GetObject("btnUndo.Image")));
			this.btnUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnUndo.Name = "btnUndo";
			this.btnUndo.Size = new System.Drawing.Size(23, 22);
			this.btnUndo.Text = "Отмена (Ctrl+Z)";
			this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
			// 
			// btnRedo
			// 
			this.btnRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnRedo.Image = ((System.Drawing.Image)(resources.GetObject("btnRedo.Image")));
			this.btnRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRedo.Name = "btnRedo";
			this.btnRedo.Size = new System.Drawing.Size(23, 22);
			this.btnRedo.Text = "Повторить (Ctrl+R)";
			this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabelFind
			// 
			this.toolStripLabelFind.Name = "toolStripLabelFind";
			this.toolStripLabelFind.Size = new System.Drawing.Size(47, 22);
			this.toolStripLabelFind.Text = "Найти: ";
			// 
			// tbFind
			// 
			this.tbFind.AcceptsReturn = true;
			this.tbFind.Name = "tbFind";
			this.tbFind.Size = new System.Drawing.Size(100, 25);
			this.tbFind.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbFind_KeyPress);
			// 
			// btnFindPrev
			// 
			this.btnFindPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnFindPrev.Image = ((System.Drawing.Image)(resources.GetObject("btnFindPrev.Image")));
			this.btnFindPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnFindPrev.Name = "btnFindPrev";
			this.btnFindPrev.Size = new System.Drawing.Size(23, 22);
			this.btnFindPrev.Text = "Поиск назад";
			this.btnFindPrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnFindPrev.Click += new System.EventHandler(this.btnFindPrev_Click);
			// 
			// btnFindNext
			// 
			this.btnFindNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnFindNext.Image = ((System.Drawing.Image)(resources.GetObject("btnFindNext.Image")));
			this.btnFindNext.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnFindNext.Name = "btnFindNext";
			this.btnFindNext.Size = new System.Drawing.Size(23, 22);
			this.btnFindNext.Text = "Поиск вперёд";
			this.btnFindNext.Click += new System.EventHandler(this.btnFindNext_Click);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
			// 
			// btnBookmarkPlus
			// 
			this.btnBookmarkPlus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnBookmarkPlus.Image = ((System.Drawing.Image)(resources.GetObject("btnBookmarkPlus.Image")));
			this.btnBookmarkPlus.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnBookmarkPlus.Name = "btnBookmarkPlus";
			this.btnBookmarkPlus.Size = new System.Drawing.Size(23, 22);
			this.btnBookmarkPlus.Text = "Закладка";
			this.btnBookmarkPlus.Click += new System.EventHandler(this.btnBookmarkPlus_Click);
			// 
			// btnBookmarkMinus
			// 
			this.btnBookmarkMinus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnBookmarkMinus.Image = ((System.Drawing.Image)(resources.GetObject("btnBookmarkMinus.Image")));
			this.btnBookmarkMinus.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnBookmarkMinus.Name = "btnBookmarkMinus";
			this.btnBookmarkMinus.Size = new System.Drawing.Size(23, 22);
			this.btnBookmarkMinus.Text = "Удалить все закладки";
			this.btnBookmarkMinus.Click += new System.EventHandler(this.btnBookmarkMinus_Click);
			// 
			// btnBookmarkPrevious
			// 
			this.btnBookmarkPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnBookmarkPrevious.Image = ((System.Drawing.Image)(resources.GetObject("btnBookmarkPrevious.Image")));
			this.btnBookmarkPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnBookmarkPrevious.Name = "btnBookmarkPrevious";
			this.btnBookmarkPrevious.Size = new System.Drawing.Size(23, 22);
			this.btnBookmarkPrevious.Text = "Перейти к предыдущей закладке (Ctrl-Shift-N)";
			this.btnBookmarkPrevious.Click += new System.EventHandler(this.btnBookmarkPrevious_Click);
			// 
			// btnBookmarkNext
			// 
			this.btnBookmarkNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnBookmarkNext.Image = ((System.Drawing.Image)(resources.GetObject("btnBookmarkNext.Image")));
			this.btnBookmarkNext.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnBookmarkNext.Name = "btnBookmarkNext";
			this.btnBookmarkNext.Size = new System.Drawing.Size(23, 22);
			this.btnBookmarkNext.Text = "Перейти к следующей закладке (Ctrl-N)";
			this.btnBookmarkNext.Click += new System.EventHandler(this.btnBookmarkNext_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// btnGoTo
			// 
			this.btnGoTo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnGoTo.Image = ((System.Drawing.Image)(resources.GetObject("btnGoTo.Image")));
			this.btnGoTo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnGoTo.Name = "btnGoTo";
			this.btnGoTo.Size = new System.Drawing.Size(79, 22);
			this.btnGoTo.Text = "Перейти ...";
			this.btnGoTo.ToolTipText = "Перейти к закладке, функции...";
			this.btnGoTo.DropDownOpening += new System.EventHandler(this.btnGoTo_DropDownOpening);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripDropDownButtonSettings
			// 
			this.toolStripDropDownButtonSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripDropDownButtonSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnHighlightCurrentLine,
            this.btnShowLineNumbers,
            this.btnShowFoldingLines,
            this.btnInvisibleChars,
            this.btnVerticalLineText,
            this.btnEnableFolding,
            this.toolStripSeparator12,
            this.btnHighlightSameWords,
            this.toolStripSubMenuIntelliSense,
            this.toolStripSeparator11,
            this.btnUnderlinePascalKeywords,
            this.btnRedStringsHighlight,
            this.btnToolStripMenuItemFONT,
            this.toolStripButtonHotKeys,
            this.btnThemes,
            this.toolStripSeparator4,
            this.btnUnload,
            this.btnAbout});
			this.toolStripDropDownButtonSettings.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonSettings.Image")));
			this.toolStripDropDownButtonSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButtonSettings.Name = "toolStripDropDownButtonSettings";
			this.toolStripDropDownButtonSettings.Size = new System.Drawing.Size(29, 22);
			this.toolStripDropDownButtonSettings.Text = "Настройки";
			// 
			// btnHighlightCurrentLine
			// 
			this.btnHighlightCurrentLine.Checked = true;
			this.btnHighlightCurrentLine.CheckOnClick = true;
			this.btnHighlightCurrentLine.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnHighlightCurrentLine.Image = ((System.Drawing.Image)(resources.GetObject("btnHighlightCurrentLine.Image")));
			this.btnHighlightCurrentLine.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnHighlightCurrentLine.Name = "btnHighlightCurrentLine";
			this.btnHighlightCurrentLine.Size = new System.Drawing.Size(313, 22);
			this.btnHighlightCurrentLine.Text = "Подсветка текущей строки";
			this.btnHighlightCurrentLine.Click += new System.EventHandler(this.btnHighlightCurrentLine_Click);
			// 
			// btnShowLineNumbers
			// 
			this.btnShowLineNumbers.Checked = true;
			this.btnShowLineNumbers.CheckOnClick = true;
			this.btnShowLineNumbers.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnShowLineNumbers.Image = ((System.Drawing.Image)(resources.GetObject("btnShowLineNumbers.Image")));
			this.btnShowLineNumbers.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnShowLineNumbers.Name = "btnShowLineNumbers";
			this.btnShowLineNumbers.Size = new System.Drawing.Size(313, 22);
			this.btnShowLineNumbers.Text = "Показывать номера строк";
			this.btnShowLineNumbers.Click += new System.EventHandler(this.btnShowLineNumbers_Click);
			// 
			// btnShowFoldingLines
			// 
			this.btnShowFoldingLines.CheckOnClick = true;
			this.btnShowFoldingLines.Image = ((System.Drawing.Image)(resources.GetObject("btnShowFoldingLines.Image")));
			this.btnShowFoldingLines.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnShowFoldingLines.Name = "btnShowFoldingLines";
			this.btnShowFoldingLines.Size = new System.Drawing.Size(313, 22);
			this.btnShowFoldingLines.Text = "Показывать линии действия блока";
			this.btnShowFoldingLines.Click += new System.EventHandler(this.btnShowFoldingLines_Click);
			// 
			// btnInvisibleChars
			// 
			this.btnInvisibleChars.CheckOnClick = true;
			this.btnInvisibleChars.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnInvisibleChars.Name = "btnInvisibleChars";
			this.btnInvisibleChars.Size = new System.Drawing.Size(313, 22);
			this.btnInvisibleChars.Text = "¶ Показывать непечатные символы";
			this.btnInvisibleChars.Click += new System.EventHandler(this.btnInvisibleChars_Click);
			// 
			// btnVerticalLineText
			// 
			this.btnVerticalLineText.Checked = true;
			this.btnVerticalLineText.CheckOnClick = true;
			this.btnVerticalLineText.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnVerticalLineText.Name = "btnVerticalLineText";
			this.btnVerticalLineText.Size = new System.Drawing.Size(313, 22);
			this.btnVerticalLineText.Text = "Вертикальная линия границы текста";
			this.btnVerticalLineText.Click += new System.EventHandler(this.btnVerticalLineText_Click);
			// 
			// btnEnableFolding
			// 
			this.btnEnableFolding.CheckOnClick = true;
			this.btnEnableFolding.Name = "btnEnableFolding";
			this.btnEnableFolding.Size = new System.Drawing.Size(313, 22);
			this.btnEnableFolding.Text = "Элементы свёртки блоков кода";
			this.btnEnableFolding.ToolTipText = "Отображать элементы для возможности свернуть/развернуть участки кода";
			this.btnEnableFolding.Click += new System.EventHandler(this.btnEnableFolding_Click);
			// 
			// toolStripSeparator12
			// 
			this.toolStripSeparator12.Name = "toolStripSeparator12";
			this.toolStripSeparator12.Size = new System.Drawing.Size(310, 6);
			// 
			// btnHighlightSameWords
			// 
			this.btnHighlightSameWords.Checked = true;
			this.btnHighlightSameWords.CheckOnClick = true;
			this.btnHighlightSameWords.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnHighlightSameWords.Name = "btnHighlightSameWords";
			this.btnHighlightSameWords.Size = new System.Drawing.Size(313, 22);
			this.btnHighlightSameWords.Text = "Подсветка одинаковых слов";
			this.btnHighlightSameWords.ToolTipText = "Подсвечивать слова в тексте, которые совпадают с текущим словом";
			this.btnHighlightSameWords.Click += new System.EventHandler(this.btnHighlightSameWords_Click);
			// 
			// toolStripSubMenuIntelliSense
			// 
			this.toolStripSubMenuIntelliSense.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSetIntelliSense,
            this.btnMouseHelp,
            this.btnAutoIdent,
            this.btnMarkChangedLines,
            this.btnAutoIdentChars,
            this.btnIntelliSenseFunctions,
            this.btnAutoCompleteBrackets,
            this.btnEvaluateByMouse,
            this.btnHints4CtrlSpace});
			this.toolStripSubMenuIntelliSense.Name = "toolStripSubMenuIntelliSense";
			this.toolStripSubMenuIntelliSense.Size = new System.Drawing.Size(313, 22);
			this.toolStripSubMenuIntelliSense.Text = "IntelliSense";
			// 
			// btnSetIntelliSense
			// 
			this.btnSetIntelliSense.Checked = true;
			this.btnSetIntelliSense.CheckOnClick = true;
			this.btnSetIntelliSense.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnSetIntelliSense.Name = "btnSetIntelliSense";
			this.btnSetIntelliSense.Size = new System.Drawing.Size(428, 22);
			this.btnSetIntelliSense.Text = "Подсказки IntelliSense";
			this.btnSetIntelliSense.ToolTipText = "Включение/Отключение подсказок ключевых слов и методов";
			this.btnSetIntelliSense.Click += new System.EventHandler(this.btnSetIntelliSense_Click);
			// 
			// btnMouseHelp
			// 
			this.btnMouseHelp.Checked = true;
			this.btnMouseHelp.CheckOnClick = true;
			this.btnMouseHelp.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnMouseHelp.Name = "btnMouseHelp";
			this.btnMouseHelp.Size = new System.Drawing.Size(428, 22);
			this.btnMouseHelp.Text = "Подсказки при наведении мышкой";
			this.btnMouseHelp.Click += new System.EventHandler(this.btnMouseHelp_Click);
			// 
			// btnAutoIdent
			// 
			this.btnAutoIdent.Checked = true;
			this.btnAutoIdent.CheckOnClick = true;
			this.btnAutoIdent.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnAutoIdent.Name = "btnAutoIdent";
			this.btnAutoIdent.Size = new System.Drawing.Size(428, 22);
			this.btnAutoIdent.Text = "Автоматический отступ";
			this.btnAutoIdent.Click += new System.EventHandler(this.btnAutoIdent_Click);
			// 
			// btnMarkChangedLines
			// 
			this.btnMarkChangedLines.Checked = true;
			this.btnMarkChangedLines.CheckOnClick = true;
			this.btnMarkChangedLines.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnMarkChangedLines.Name = "btnMarkChangedLines";
			this.btnMarkChangedLines.Size = new System.Drawing.Size(428, 22);
			this.btnMarkChangedLines.Text = "Помечать изменённые строки";
			this.btnMarkChangedLines.Click += new System.EventHandler(this.btnMarkChangedLines_Click);
			// 
			// btnAutoIdentChars
			// 
			this.btnAutoIdentChars.Checked = true;
			this.btnAutoIdentChars.CheckOnClick = true;
			this.btnAutoIdentChars.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnAutoIdentChars.Name = "btnAutoIdentChars";
			this.btnAutoIdentChars.Size = new System.Drawing.Size(428, 22);
			this.btnAutoIdentChars.Text = "Автоматический отступ конструкций";
			this.btnAutoIdentChars.Visible = false;
			this.btnAutoIdentChars.Click += new System.EventHandler(this.btnAutoIdentChars_Click);
			// 
			// btnIntelliSenseFunctions
			// 
			this.btnIntelliSenseFunctions.Checked = true;
			this.btnIntelliSenseFunctions.CheckOnClick = true;
			this.btnIntelliSenseFunctions.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnIntelliSenseFunctions.Name = "btnIntelliSenseFunctions";
			this.btnIntelliSenseFunctions.Size = new System.Drawing.Size(428, 22);
			this.btnIntelliSenseFunctions.Text = "Подсказки для параметров функций";
			this.btnIntelliSenseFunctions.ToolTipText = "Включение/Отключение подсказок для функций";
			this.btnIntelliSenseFunctions.Click += new System.EventHandler(this.btnIntelliSenseFunctions_Click);
			// 
			// btnAutoCompleteBrackets
			// 
			this.btnAutoCompleteBrackets.CheckOnClick = true;
			this.btnAutoCompleteBrackets.Name = "btnAutoCompleteBrackets";
			this.btnAutoCompleteBrackets.Size = new System.Drawing.Size(428, 22);
			this.btnAutoCompleteBrackets.Text = "Автозавершение скобок и кавычек";
			this.btnAutoCompleteBrackets.Click += new System.EventHandler(this.btnAutoCompleteBrackets_Click);
			// 
			// btnEvaluateByMouse
			// 
			this.btnEvaluateByMouse.Checked = true;
			this.btnEvaluateByMouse.CheckOnClick = true;
			this.btnEvaluateByMouse.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnEvaluateByMouse.Name = "btnEvaluateByMouse";
			this.btnEvaluateByMouse.Size = new System.Drawing.Size(428, 22);
			this.btnEvaluateByMouse.Text = "Вычислять значение в режиме отладки при наведении мышкой";
			this.btnEvaluateByMouse.Click += new System.EventHandler(this.btnEvaluateByMouse_Click);
			// 
			// btnHints4CtrlSpace
			// 
			this.btnHints4CtrlSpace.CheckOnClick = true;
			this.btnHints4CtrlSpace.Name = "btnHints4CtrlSpace";
			this.btnHints4CtrlSpace.Size = new System.Drawing.Size(428, 22);
			this.btnHints4CtrlSpace.Text = "Подсказки только по Ctrl-Space";
			this.btnHints4CtrlSpace.Click += new System.EventHandler(this.btnHints4CtrlSpace_Click);
			// 
			// toolStripSeparator11
			// 
			this.toolStripSeparator11.Name = "toolStripSeparator11";
			this.toolStripSeparator11.Size = new System.Drawing.Size(310, 6);
			// 
			// btnUnderlinePascalKeywords
			// 
			this.btnUnderlinePascalKeywords.Checked = true;
			this.btnUnderlinePascalKeywords.CheckOnClick = true;
			this.btnUnderlinePascalKeywords.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnUnderlinePascalKeywords.Name = "btnUnderlinePascalKeywords";
			this.btnUnderlinePascalKeywords.Size = new System.Drawing.Size(313, 22);
			this.btnUnderlinePascalKeywords.Text = "Подчеркивать ключевые слова PascalScript";
			this.btnUnderlinePascalKeywords.Click += new System.EventHandler(this.ToolStripMenuItemAltPascalScriptHighlight_Click);
			// 
			// btnRedStringsHighlight
			// 
			this.btnRedStringsHighlight.CheckOnClick = true;
			this.btnRedStringsHighlight.Name = "btnRedStringsHighlight";
			this.btnRedStringsHighlight.Size = new System.Drawing.Size(313, 22);
			this.btnRedStringsHighlight.Text = "Яркая подсветка строк";
			this.btnRedStringsHighlight.Click += new System.EventHandler(this.btnRedStringsHighlight_Click);
			// 
			// btnToolStripMenuItemFONT
			// 
			this.btnToolStripMenuItemFONT.CheckOnClick = true;
			this.btnToolStripMenuItemFONT.Name = "btnToolStripMenuItemFONT";
			this.btnToolStripMenuItemFONT.Size = new System.Drawing.Size(313, 22);
			this.btnToolStripMenuItemFONT.Text = "Альтернативный шрифт RobotoMono";
			this.btnToolStripMenuItemFONT.Click += new System.EventHandler(this.btnToolStripMenuItemFONT_Click);
			// 
			// toolStripButtonHotKeys
			// 
			this.toolStripButtonHotKeys.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonHotKeys.Image")));
			this.toolStripButtonHotKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonHotKeys.Name = "toolStripButtonHotKeys";
			this.toolStripButtonHotKeys.Size = new System.Drawing.Size(313, 22);
			this.toolStripButtonHotKeys.Text = "Горячие клавиши ...";
			this.toolStripButtonHotKeys.Click += new System.EventHandler(this.toolStripButtonHotKeys_Click);
			// 
			// btnThemes
			// 
			this.btnThemes.Name = "btnThemes";
			this.btnThemes.Size = new System.Drawing.Size(313, 22);
			this.btnThemes.Text = "Цветовые темы";
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(310, 6);
			// 
			// btnUnload
			// 
			this.btnUnload.Image = global::HMSEditorNS.Properties.Resources.Output_16xLG;
			this.btnUnload.Name = "btnUnload";
			this.btnUnload.Size = new System.Drawing.Size(313, 22);
			this.btnUnload.Text = "Выгрузить программу из памяти";
			this.btnUnload.Click += new System.EventHandler(this.btnUnload_Click);
			// 
			// btnAbout
			// 
			this.btnAbout.Name = "btnAbout";
			this.btnAbout.Size = new System.Drawing.Size(313, 22);
			this.btnAbout.Text = "О программе";
			this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.White;
			this.imageList1.Images.SetKeyName(0, "Class_489.png");
			this.imageList1.Images.SetKeyName(1, "CodeSnippet_6225.png");
			this.imageList1.Images.SetKeyName(2, "Enum_582.png");
			this.imageList1.Images.SetKeyName(3, "FieldIcon.png");
			this.imageList1.Images.SetKeyName(4, "Field-Private_545.png");
			this.imageList1.Images.SetKeyName(5, "Function_8941.png");
			this.imageList1.Images.SetKeyName(6, "IntelliSense-Keywords_6226.png");
			this.imageList1.Images.SetKeyName(7, "Method_636.png");
			this.imageList1.Images.SetKeyName(8, "Procedure_8937.png");
			this.imageList1.Images.SetKeyName(9, "Bookmark_5776.png");
			this.imageList1.Images.SetKeyName(10, "Constant_495.png");
			this.imageList1.Images.SetKeyName(11, "Event_594.png");
			this.imageList1.Images.SetKeyName(12, "Template_Application_16xLG.png");
			// 
			// HMSEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.DoubleBuffered = true;
			this.Name = "HMSEditor";
			this.Size = new System.Drawing.Size(948, 530);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.Editor)).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			this.tsMain.ResumeLayout(false);
			this.tsMain.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		public FastColoredTextBoxNS.FastColoredTextBox Editor;
		private System.Windows.Forms.ToolStrip tsMain;
		private System.Windows.Forms.ToolStripButton btnNavigateBack;
		private System.Windows.Forms.ToolStripButton btnNavigateForward;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton btnOpen;
		private System.Windows.Forms.ToolStripButton btnSave;
		private System.Windows.Forms.ToolStripButton btnPrint;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton btnCut;
		private System.Windows.Forms.ToolStripButton btnCopy;
		private System.Windows.Forms.ToolStripButton btnPaste;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
		private System.Windows.Forms.ToolStripButton btnUndo;
		private System.Windows.Forms.ToolStripButton btnRedo;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripLabel toolStripLabelFind;
		private System.Windows.Forms.ToolStripTextBox tbFind;
		private System.Windows.Forms.ToolStripButton btnFindPrev;
		private System.Windows.Forms.ToolStripButton btnFindNext;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripButton btnBookmarkPlus;
		private System.Windows.Forms.ToolStripButton btnBookmarkMinus;
		private System.Windows.Forms.ToolStripButton btnBookmarkPrevious;
		private System.Windows.Forms.ToolStripButton btnBookmarkNext;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripDropDownButton btnGoTo;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripMenuItem toolStripButtonHotKeys;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonSettings;
		private System.Windows.Forms.ToolStripMenuItem btnHighlightSameWords;
		private System.Windows.Forms.ToolStripMenuItem btnUnderlinePascalKeywords;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
		private System.Windows.Forms.ToolStripMenuItem btnRedStringsHighlight;
		private System.Windows.Forms.ToolStripMenuItem btnToolStripMenuItemFONT;
		private System.Windows.Forms.ToolStripMenuItem btnVerticalLineText;
		private System.Windows.Forms.ToolStripMenuItem btnInvisibleChars;
		private System.Windows.Forms.ToolStripMenuItem btnHighlightCurrentLine;
		private System.Windows.Forms.ToolStripMenuItem btnShowLineNumbers;
		private System.Windows.Forms.ToolStripMenuItem btnShowFoldingLines;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem btnAbout;
		private System.Windows.Forms.ToolStripMenuItem btnUnload;
		private System.Windows.Forms.ToolStripMenuItem toolStripSubMenuIntelliSense;
		private System.Windows.Forms.ToolStripMenuItem btnSetIntelliSense;
		private System.Windows.Forms.ToolStripMenuItem btnMouseHelp;
		private System.Windows.Forms.ToolStripMenuItem btnAutoIdent;
		private System.Windows.Forms.ToolStripMenuItem btnMarkChangedLines;
		private System.Windows.Forms.ToolStripMenuItem btnAutoIdentChars;
		private System.Windows.Forms.ToolStripMenuItem btnIntelliSenseFunctions;
		private System.Windows.Forms.ToolStripMenuItem btnAutoCompleteBrackets;
		private System.Windows.Forms.ToolStripMenuItem btnEvaluateByMouse;
		private System.Windows.Forms.ToolStripMenuItem btnHints4CtrlSpace;
		private System.Windows.Forms.ToolStripMenuItem btnThemes;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem btnContextMenuBack;
		private System.Windows.Forms.ToolStripMenuItem btnContextMenuForward;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemUndo;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemRedo;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
		private System.Windows.Forms.ToolStripMenuItem btnContextMenuCommentBlock;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSelectAll;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCut;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCopy;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemPaste;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDelete;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
		private System.Windows.Forms.ToolStripMenuItem btnGotoContextMenu;
		private System.Windows.Forms.ToolStripMenuItem btnContextMenuGotoDef;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
		private System.Windows.Forms.ToolStripMenuItem btnContextMenuToggleBookmark;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemBookmarkClear;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemClearBreakpoints;
		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemZoom100;
		private System.Windows.Forms.ToolStripMenuItem btnInsertTemplate;
		private System.Windows.Forms.ToolStripMenuItem btnContextMenuToolBar;
		private System.Windows.Forms.ToolStripMenuItem btnEnableFolding;
	}
}
