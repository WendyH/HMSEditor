/* This code is released under WTFPL Version 2 (http://www.wtfpl.net/) * Created by WendyH. Copyleft. */
using System;
using System.Collections.Generic;
using System.Text;
using FastColoredTextBoxNS;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;

namespace HMSEditorNS {
	public sealed partial class HMSEditor: UserControl {

		#region Static
		public  static string     MsgCaption       = "HMS Editor";
		public  static bool       DebugMe          = false;
		public  static bool       Exited           = false;
		public  static bool       SilentMode       = false;
		public  static HMSEditor  ActiveEditor     = null;
		public  static bool       NeedRestart      = false;
		public  static string     NeedCopyNewFile  = "";
		private static bool       MouseTimerEnable = false;

		private static EditorList Attaches         = new EditorList();
		private static IntPtr     hookHMS          = IntPtr.Zero;
		private static IntPtr     hookMouse        = IntPtr.Zero;
		private static uint       HMSProcessID     = 0;
		private static IntPtr     HMSProcessHWND   = IntPtr.Zero;

		public static INI Settings = new INI(HMS.WorkingDir + HMS.DS + "HMSEditor.ini");

		#region Regular Expressions Magnetic Field
		private static Regex regexProceduresCPP    = new Regex(@"[\r\n]\s*?(?<type>\w+)\s+(\w+)\s*?\(", RegexOptions.Singleline | RegexOptions.Compiled);
		private static Regex regexProceduresPascal = new Regex(@"\b(?:procedure|function)\s+(\w+)"    , RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private static Regex regexProceduresBasic  = new Regex(@"[\r\n]\s*?sub\s+(\w+)"               , RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private static Regex regexProceduresYAML   = new Regex(@"[\r\n]\s*?(\w+)\s*?:"                , RegexOptions.Singleline | RegexOptions.Compiled);
		public  static Regex regexExcludeWords     = new Regex(@"\b(for|if|else|return|true|false|while|do)\b", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private static Regex regexDetectProcedure  = new Regex(@"\b(void|procedure)" , RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
		private static Regex regexPartOfLine       = new Regex(@"\b(.*?\s.*?\s.*?)\b", RegexOptions.Compiled);

		private static Regex regexStringAndCommentsCPP    = new Regex(@"""(\\[\s\S]|[^""])*""|'(\\[\s\S]|[^'])*'|(//.*|\/\*[\s\S]*?\*\/)", RegexOptions.Compiled);
		private static Regex regexStringAndCommentsPascal = new Regex(@"""(\\[\s\S]|[^""\r])*""|'(\\[\s\S]|[^'\r])*'|(//.*|\{[\s\S]*?\})", RegexOptions.Compiled);
		private static Regex regexStringAndCommentsBasic  = new Regex(@"""(\\[\s\S]|[^""])*""|('.*)"                                     , RegexOptions.Compiled);

		private static Regex regexSearchConstantsCPP      = new Regex(@"#define\s+(\w+)(.*)"                             , RegexOptions.Compiled);
		private static Regex regexSearchConstantsPascal1  = new Regex(@"\bconst\b(.*?)\b(var|procedure|function|begin)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
		private static Regex regexSearchConstantsPascal2  = new Regex(@"([\w]+)\s*?=\s*?(.*?)[;\r\n]"                    , RegexOptions.Compiled);

		private static Regex regexSearchVarsCPP           = new Regex(@"(?<type>\w+)\s+(?<vars>[^;{}]+)"   , RegexOptions.Compiled);
		private static Regex regexSearchVarsJS            = new Regex(@"(?<type>\w+)\s+(?<vars>[^;{}]+)"   , RegexOptions.Compiled);
		private static Regex regexSearchVarsPascal        = new Regex(@"(?<vars>[\w ,]+):(?<type>[^=;\)]+)", RegexOptions.Compiled);

		private static Regex regexTwoWords                = new Regex(@"(\w+)\s+(\w+)\s*$"    , RegexOptions.Compiled);
//		private static Regex regexBrackets                = new Regex(@"\([^\)\(]*\)"         , RegexOptions.Compiled);
		private static Regex regexAssignment              = new Regex(@"=[^,$]+"              , RegexOptions.Compiled);
		private static Regex regexConstantKeys            = new Regex(@"\b(var|const)\b"      , RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private static Regex regexNotValidCharsInVars     = new Regex(@"[{}\(\[\]]"           , RegexOptions.Compiled);
		private static Regex regexExractConstantValue     = new Regex(@"(""[^""]+""|'[^']+')" , RegexOptions.Compiled);
		private static Regex regexBrackets                = new Regex(@"(\(|\)|\{|\})"        , RegexOptions.Compiled);


		private static Regex regexIsNum      = new Regex(@"(\d|\$)", RegexOptions.Compiled);
		private static Regex regexIsStr      = new Regex(@"(""|')" , RegexOptions.Compiled);
		private static Regex regexAllSymbols = new Regex("."       , RegexOptions.Compiled);
		private static Regex regexLineBreaks = new Regex(@"[\r\n]" , RegexOptions.Compiled);
		private static Regex regexNotNewLine = new Regex(@"[^\n]"  , RegexOptions.Compiled);

		private static RegexOptions StdOpt = RegexOptions.Singleline | RegexOptions.IgnoreCase; // Стандартные флаги RegexOptions
		#endregion Regular Expressions magnetic filed

		private static string ReturnSpaces(Match m) { return regexAllSymbols.Replace(m.Value, " "); }
		private static MatchEvaluator evaluatorSpaces = new MatchEvaluator(ReturnSpaces);

		private static List<IntPtr> CreatedControls = new List<IntPtr>(); // Коллекция созданных объектов, перехваченных по событию EVENT_OBJECT_CREATE (чтобы по второму разу их не обрабатывать)
		private static char CensChar = ' '; // Символ замены строк и комментариев при обработке текста на поиск переменных и проч.

		private static UnsafeNativeMethods.WinEventDelegate  procHookHMS = new UnsafeNativeMethods.WinEventDelegate(WinEventProc); // Делегат процедуры отлавливания событий от другого процесса
		private static UnsafeNativeMethods.LowLevelMouseProc mouseHookCallBack = MouseHookCallback;

		private static IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
			if (nCode >= 0 && NativeMethods.MouseMessages.WM_LBUTTONDOWN == (NativeMethods.MouseMessages)wParam) {
				NativeMethods.MSLLHOOKSTRUCT hookStruct = (NativeMethods.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(NativeMethods.MSLLHOOKSTRUCT));
				IntPtr hwnd = NativeMethods.WindowFromPoint(hookStruct.pt);
				if (hwnd != IntPtr.Zero) {
					HMSEditor editor = Attaches.FindByHandleButtonRunOrStep(hwnd);
					if (editor != null) { editor.DebugMode = true; editor.NeedCheckDebugState = true; }
				}
			}
			return UnsafeNativeMethods.CallNextHookEx(hookMouse, nCode, wParam, lParam);
		}

		private string RemoveLinebeaks(string text) { return regexLineBreaks.Replace(text, ""); }

		[EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
		static public bool WatchHMS() {
			Exited = false; bool success = false;
			if (hookHMS == IntPtr.Zero) {
				Process[] processes = Process.GetProcessesByName("hms");
				if (processes.Length > 0) {
					HMSProcessID   = (uint)processes[0].Id;
					HMSProcessHWND = processes[0].MainWindowHandle;
					hookHMS = UnsafeNativeMethods.SetWinEventHook(NativeMethods.EVENT_SYSTEM_FOREGROUND, NativeMethods.EVENT_OBJECT_LOCATIONCHANGE, IntPtr.Zero, procHookHMS, HMSProcessID, 0, NativeMethods.WINEVENT_OUTOFCONTEXT | NativeMethods.WINEVENT_SKIPOWNPROCESS);
					hookMouse = UnsafeNativeMethods.SetWindowsHookEx(NativeMethods.WH_MOUSE_LL, mouseHookCallBack, NativeMethods.GetModuleHandle("user32"), 0);
					success = true;
				} else {
					MessageBox.Show("Home Media Server не запущен!\nПрограмма HMS должна быть запущена первой.", MsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				}
			}
			return success;
		}

		static private IntPtr GetHwndByClassName(string classname, IntPtr parentHwnd) {
			IntPtr hWnd = IntPtr.Zero; StringBuilder name = new StringBuilder(256);
			List<IntPtr> allChildWindows = new WindowHandleInfo(parentHwnd).GetAllChildHandles();
			foreach (IntPtr currentHwnd in allChildWindows) {
				NativeMethods.GetClassName(currentHwnd, name, name.Capacity);
				if (name.Length == 0) continue;
				if (name.ToString() == classname) { hWnd = currentHwnd; break; }
			}
			return hWnd;
		}

		public bool ToLock() {
			int countout = 20; // Maximum - two sec
			while (Locked && (countout > 0)) { Thread.Sleep(100); countout--; } // Waiting if locked
			if (Locked) return false;
			Locked = true;
			return Locked;
		}

		public static void NewMemoOrComboBox(IntPtr hWndNew, bool isMemo = false) {
			StringBuilder sb = new StringBuilder(250);
			string classname = "";
			IntPtr hwnd      = NativeMethods.GetParent(hWndNew);
			IntPtr hParent   = hwnd;
			IntPtr hWndScriptFrame  = IntPtr.Zero;
			IntPtr hWndScriptDialog = IntPtr.Zero;
			while (hwnd != IntPtr.Zero) {
				NativeMethods.GetClassName(hwnd, sb, sb.Capacity);
				classname = sb.ToString();
				if      (classname == "THmsScriptFrame"             )   hWndScriptFrame  = hwnd;
				else if (classname == "THmsScriptDialog"            ) { hWndScriptDialog = hwnd; break; }
				else if (classname == "THmsProcessMediaDialog"      ) { hWndScriptDialog = hwnd; break; }
				else if (classname == "THmsTranscodingProfileDialog") { hWndScriptDialog = hwnd; break; }
				hwnd = NativeMethods.GetParent(hwnd);
			}
			if ((hWndScriptFrame != IntPtr.Zero) && (hWndScriptDialog != IntPtr.Zero)) {
				hWndEvaluateDialog      = IntPtr.Zero;
				hWndCustomInnerTextEdit = IntPtr.Zero;
				hWndCustomInnerMemo     = IntPtr.Zero;
				HMSEditor editor = Attaches[hWndScriptFrame];
				editor.hWndScriptDialog = hWndScriptDialog;
				if (isMemo) {
					editor.hWndMemo       = hWndNew;
					editor.hWndMemoParent = hParent;
				} else
					editor.hWndComboBox   = hWndNew;
				editor.DialogClass = classname;
				editor.TryAttach();
			}
		}

		private static bool CheckCreatedEvaluateDialog() {
			if (hWndEditor4EvalCreate == IntPtr.Zero || (hWndEvaluateDialog == IntPtr.Zero) || (hWndCustomInnerTextEdit == IntPtr.Zero) || (hWndCustomInnerMemo == IntPtr.Zero)) return false;
			// all controls of THmsEvaluateDialog created
			NativeMethods.ShowWindow(hWndEvaluateDialog, NativeMethods.SW_HIDE);         // hide dialog
			NativeMethods.SetTextOfControl(hWndCustomInnerTextEdit, EvalName);           // set variable name
			NativeMethods.SendKeyDown(hWndCustomInnerTextEdit, NativeMethods.VK_RETURN); // press Enter key
			EvalResult = NativeMethods.GetTextFromControl(hWndCustomInnerMemo);          // get variable value
			NativeMethods.SendMessage(hWndEvaluateDialog, NativeMethods.WM_CLOSE);       // close dialog
			//Console.WriteLine(EvalResult);
			hWndEditor4EvalCreate = IntPtr.Zero;
			return true;
		}

		private static int    _buttonsCount  = 0;
		private static bool   _createEvalDlg = false;
		private static string EvalName    = "";
		private static string EvalResult  = "";
		private static IntPtr hWndBntEval = IntPtr.Zero;
		private static IntPtr hWndBntStep = IntPtr.Zero;
		private static IntPtr hWndBntRun  = IntPtr.Zero;
		private static IntPtr hWndEvaluateDialog      = IntPtr.Zero;
		private static IntPtr hWndCustomInnerMemo     = IntPtr.Zero;
		private static IntPtr hWndCustomInnerTextEdit = IntPtr.Zero;
		private static IntPtr hWndEditor4EvalCreate   = IntPtr.Zero;
		static void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime) {
			if ((idObject != 0 || idChild != 0) && ((uint)idObject != 0xfffffff8)) return;
			StringBuilder sb = new StringBuilder(250);   // needs to be big enough for the classname text

			//NativeMethods.GetClassName(hwnd, sb, sb.Capacity);
			//Console.WriteLine("eventType: {0:x8} hwnd: {1:x8}  ClassName: {2}  idObject: {3:x8} ", eventType, hwnd.ToInt32(), sb.ToString(), idObject);
			//string n = sb.ToString();
			//if (n == "TcxCustomInnerTextEdit") Console.WriteLine("eventType: {0:x8} hwnd: {1:x8}  ClassName: {2}  idObject: {3:x8} ", eventType, hwnd.ToInt32(), sb.ToString(), idObject);
			//if (n == "TcxCustomInnerMemo"    ) Console.WriteLine("eventType: {0:x8} hwnd: {1:x8}  ClassName: {2}  idObject: {3:x8} ", eventType, hwnd.ToInt32(), sb.ToString(), idObject);

			bool clearGarbage = false;
			if (eventType == NativeMethods.EVENT_OBJECT_CREATE) {
				if (CreatedControls.Contains(hwnd)) return;
				CreatedControls.Add(hwnd);
				NativeMethods.GetClassName(hwnd, sb, sb.Capacity);
				string name = sb.ToString();
				if (DebugMe) Console.WriteLine("EVENT_OBJECT_CREATE ClassName: {0} hwnd {1:x8} idObject {2:x8}", name, hwnd.ToInt32(), idObject);
				if      (name == "THmsScriptFrame"           ) Attaches.New(hwnd);
				else if (name == "TSynMemo"                  ) NewMemoOrComboBox(hwnd, true );
				else if (name == "TcxCustomComboBoxInnerEdit") NewMemoOrComboBox(hwnd, false);
				else if (name == "THmsEvaluateDialog"        ) {
					hWndEvaluateDialog      = hwnd;
					hWndCustomInnerTextEdit = IntPtr.Zero;
					hWndCustomInnerMemo     = IntPtr.Zero;
				} 
				else if ((hWndEvaluateDialog != IntPtr.Zero) && (hWndCustomInnerTextEdit == IntPtr.Zero) && (name == "TcxCustomInnerTextEdit")) {
					hWndCustomInnerTextEdit = hwnd;
				} 
				else if ((hWndEvaluateDialog != IntPtr.Zero) && (hWndCustomInnerMemo     == IntPtr.Zero) && (name == "TcxCustomInnerMemo"    )) {
					hWndCustomInnerMemo     = hwnd;
				} 
				else if (name == "TCBitBtn32") {
					_buttonsCount++;
					//Console.WriteLine("EVENT_OBJECT_CREATE ClassName: {0} {2} hwnd {1:x8}", name, hwnd.ToInt32(), _buttonsCount);
					if      (_buttonsCount == 6) hWndBntRun  = hwnd;
					else if (_buttonsCount == 5) hWndBntStep = hwnd;
					else if (_buttonsCount == 4) hWndBntEval = hwnd;
					return;
				}
				_buttonsCount = 0;
			} else if (HMSProcessHWND == hwnd && eventType == NativeMethods.EVENT_OBJECT_DESTROY) {
				Exit();
				if (SilentMode) Application.Exit();

			} else if (hWndEvaluateDialog!=IntPtr.Zero && eventType == NativeMethods.EVENT_SYSTEM_FOREGROUND) {
				NativeMethods.GetClassName(hwnd, sb, sb.Capacity);
				string name = sb.ToString();
				if (_createEvalDlg && name == "THmsEvaluateDialog") {
					_createEvalDlg = false;
					NativeMethods.SendNotifyMessage(hWndEvaluateDialog, NativeMethods.WM_CLOSE);       // close dialog
					if (hWndEditor4EvalCreate != IntPtr.Zero) {
						HMSEditor editor = Attaches.FindByHandle(hWndEditor4EvalCreate);
						if (editor != null) {
							editor.DebugMode = true;
							editor.Editor.NeedRecalc(true);
							NativeMethods.SendNotifyMessage(editor.hWndMemo, NativeMethods.WM_SETFOCUS);
						}
					}
				}
			} else {
				HMSEditor editor = Attaches.FindByHandle(hwnd);
				if (editor != null) {
					if ((hwnd == editor.hWndScriptDialog) && (eventType == NativeMethods.EVENT_OBJECT_DESTROY)) {
						editor.Unattach();
						clearGarbage = true;

					} else if ((hwnd == editor.hWndMemo) && (eventType == NativeMethods.EVENT_OBJECT_LOCATIONCHANGE) && !editor.UseWaitCursor) {
						editor.ResetTimerForHiddenHmsEditor(); // if pressed Ctrl-D (hide HMSEditor) and edited text in HMS
						editor.UpdateEditor();

					} else if ((hwnd == editor.hWndComboBox) && (eventType == NativeMethods.EVENT_OBJECT_DESTROY) && ((uint)idObject == 0xfffffff8)) {
						editor.UpdateScriptLanguage();
					}
				}
				if (clearGarbage) {
					editor = null;
					GC.Collect();
					GC.WaitForPendingFinalizers();
				}

			}
		}

		public static void Exit() {
			if (hookHMS != IntPtr.Zero) {
				UnsafeNativeMethods.UnhookWinEvent(hookHMS);
				hookHMS = IntPtr.Zero;
			}
			if (hookMouse != IntPtr.Zero) {
				UnsafeNativeMethods.UnhookWindowsHookEx(hookMouse);
				hookMouse = IntPtr.Zero;
			}
			foreach (var item in Attaches) item.AttachExit();
			Exited = true;
		}

		public static void MouseTimer_Task(object StateObj) {
			if (!MouseTimerEnable) return;
			if (ActiveEditor != null) {
				if (!ActiveEditor.Visible) {
					ActiveEditor.Invoke((MethodInvoker)delegate { ActiveEditor.SwitchEditors(); });
				} else MouseHelpTimer.Task(ActiveEditor);
			}
		}
		#endregion Static

		// Constructors
		public HMSEditor() {
			InitializeComponent();
			SetAutoCompleteMenu();
		}

		public HMSEditor(IntPtr hWnd) {
			hWndScriptFrame = hWnd;
			InitializeComponent();
			SetAutoCompleteMenu();
			Editor.LostFocus += Editor_LostFocus;
			//tsMain.Visible = false; // for attached default is not visible, restore saved value in LoadSettings()
		}

		// Fields
		public  bool   Locked          = false;
		public  bool   Attached        = false;
		public  bool   NeedCheckDebugState = false;

		private IntPtr hWndScriptDialog= IntPtr.Zero;
		private IntPtr hWndScriptFrame = IntPtr.Zero;
		private IntPtr hWndMemoParent  = IntPtr.Zero;
		private IntPtr hWndMemo        = IntPtr.Zero;
		private IntPtr hWndComboBox    = IntPtr.Zero;

		public  string Filename           = HMS.TemplatesDir;
		public  int    LastPtocedureIndex = -1;
		private int    LastTextLenght     = -1;
		public  bool   CustomFont         = false;
		private bool   NeedRecalcVars     = false;
		private bool   CheckFunctionHelp  = false;
		private string CurrentValidTypes  = "";  // Sets in CreateAutocomplete() procedure

		public bool Modified       { get { return Editor.IsChanged     ; } set { Editor.IsChanged      = value; } }
		public int  SelectionStart { get { return Editor.SelectionStart; } set { Editor.SelectionStart = value; } }

		public string       DialogClass = "Main";
		public ValueToolTip ValueHint   = new ValueToolTip();

		public AutocompleteItems LocalVars = new AutocompleteItems();
		public AutocompleteItems Variables = new AutocompleteItems();
		public AutocompleteItems Functions = new AutocompleteItems();

		private System.Threading.Timer MouseTimer = new System.Threading.Timer(MouseTimer_Task, null, Timeout.Infinite, Timeout.Infinite);
		public  Point       MouseLocation         = new Point();
		private Style       InvisibleCharsStyle   = new InvisibleCharsRenderer(Pens.Gray);
		private Color       ColorCurrentLine      = Color.FromArgb(100, 210, 210, 255);
		private Color       ColorChangedLine      = Color.FromArgb(255, 152, 251, 152);
		private MarkerStyle SameWordsStyle        = new MarkerStyle(new SolidBrush(Color.FromArgb(33, Color.Gray)));
		private DateTime    LastNavigatedDateTime = DateTime.Now;

		private MatchEvaluator evaluatorSameLines  = new MatchEvaluator(MatchReturnEmptyLines);
		public  AutocompleteMenu PopupMenu;

		private bool TextBoxFindChanged = false;

		public event EventHandler<TextChangedEventArgs> TextChangedDelayed;

		public bool AutoCompleteBrackets     { get { return Editor.AutoCompleteBrackets    ; } set { Editor.AutoCompleteBrackets    = value; } }
		public bool AutoIdent                { get { return Editor.AutoIndent              ; } set { Editor.AutoIndent              = value; } }
		public bool AutoIndentChars          { get { return Editor.AutoIndentChars         ; } set { Editor.AutoIndentChars         = value; } }
		public bool AutoIndentExistingLines  { get { return Editor.AutoIndentExistingLines ; } set { Editor.AutoIndentExistingLines = value; } }
		public bool ToolStripVisible         { get { return tsMain.Visible                 ; } set { tsMain.Visible                 = value; } }
		public bool DebugMode                { get { return Editor.DebugMode               ; } set { Editor.DebugMode               = value; } }
		public bool EnableFunctionToolTip = true;
		public bool EnableEvaluateByMouse = true;

		public string ScriptLanguage {
			get {
				if (Editor.Language == Language.CPPScript) return "C++Script";
				return Editor.Language.ToString();
			}
			set {
				Editor.ClearStylesBuffer();
				Editor.Range.ClearStyle(StyleIndex.All);
				switch (value) {
					case "C++Script"   : Editor.Language = Language.CPPScript   ; break;
					case "PascalScript": Editor.Language = Language.PascalScript; break;
					case "BasicScript" : Editor.Language = Language.BasicScript ; break;
					case "JScript"     : Editor.Language = Language.JScript     ; break;
					default            : Editor.Language = Language.YAML        ; break;
				}
				CreateAutocomplete();
				Editor.OnSyntaxHighlight(new TextChangedEventArgs(Editor.Range));
			}
		}

		public override string Text { get { return Editor.Text; } set { Editor.Text = value; } }

		#region Fuctions and procedures
		private void Editor_LostFocus(object sender, EventArgs e) {
			HideAllToolTipsAndHints();
		}

		private void HideAllToolTipsAndHints() {
			HideToolTip4Function(true);
			if (Editor.ToolTip != null) Editor.ToolTip.Hide(Editor);
		}

		private void HideToolTip4Function(bool noCheckLine = false) {
			if (IsDisposed) return;
			if (Editor != null && !Editor.IsDisposed && Editor.ToolTip4Function.Visible) {
				if (noCheckLine || Editor.Selection.Start.iLine != Editor.ToolTip4Function.iLine) {
					Editor.ToolTip4Function.Hide(Editor);
				}
			}
		}

		public void ResetTimerForHiddenHmsEditor() {
			if (!Visible && MouseTimerEnable) MouseTimer.Change(5000, Timeout.Infinite); // reset timer no showing editor
		}

		public bool KnownHMSControlHWnd(IntPtr hWnd) {
			return ((hWndScriptDialog  == hWnd)
				   || (hWndScriptFrame == hWnd)
				   || (hWndMemo        == hWnd)
				   || (hWndComboBox    == hWnd)
				   || (hWndBntRun      == hWnd)
				   || (hWndBntStep     == hWnd));
		}

		public bool IsScriptFrameHWnd(IntPtr hWnd) { return (hWndScriptFrame == hWnd); }
		public bool IsHWndButtonRunOrStep(IntPtr hWnd) { return ((hWndBntRun  == hWnd) || (hWndBntStep == hWnd)); }

		public void HighlightInvisibleChars(bool flag) {
			Editor.Range.ClearStyle(InvisibleCharsStyle);
			if (flag) Editor.Range.SetStyle(InvisibleCharsStyle, @".$|.\r\n|\s");
			Editor.Invalidate();
		}

		public void HighlightCurrentLine(bool flag) {
			Editor.CurrentLineColor = flag ? ColorCurrentLine : Color.Transparent;
			Editor.Invalidate();
		}

		public void ShowLineNumbers(bool flag) {
			Editor.ShowLineNumbers = flag;
		}

		public void ShowFoldingLines(bool flag) {
			Editor.ShowFoldingLines = flag;
			Editor.Invalidate();
		}

		public void Undo() {
			if (Editor.UndoEnabled) Editor.Undo();
		}

		public void Redo() {
			if (Editor.RedoEnabled) Editor.Redo();
		}

		public bool NavigateBackward() {
			DateTime max = new DateTime();
			int iLine = -1;
			for (int i = 0; i < Editor.LinesCount; i++)
				if (Editor[i].LastVisit < LastNavigatedDateTime && Editor[i].LastVisit > max) {
					max = Editor[i].LastVisit;
					iLine = i;
				}

			if (iLine >= 0) {
				Editor.Navigate(iLine);
				LastNavigatedDateTime = Editor[iLine].LastVisit;
				Editor.Focus();
				Editor.Invalidate();
				return true;
			}
			return false;
		}

		public bool NavigateForward() {
			DateTime min = DateTime.Now;
			int iLine = -1;
			for (int i = 0; i < Editor.LinesCount; i++)
				if (Editor[i].LastVisit > LastNavigatedDateTime && Editor[i].LastVisit < min) {
					min = Editor[i].LastVisit;
					iLine = i;
				}

			if (iLine >= 0) {
				Editor.Navigate(iLine);
				LastNavigatedDateTime = Editor[iLine].LastVisit;
				Editor.Focus();
				Editor.Invalidate();
				return true;
			}
			return false;
		}

		public void FindKeyPressed(KeyPressEventArgs e, string text) {
			if (e.KeyChar == '\r')
				FindText(text);
			else
				TextBoxFindChanged = true;
		}

		public void FindText(string text, bool forward = true) {
			TextBoxFindChanged = false;
			Range r = TextBoxFindChanged ? Editor.Range.Clone() : Editor.Selection.Clone();
			if (forward) {
				r.End   = new Place(Editor[Editor.LinesCount - 1].Count, Editor.LinesCount - 1);
			} else {
				r.Start = new Place(0, 0);
				r.End   = new Place(Editor.Selection.End.iChar, Editor.Selection.End.iLine);
			}
			var   pattern = Regex.Escape(text);
			bool  founded = false;
			Range foundRange = null;
			foreach (var found in r.GetRanges(pattern)) {
				founded    = true;
				foundRange = found;
				if (forward) break;
			}
			if (founded) {
				foundRange.Inverse();
				Editor.Selection = foundRange;
				Editor.DoSelectionVisible();
			} else {
				MessageBox.Show("\"" + text + "\"" + " не найдено.", MsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}
		}

		public void Breakpoint(int iLine = -1) {
			if (iLine == -1) iLine = Editor.Selection.Start.iLine;
			if (Editor.Breakpoints.Contains(iLine)) {
				Editor.UnbreakpointLine(iLine);
			} else {
				string name = regexPartOfLine.Match(Editor.Lines[iLine]).Value;
				Editor.BreakpointLine(Editor.Selection.Start.iLine, "Точка останова " + (Editor.Breakpoints.counter+1) + " " + name + "...");
			}
		}

		public void Bookmark(int iLine = -1) {
			if (iLine == -1) iLine = Editor.Selection.Start.iLine;
			if (Editor.Bookmarks.Contains(iLine)) {
				Editor.UnbookmarkLine(iLine);
			} else {
				string name = regexPartOfLine.Match(Editor.Lines[iLine]).Value;
				Editor.BookmarkLine(Editor.Selection.Start.iLine, "Закладка " + (Editor.Bookmarks.counter+1) + " " + name + "...");
			}
		}

		public void BookmarkClear() {
			Editor.Bookmarks.Clear();
			Editor.Invalidate();
		}

		public void BookmarkPrevious() {
			Editor.GotoPrevBookmark(Editor.Selection.Start.iLine);
		}

		public void BookmarkNext() {
			Editor.GotoNextBookmark(Editor.Selection.Start.iLine);
		}

		public void HotKeysDialog() {
			var form = new HotkeysEditorForm(Editor.HotkeysMapping);
			if (form.ShowDialog() == DialogResult.OK)
				Editor.HotkeysMapping = form.GetHotkeys();
		}

		private string FileDialogFilter() {
			return "All files (*.*)|*.*|" +
			       "PascalScript files (*.pas)|*.pas|" +
			       "C++Script files (*.cpp)|*.cpp|" +
			       "JavaScript files (*.js)|*.js|" +
			       "BasicScript files (*.bas, *.vb)|*.bas;*.vb|" + 
			       "Yaml files (*.yml)|*.yml|" +
			       "Text files (*.txt)|*.txt";
		}

		private int FileDialogIndexFilter() {
			switch (Editor.Language) {
				case Language.PascalScript: return 2;
				case Language.CPPScript   : return 3;
				case Language.JScript     : return 4;
				case Language.BasicScript : return 5;
				case Language.YAML        : return 6;
			}
			return 1;
		}

		public void OpenFile() {
			OpenFileDialog fileFialog = new OpenFileDialog();
			if (Filename.Length > 0) {
				fileFialog.InitialDirectory = Path.GetDirectoryName(Filename);
				fileFialog.FileName         = Path.GetFileName     (Filename);
			}
			fileFialog.Filter           = FileDialogFilter();
			fileFialog.FilterIndex      = FileDialogIndexFilter();
			fileFialog.RestoreDirectory = true;
			fileFialog.Title            = "Выбор файла скрипта";
			if (fileFialog.ShowDialog() == DialogResult.OK) {
				Filename = fileFialog.FileName;
				if (File.Exists(Filename)) Editor.OpenFile(Filename);
			}

		}

		public void SaveFile() {
			SaveFileDialog fileFialog = new SaveFileDialog();
			if (Filename.Length > 0) {
				fileFialog.InitialDirectory = Path.GetDirectoryName(Filename);
				fileFialog.FileName         = Path.GetFileName     (Filename);
			}
			fileFialog.Filter           = FileDialogFilter();
			fileFialog.FilterIndex      = FileDialogIndexFilter();
			fileFialog.RestoreDirectory = true;
			fileFialog.Title            = "Выбор файла скрипта";
			if (fileFialog.ShowDialog() == DialogResult.OK) {
				Filename = fileFialog.FileName;
				try {
					if (File.Exists(Filename)) File.Delete(Filename);
					File.WriteAllText(Filename, Editor.Text, Encoding.UTF8);
					Modified = false;
					if (TextChangedDelayed != null) TextChangedDelayed(this, null);
				} catch (Exception e) {
					HMS.LogError(e.ToString());
				}
			}

		}

		[EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
		public void Print() {
			var settings = new PrintDialogSettings();
			settings.Title  = Filename;
			settings.Header = "&b&w&b";
			settings.Footer = "&b&p";
			settings.ShowPrintPreviewDialog = true;
			Editor.Print(settings);
		}

		public void AttachExit() {
			if (hWndMemo != System.IntPtr.Zero)
				NativeMethods.ShowWindow(hWndMemo, NativeMethods.SW_SHOW);
			Dispose();
		}

		public void TryAttach() {
			if (Attached || (hWndScriptFrame == IntPtr.Zero) || (hWndMemo == IntPtr.Zero) || (hWndComboBox == IntPtr.Zero)) return;
			Attached = true;
			LoadSettings();
			if (DebugMe) {
				Console.WriteLine("hWnd THmsScriptDialog {0:x8}", hWndScriptDialog.ToInt32());
				Console.WriteLine("hWnd THmsScriptFrame  {0:x8}", hWndScriptFrame .ToInt32());
				Console.WriteLine("hWnd TSynMemo {0:x8}"        , hWndMemo        .ToInt32());
				Console.WriteLine("hWnd TcxComboBox {0:x8}"     , hWndComboBox    .ToInt32());
			}

			StringBuilder sb = new StringBuilder(250); // needs to be big enough for the whole text

			// Get script language
			UpdateScriptLanguage();
			// Hide HMS memo editor
			if (!DebugMe) NativeMethods.ShowWindow(hWndMemo, NativeMethods.SW_HIDE);
			// Set our control to HMS window, which contains memo editor
			NativeMethods.SetParent(Handle, hWndMemoParent);
			// Resize and set content our editor from HMS memo
			UpdateEditor(true);
			GetTextFromMemo(true);
		}

		private void SwitchEditors() {
			if (Attached) {
				if (Editor.Visible) {
					PopupMenu.Close(ToolStripDropDownCloseReason.CloseCalled);
					HideToolTip4Function(true);
					Visible = false;
					MouseTimer.Change(5000, Timeout.Infinite);
					UpdateHmsCaret();
					MouseTimerEnable = true;
					NativeMethods.ShowWindow(hWndMemo, NativeMethods.SW_SHOW);
				} else {
					NativeMethods.ShowWindow(hWndMemo, NativeMethods.SW_HIDE);
					Visible = true;
					MouseTimerEnable = false;
					UpdateEditor();
				}
			}

		}

		public void ClearUndo() {
			Editor.ClearUndo();
		}

		public void Unattach() {
			CreatedControls.Clear();
			if (Attaches.Contains(this)) Attaches.Remove(this);
			SaveSettings();
			Dispose();
		}

		void UpdateCaret() {
			if (Attached && Visible) {
				int pos = NativeMethods.GetCaretPosition(hWndMemo);
                if (pos > 0 && Editor.SelectionStart != pos) {
					Editor.SelectionStart = pos;
					Editor.DoCaretVisible();
					if (DebugMode) {
						Editor.HmsDebugLine = Editor.PositionToPlace(pos).iLine;
						//if (NeedCheckDebugState) CheckDebugState();
						Editor.NeedRecalc();
					}
				}
			}
		}

		void UpdateScriptLanguage() {
			if (Attached && Visible) {
				ScriptLanguage = NativeMethods.GetTextFromControl(hWndComboBox);
			}
		}

		void UpdateEditor(bool firstInit = false) {
			if (!Attached || !Visible || (hWndMemo == IntPtr.Zero)) return;
			NativeMethods.RECT rectEditor = new NativeMethods.RECT();
			NativeMethods.RECT rectParent = new NativeMethods.RECT();
			NativeMethods.GetWindowRect(hWndMemo      , ref rectEditor);
			NativeMethods.GetWindowRect(hWndMemoParent, ref rectParent);
			int left = rectEditor.Left - rectParent.Left;
			int top  = rectEditor.Top  - rectParent.Top;
			if (left == 10) { // resize
				UseWaitCursor = true;
				int w = rectEditor.Right  - rectEditor.Left;
				int h = rectEditor.Bottom - rectEditor.Top;
				if (DebugMe) {
					int newWidth = w / 2;
					NativeMethods.SetWindowPos(hWndMemo, 0, left + newWidth, top, newWidth, h, NativeMethods.SWP_ASYNCWINDOWPOS | NativeMethods.SWP_NOSENDCHANGING);
					NativeMethods.SetWindowPos(Handle, 0, left, top, newWidth, h, NativeMethods.SWP_ASYNCWINDOWPOS | NativeMethods.SWP_NOSENDCHANGING);
				} else {
					NativeMethods.SetWindowPos(Handle, 0, left, top, w, h, NativeMethods.SWP_ASYNCWINDOWPOS | NativeMethods.SWP_NOSENDCHANGING);
				}
				UseWaitCursor = false;
			}
			if (!firstInit) GetTextFromMemo();
			Editor.Focus();
		}

		void GetTextFromMemo(bool firstInit = false) {
			string text = NativeMethods.GetTextFromControl(hWndMemo);
			if (text.Length > 0) {
				if (LastTextLenght != text.Length) {
					Editor.Text      = text;
					Editor.IsChanged = false;
				}
				LastTextLenght = text.Length;
				if (firstInit) {
					Editor.ClearUndo();
				} else {
					UpdateCaret();
				}
			}
		}

		public void UpdateHmsCode() {
			if (Attached && hWndMemo != IntPtr.Zero) {
				NativeMethods.SetTextOfControl(hWndMemo, Editor.Text);
				UpdateHmsCaret();
			}
		}

		public void UpdateHmsCaret(int iLine = -1) {
			if (Attached) {
				int pos = (iLine == -1) ? Editor.SelectionStart : Editor.PlaceToPosition(new Place(0, iLine));
				NativeMethods.SendPosition(hWndMemo, pos);
			}
		}

		/// <summary>
		/// Apply settings from .ini file to the this objects
		/// </summary>
		public void LoadSettings() {
			try { Settings.Load(); } catch (Exception e) { HMS.LogError(e.ToString()); Console.WriteLine("Error loading config file '" + Settings.File + "'", e); return; }

			string section = DialogClass, sVal;
			tsMain.Visible                   = Settings.Get("ToolStripVisible"    , section, tsMain.Visible);
			btnHighlightCurrentLine .Checked = Settings.Get("HighlightCurrentLine", section, btnHighlightCurrentLine .Checked);
			btnShowLineNumbers      .Checked = Settings.Get("ShowLineNumbers"     , section, btnShowLineNumbers      .Checked);
			btnShowFoldingLines     .Checked = Settings.Get("ShowFoldingLines"    , section, btnShowFoldingLines     .Checked);
			btnHighlightSameWords   .Checked = Settings.Get("HighlightSameWords"  , section, btnHighlightSameWords   .Checked);
			btnSetIntelliSense      .Checked = Settings.Get("IntelliSense"        , section, btnSetIntelliSense      .Checked);
			btnHints4CtrlSpace      .Checked = Settings.Get("IntelliOnlyCtrlSpace", section, btnHints4CtrlSpace      .Checked);
			btnIntelliSenseFunctions.Checked = Settings.Get("EnableFunctionHelp"  , section, btnIntelliSenseFunctions.Checked);
			btnEvaluateByMouse      .Checked = Settings.Get("EvaluateByMouse"     , section, btnEvaluateByMouse      .Checked);
			btnAutoCompleteBrackets .Checked = Settings.Get("AutoCompleteBrackets", section, btnAutoCompleteBrackets .Checked);
			btnAutoIdent            .Checked = Settings.Get("AutoIdent"           , section, btnAutoIdent            .Checked);
			btnAutoIdentChars       .Checked = Settings.Get("AutoIdentChars"      , section, btnAutoIdentChars       .Checked);
			btnMarkChangedLines     .Checked = Settings.Get("MarkChangedLines"    , section, btnMarkChangedLines     .Checked);
			btnMouseHelp            .Checked = Settings.Get("MouseHelp"           , section, btnMouseHelp            .Checked);
			btnRedStringsHighlight  .Checked = Settings.Get("StringsHighlight"    , section, btnRedStringsHighlight  .Checked);
			btnToolStripMenuItemFONT.Checked = Settings.Get("AlternateFont"       , section, btnToolStripMenuItemFONT.Checked);
			btnVerticalLineText     .Checked = Settings.Get("VerticalLineText"    , section, btnVerticalLineText     .Checked);

			btnUnderlinePascalKeywords.Checked = Settings.Get("UnderlinePascalKeywords", section, btnUnderlinePascalKeywords.Checked);
			Editor.SyntaxHighlighter.AltPascalKeywordsHighlight = btnUnderlinePascalKeywords.Checked;
			Editor.SyntaxHighlighter.RedStringsHighlight        = btnRedStringsHighlight    .Checked;

			Editor.Font = new Font("Consolas", 9.75f, FontStyle.Regular, GraphicsUnit.Point);
			if ((Editor.Font.Name.ToLower().IndexOf("consolas") < 0) && (HMS.PFC.Families.Length > 1)) {
				btnToolStripMenuItemFONT.Visible = false;
				Editor.Font = new Font(HMS.PFC.Families[1], 10f, FontStyle.Regular, GraphicsUnit.Point);
			} else if (btnToolStripMenuItemFONT.Checked) {
				btnToolStripMenuItemFONT_Click(null, new EventArgs());
            }

			sVal = Settings.Get("Zoom", section, "100");
			Editor.Zoom = Int32.Parse(sVal);

			PopupMenu.OnlyCtrlSpace     = btnHints4CtrlSpace      .Checked;
			PopupMenu.Enabled           = btnSetIntelliSense      .Checked;
			Editor.AutoCompleteBrackets = btnAutoCompleteBrackets .Checked;
			Editor.AutoIndent           = btnAutoIdent            .Checked;
			Editor.AutoIndentChars      = btnAutoIdentChars       .Checked;
			Editor.ChangedLineColor     = btnMarkChangedLines     .Checked ? ColorChangedLine : Color.Transparent;
			EnableFunctionToolTip       = btnIntelliSenseFunctions.Checked;
			EnableEvaluateByMouse       = btnEvaluateByMouse      .Checked;

			Filename       = Settings.Get("LastFile", section, Filename);
			ScriptLanguage = Settings.Get("Language", section, "C++Script");

			HighlightCurrentLine(btnHighlightCurrentLine.Checked);
			ShowLineNumbers (btnShowLineNumbers .Checked);
			ShowFoldingLines(btnShowFoldingLines.Checked);
			btnMouseHelp_Click(null, new EventArgs());
			btnVerticalLineText_Click(null, new EventArgs());

			string hotkeys = Settings.Get("Map", "Hotkeys", "");
			if (hotkeys.Length > 0) {
				HotkeysMapping ourMap = HotkeysMapping.Parse(hotkeys);
				foreach(var pair in ourMap)
					Editor.HotkeysMapping[pair.Key] = pair.Value;
			}

			Editor.Refresh();
		}

		/// <summary>
		/// Save settings in the external .conf file
		/// </summary>
		public void SaveSettings() {
			if (DebugMe) Console.WriteLine("Saving settings to file '" + Settings.File + "'");
			try {
				string section = DialogClass;
				Settings.Set("ToolStripVisible"    , tsMain.Visible                  , section);
				Settings.Set("HighlightCurrentLine", btnHighlightCurrentLine .Checked, section);
				Settings.Set("ShowLineNumbers"     , btnShowLineNumbers      .Checked, section);
				Settings.Set("HighlightSameWords"  , btnHighlightSameWords   .Checked, section);
				Settings.Set("IntelliSense"        , btnSetIntelliSense      .Checked, section);
				Settings.Set("ShowFoldingLines"    , btnShowFoldingLines     .Checked, section);
				Settings.Set("HighlightSameWords"  , btnHighlightSameWords   .Checked, section);
				Settings.Set("IntelliSense"        , btnSetIntelliSense      .Checked, section);
				Settings.Set("IntelliOnlyCtrlSpace", btnHints4CtrlSpace      .Checked, section);
				Settings.Set("EnableFunctionHelp"  , btnIntelliSenseFunctions.Checked, section);
				Settings.Set("EvaluateByMouse"     , btnEvaluateByMouse      .Checked, section);
				Settings.Set("AutoCompleteBrackets", btnAutoCompleteBrackets .Checked, section);
				Settings.Set("AutoIdent"           , btnAutoIdent            .Checked, section);
				Settings.Set("AutoIdentChars"      , btnAutoIdentChars       .Checked, section);
				Settings.Set("MarkChangedLines"    , btnMarkChangedLines     .Checked, section);
				Settings.Set("MouseHelp"           , btnMouseHelp            .Checked, section);
				Settings.Set("StringsHighlight"    , btnRedStringsHighlight  .Checked, section);
				Settings.Set("AlternateFont"       , btnToolStripMenuItemFONT.Checked, section);
				Settings.Set("VerticalLineText"    , btnVerticalLineText     .Checked, section);

				Settings.Set("LastFile"            , Filename                        , section);
				Settings.Set("Language"            , ScriptLanguage                  , section);
				Settings.Set("Zoom"                , Editor.Zoom                     , section);

				Settings.Set("UnderlinePascalKeywords", btnUnderlinePascalKeywords.Checked, section);

				string hotkeys = GetHotKeysMapping();
				if (hotkeys.Length > 0)
					Settings.Set("Map", hotkeys, "Hotkeys");

				Settings.Save();

			} catch (Exception e) {
				HMS.LogError(e.ToString());
				Console.WriteLine("Error saving settings to file '" + Settings.File + "'", e);
			}
		}

		private string GetHotKeysMapping() {
			HotkeysMapping ourMap = new HotkeysMapping();
			HotkeysMapping defMap = new HotkeysMapping();
			ourMap.Clear();
			defMap.InitDefault();
			foreach(var m in Editor.HotkeysMapping) {
				if (defMap.ContainsKey(m.Key)) continue;
				ourMap.Add(m.Key, m.Value);
			}
			return ourMap.ToString();
		}


		public bool LoadFile(string filename) {
			bool success = false;
			if (!String.IsNullOrEmpty(filename) && File.Exists(filename)) {
				Editor.Clear();
				Editor.Text = File.ReadAllText(filename, Encoding.UTF8);
				Editor.ClearUndo();
				Modified = false;
				success = true;
			}
			return success;
		}

		private void ToggleBreakpoint(int iLine = -1) {
			if (Attached) {
				Breakpoint(iLine);
				UpdateHmsCaret(iLine);
				NativeMethods.SendNotifyMessage(hWndMemo, NativeMethods.WM_KEYDOWN, (IntPtr)NativeMethods.VK_F5, IntPtr.Zero);
				return;
			}
		}

		private void DedugRun(int AltKeys) {
			if (!Attached) return;
			DebugMode           = true;
			NeedCheckDebugState = true;
			NativeMethods.SendNotifyMessage(hWndScriptDialog, NativeMethods.WM_ACTIVATE);
			NativeMethods.SendNotifyKey(hWndMemo, NativeMethods.VK_F9 | AltKeys);
		}

		private void SendKey(int key) {
			if (!Attached) return;
			NativeMethods.SendNotifyMessage(hWndMemo, NativeMethods.WM_KEYDOWN, (IntPtr)(key), IntPtr.Zero);
		}

		private void DebugStep() {
			DebugMode           = true;
			NeedCheckDebugState = true;
			NativeMethods.SendNotifyMessage(hWndScriptDialog, NativeMethods.WM_ACTIVATE); // Без этого TMemo не генерит событие EVENT_OBJECT_LOCATIONCHANGE (перемещение каретки)
			NativeMethods.SendNotifyKey(hWndMemo, NativeMethods.VK_F8);
		}
		#endregion Function and procedures

		#region Control Events
		private void Editor_KeyDown(object sender, KeyEventArgs e) {
			if      (e.KeyCode == Keys.F11   ) tsMain.Visible = !tsMain.Visible;
			else if (e.KeyCode == Keys.F12   ) GotoDefinition();
			else if (e.KeyCode == Keys.Escape) HideAllToolTipsAndHints();
			else if (e.Alt) {
				if      (e.KeyCode == Keys.D1) Editor.SetBookmarkByName(Editor.Selection.Start.iLine, "1");
				else if (e.KeyCode == Keys.D2) Editor.SetBookmarkByName(Editor.Selection.Start.iLine, "2");
				else if (e.KeyCode == Keys.D3) Editor.SetBookmarkByName(Editor.Selection.Start.iLine, "3");
				else if (e.KeyCode == Keys.D4) Editor.SetBookmarkByName(Editor.Selection.Start.iLine, "4");
				else if (e.KeyCode == Keys.D5) Editor.SetBookmarkByName(Editor.Selection.Start.iLine, "5");
				else if (e.KeyCode == Keys.D6) Editor.SetBookmarkByName(Editor.Selection.Start.iLine, "6");
				else if (e.KeyCode == Keys.D7) Editor.SetBookmarkByName(Editor.Selection.Start.iLine, "7");
				else if (e.KeyCode == Keys.D8) Editor.SetBookmarkByName(Editor.Selection.Start.iLine, "8");
				else if (e.KeyCode == Keys.D9) Editor.SetBookmarkByName(Editor.Selection.Start.iLine, "9");
			} else if (e.Control) {
				if      (e.KeyCode == Keys.D1) Editor.GotoBookmarkByName("1");
				else if (e.KeyCode == Keys.D2) Editor.GotoBookmarkByName("2");
				else if (e.KeyCode == Keys.D3) Editor.GotoBookmarkByName("3");
				else if (e.KeyCode == Keys.D4) Editor.GotoBookmarkByName("4");
				else if (e.KeyCode == Keys.D5) Editor.GotoBookmarkByName("5");
				else if (e.KeyCode == Keys.D6) Editor.GotoBookmarkByName("6");
				else if (e.KeyCode == Keys.D7) Editor.GotoBookmarkByName("7");
				else if (e.KeyCode == Keys.D8) Editor.GotoBookmarkByName("8");
				else if (e.KeyCode == Keys.D9) Editor.GotoBookmarkByName("9");
			} else if (e.KeyCode == Keys.Oemcomma || (e.Shift && e.KeyCode == Keys.D9)) {
				CheckFunctionHelp = true;
			}

			if (Attached) {
				int AltKeys = 0;
				if (e.Alt    ) AltKeys += NativeMethods.VK_MENU;
				if (e.Shift  ) AltKeys += NativeMethods.VK_SHIFT;
				if (e.Control) AltKeys += NativeMethods.VK_CONTROL;

				if      (e.KeyCode == Keys.F5) ToggleBreakpoint();
				else if (e.KeyCode == Keys.F7) SendKey(NativeMethods.VK_F7 | AltKeys);
				else if (e.KeyCode == Keys.F8) DebugStep();
				else if (e.KeyCode == Keys.F9) DedugRun(AltKeys);
				if (e.Control && (e.KeyCode == Keys.D)) SwitchEditors();
			}

		}

		private void Editor_SelectionChangedDelayed(object sender, EventArgs e) {
			// Remember last visit time of the line in the textbox
			if (Editor.Selection.IsEmpty && Editor.Selection.Start.iLine < Editor.LinesCount) {
				if (LastNavigatedDateTime != Editor[Editor.Selection.Start.iLine].LastVisit) {
					Editor[Editor.Selection.Start.iLine].LastVisit = DateTime.Now;
					LastNavigatedDateTime = Editor[Editor.Selection.Start.iLine].LastVisit;
				}
			}
			if (btnHighlightSameWords.Checked) HighlightSameWords();
			if (btnSetIntelliSense   .Checked) UpdateCurrentVisibleVariables();
			UpdateHmsCaret();
		}

		private void Editor_TextChangedDelayed(object sender, TextChangedEventArgs e) {
			Locked = true;       // Say to other processes we is busy - don't tuch us!
			BuildFunctionList(); // Only when text changed - build the list of functions
			UpdateHmsCode();
			if (EnableFunctionToolTip && CheckFunctionHelp) CheckPositionIsInParametersSequence();
			if (TextChangedDelayed != null) TextChangedDelayed(this, e);
			Locked = false;
		}

		private void Editor_TextChanged(object sender, TextChangedEventArgs e) {
			NeedRecalcVars = true;
		}

		private void btnOpen_Click(object sender, EventArgs e) {
			OpenFile();
		}

		private void btnSave_Click(object sender, EventArgs e) {
			SaveFile();
		}

		private void btnPrint_Click(object sender, EventArgs e) {
			Print();
		}

		private void btnCut_Click(object sender, EventArgs e) {
			Editor.Cut();
		}

		private void btnCopy_Click(object sender, EventArgs e) {
			Editor.Copy();
		}

		private void btnPaste_Click(object sender, EventArgs e) {
			Editor.Paste();
		}

		private void btnInvisibleChars_Click(object sender, EventArgs e) {
			HighlightInvisibleChars(btnInvisibleChars.Checked);
		}

		private void btnHighlightCurrentLine_Click(object sender, EventArgs e) {
			HighlightCurrentLine(btnHighlightCurrentLine.Checked);
		}

		private void btnShowLineNumbers_Click(object sender, EventArgs e) {
			ShowLineNumbers(btnShowLineNumbers.Checked);
		}

		private void btnShowFoldingLines_Click(object sender, EventArgs e) {
			ShowFoldingLines(btnShowFoldingLines.Checked);
		}

		private void btnUndo_Click(object sender, EventArgs e) {
			Undo();
		}

		private void btnRedo_Click(object sender, EventArgs e) {
			Redo();
		}

		private void btnNavigateBack_Click(object sender, EventArgs e) {
			NavigateBackward();
		}

		private void btnNavigateForward_Click(object sender, EventArgs e) {
			NavigateForward();
		}

		private void tbFind_KeyPress(object sender, KeyPressEventArgs e) {
			FindKeyPressed(e, tbFind.Text);
			tbFind.Focus();
		}

		private void btnFindPrev_Click(object sender, EventArgs e) {
			FindText(tbFind.Text, false);
		}

		private void btnFindNext_Click(object sender, EventArgs e) {
			FindText(tbFind.Text);
		}

		private void btnBookmarkPlus_Click(object sender, EventArgs e) {
			Bookmark();
		}

		private void btnBookmarkMinus_Click(object sender, EventArgs e) {
			if (Editor.Bookmarks.Count < 1) return;

			string txt = "";
			foreach (var b in Editor.Bookmarks) txt += b.Name + "\n";

			if (MessageBox.Show("Удалить все вкладки?\n"+ txt, MsgCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question)== DialogResult.Yes)
				BookmarkClear();
		}

		private void btnBookmarkPrevious_Click(object sender, EventArgs e) {
			BookmarkPrevious();
		}

		private void btnBookmarkNext_Click(object sender, EventArgs e) {
			BookmarkNext();
		}

		private void btnGoTo_DropDownOpening(object sender, EventArgs e) {
			btnGoTo.DropDownItems.Clear();
			foreach (var bookmark in Editor.Bookmarks) {
				ToolStripItem item = btnGoTo.DropDownItems.Add(bookmark.Name, imageList1.Images[9]);
				item.Tag = bookmark;
				item.Click += (o, a) => {
					var b = (Bookmark)(o as ToolStripItem).Tag;
					b.DoVisible();
				};
			}
			// --------------------------------------------------------
			foreach (HMSItem item in Functions) {
				ToolStripItem tipItem = btnGoTo.DropDownItems.Add(item.MenuText, imageList1.Images[item.ImageIndex]);
				tipItem.Tag = item.PositionStart;
				tipItem.Click += (o, a) => {
					try {
						Editor.SelectionStart = (int)(o as ToolStripItem).Tag;
						Editor.DoRangeVisible(Editor.Selection, true);
						Editor.Invalidate();
					} catch { }
				};
			}
		}

		private void toolStripButtonHotKeys_Click(object sender, EventArgs e) {
			HotKeysDialog();
		}

		private void btnHighlightSameWords_Click(object sender, EventArgs e) {
			if (!btnHighlightSameWords.Checked)
				Editor.Range.ClearStyle(SameWordsStyle);
			else
				HighlightSameWords();
		}

		private void btnSetIntelliSense_Click(object sender, EventArgs e) {
			PopupMenu.Enabled = btnSetIntelliSense.Checked;
		}

		private void btnIntelliSenseFunctions_Click(object sender, EventArgs e) {
			EnableFunctionToolTip = btnIntelliSenseFunctions.Checked;
		}

		private void btnEvaluateByMouse_Click(object sender, EventArgs e) {
			EnableEvaluateByMouse = btnEvaluateByMouse.Checked;
		}

		private void btnAutoCompleteBrackets_Click(object sender, EventArgs e) {
			Editor.AutoCompleteBrackets = btnAutoCompleteBrackets.Checked;
		}

		private void btnHints4CtrlSpace_Click(object sender, EventArgs e) {
			PopupMenu.OnlyCtrlSpace = btnHints4CtrlSpace.Checked;
		}

		private void btnAutoIdent_Click(object sender, EventArgs e) {
			Editor.AutoIndent = btnAutoIdent.Checked;
		}

		private void btnAutoIdentChars_Click(object sender, EventArgs e) {
			Editor.AutoIndentChars = btnAutoIdentChars.Checked;
		}

		private void btnMarkChangedLines_Click(object sender, EventArgs e) {
			Editor.ChangedLineColor = btnMarkChangedLines.Checked ? ColorChangedLine : Color.Transparent;
		}

		private void ToolStripMenuItemCut_Click(object sender, EventArgs e) {
			Editor.Cut();
		}

		private void ToolStripMenuItemCopy_Click(object sender, EventArgs e) {
			Editor.Copy();
		}

		private void ToolStripMenuItemPaste_Click(object sender, EventArgs e) {
			Editor.Paste();
		}

		private void ToolStripMenuItemDelete_Click(object sender, EventArgs e) {
			Editor.Delete();
		}

		private void ToolStripMenuItemBookmarkClear_Click(object sender, EventArgs e) {
			BookmarkClear();
		}

		private void ToolStripMenuItemClearBreakpoints_Click(object sender, EventArgs e) {
			List<int> lines = new List<int>();
			foreach (Bookmark b in Editor.Breakpoints) lines.Add(b.LineIndex);
			foreach (int iLine in lines) ToggleBreakpoint(iLine);
		}

		private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
			ToolStripMenuItemZoom100         .Enabled = (Editor.Zoom != 100);
			ToolStripMenuItemUndo            .Enabled = Editor.UndoEnabled;
			ToolStripMenuItemRedo            .Enabled = Editor.RedoEnabled;
			ToolStripMenuItemBookmarkClear   .Enabled = (Editor.Bookmarks.Count > 0);
			ToolStripMenuItemClearBreakpoints.Enabled = (Editor.Breakpoints.Count > 0);
		}

		private void ToolStripMenuItemZoom100_Click(object sender, EventArgs e) {
			Editor.Zoom = 100;
		}

		private void ToolStripMenuItemUndo_Click(object sender, EventArgs e) {
			Undo();
		}

		private void ToolStripMenuItemRedo_Click(object sender, EventArgs e) {
			Redo();
		}

		private void ToolStripMenuItemAltPascalScriptHighlight_Click(object sender, EventArgs e) {
			Editor.SyntaxHighlighter.AltPascalKeywordsHighlight = btnUnderlinePascalKeywords.Checked;
			Editor.OnSyntaxHighlight(new TextChangedEventArgs(Editor.Range));
		}

		private void btnRedStringsHighlight_Click(object sender, EventArgs e) {
			Editor.SyntaxHighlighter.RedStringsHighlight = btnRedStringsHighlight.Checked;
			Editor.OnSyntaxHighlight(new TextChangedEventArgs(Editor.Range));
		}

		private void Editor_SelectionChanged(object sender, EventArgs e) {
			HideToolTip4Function();
		}

		private void Editor_Scroll(object sender, ScrollEventArgs e) {
			HideToolTip4Function(true);
		}

		private void btnToolStripMenuItemFONT_Click(object sender, EventArgs e) {
			if (btnToolStripMenuItemFONT.Checked && (HMS.PFC.Families.Length > 1))
				Editor.Font = new Font(HMS.PFC.Families[1], 10f, FontStyle.Regular, GraphicsUnit.Point);
			else
				Editor.Font = new Font("Consolas", 9.75f, FontStyle.Regular, GraphicsUnit.Point);
		}

		private void EditorMouseClick(object sender, MouseEventArgs e) {
			if (e.X < (Editor.LeftIndent - 12)) {
				int iFirstLine = Editor.YtoLineIndex();
				int yFirstLine = Editor.LineInfos[iFirstLine].startY - Editor.VerticalScroll.Value;
                int iLine = (int)((e.Y - yFirstLine) / (Editor.Font.Height - 1)) + iFirstLine;
				ToggleBreakpoint(iLine);
			}
		}

		private void Editor_MouseMove(object sender, MouseEventArgs e) {
			if (MouseTimerEnable) {
				if (MouseLocation == e.Location) {
					// Mouse stopped
				} else {
					// Mouse mooving
					MouseLocation = e.Location;
					ActiveEditor  = this;
					if (ValueHint.Visible) ValueHint.Visible = false;
                    MouseTimer.Change(500, Timeout.Infinite); // Show help tooltip from mouse cursor
				}
			}
		}

		private void btnMouseHelp_Click(object sender, EventArgs e) {
			MouseTimerEnable = btnMouseHelp.Checked;
		}

		private void Editor_MouseClick(object sender, MouseEventArgs e) {
			EditorMouseClick(sender, e);
		}

		private void btnVerticalLineText_Click(object sender, EventArgs e) {
			Editor.PreferredLineWidth = btnVerticalLineText.Checked ? 80 : 0;
		}

		private void btnAbout_Click(object sender, EventArgs e) {
			AboutDialog aboutDialog = new AboutDialog();
			aboutDialog.ShowDialog();
		}

		private void btnUnload_Click(object sender, EventArgs e) {
			string msg = "Выйти и выгрузить редактор из памяти?\n\n" +
						 "После выгрузки, открытие редактирования скриптов будет происходить в стандартном редакторе.\n";
			if (MessageBox.Show(msg, MsgCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
				Exit();
				Application.Exit();
			}
		}
		#endregion Control Events

		#region Smart IDE functions
		private static string MatchReturnEmptyLines(Match m) { return regexNotNewLine.Replace(m.Value, CensChar.ToString()); }
		private static string MatchRemoveLinebreaks(Match m) { return regexLineBreaks.Replace(m.Value, String.Empty); }

		public Place PointToPlace(Point point) { return Editor.PointToPlace(point); }

		public string EvalVariableValue(string varName) {
			if (!Attached) return "";
			EvalName   = varName;
			EvalResult = "";
			hWndEditor4EvalCreate = hWndScriptFrame;
			if (!CheckCreatedEvaluateDialog()) {
				_createEvalDlg   = true;
				NativeMethods.SendNotifyMessage(hWndBntEval, NativeMethods.BM_CLICK);  // click on evaluate button
			}
			return EvalResult;
		}

		public bool CheckDebugState() {
			NeedCheckDebugState = false;
			Editor.DebugMode = EvalVariableValue("DebugMode") == "True";
			return Editor.DebugMode;
		}

		private void HighlightSameWords() {
			Editor.Range.ClearStyle(SameWordsStyle);
			if (!Editor.Selection.IsEmpty) return;
			var fragment = Editor.Selection.GetFragment(@"\w");
			string text = fragment.Text;
			if (text.Length > 0) {
				Editor.Range.SetStyle(SameWordsStyle, "\\b" + text + "\\b", RegexOptions.Multiline);
			}
		}

		public string WhithoutStringAndComments(string txt, bool trimNotClosed = false) {
			switch (Editor.Language) {
				case Language.CPPScript   : txt = regexStringAndCommentsCPP   .Replace(txt, evaluatorSameLines); break;
				case Language.PascalScript: txt = regexStringAndCommentsPascal.Replace(txt, evaluatorSameLines); break;
				case Language.JScript     : txt = regexStringAndCommentsCPP   .Replace(txt, evaluatorSameLines); break;
				case Language.BasicScript : txt = regexStringAndCommentsBasic .Replace(txt, evaluatorSameLines); break;
			}
			if (trimNotClosed) {
				txt = Regex.Replace(txt, "[\"'].*", "");
			}
			return txt;
		}

		private static Regex regexTextOfComment = new Regex(@"^\s*?//.*?(\w.*)", RegexOptions.Compiled);
		private void BuildFunctionList() {
			Functions.Clear();
			MatchCollection mc = null;
			string startBlock = "", endBlock = "";
			string txt = WhithoutStringAndComments(Editor.Text);
			switch (Editor.Language) {
				case Language.CPPScript   :
				case Language.JScript     : mc = regexProceduresCPP   .Matches(txt); startBlock = "{"; endBlock = "}"; break;
				case Language.PascalScript: mc = regexProceduresPascal.Matches(txt); startBlock = @"\b(begin|try)\b"; endBlock = @"\b(end)\b"; break;
				case Language.BasicScript : mc = regexProceduresBasic .Matches(txt); startBlock = @"\b(Sub|Function)\b"; endBlock = @"\bEnd (Sub|Function)\b"; break;
			}

			if (mc != null) {
				foreach (Match m in mc) {
					string name = m.Groups[1].Value;
					if (regexExcludeWords.IsMatch(m.Value)) continue;
					HMSItem item = new HMSItem();
					item.Type          = m.Groups["type"].Value;
					item.Text          = name;
					item.MenuText      = name;
					item.Kind          = regexDetectProcedure.IsMatch(m.Value) ? DefKind.Procedure : DefKind.Function;
					item.ImageIndex    = (item.Kind == DefKind.Function) ? Images.Function : Images.Procedure;
					item.ToolTipTitle  = name;
					item.ToolTipText   = ((item.Kind == DefKind.Function) ? "Функция" : "Процедура") + " (объявлена в скрипте)";
					item.PositionStart = m.Groups[1].Index;
					item.PositionEnd   = item.PositionStart + m.Groups[1].Value.Length;
					// check comment before procedure
					int iLine = Editor.PositionToPlace(item.PositionStart).iLine;
					if (iLine > 0) {
						Match matchComment = regexTextOfComment.Match(Editor.Lines[iLine-1]);
						if (matchComment.Success) item.Help = matchComment.Groups[1].Value;
					}
					// search end of procedure
					if (startBlock.Length > 0) {
						var stack = new Stack<string>();
						MatchCollection mc2 = Regex.Matches(txt.Substring(item.PositionStart), "(" + startBlock + "|" + endBlock + ")", StdOpt);
						foreach (Match m2 in mc2) {
							if (Regex.IsMatch(m2.Value, startBlock, StdOpt)) stack.Push(startBlock);
							else if (stack.Count > 0) stack.Pop();
							item.PositionEnd = item.PositionStart + m2.Groups[1].Index;
							if (stack.Count < 1) break;
						}
						item.PositionEnd += endBlock.Length;
					}
					string s = new Range(Editor, Editor.PositionToPlace(item.PositionStart), Editor.PositionToPlace(item.PositionEnd)).Text;
					Match m3 = Regex.Match(s, @"^(.*?)(\bvar\b|" + startBlock + ")", RegexOptions.IgnoreCase | RegexOptions.Singleline);
					if (m3.Success) item.ToolTipTitle = m3.Groups[1].Value.Trim().Replace("\r", "").Replace("\n", "");
					if (item.Kind == DefKind.Function) {
						if (Editor.Language == Language.PascalScript) {
							item.Type = HMS.GetVarTypePascalFormat(item.ToolTipTitle);
						} else {
							item.ToolTipTitle = item.Type + " " + item.ToolTipTitle;
						}
					}
					Functions.Add(item);
				}
			}
			// add info about main start procedure

			if (Functions.LastEndPosition < txt.Length) {
				Match matchMainProc = Regex.Match(txt.Substring(Functions.LastEndPosition), startBlock, StdOpt);
				if (matchMainProc.Success) {
					HMSItem item = new HMSItem();
					item.Type          = "MainProcedure";
					item.Text          = "Главная процедура";
					item.MenuText      = item.Text;
					item.Kind          = DefKind.Procedure;
					item.Help          = "Процедура, с которой начинается запуск скрипта";
					item.ImageIndex    = Images.Procedure;
					item.PositionStart = Functions.LastEndPosition + matchMainProc.Index;
					item.PositionEnd   = txt.Length - 1;
					Functions.Add(item);
				}
			}
			PopupMenu.Items.SetVisibleFunctionsItems(Functions);
		}

		private string CurrentWord() {
			Range r = new Range(Editor, Editor.Selection.Start, Editor.Selection.Start);
			return r.GetFragment(@"[\w]").Text;
		}

		private string GetGlobalContext() {
			char[] txt = WhithoutStringAndComments(Editor.Text).ToCharArray();
			foreach (HMSItem item in Functions) {
				for (int i = item.PositionStart; i < item.PositionEnd; i++) {
					if (i >= txt.Length) break;
					txt[i] = txt[i] != '\n' ? CensChar : '\n';
				}
			}
			return new string(txt);
		}

		private HMSItem GetCurrentProcedure(int position) {
			foreach (var item in Functions) if ((position > item.PositionStart) && (position < item.PositionEnd)) return item;
			return null;
		}

		private void GotoDefinition() {
			Editor_SelectionChangedDelayed(null, new EventArgs());
			string name = CurrentWord();
			if      (Variables.ContainsName(name)) GotoPosition(Variables[name].PositionStart);
			else if (LocalVars.ContainsName(name)) GotoPosition(LocalVars[name].PositionStart);
			else if (Functions.ContainsName(name)) GotoPosition(Functions[name].PositionStart);
		}

		private void UpdateCurrentVisibleVariables(int position = -1) {
			if (Editor.Language == Language.YAML) return;
			if (position < 0) position = Editor.SelectionStart;
			HMSItem itemFunction = GetCurrentProcedure(position);

			if (itemFunction != null) {
				if ((itemFunction.PositionStart == LastPtocedureIndex) && !NeedRecalcVars) return; // We are in same procedure - skip update
				LastPtocedureIndex = itemFunction.PositionStart;
			} else if ((LastPtocedureIndex == 0) && !NeedRecalcVars) {
				return;
			} else
				LastPtocedureIndex = 0;

			NeedRecalcVars = false;
			LocalVars.Clear();

			if (itemFunction!=null) {
				string context = WhithoutStringAndComments(Editor.GetRange(itemFunction.PositionStart, itemFunction.PositionEnd).Text);
				if (context.Length > 0) GetVariables(context, itemFunction.PositionStart, LocalVars);
				if (itemFunction.Kind == DefKind.Function) {
					HMSItem hmsItem = new HMSItem("Result");
					hmsItem.ImageIndex    = Images.Field;
					hmsItem.MenuText      = hmsItem.Text;
					hmsItem.Type          = itemFunction.Type;
					hmsItem.PositionStart = itemFunction.PositionStart;
					hmsItem.PositionEnd   = itemFunction.PositionEnd;
					hmsItem.ToolTipText   = "Переменная, хранящая значение, которое будет возвращено функцией как результат.";
					hmsItem.Help          = "Используется в PascalScript, но видна как переменная и в других режимах синтаксиса.\nИмеет такой-же тип, как и функция, в которой она видна.";
					hmsItem.ToolTipTitle  = "Result: " + hmsItem.Type;
					LocalVars.Add(hmsItem);
				}
			} else {
				Variables.Clear();
				string contextGlobal = GetGlobalContext();
				if (contextGlobal.Length > 0) GetVariables(contextGlobal, 0, Variables);
			}
			PopupMenu.Items.SetVisibleVariablesItems(Variables);
			PopupMenu.Items.SetLocalssVariablesItems(LocalVars);
		}

		private string GetTypeOfConstant(string part) {
			if (part.Length == 0        ) return "String";
			if (regexIsNum.IsMatch(part)) return "Integer";
			if (regexIsStr.IsMatch(part)) return "String";
			return "Variant";
		}

		private void GetVariables(string txt, int indexContext, AutocompleteItems ITEMS) {
			MatchCollection mc = null; bool isGlobalContext = (indexContext == 0);
			// Collect constants
			if (isGlobalContext) {
				switch (Editor.Language) {
					case Language.CPPScript:
						mc = regexSearchConstantsCPP.Matches(txt);
						foreach (Match m in mc) {
							string name = m.Groups[1].Value;
							string sval = m.Groups[2].Value.Trim(); // Value
							if (!ITEMS.ContainsName(name)) {
								HMSItem item = new HMSItem();
								item.Global        = isGlobalContext;
								item.Kind          = DefKind.Constant;
								item.ImageIndex    = Images.Enum;
								item.Text          = name.Trim();
								item.MenuText      = RemoveLinebeaks(item.Text);
								item.ToolTipTitle  = item.Text;
								item.ToolTipText   = "Объявленная константа";
								item.Type          = GetTypeOfConstant(sval);
								item.PositionStart = m.Groups[1].Index + indexContext;
								item.PositionEnd   = item.PositionStart + name.Length;
								if (item.Type.Length > 0) item.ToolTipText += "\nТип: " + item.Type;
								if ((sval.Length == 0) || (sval == ";")) sval = regexExractConstantValue.Match(Editor.Text.Substring(m.Groups[2].Index, 96)).Value;
								if (sval.Length > 0) item.Help += "\nЗначение: " + sval;
								ITEMS.Add(item);
							}
						}
						break;
					case Language.PascalScript:
						Match c = regexSearchConstantsPascal1.Match(txt);
						if (c.Success) {
							mc = regexSearchConstantsPascal2.Matches(c.Groups[1].Value);
							foreach (Match m in mc) {
								string name = m.Groups[1].Value;
								string sval = m.Groups[2].Value.Trim(); // Value
								if (!ITEMS.ContainsName(name)) {
									HMSItem item = new HMSItem();
									item.Global        = isGlobalContext;
									item.Kind          = DefKind.Constant;
									item.PositionStart = c.Groups[1].Index + m.Index + indexContext;
									item.PositionEnd   = item.PositionStart + name.Length;
									item.ImageIndex    = Images.Enum;
									item.Text          = name.Trim();
									item.MenuText      = RemoveLinebeaks(item.Text);
									item.ToolTipTitle  = item.Text;
									item.ToolTipText   = "Объявленная константа";
									item.Type          = GetTypeOfConstant(sval);
									if (item.Type.Length > 0) item.ToolTipText += "\nТип: " + item.Type;
									if ((sval.Length == 0) || (sval == ";")) sval = regexExractConstantValue.Match(Editor.Text.Substring(m.Groups[2].Index, 96)).Value;
									if (sval.Length > 0) item.Help += "\nЗначение: " + sval;
									ITEMS.Add(item);
								}
							}

						}
						break;
				}
			}

			mc = null;
			switch (Editor.Language) {
				case Language.CPPScript   : mc = regexSearchVarsCPP   .Matches(txt); break;
				case Language.JScript     : mc = regexSearchVarsJS    .Matches(txt); break;
				case Language.PascalScript: mc = regexSearchVarsPascal.Matches(txt); break;
			}
			if (mc != null) {
				foreach (Match m in mc) {
					int    index = m.Groups["vars"].Index;
					string names = m.Groups["vars"].Value;
					string type  = m.Groups["type"].Value.Trim();
					if (!ValidHmsType(type)) continue;
					names = HMS.GetTextWithoutBrackets(names); // Убираем скобки и всё что в них
					names = regexAssignment  .Replace(names, evaluatorSpaces); // Убираем присвоение - знак равно и после
					names = regexConstantKeys.Replace(names, evaluatorSpaces); // Убираем ключевые слова констант (var, const)
					string[] aname = names.Split(',');
					foreach (string namePart in aname) {
						string name = namePart;
						if ((namePart.Trim().Length != 0) && !regexExcludeWords.IsMatch(namePart)) {
							if (Regex.IsMatch(name, @"\b(\w+).*?\b(\w+).*?\b(\w+)")) continue;
							Match m2 = regexTwoWords.Match(name);
							if (m2.Success) {
								bool typeFirst = (index > m.Groups["type"].Index);
								type   = m2.Groups[typeFirst ? 1 : 2].Value;
								name   = m2.Groups[typeFirst ? 2 : 1].Value;
								index += m2.Groups[typeFirst ? 2 : 1].Index;
							}

							if (!regexNotValidCharsInVars.IsMatch(name) && !ITEMS.ContainsName(name)) {
								HMSItem item = new HMSItem();
								item.Global        = isGlobalContext;
								item.Kind          = DefKind.Variable;
								item.Text          = name.Trim();
								item.Type          = type.Trim();
								item.MenuText      = RemoveLinebeaks(item.Text);
								item.ToolTipTitle  = item.Text;
								item.ToolTipText   = item.Global ? "Глобальная переменная" : "Локальная переменная";
								item.PositionStart = index + (name.Length - name.TrimStart().Length) + indexContext;
								item.PositionEnd   = item.PositionStart + name.Length;
								item.ImageIndex    = Images.Field;
								if (item.Type.Length > 0) item.ToolTipText += "\nТип: " + item.Type;
								ITEMS.Add(item);
							}
							if (m2.Success) index -= m2.Groups["vars"].Index;
						}
						index += namePart.Length + 1;
					}
				}
			}
		}

		private bool ValidHmsType(string type) {
			string lowertype = type.ToLower();
			if (CurrentValidTypes.IndexOf("|" + lowertype + "|") >= 0) return true;
			if (HMS.ClassesString.IndexOf("|" + lowertype + "|") >= 0) return true;
			return false;
		}

		public HMSItem GetHMSItemByText(string text) {
			string partAfterDot;
			return GetHMSItemByText(text, out partAfterDot);
		}

		public HMSItem GetHMSItemByText(string text, out string partAfterDot, bool returnItemBeforeDot = false) {
			HMSItem      item = null;
			HMSClassInfo info = new HMSClassInfo();
			
			string[] names = text.ToLower().Split('.');
			int count = 0; partAfterDot = "";
			foreach (string word in names) {
				string name = HMS.GetTextWithoutBrackets(word);
				count++; partAfterDot = name;
				if (returnItemBeforeDot && (count >= names.Length)) break; // return last item before the dot
				if (info.Name.Length > 0) {
					// search in class members
					                  item = info.MemberItems.GetItemOrNull(name);
					if (item == null) item = info.StaticItems.GetItemOrNull(name);
					if (item != null) info = HMS.HmsClasses[item.Type];
				} else {
					// try get variabe
					if (item == null) item =         LocalVars.GetItemOrNull(name); // try visible known variables
					if (item == null) item =         Variables.GetItemOrNull(name); // try visible known variables
					if (item == null) item =         Functions.GetItemOrNull(name); // try functions in script
					if (item == null) item = HMS.ItemsVariable.GetItemOrNull(name); // try internal variables
					if (item == null) item = HMS.ItemsConstant.GetItemOrNull(name); // try internal constants
					if (item == null) item = HMS.ItemsFunction.GetItemOrNull(name); // try internal functions
					if (item == null) item = HMS.ItemsClass   .GetItemOrNull(name); // try internal classes
					if (count < names.Length) {
						if (item != null) {
							info = HMS.HmsClasses[item.MenuText];
							if (info.Name.Length == 0) info = HMS.HmsClasses[item.Type];
						} else break;
					}
				}
			}
			return item;
		}

		private string NameCase(string s) {
			if (s.Length > 0) {
				char[] c = s.Trim().ToLower().ToCharArray();
				c[0] = c[0].ToString().ToUpper().ToCharArray()[0];
				s = new String(c);
			}
			return s;
		}

		private void GotoPosition(int position) {
			Editor.SelectionStart = position;
			Editor.DoRangeVisible(Editor.Selection, true);
			Editor.Invalidate();
		}
		private static Regex regexSplitFuncParam   = new Regex("[,;]", RegexOptions.Compiled);
		private static Regex regexFoundOurFunction = new Regex(@"([\w\.]+)\s*?[\(\[]([^\)\]]*)$", RegexOptions.Compiled | RegexOptions.Singleline);

		private void CheckPositionIsInParametersSequence() {
			string name, parameters;

			int   iBack = Math.Max(Editor.Selection.Start.iLine - 3, 0); // Look back 3 lines maximum
			Place place = new Place(0, iBack);
			Range range = new Range(Editor, place, Editor.Selection.Start);
			CheckFunctionHelp = false;
            HMS.CurrentParamType = "";
			string text = WhithoutStringAndComments(range.Text, true);
			Match m = regexFoundOurFunction.Match(text);
			if (m.Success) {
				name       = m.Groups[1].Value;
				parameters = m.Groups[2].Value;
				Place pp = Editor.PositionToPlace(Editor.PlaceToPosition(range.Start) + m.Index);
				int iLinesCorrect = Editor.Selection.Start.iLine - pp.iLine + 1;
				Point p  = Editor.PositionToPoint(Editor.PlaceToPosition(range.Start) + m.Index);
				p.Offset(0, Editor.CharHeight * iLinesCorrect + 2 );
				Editor.ToolTip4Function.iLine = Editor.Selection.Start.iLine;
				ShowFunctionToolTip(p, name, parameters);
			} else {
				if (Editor.ToolTip4Function.Visible)
					HideToolTip4Function(true);
			}
			return;
		}

		private void ShowFunctionToolTip(Point p, string name, string parameters="") {
			int paramNum = regexSplitFuncParam.Split(parameters).Length;
			HMS.CurrentParamType = "";
			HMSItem item = GetHMSItemByText(name);
			if (item != null) {
				if ((Editor.SelectionStart >= item.PositionStart) && (Editor.SelectionStart <= item.PositionEnd)) return; // we writing this function
				Editor.ToolTip4Function.ShowFunctionParams(item, paramNum, Editor, p);
				if (HMS.CurrentParamType.Length > 0) PopupMenu.Show(false);
            }
		}

		#endregion

		private void SetAutoCompleteMenu() {
			PopupMenu = new AutocompleteMenu(Editor, this);
			PopupMenu.ImageList         = imageList1;
			PopupMenu.SelectedColor     = Color.DeepSkyBlue;
			PopupMenu.MinFragmentLength = 1;
			PopupMenu.Items.MaximumSize = new Size(200, 300);
			PopupMenu.Items.Width       = 200;
		}

		private void CreateAutocomplete() {
			if (PopupMenu == null || PopupMenu.IsDisposed) return;
			string hmsTypes = HMS.HmsTypesStringWithHelp;
			string keywords = "", snippets = "";
			string hlp = "", key = "";
			CurrentValidTypes = HMS.HmsTypesString;
			switch (ScriptLanguage) {
				case "C++Script":
					hmsTypes = hmsTypes.Replace("Integer|", "").Replace("Boolean|", "");
					keywords = "#include|#define|new|break|continue|exit|delete|return|if|else|switch|default|case|do|while|for|try|finally|except|in|is|";
					snippets = "if (^) {\n}|if (^) {\n}\nelse {\n}|for (^;;) {\n}|while (^) {\n}|do {\n^}while ();";
					CurrentValidTypes += "int|long|void|bool|float|";
					break;
				case "PascalScript":
					keywords = "Program|Uses|Const|Var|Not|In|Is|OR|XOR|DIV|MOD|AND|SHL|SHR|Break|Continue|Exit|Begin|End|If|Then|Else|Casr|Of|Repeat|Until|While|Do|For|To|DownTo|Try|Finally|Except|With|Function|Procedure";
					snippets = "If ^ Then |If (^) Then Begin\nEnd else Begin\nEnd;";
					break;
				case "BasicScript":
					keywords = "EOL|IMPORTS|DIM|AS|NOT|IN|IS|OR|XOR|MOD|AND|ADDRESSOF|BREAK|CONTINUE|EXIT|DELETE|SET|RETURN|IF|THEN|END|ELSEIF|ELSE|SELECT|CASE|DO|LOOP|UNTIL|WHILE|WEND|FOR|TO|STEP|NEXT|TRY|FINALLY|CATCH|WITH|SUB|FUNCTION|BYREF|BYVAL";
					break;
				case "JScript":
					hmsTypes = "var";
					keywords = "import|new|in|is|break|continue|exit|delete|return|if|else|switch|default|case|do|while|for|try|finally|except|function|with";
					break;
			}
			HMS.KeywordsString = keywords.ToLower();
			snippets += "|ShowMessage(\"^\");|HmsLogMessage(1, \"^\");";

			var items = new AutocompleteItems();

			foreach (var s in keywords.Split('|')) if (s.Length > 0) items.Add(new HMSItem(s, Images.Keyword, s, s, "Ключевое слово"));
			foreach (var s in snippets.Split('|')) if (s.Length > 0) items.Add(new SnippetHMSItem(s) { ImageIndex = Images.Snippet });

			foreach (var name in hmsTypes.Split('|')) {
				Match m = Regex.Match(name, "{(.*?)}");
				if (m.Success) hlp = m.Groups[1].Value;
				key = Regex.Replace(name, "{.*?}", "");
				items.Add(new HMSItem(key, Images.Keyword, key, key, hlp));
			}

			PopupMenu.Items.SetAutocompleteItems(items);

			PopupMenu.Items.AddAutocompleteItems(HMS.ItemsFunction);
			PopupMenu.Items.AddAutocompleteItems(HMS.ItemsVariable);
			PopupMenu.Items.AddAutocompleteItems(HMS.ItemsConstant);
			PopupMenu.Items.AddAutocompleteItems(HMS.ItemsClass   );

			// Set templates for selected script language
			btnInsertTemplate.DropDownItems.Clear();
			AddTemplateItemsRecursive(btnInsertTemplate, HMS.Templates[Editor.Language]);
			btnInsertTemplate.Visible = btnInsertTemplate.DropDownItems.Count > 0;
		}

		private void AddTemplateItemsRecursive(ToolStripMenuItem menuItem, Templates templates) {
			foreach (TemplateItem templateItem in templates) {
				ToolStripItem item = HMS.SetTemplateMenuItem(menuItem, templateItem.Name, templateItem.Text);
				if (templateItem.Submenu) {
					AddTemplateItemsRecursive((ToolStripMenuItem)item, templateItem.ChildItems);

				} else {
					item.Click += (o, a) => {
						if (HMSEditor.ActiveEditor != null) {
							Editor.InsertText((o as ToolStripItem).AccessibleDescription.Trim());
						}
					};
				}
			}

		}

		private static Regex regexCheckRussian = new Regex("[а-яА-Я]", RegexOptions.Compiled);
		private static bool isUTF8(string str) {
			return regexCheckRussian.IsMatch(str);
		}

	}

}
