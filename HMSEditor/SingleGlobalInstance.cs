using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace HMSEditorNS {
	/// <summary>
	/// Класс, позволяющий создать только один экземпляр в системе. Если свойство Success после создания объекта этого класса равен false, то значит уже запущено приложение таким GUID.
	/// </summary>
	class SingleGlobalInstance: IDisposable {
		public bool Success = false;
		Mutex mutex;

		/// <summary>
		/// Попытка создания мутекса с именем GUID приложения, 
		/// </summary>
		/// <param name="timeOut">Время в миллисекундах, которое команда получения мутекса готова ждать (Timeout)</param>
		public SingleGlobalInstance(int timeOut) {
			// Получаем GUID и формируем имя мутекса
			string appGuid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();
			string mutexId = string.Format("Global\\{{{0}}}", appGuid);
			// Создание мутекса
			mutex = new Mutex(false, mutexId);

			// Установка прав доступа к нему
			var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
			var securitySettings = new MutexSecurity();
			securitySettings.AddAccessRule(allowEveryoneRule);
			mutex.SetAccessControl(securitySettings);
			try {
				// Попытка получить эксклюзивный доступ к мутексу с таким именем (если не получится, значит нас опередили)
				Success = mutex.WaitOne(timeOut, false);

			} catch (AbandonedMutexException) {
				// "Брошенный" мутекс (видимо завершили задачу в "Диспетчер задач")
				Success = true;
			}
		}

		/// <summary>
		/// Удаление объекта из памяти
		/// </summary>
		public void Dispose() {
			if (mutex != null) {
				if (Success)
					mutex.ReleaseMutex();
				mutex.Close();
			}
		}
	}
}
