using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Softumus.Blog.NotifyPropertyChanged
{
	public class NotificationObject
		: INotifyPropertyChanged
	{
		#region Implementation of INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		public void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
		{
			var propertyName = ((MemberExpression)propertyExpression.Body).Member.Name;

			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
