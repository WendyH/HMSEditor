/* This code is released under WTFPL Version 2 (http://www.wtfpl.net/) * Created by WendyH. Copyleft. */
using System;
using System.Windows.Forms; // Application
using System.Reflection;    // Assembly

namespace HMSEditorNS {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {

			// Запоминаем агрументы запуска (могут понадобиться при перезапуске после обновления)
			HMS.StartArgs = string.Join(" ", args); 
			// Заголовок для всех MessageBox
			HMSEditor.MsgCaption += " v" + Application.ProductVersion;

			// Класс SingleGlobalInstance используется для проверки, не запущена ли уже другая копия программы
			using (SingleGlobalInstance instance = new SingleGlobalInstance(1000)) {
				if (!instance.Success) {
					MessageBox.Show("HMS Editor уже запущен!", HMSEditor.MsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);

				} else {
					// Всё норм, запускаемся. Для начала вставляем обработку события при неудачных зависимостях, а там загрузим внедрённые dll
					AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

					try {
						Application.EnableVisualStyles();
						Application.SetCompatibleTextRenderingDefault(false);

						// Загружаем встроенные шрифты
						HMS.AddFontFromResource("RobotoMono-Regular.ttf");
						HMS.AddFontFromResource("Roboto-Regular.ttf");

						// Заполняем базу знаний функций, классов, встроенных констант и переменных...
						HMS.InitAndLoadHMSKnowledgeDatabase();

						HMSEditor.DebugMe = CheckKey(args, "-debugga");

						if (CheckKey(args, "-givemesomemagic")) {
							// Запуск "тихого" режима
							HMSEditor.SilentMode = true;
                            if (HMSEditor.WatchHMS())
								Application.Run();
						} else {
							// Запуск в обычном режиме с появлением отдельного самостоятельного окна
							Application.Run(new FormMain());
						}

						// Проверяем, были ли выполнены все действия при выходе (снятие хуков и проч.)
						if (!HMSEditor.Exited) HMSEditor.Exit();

					} catch (Exception e) {
						MessageBox.Show("Очень жаль, но работа программы невозможна.\n\n"+ e.ToString(), HMSEditor.MsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
						HMS.LogError(e.ToString());

					}

				}
			}
		}

		/// <summary>
		/// Проверка на присутствие указанного ключа в аргументах запуска программы
		/// </summary>
		/// <param name="args">Массив аргументов</param>
		/// <param name="key">Проверяемый ключ (указывается вместе с "-")</param>
		/// <returns>Возвращает True, если такой аргумент присутствует</returns>
		static bool CheckKey(string[] args, string key) {
			key = key.Trim().ToLower();
			foreach (string arg in args)
				if (arg.Trim().ToLower() == key) return true;
			return false;
		}

		/// <summary>
		/// Функция, вызываемая при событии, в случае неудачного определения зависимостей (а оно произойдёт, поверьте). Тут мы это пытаемся исправить.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) {
			// Загрузка внедрённых библиотек (dll) из ресурсов в память
			byte[] buffer;
			string resource = "HMSEditorNS.Resources.Ionic.Zip.Reduced.dll"; // Мини-библиотека для работы с zip (ибо в .NET 2.0 нет поддержки zip файлов)
			Assembly assembly = Assembly.GetExecutingAssembly();
			using (System.IO.Stream stm = assembly.GetManifestResourceStream(resource)) {
				buffer = new byte[(int)stm.Length];
				stm.Read(buffer, 0, (int)stm.Length);
				return Assembly.Load(buffer);
			}
		}

	}
}
