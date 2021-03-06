using System.Collections.Generic;
using Common.Interfaces;

namespace GamePlay.Client.Model
{
	/// <summary>
	/// 本地自动控制的设定
	/// </summary>
	[System.Serializable]
	public class ClientLocalSettings : ISubject<ClientLocalSettings>
	{
		private bool li = true;
		private bool he = false;
		private bool ming = false;
		private bool qie = false;
		private bool huan = true;

		public bool Li
		{
			get => li;
			set
			{
				li = value;
				NotifyObservers();
			}
		}

		public bool He
		{
			get => he;
			set
			{
				he = value;
				NotifyObservers();
			}
		}

		public bool Ming
		{
			get => ming;
			set
			{
				ming = value;
				NotifyObservers();
			}
		}

		public bool Qie
		{
			get => qie;
			set
			{
				qie = value;
				NotifyObservers();
			}
		}
		public bool Huan
		{
			get => huan;
			set
			{
				huan = value;
				ET.MahjoneHelper.OnChangeTileButton(huan);
				NotifyObservers();
			}
		}
		private IList<IObserver<ClientLocalSettings>> observers = new List<IObserver<ClientLocalSettings>>();

		public void AddObserver(IObserver<ClientLocalSettings> observer)
		{
			if (observer != null) observers.Add(observer);
		}

		public void RemoveObserver(IObserver<ClientLocalSettings> observer)
		{
			observers.Remove(observer);
		}

		public void NotifyObservers()
		{
			foreach (var observer in observers)
			{
				observer.UpdateStatus(this);
			}
		}

		public void Reset()
		{
			li = true;
			he = false;
			ming = false;
			qie = false;
			huan = true;
			NotifyObservers();
		}
	}
}