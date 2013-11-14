using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Softumus.Blog.NotifyPropertyChanged
{
	public static class On
	{
		public static void PropertyChanged<T>(PropertyChangedEventArgs e, Expression<Func<T>> propertyExpression, Action action)
		{
			var expression = (propertyExpression.Body is UnaryExpression)
									? (MemberExpression)((UnaryExpression)propertyExpression.Body).Operand
									: (MemberExpression)propertyExpression.Body;

			var name = expression.Member.Name;

			if (e.PropertyName == name)
				action();
		}

		public static void PropertyChanged<TSource>(PropertyChangedEventArgs e, Expression<Func<TSource, object>> propertyExpression, Action action)
		{
			var expression = (propertyExpression.Body is UnaryExpression)
									? (MemberExpression)((UnaryExpression)propertyExpression.Body).Operand
									: (MemberExpression)propertyExpression.Body;

			var name = expression.Member.Name;

			if (e.PropertyName == name)
				action();
		}
	}
}
